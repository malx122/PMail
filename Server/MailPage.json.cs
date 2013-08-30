using Starcounter;
using System;

[MailPage_json]
partial class MailPage : Page {

    void Handle(Input.Send input) {
        ((Mail)Data).Date = DateTime.Now;
        this.Transaction.Commit();
    }
    void Handle(Input.Discard input) {
        this.Transaction.Rollback();
    }
}

[MailPage_json.To]
partial class MailTo : Json {
    void Handle(Input.Address input) {
        this.Options = Db.SQL("SELECT a FROM MailAddress a WHERE a.Address STARTS WITH ?", input.Value);
    }
}

