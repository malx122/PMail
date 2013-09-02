
using Starcounter;
using System;
using System.Threading;

[Database]
public class Mail {
    static int GlobalId;
    public int Id;
    public Thread Thread;
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
}
