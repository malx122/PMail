using Starcounter;

partial class MailPage : Json<Mail> {
    public string Uri {
    get {
        return "/emails/" + Data.Id;
   }
  }
}
