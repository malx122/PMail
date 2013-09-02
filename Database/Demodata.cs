
using Starcounter;

public class DemoData {
    public static void Create() {
        Db.Transaction(() => {
            Db.SlowSQL("DELETE FROM Mailbox");
            Db.SlowSQL("DELETE FROM EmailAddress");
            Db.SlowSQL("DELETE FROM Contact");
            Db.SlowSQL("DELETE FROM Mail");
            Db.SlowSQL("DELETE FROM Country");
            Db.SlowSQL("DELETE FROM EmailAddressRole");
            Db.SlowSQL("DELETE FROM PhoneNumberRole");
            Db.SlowSQL("DELETE FROM Thread");

            var drafts = new Mailbox() { Name = "Drafts", Icon="settings" };
            var inbox = new Mailbox() { Name = "Inbox" };
            var sent = new Mailbox() { Name = "Sent" };

            var lisa = new Contact() { Id = 91, FirstName = "Lisa", LastName = "Gherardini" };

            var work = new EmailAddressRole() { Name = "Work" };
            var home = new EmailAddressRole() { Name = "Home" };

            new PhoneNumberRole() { Name = "Mobile" };
            new PhoneNumberRole() { Name = "Work" };
            new PhoneNumberRole() { Name = "Home" };
            new PhoneNumberRole() { Name = "Work Fax" };
            new PhoneNumberRole() { Name = "Home Fax" };

            var me = new EmailAddress() { Address = "me@example.com" };
            var them1 = new EmailAddress() { Address = "lisa@them.com", Contact = lisa, Role = home };
            them1.Role = new EmailAddressRole() { Name = "2nd Work" };
            var t1 = new Thread();

            var them2 = new EmailAddress() { Address = "joe@spammers.com" };
            var t2 = new Thread();

            new Country() { Name = "Sweden", DialCode = "+46" };
            new Country() { Name = "Norway", DialCode = "+47" };
            new Country() { Name = "Poland", DialCode = "+48" };
            new Country() { Name = "Germany", DialCode = "+49" };

            new Mail() { Id = 123, Thread = t1, From = them1, To = me, Subject = "Hi there", Content = "How are you", Mailbox = inbox };
            new Mail() { Id = 124, Thread = t2, From = them2, To = me, Subject = "Buy diet pills", Content = "Guaranteed results", Mailbox = inbox };
            new Mail() { Id = 125, Thread = t2, From = them2, To = me, Subject = "Business opportunity", Content = "Call me", Mailbox = inbox };
            new Mail() { Id = 126, Thread = t1, From = me, To = them2, Subject = "Re: But diet pill", Content = "No thank you", Mailbox = sent };
        });
    }
}