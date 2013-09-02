using Starcounter;
using System;
using System.Threading;

[Database]
public class PhoneNumberRole : Role {
}

[Database]
public class Country {
    public string Name;
    public string DialCode;
    public string FlagUri;
}

[Database]
public class Role {
    public string Name;
    public string Icon;
    public bool UserDefined;
}

[Database]
public class PhoneNumber {
    public string Number;
    public Contact Contact;
    public PhoneNumberRole Role;
    public Country Country;

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

    public int SearchCountry {
        get {
            var countries = Db.SQL("SELECT c FROM Country c");
            var i = 0;
            foreach (Country c in countries) {
                if (c.Equals(this.Country)) {
                    return i;
                }
                i++;
            }
            return -1;
        }
    }
}

