using Starcounter;

[Database]
public class Contact {
    public int Id;
    public string FirstName;
    public string LastName;

    public string Uri {
        get {
            return "/contacts/" + Id;
        }
    }
}
