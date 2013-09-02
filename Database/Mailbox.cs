using Starcounter;

[Database]
public class Mailbox {
    public string Name, Icon;

    public Rows<Thread> Threads {
        get {
            return Db.SlowSQL<Thread>("SELECT Thread FROM Mail m WHERE Mailbox = ? GROUP BY Thread", this ); //select all Threads with at least one Mail in requested Mailbox
        }
    }
}
