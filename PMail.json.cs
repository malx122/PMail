using Starcounter;
using Starcounter.Templates;

[PMail_json]
partial class PMail : View {
    [PMail_json.Mailboxes]
    partial class MailboxesObj : Json {
    }

	[PMail_json.Mails]
	partial class MailsObj : Json {
	}
}
