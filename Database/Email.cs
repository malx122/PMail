
using Starcounter;
using System;
using System.Threading;

[Database]
public class Mail {
    public Thread Thread;
    public DateTime Date;
    public EmailAddress From;
    public EmailAddress To;
    public string Subject;
    public string Content;
    public Mailbox Mailbox;

    public Mail() {
        Date = DateTime.Now;
    }

    public string Id
    {
        get
        {
            return DbHelper.GetObjectID(this);
        }
    }
   
}
