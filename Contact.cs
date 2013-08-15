using Starcounter;

[Database]
public class Contact {
    public int Id;
    public string FirstName;
    public string LastName;

    public string DisplayName {
        get {
            if (FirstName != "" && LastName != "") {
                return LastName + ", " + FirstName;
            }
            else if (FirstName != "") {
                return FirstName;
            }
            else {
                return LastName;
            }
        }
    }

    public string Uri {
        get {
            return "/contacts/" + Id;
        }
    }

    public SqlResult<MailAddress> Addresses {
        get {
            return Db.SQL<MailAddress>("SELECT m FROM MailAddress m WHERE Contact=?", this);
        }
    }
}
