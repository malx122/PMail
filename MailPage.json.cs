using Starcounter;

partial class MailPage : Json<Mail> {
  public string Uri {
    get {
        return "/mails/" + Data.Id;
    }
   }

  [MailPage.json.From]
  partial class FromObj : Json<MailAddress> {
  }

  [MailPage.json.To]
  partial class ToObj : Json<MailAddress> {
  }

  [MailPage.json.To.Options]
  partial class ToOptionsObj : Json<MailAddress> {
  }

  void Handle(Input.To.Address input) {
      this.To.Options = Db.SQL("SELECT a FROM MailAddress a WHERE a.Address STARTS WITH ?", input.Value);
  }

  void Handle(Input.Send input) {
      this.Transaction.Commit();
  }

  void Handle(Input.Discard input) {
      this.Transaction.Rollback();
  }  
}
