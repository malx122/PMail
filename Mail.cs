using Starcounter;

[Database]
public class Mail {
    public int Id;
    public string From;
    public string To;
    public string Subject;
    public string Content;
    public Mailbox Mailbox;

	public string Uri {
		get {
			return "/mails/" + Id;
		}
	}
}
