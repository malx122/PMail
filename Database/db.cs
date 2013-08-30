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


[Database]
public class Thread {
    public int Id;
    public string Uri { get { return "/threads/" + Id; } }
    public EmailAddress From { get { return this.LastMail.From; } }
    public string Subject { get { return this.LastMail.Subject; } }
    public Mail LastMail { get { return Db.SQL<Mail>("SELECT m FROM Mail m WHERE m.Thread=? ORDER BY m.Id DESC", this).First; } } //using Id because "ORDER BY m.Date" produces SQL syntax error (probably because a reserved word was used as property name) } }
    public SqlResult<Mail> Mails { get { return Db.SQL<Mail>("SELECT m FROM Mail m WHERE Thread=?", this); }}
    public int CountMails { get { return Db.SlowSQL<int>("SELECT COUNT(*) FROM Mail m WHERE Thread=?", this).First; } }
}

[Database]
public class Mail {
    static int GlobalId;
    public int Id;
    public DateTime Date;
    public EmailAddress From;
    public EmailAddress To;
    public string Subject;
    public string Content;
    public Mailbox Mailbox;
    public Mail() {
        Id = Interlocked.Increment(ref GlobalId);
        Date = DateTime.Now;
    }
    public string Uri { get { return "/mails/" + Id; } }
}
