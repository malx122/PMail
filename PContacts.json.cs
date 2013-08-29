using Starcounter;
using Starcounter.Templates;

[PContacts_json]
partial class PContacts : View {
	[BindChildren(Bound.Auto)]
    [PContacts_json.Contacts]
    partial class ContactsObj : Json {
    }
}
