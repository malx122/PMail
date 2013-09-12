using Starcounter;                                  // Most stuff relating to the database, JSON and communication is in this namespace

[Master_json]                                       // This attribute tells Starcounter that the class corresponds to an object in the JSON-by-example file.
partial class Master : Page {

    /// <summary>
    /// Every application in Starcounter works like a console application. They have an .EXE ending. They have a Main() function and
    /// they can do console output. However, they are run inside the scope of a database rather than connecting to it.
    /// </summary>
    static void Main() {

        Handle.POST("/init-demo-data", () => {      // The Handle class is where you register new handlers for incomming requests.
            DemoData.Create();                      // Will create some demo data.
            return 201;                             // Returning an integer is the shortcut for returning a response with a status code.
        });

        Handle.GET("/", () => {                     // The root page of our little PMail website.
            var m = Master.GET("/master");          // Create the view model for the main application frame.
            MailApp p = new MailApp();              // The email application also consists of a view model.
            p.Html = "/mailboxes.html";             // Starcounter is a generic server and does not know of Html, so this is a variable we create in Page.json
            p.AddSomeNiceMenuItems(m);              // Adds some menu items in the main menu (by modifying the master view model)
            m.ApplicationPage = p;                  // Place the email applications view model inside the main application frame as its subpage.
            return p;                               // Returns the home page. As you can see in Page.json, we taught it how to serve both HTML and the JSON view model without any extra work.
        });

        Handle.GET("/master", () => {
            Master m = new Master() {               // This is the root view model for our application. A view model is a JSON object/tree.
                Html = "/master.html",              // This is just a field we added to allow the client to know what Html to load. No magic.
            };            
            // m.Location[ MimeType.Application_Json ] = Server.Store(m, 10000);
            Session.Data = m;                       // Save the JSON on the server to be accessed using GET/PATCH to allow it to be used as a view model in web components.
            return m;                               // Return the JSON or the HTML depending on the type asked for. See Page.json on how Starcounter knowns what to return.
        });

        Handle.GET("/mailboxes/{?}", (string name) => {
            MailApp p = MailApp.GET("/");           // If this is a first time call (like if the URL came from a bookmark), this call will call into the "/" handler above. If this is a super-fast SPA call (i.e. the user is navigating from within the application, this call does nothing else than to return the already existing view model. This means that the server does nothing and the client does nothing. The stuff is already created at both ends.
            p.FocusedMailbox.Data = Db.SQL<Mailbox>("SELECT m FROM Mailbox m WHERE Name=?", name).First; // By setting the Data property on a JSON object, we bind its properties to the properties of the database object.
            return p;                               // Return the JSON, or the JSON-Patch or the Html depending on what the client asks for.
        });

        Handle.GET("/mailboxes/{?}/threads/{?}", (string name, string id) => {
            var thread = Db.SQL<Thread>("SELECT t FROM Thread t WHERE Id=?", id).First; // The database object representing a set of emails (a discussion)
            var page = new ThreadPage() {           // The viewmodel/html for the email thread
                Html = "/thread.html",
                Data = thread                       // Connect the database object to the view model
            };
            var box = MailApp.GET("/mailboxes/" + name);    // Let's get the parent page...
            box.FocusedMailbox.FocusedThread = page;        // ...and place the thread page inside it
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
            var p = new ContactApp() { Html = "/contacts.html" };
            p.AddSomeNiceMenuItems(m);
            p.Contacts = Db.SQL("SELECT c FROM Contact c");
            m.ApplicationPage = p;
            return p;
        });

        Handle.GET("/contacts/{?}", (string id) => {
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




