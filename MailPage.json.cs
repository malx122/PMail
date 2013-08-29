using Starcounter;
using System;
using Starcounter.Templates;

[BindChildren(Bound.Auto)]
[MailPage_json]
partial class MailPage : View {
  public string Uri {
    get {
        return "/mails/" + ((Mail)Data).Id;
    }
   }

	[BindChildren(Bound.Auto)]
  [MailPage_json.From]
  partial class FromObj : Json {
  }

	[BindChildren(Bound.Auto)]
  [MailPage_json.To]
  partial class ToObj : Json {
	  void Handle(Input.Address input) {
		this.Options = Db.SQL("SELECT a FROM MailAddress a WHERE a.Address STARTS WITH ?", input.Value);
	  }
  }

	[BindChildren(Bound.Auto)]
  [MailPage_json.To.Options]
  partial class ToOptionsObj : Json {
  }

  void Handle(Input.Send input) {
      ((Mail)Data).Date = DateTime.Now;
      this.Transaction.Commit();
  }

  void Handle(Input.Discard input) {
      this.Transaction.Rollback();
  }  
}
