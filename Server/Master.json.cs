using Starcounter;

[Master_json]
partial class Master : Page {


    static void Main(string[] args) {

        Handle.POST("/init-demo-data", () => {
            DemoData.Create();
            return 201;
        });

        Handle.GET("/", () => {
            Master m = new Master() {
                Html = (string)X.GET("/master.html")
            };
            Session.Data = m;
            return m;
        });

        Handle.GET("/mailboxes", () => {
            Master m = (Master)X.GET("/");
            MailApp p = new MailApp() {
                Html = (string)X.GET("/mailboxes.html")
            };
            p.Mailboxes = Db.SQL("SELECT m FROM Mailbox m");
            m.ApplicationPage = p;
            return p;
        });

        Handle.GET("/mailboxes/{?}", (string name) => {
            MailApp p = MailApp.GET("/pmail");
            p.FocusedMailbox.Data = Db.SQL("SELECT m FROM Mailbox m WHERE Name=?", name).First;
            p.Threads = Db.SlowSQL("SELECT Thread FROM Mail m WHERE Mailbox = ? GROUP BY Thread", p.FocusedMailbox.Data); //select all Threads with at least one Mail in requested Mailbox
            return p;
        });



        Handle.GET("/mailboxes/{?}/threads/{?}", (string name, int id) => {
            var thread = Db.SQL<Thread>("SELECT t FROM Thread t WHERE Id=?", id).First;
            var box = MailApp.GET("/mailboxes/" + name);
            var page = new ThreadPage() {
                Html = X.GET<string>("/thread.html"),
                Data = thread
            };
            box.FocusedThread = page;
            return page;
        });

        Handle.GET("/mails/compose", () => {
            var p = MailApp.GET("/mails");
            var m = new ThreadPage() { Html = (string)X.GET("/thread.html") };
            m.Transaction = new Transaction();
            m.Transaction.Add(() => { m.Data = new Mail(); });
            p.FocusedThread = m;
            return m;
        });

        Handle.GET("/contacts", () => {
            Master m = Master.GET("/");
            var p = new ContactApp() { Html = (string)X.GET("/partials/pcontacts.html") };
            p.Contacts = Db.SQL("SELECT c FROM Contact c");
            m.ApplicationPage = p;
            return p;
        });

        Handle.GET("/contacts/{?}", (int id) => {
            var contact = Db.SQL<Contact>("SELECT c FROM Contact c WHERE Id=?", id).First;
            var p = ContactApp.GET("/contacts");
            var page = new ContactPage() {
                Html = (string)X.GET("/partials/contact.html"),
                Data = contact,
                Addresses = contact.Addresses,
                PhoneNumbers = contact.PhoneNumbers
            };
            page.Transaction = new Transaction();
            p.FocusedContact = page;
            return p;
        });

        Handle.GET("/contacts/create", () => {
            var p = ContactApp.GET("/pcontacts");
            var page = new ContactPage() {
                Html = X.GET<string>("/partials/contact.html")
            };
            page.Transaction = new Transaction();
            page.Transaction.Add(() => { page.Data = new Contact(); });
            p.FocusedContact = page;
            return page;
        });
    }

}




