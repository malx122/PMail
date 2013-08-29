
using Starcounter;
using Starcounter.Advanced;
using Starcounter.Internal;
using Starcounter.Templates;

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
                Db.SlowSQL("DELETE FROM MailAddressRole");
                Db.SlowSQL("DELETE FROM PhoneNumberRole");

                var drafts = new Mailbox() { Name = "Drafts" };
                var inbox = new Mailbox() { Name = "Inbox" };
                var sent = new Mailbox() { Name = "Sent" };

                var lisa = new Contact() { Id = 91, FirstName = "Lisa", LastName = "Gherardini" };

                var work = new MailAddressRole() { Name = "Work" };
                var home = new MailAddressRole() { Name = "Home" };

                new PhoneNumberRole() { Name = "Mobile" };
                new PhoneNumberRole() { Name = "Work" };
                new PhoneNumberRole() { Name = "Home" };
                new PhoneNumberRole() { Name = "Work Fax" };
                new PhoneNumberRole() { Name = "Home Fax" };

                var me = new MailAddress() { Address = "me@example.com" };
                var them1 = new MailAddress() { Address = "lisa@them.com", Contact = lisa, Role = home };
                them1.Role = new MailAddressRole() { Name = "2nd Work" };

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
            Master m = new Master() {
                Html = (string)X.GET("/master.html")
            };
            Session.Data = m;
            return m;
        });

        Handle.GET("/pmail", () => {
            Master m = (Master)X.GET("/");
            PMail p = new PMail() {
                Html = (string)X.GET("/partials/pmail.html")
            };
            p.Mailboxes = Db.SQL("SELECT m FROM Mailbox m");
            m.ApplicationPage = p;
            return p;
        });

        Handle.GET("/mailboxes/{?}", (string name) => {
            PMail p = PMail.GET("/pmail");
            p.FocusedMailbox.Data = (IBindable)Db.SQL("SELECT m FROM Mailbox m WHERE Name=?", name).First;
            p.Mails = Db.SQL("SELECT e FROM Mail e WHERE Mailbox=?", p.FocusedMailbox.Data);
            return p;
        });

        Handle.GET("/mails/{?}", (int id) => {
            var mail = Db.SQL<Mail>("SELECT e FROM Mail e WHERE Id=?", id).First;
            var p = (PMail)X.GET("/mailboxes/" + mail.Mailbox.Name);
            var page = new MailPage() {
                Html = (string)X.GET("/partials/mail.html"),
                Data = (IBindable)mail
            };
            p.FocusedMail = page;
            return page;
        });

        Handle.GET("/mails/compose", () => {
            var p = (PMail)X.GET("/pmail");
			var m = new MailPage() { Html = (string)X.GET("/partials/compose.html") };
            m.Transaction = new Transaction();
            m.Transaction.Add(() => { m.Data = (IBindable)new Mail(); } );
            p.FocusedMail = m;
            return m;
        });

        Handle.GET("/pcontacts", () => {
            Master m = Master.GET("/");
            PContacts p = new PContacts() { Html = (string)X.GET("/partials/pcontacts.html") };
            p.Contacts = Db.SQL("SELECT c FROM Contact c");
            m.ApplicationPage = p;
            return p;
        });

        Handle.GET("/contacts/{?}", (int id) => {
            var contact = Db.SQL<Contact>("SELECT c FROM Contact c WHERE Id=?", id).First;
            var p = PContacts.GET("/pcontacts");
            var page = new ContactPage() {
                Html = (string)X.GET("/partials/contact.html"),
                Data = (IBindable)contact,
                _Addresses = contact.Addresses,
                _PhoneNumbers = contact.PhoneNumbers
            };
            page.Transaction = new Transaction();
            p.FocusedContact = page;
            return page;
        });

        Handle.GET("/contacts/create", () => {
            PContacts p = (PContacts)X.GET("/pcontacts");
            var page = new ContactPage() {
                Html = (string)X.GET("/partials/contact.html")
            };
            page.Transaction = new Transaction();
            page.Transaction.Add(() => { page.Data = (IBindable)new Contact(); });
            p.FocusedContact = page;
            return page;
        });
    }
}