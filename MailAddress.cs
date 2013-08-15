using Starcounter;

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
public class MailAddressRole : Role {
    
}

[Database]
public class PhoneRole : Role {
    public Country Country;
}

[Database]
public class Role {
    public string Name;
    public string Icon;
    public bool UserDefined;
}

[Database]
public class Country {
    public string Name;
    public string DialCode;
    public string FlagUri;
}

