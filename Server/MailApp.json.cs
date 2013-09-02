using Starcounter;

[MailApp_json]
partial class MailApp : Page {}


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
