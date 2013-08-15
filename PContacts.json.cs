using Starcounter;

partial class PContacts : Json {
    [PContacts.json.Contacts]
    partial class ContactsObj : Json<Contact> {
    }
}
