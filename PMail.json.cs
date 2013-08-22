using Starcounter;

[PMail_json]
partial class PMail : View<object> {
    [PMail_json.Mailboxes]
    partial class MailboxesObj : Json<Mailbox> {
    }

	[PMail_json.Mails]
	partial class MailsObj : Json<Mail> {
	}
}
