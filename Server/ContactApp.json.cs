using Starcounter;
using System;
using Starcounter.Advanced;
using Starcounter.Templates;


[ContactApp_json]
partial class ContactApp : Page {
}

[ContactApp_json.FocusedContact]
partial class ContactPage : Page {
    protected override void OnData() {
        this.MailAddressRoleOptions = Db.SQL("SELECT r FROM EmailAddressRole r");
        this.PhoneNumberRoleOptions = Db.SQL("SELECT r FROM PhoneNumberRole r");
    }
    
    void Handle(Input.AddAddress input) {
        var address = new EmailAddress();
        address.Contact = (Contact)this.Data;
        this.Addresses.Add().Data = address;
        this.Transaction.Commit();
    }

    void Handle(Input.AddPhoneNumber input) {
        var phoneNumber = new PhoneNumber();
        phoneNumber.Contact = (Contact)this.Data;
        this.PhoneNumbers.Add().Data = phoneNumber;
        this.Transaction.Commit();
    }

    void Handle(Input.Delete input) {
        Data.Delete();
        Transaction.Commit();
        ((ContactApp)Parent).FocusedContact = null;
    }
}

[ContactApp_json.FocusedContact.Addresses]
partial class AddressesObj : Json, IBound<EmailAddress> {

    void Handle(Input.Address input) {
            this.Address = input.Value;
            this.Transaction.Commit();
    }

    void Handle(Input.SearchRole input) {
            var role = SQL("SELECT r FROM MailAddressRole r WHERE Name = ?", input.Value).First;
            if (role != null) {
                Data.Role = role;
            }
            else {
                var newRole = new EmailAddressRole();
                newRole.Name = input.Value;

                this.Data.Role = newRole;

                var m = Master.GET("/");
                var p = (ContactApp) m.ApplicationPage;
                p.FocusedContact.MailAddressRoleOptions.Add().Data = (IBindable)newRole;
            }
            this.Transaction.Commit();
    }

    void Handle(Input.Remove input) {
        if (this.Parent is Arr) {
            (this.Parent as Arr).Remove(this);
        }
            this.Data.Delete();
            this.Transaction.Commit();
    }
}

[ContactApp_json.FocusedContact.PhoneNumbers]
partial class PhoneNumbersObj : Json, IBound<PhoneNumber> {
        protected override void OnData() {
            this.Countries = SQL("SELECT c FROM Country c");
        }

        void Handle(Input.Number input) {
            this.Number = input.Value;
            this.Transaction.Commit();
        }

        void Handle(Input.SearchRole input) {
            var role = SQL("SELECT r FROM PhoneNumberRole r WHERE Name = ?", input.Value).First;
            if (role != null) {
				Data.Role = role;
            }
            else {
                var newRole = new PhoneNumberRole();
                newRole.Name = input.Value;

				Data.Role = newRole;

                var m = Master.GET("/");
                var p = (ContactApp)m.ApplicationPage;
                ((ContactPage)p.FocusedContact).PhoneNumberRoleOptions.Add().Data = (IBindable)newRole;
            }
            this.Transaction.Commit();
        }

        void Handle(Input.SearchCountry input) {
            var selectedIndex = Convert.ToInt32(input.Value);
            this.SearchCountry = selectedIndex;
			((PhoneNumber)(this.Data)).Country = (Country)this.Countries[selectedIndex].Data;
            this.Transaction.Commit();
        }

        void Handle(Input.Remove input) {
            if (Parent is Arr) {
                (Parent as Arr).Remove(this);
            }
            this.Data.Delete();
            this.Transaction.Commit();
        }
}

