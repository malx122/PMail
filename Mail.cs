using System.Threading;
using Starcounter;
using System;

[Database]
public class Mail {
    static int GlobalId;

    public int Id;
    public DateTime Date;
    public MailAddress From;
    public MailAddress To;
    public string Subject;
    public string Content;
    public Mailbox Mailbox;

    public Mail() {
        Id = Interlocked.Increment(ref GlobalId);
        Date = DateTime.Now;
        From = Db.SQL<MailAddress>("SELECT m FROM MailAddress m").First;
        To = new MailAddress();
        Subject = "";
        Content = "";
        Mailbox = Db.SQL<Mailbox>("SELECT m FROM Mailbox m").First;
    }

	public string Uri {
		get {
			return "/mails/" + Id;
		}
	}
}
