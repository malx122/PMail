using Starcounter;

[PContacts_json]
partial class PContacts : View<object> {
    [PContacts_json.Contacts]
    partial class ContactsObj : Json<Contact> {
    }
}
