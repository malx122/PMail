using Starcounter;

partial class PMail : View<object> {
    [PMail.json.Mailboxes]
    partial class MailboxesObj : Json<Mailbox> {
    }

	[PMail.json.Mails]
	partial class MailsObj : Json<Mail> {
	}
}
