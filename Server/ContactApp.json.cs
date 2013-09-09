using Starcounter;
using System;
using Starcounter.Advanced;
using Starcounter.Templates;

/// <summary>
/// This is the view-model for the contact person application. A view-model is an abstract model of the application.
/// Using Starcounter you can define these models by defining JSON trees with attached code-behind. The JSON tree is
/// an example on how an instance of the view-model looks like. Using the [xxxx_json] attributes, you can connect
/// an object in the tree to a code-behind class. In the code-behind, you can handle input from the user. You can
/// at any time update the view model on the server. Upon the next request for JSON-Patches (RFCXXXX), these changes
/// will be sent to the client where they will update the user interface (see Web Components or custom frameworks such
/// as Polymer or AngularJs).
/// </summary>
[ContactApp_json]
partial class ContactApp : Page {

    /// <summary>
    /// By changing view models on the server, patches are logged and can be asked for 
    /// by the client (by asking for the JSON-Patch log (mime-type application/json+patch).
    /// Here we add menu items by changing the view model of the master page.
    /// </summary>
    /// <param name="master">The view model of the main application</param>
    public void AddSomeNiceMenuItems(Master master) {
        master.ApplicationName = "Contacts";        // All of these properties are stuff we made up in the ContactApp.json file. Starcounter does not know about any user interface stuff. It simple deals with JSON trees that form the view model of the application. For example, this property represents the name of the select app. 
        master.Menu.Items.Clear();                  // The menues are represented by an array in the view-model. Remove all the current menu items (by removing all elements in the array
        var create = master.Menu.Items.Add();       // Add a menu item for the registration of new contacts. The Add() method ads an element to an array. The array is typed as the JSON-by-example has declared the kind of object it contains (the first object in an array in the sample .json file defines a prototype for the class).
        create.Label = "Add new contact";           // Each element in the array is a JSON object containing the text and icon for the menu item
        foreach (var c in Db.SQL<Contact>("SELECT c FROM Contact c")) { // Using strings in SQL selects is faster than using LINQ as the query will be replaced by a cached function call using the the string as a key.
            var item = master.Menu.Items.Add();     // Add a menu item for each contact group (TODO!)
            item.Label = c.DisplayName;             // The text of the menu item is the name of the contact group
            item.Uri = "/contacts/" + c.Id;         // In the HTML (master.html), we bind a link (a href) to this value. I.e. each menu item is a URL. If the user follows the link from within the application, Starcounter will return whatever you return in the GET handler. However, if you use the Puppet.js framework, and the user follows the link from within the application, the Puppet.js framework will only ask for the patches (using JSON-Patch) and the server will only send the delta of the current state of the page (html and view model) to morph the existing page into the new page. This makes the application respond very fast, reduces network traffic and retains all other state of the page such that animations, widgets and other stuff keep rolling. This makes your application a SPA (single page application) and a regular hyperlinked application at the same time. You get the best of both worlds.
        }
    }
}

/// <summary>
/// This view-model represents the contact person page in the contact application.
/// The code-behind is not limited to the root of the view model. Nested objects and objects inside arrays can also have code-behind attached to them.
/// This is accomplished by providing a [xxxx_json] tag with a path to the JSON object in the .json file. Even if this JSON object and this class
/// if defined in the contact app view model, the contact page is stand-alone and can be inherited and reused in other parts of the application.
/// For instance, if you have a list of contact persons, the list item can inherit the ContactPage class. In this way, you will never have
/// to repeat yourself. In the PMail application, you can find that you can edit contact persons in a Excel like spreadsheet. The same validation
/// code applies in the form as in each row in the list. Also, events such as the delete button in the page is also available in the list.
/// </summary>
[ContactApp_json.FocusedContact]
partial class ContactPage : Page {

    /// <summary>
    /// Called whenever a new contact person database object is attached (bound) to this view-model. In this case,
    /// we will add the possible options the user can choose from in the comboboxes in the form when adding emails
    /// and phone numbers (so the user can pick 'Home' or 'Work' as the role for the phone number for example).
    /// </summary>
    protected override void OnData() {
        EmailAddressRoleOptions = Db.SQL("SELECT r FROM EmailAddressRole r");    // We can assign a SQL result (an enumeratable of objects) to a view-model array. This will create instances of the view-model class of that array and set the Data attribute of each new view-model object.
        PhoneNumberRoleOptions = Db.SQL("SELECT r FROM PhoneNumberRole r");
    }
    
    void Handle(Input.AddAddress input) {
        var address = new EmailAddress();
        address.Contact = (Contact)this.Data;
        Addresses.Add().Data = address;
    }

    void Handle(Input.AddPhoneNumber input) {
        var phoneNumber = new PhoneNumber();
        phoneNumber.Contact = (Contact)this.Data;
        PhoneNumbers.Add().Data = phoneNumber;
    }

    void Handle(Input.Delete input) {
        Data.Delete();
        Transaction.Commit();
        ((ContactApp)Parent).FocusedContact = null;
    }
}

[ContactApp_json.FocusedContact.Addresses]
partial class AddressesObj : Json, IBound<EmailAddress> {

    /// <summary>
    /// The user has entered text in the mailaddress role (i.e. Home, Work, etc.) This is simply a PATCH
    /// message on the view model using the standard JSON-Patch format (RFCXXXX)
    /// </summary>
    /// <param name="input">The input event contains the new value etc.</param>
    void Handle(Input.SearchRole input) {
            var role = SQL("SELECT r FROM MailAddressRole r WHERE Name = ?", input.Value).First;
            if (role != null)
                Data.Role = role;
            else {
                var newRole = new EmailAddressRole();
                newRole.Name = input.Value;

                this.Data.Role = newRole;

                Master m = Master.GET("/master");
                var p = (ContactApp) m.ApplicationPage;
                p.FocusedContact.EmailAddressRoleOptions.Add().Data = newRole;
            }
    }

    /// <summary>
    /// The user has pressed the Remove button on the email address. This is simply a PATCH
    /// message on the view model using the standard JSON-Patch format (RFCXXXX)
    /// </summary>
    /// <param name="input">The event information</param>
    void Handle(Input.Remove input) {
        if (this.Parent is Arr)
            (this.Parent as Arr).Remove(this);
        this.Data.Delete();
    }
}

[ContactApp_json.FocusedContact.PhoneNumbers]
partial class PhoneNumbersObj : Json, IBound<PhoneNumber> {
        protected override void OnData() {
            Countries = SQL("SELECT c FROM Country c");
        }

        void Handle(Input.SearchRole input) {
            var role = SQL("SELECT r FROM PhoneNumberRole r WHERE Name = ?", input.Value).First;
            if (role != null)
				Data.Role = role;
            else {
                var newRole = new PhoneNumberRole();
                newRole.Name = input.Value;

				Data.Role = newRole;

                Master m = Master.GET("/master");
                var p = (ContactApp)m.ApplicationPage;
                ((ContactPage)p.FocusedContact).PhoneNumberRoleOptions.Add().Data = newRole;
            }
        }

        void Handle(Input.SearchCountry input) {
            var selectedIndex = Convert.ToInt32(input.Value);
            SearchCountry = selectedIndex;
			((PhoneNumber)(this.Data)).Country = (Country)this.Countries[selectedIndex].Data;
        }

        void Handle(Input.Remove input) {
            if (Parent is Arr)
                (Parent as Arr).Remove(this);
            Data.Delete();
        }
}

