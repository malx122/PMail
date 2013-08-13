using Starcounter;

[Database]
public class MailAddress {
    public string Address;
    public Contact Contact;

    public MailAddress() {
        Address = "";
    }
}
