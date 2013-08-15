using Starcounter;

partial class PMail : Json {
    [PMail.json.Mailboxes]
    partial class MailboxesObj : Json<Mailbox> {
    }

	[PMail.json.Mails]
	partial class MailsObj : Json<Mail> {
	}
}
