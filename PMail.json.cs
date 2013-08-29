using Starcounter;
using Starcounter.Templates;

[PMail_json]
partial class PMail : View {
	[BindChildren(Bound.Auto)]
    [PMail_json.Mailboxes]
    partial class MailboxesObj : Json {
    }

	[BindChildren(Bound.Auto)]
	[PMail_json.Mails]
	partial class MailsObj : Json {
	}
}
