using Starcounter;
using System;

//move Mail.cs here after merge with `threads` branch

[Database]
public class Mailbox {
    public string Name;
}

[Database]
public class MailAddress {
    public string Address;
    public Contact Contact;
    public MailAddressRole Role;

    public MailAddress() {
        Address = "";
    }
}

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

    public SqlResult<PhoneNumber> PhoneNumbers {
        get {
            return Db.SQL<PhoneNumber>("SELECT p FROM PhoneNumber p WHERE Contact=?", this);
        }
    }
}

[Database]
public class MailAddressRole : Role {

}

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

    public PhoneNumber() {
        Number = "";
    }
}