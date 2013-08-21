using Starcounter;

partial class PContacts : View<object> {
    [PContacts.json.Contacts]
    partial class ContactsObj : Json<Contact> {
    }
}
