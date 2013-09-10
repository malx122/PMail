using Starcounter;

[MailApp_json]
partial class MailApp : Page {

    /// <summary>
    /// Do some basic initialization
    /// </summary>
    /// <param name="master">We take the master app as a parameter as we want to change the contents of the main menu</param>
    public MailApp( Master master ) {
        master.ApplicationName = "Mail";
        Mailboxes = Db.SQL("SELECT m FROM Mailbox m");
        AddSomeNiceMenuItems(master);
    }

    /// JSON objects can be manipulated on the server at any time. In this method, we add some items to a JSON array
    /// containing menu choices. These will be rendered on the client in the main side bar menu.
    public void AddSomeNiceMenuItems(Master master) {
        master.ApplicationName = "Mails";
        master.Menu.Items.Clear();                  // These are just JSON properties we created in the Master.json file. They can have any name and value.
        var create = master.Menu.Items.Add();       // Adds a new item to an array. The { Menu: Items: [ { ... } ] } can be found in Master.json.
        create.Label = "Compose new email";         // Let's have a menu button to allow us to create new emails.
        create.Uri = "/compose-mail";               // This is the link to the page allowing the user to compose new emails
        foreach (var mb in Db.SQL<Mailbox>("SELECT m FROM Mailbox m")) { // Let's add all mailboxes to the menu
            var item = master.Menu.Items.Add();
            item.Label = mb.Name;
            item.Icon = mb.Icon;
            item.Uri = "/mailboxes/" + mb.Name;     // Each mailbox has its own URL.
        }
    }
}


[MailApp_json.FocusedMailbox.Threads]
partial class ThreadRow : ThreadPage {
}

[MailApp_json.FocusedMailbox]
partial class MailBoxPage : Page, IBound<Mailbox> {
    protected override string UriFragment {
        get {
            return "/mailboxes/" + Data.Name;
        }
    }
}

[MailApp_json.FocusedMailbox.FocusedThread]
partial class ThreadPage : Page, IBound<Thread> {
    protected override string UriFragment {
        get {
            return "/threads/" + Data.Id;
        }
    }

}

[MailApp_json.FocusedMailbox.FocusedThread.Mails]
partial class MailsElement : MailPage {
}
