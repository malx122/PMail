using Starcounter;
using System.Threading;

[Database]
public class Contact {
    static int GlobalId;

    public int Id;
    public string NamePrefix;
    public string FirstName;
    public string MiddleName;
    public string LastName;
    public string NameSuffix;
    public string Note;

    public Contact() {
        Id = Interlocked.Increment(ref GlobalId);
    }

    public string DisplayName {
        get {
            string[] all = new string[] { NamePrefix, FirstName, MiddleName, LastName, NameSuffix };
            string complete = string.Join(" ", all);
            return System.Text.RegularExpressions.Regex.Replace(complete, @"\s+", " ");
        }
        set {
            //if we allow to edit contact name details in a single `DisplayName` field,
            //here goes logic how to parse this field into actual properties
        }
    }

    public string Uri { get { return "/contacts/" + Id; } }

    public SqlResult<EmailAddress> Addresses { get { return Db.SQL<EmailAddress>("SELECT m FROM MailAddress m WHERE Contact=?", this); } }

    public SqlResult<PhoneNumber> PhoneNumbers  { get { return Db.SQL<PhoneNumber>("SELECT p FROM PhoneNumber p WHERE Contact=?", this); } }
}
