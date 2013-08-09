
using Starcounter;
using Starcounter.Internal;

class Program {
    static void Main(string[] args) {

        Handle.POST("/init-demo-data", () => {
                Db.Transaction(() => {
                    Db.SlowSQL("DELETE FROM Mailbox");
                    Db.SlowSQL("DELETE FROM MailAddress");
                    Db.SlowSQL("DELETE FROM Mail");
                    var inbox = new Mailbox() { Name = "Inbox" };
                    var sent = new Mailbox() { Name = "Sent" };
                    var me = new MailAddress() { Address = "me@example.com" };
                    var them1 = new MailAddress() { Address = "lisa@them.com" };
                    var them2 = new MailAddress() { Address = "joe@spammers.com" };
                    new Mail() { Id=123, From=them1, To=me, Subject="Hi there", Content="How are you", Mailbox=inbox };
                    new Mail() { Id=124, From=them2, To=me, Subject="Buy diet pills", Content="Guaranteed results", Mailbox=inbox };
                    new Mail() { Id=125, From=them2, To=me, Subject="Business opportunity", Content="Call me", Mailbox=inbox };
                    new Mail() { Id=126, From=me, To=them2, Subject="Re: But diet pill", Content = "No thank you", Mailbox=sent };
                });
                return 201;
            });

        Handle.GET("/", () => {
            Master m = new Master() { Html = "master.html" };
            Session.Data = m;
            return m;
        });

        Handle.GET("/pmail", () => {
            Master m = (Master)X.GET("/");
            PMail p = new PMail() { Html = "partials/pmail.html" };
            p.Mailboxes = Db.SQL("SELECT m FROM Mailbox m");
            m.ApplicationPage = p;
            return p;
        });

        Handle.GET("/mailboxes/{?}", (string name) => {
            PMail p = (PMail)X.GET("/pmail");
            p.FocusedMailbox.Data = Db.SQL("SELECT m FROM Mailbox m WHERE Name=?", name).First;
            p.Mails = Db.SQL("SELECT e FROM Mail e WHERE Mailbox=?", p.FocusedMailbox.Data);
            return p;
        });

        Handle.GET("/mails/{?}", (int id) => {
            var mail = Db.SQL("SELECT e FROM Mail e WHERE Id=?", id).First;
            PMail p = (PMail)X.GET("/mailboxes/" + mail.Mailbox.Name);
            var page = new MailPage() {
                Html = "partials/mail.html",
                Data = mail
            };
            p.FocusedMail = page;
            return page;
        });

        Handle.GET("/mails/new-email", () => {
            return null;
        });
    }
}