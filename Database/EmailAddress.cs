using Starcounter;

[Database]
public class EmailAddress {
    public string Address;
    public Contact Contact;
    public EmailAddressRole Role;

    public string SearchRole {
        get {
            if (this.Role != null) {
                return this.Role.Name;
            }
            else {
                return "";
            }
        }
    }
}
