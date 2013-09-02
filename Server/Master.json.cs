using Starcounter;
using Starcounter.Advanced;

[Master_json]
partial class Master : Page {


    static void Main(string[] args) {

        Handle.POST("/init-demo-data", () => {
            DemoData.Create();
            return 201;
        });

        Handle.GET("/master", () => {
            Master m = new Master() {
                Html = "/master.html",
            };
            Session.Data = m;
            return m;
        });

        Handle.GET("/", () => {
            var m = Master.GET("/master");
            m.ApplicationName = "Mail";

            m.Menu.Items.Clear();
            var create = m.Menu.Items.Add();
            create.Label = "Compose new email";
            foreach (var mb in Db.SQL<Mailbox>("SELECT m FROM Mailbox m")) {
                var item = m.Menu.Items.Add();
                item.Label = mb.Name;
                item.Icon = mb.Icon;
                item.Uri = "/mailboxes/" + mb.Name;
            }

            MailApp p = new MailApp() {
                Html = "/mailboxes.html",
            };
            p.Mailboxes = Db.SQL("SELECT m FROM Mailbox m");
            m.ApplicationPage = p;
            return p;
        });

        Handle.GET("/mailboxes/{?}", (string name) => {
            MailApp p = MailApp.GET("/");
            p.FocusedMailbox.Data = Db.SQL<Mailbox>("SELECT m FROM Mailbox m WHERE Name=?", name).First;
            return p;
        });

        Handle.GET("/mailboxes/{?}/threads/{?}", (string name, int id) => {
            var thread = Db.SQL<Thread>("SELECT t FROM Thread t WHERE Id=?", id).First;
            var box = MailApp.GET("/mailboxes/" + name);
            var page = new ThreadPage() {
                Html = "/thread.html",
                Data = thread
            };
            box.FocusedMailbox.FocusedThread = page;
            return page;
        });

        Handle.GET("/compose-mail", () => {
            var p = Master.GET("/master");
            var m = new MailPage() { Html = "/email-compose.html" };
            m.Transaction = new Transaction();
            m.Transaction.Add(() => { m.Data = new Mail(); });
            p.ApplicationPage = m;
            return m;
        });

        Handle.GET("/contacts", () => {
            Master m = Master.GET("/master");
            m.ApplicationName = "Contacts";
            m.Menu.Items.Clear();
            var create = m.Menu.Items.Add();
            create.Label = "Add new contact";
            foreach (var c in Db.SQL<Contact>("SELECT c FROM Contact c")) {
                var item = m.Menu.Items.Add();
                item.Label = c.DisplayName;
                item.Uri = "/contacts/" + c.Id;
            }

            var p = new ContactApp() { Html = "/contacts.html" };
            p.Contacts = Db.SQL("SELECT c FROM Contact c");
            m.ApplicationPage = p;
            return p;
        });

        Handle.GET("/contacts/{?}", (int id) => {
            var contact = Db.SQL<Contact>("SELECT c FROM Contact c WHERE Id=?", id).First;
            var p = ContactApp.GET("/contacts");
            var page = new ContactPage() {
                Html = "/contact.html",
                Data = contact,
                Addresses = contact.Addresses,
                PhoneNumbers = contact.PhoneNumbers
            };
            page.Transaction = new Transaction();
            p.FocusedContact = page;
            return p;
        });

        Handle.GET("/contacts/create", () => {
            var p = ContactApp.GET("/contacts");
            var page = new ContactPage() {
                Html = "/contact.html"
            };
            page.Transaction = new Transaction();
            page.Transaction.Add(() => { page.Data = new Contact(); });
            p.FocusedContact = page;
            return page;
        });
    }

}




