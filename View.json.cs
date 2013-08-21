using Starcounter;

partial class View<T> : Json<T> {


    // Browsers will ask for "text/html" and we will give it to them
    // by loading the contents of the URI in our Html property
    public override string AsMimeType(MimeType type) {
        if (type == MimeType.Text_Html) {
            return X.GET<string>(this.Html);
        }
        return base.AsMimeType(type);
    }

}