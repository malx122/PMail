using Starcounter;
using System;

[Database]
public class Contact {
    public int Id;
    public string NamePrefix;
    public string FirstName;
    public string MiddleName;
    public string LastName;
    public string NameSuffix;

    public Contact() {
        NamePrefix = "";
        FirstName = "";
        MiddleName = "";
        LastName = "";
        NameSuffix = "";
    }

    public string DisplayName {
        get {
            string[] all = new string[] { NamePrefix, FirstName, MiddleName, LastName, NameSuffix };
            string complete = String.Join(" ", all);
            return System.Text.RegularExpressions.Regex.Replace(complete, @"\s+", " ");
        }
        set {
            //if we allow to edit contact name details in a single `DisplayName` field,
            //here goes logic how to parse this field into actual properties
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
