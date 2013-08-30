using Starcounter;

[MailApp_json]
partial class MailApp : Page {}


[MailApp_json.FocusedThread]
partial class ThreadPage : Page {
}

[MailApp_json.FocusedThread.Mails]
partial class MailsElement : MailPage {
}
