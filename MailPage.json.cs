using Starcounter;
using System;

[MailPage_json]
partial class MailPage : View<Mail> {
  public string Uri {
    get {
        return "/mails/" + Data.Id;
    }
   }

  [MailPage_json.From]
  partial class FromObj : Json<MailAddress> {
  }

  [MailPage_json.To]
  partial class ToObj : Json<MailAddress> {
  }

  [MailPage_json.To.Options]
  partial class ToOptionsObj : Json<MailAddress> {
      void Handle(Input.Address input) {
          this.To.Options = Db.SQL("SELECT a FROM MailAddress a WHERE a.Address STARTS WITH ?", input.Value);
      }
  }


  void Handle(Input.Send input) {
      Data.Date = DateTime.Now;
      this.Transaction.Commit();
  }

  void Handle(Input.Discard input) {
      this.Transaction.Rollback();
  }  
}
