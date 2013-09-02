
using Starcounter;
[Database]
public class Thread {
    public int Id;
    public EmailAddress From { get { return this.LastMail.From; } }
    public string Subject { get { return this.LastMail.Subject; } }
    public Mail LastMail { get { return Db.SQL<Mail>("SELECT m FROM Mail m WHERE m.Thread=? ORDER BY m.Id DESC", this).First; } } //using Id because "ORDER BY m.Date" produces SQL syntax error (probably because a reserved word was used as property name) } }
    public SqlResult<Mail> Mails { get { return Db.SQL<Mail>("SELECT m FROM Mail m WHERE Thread=?", this); } }
    public long CountMails { get { return Db.SlowSQL<long>("SELECT COUNT(*) FROM Mail m WHERE Thread=?", this).First; } }
}
