
using Starcounter;
using Starcounter.Internal;

class Program {
    static void Main(string[] args) {

        Handle.POST("/init-demo-data", () => {
            Db.Transaction(() =>
            {
                Db.SlowSQL("DELETE FROM Mailbox");
                Db.SlowSQL("DELETE FROM MailAddress");
                Db.SlowSQL("DELETE FROM Contact");
                Db.SlowSQL("DELETE FROM Mail");
                Db.SlowSQL("DELETE FROM Country");

                var drafts = new Mailbox() { Name = "Drafts" };
                var inbox = new Mailbox() { Name = "Inbox" };
                var sent = new Mailbox() { Name = "Sent" };

                var lisa = new Contact() { Id = 91, FirstName = "Lisa", LastName = "Gherardini" };

                var work = new MailAddressRole() { Name = "Work" };
                var home = new MailAddressRole() { Name = "Home" };

                var me = new MailAddress() { Address = "me@example.com" };
                var them1 = new MailAddress() { Address = "lisa@them.com", Contact = lisa, Role = home };
                them1.Role = new MailAddressRole() { Name = "Mistress" };

                var them2 = new MailAddress() { Address = "joe@spammers.com" };

                new Country() { Name = "Sweden", DialCode = "+46" };
                new Country() { Name = "Norway", DialCode = "+47" };
                new Country() { Name = "Poland", DialCode = "+48" };
                new Country() { Name = "Germany", DialCode = "+49" };

                new Mail() { Id = 123, From = them1, To = me, Subject = "Hi there", Content = "How are you", Mailbox = inbox };
                new Mail() { Id = 124, From = them2, To = me, Subject = "Buy diet pills", Content = "Guaranteed results", Mailbox = inbox };
                new Mail() { Id = 125, From = them2, To = me, Subject = "Business opportunity", Content = "Call me", Mailbox = inbox };
                new Mail() { Id = 126, From = me, To = them2, Subject = "Re: But diet pill", Content = "No thank you", Mailbox = sent };
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

        Handle.GET("/mails/compose", () => {
            PMail p = (PMail)X.GET("/pmail");
            var m = new MailPage() { Html = "partials/compose.html" };
            m.Transaction = new Transaction();
            m.Transaction.Add(() => { m.Data = new Mail(); } );
            p.FocusedMail = m;
            return m;
        });

        Handle.GET("/pcontacts", () => {
            Master m = (Master)X.GET("/");
            PContacts p = new PContacts() { Html = "partials/pcontacts.html" };
            p.Contacts = Db.SQL("SELECT c FROM Contact c");
            m.ApplicationPage = p;
            return p;
        });

        Handle.GET("/contacts/{?}", (int id) => {
            var contact = Db.SQL("SELECT c FROM Contact c WHERE Id=?", id).First;
            PContacts p = (PContacts)X.GET("/pcontacts");
            var page = new ContactPage() {
                Html = "partials/contact.html",
                Data = contact,
                _Addresses = contact.Addresses,
                _PhoneNumbers = contact.PhoneNumbers
            };
            page.Transaction = new Transaction();
            p.FocusedContact = page;
            return page;
        });
    }
}