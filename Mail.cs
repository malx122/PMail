using System.Threading;
using Starcounter;

[Database]
public class Mail {
    static int GlobalId;

    public int Id;
    public MailAddress From;
    public MailAddress To;
    public string Subject;
    public string Content;
    public Mailbox Mailbox;

    public Mail() {
        Id = Interlocked.Increment(ref GlobalId);
        From = Db.SQL("SELECT m FROM MailAddress m").First;
        To = new MailAddress();
        Subject = "";
        Content = "";
        Mailbox = Db.SQL("SELECT m FROM Mailbox m").First;
    }

	public string Uri {
		get {
			return "/mails/" + Id;
		}
	}
}
