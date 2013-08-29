using Starcounter;
using System;
using Starcounter.Advanced;
using Starcounter.Templates;

[ContactPage_json]
partial class ContactPage : View {
    protected override void OnData() {
        this.MailAddressRoleOptions = Db.SQL("SELECT r FROM MailAddressRole r");
        this.PhoneNumberRoleOptions = Db.SQL("SELECT r FROM PhoneNumberRole r");
    }

    [ContactPage_json.MailAddressRoleOptions]
    partial class MailAddressRoleOptionsObj : Json {
    }

    [ContactPage_json._Addresses]
    partial class AddressesObj : Json {
        void Handle(Input.Address input) {
            this.Address = input.Value;
            this.Transaction.Commit();
        }

        void Handle(Input.SearchRole input) {
            var role = SQL("SELECT r FROM MailAddressRole r WHERE Name = ?", input.Value).First;
            if (role != null) {
                ((MailAddress)(this.Data)).Role = role;
            }
            else {
                var newRole = new MailAddressRole();
                newRole.Name = input.Value;

				((MailAddress)(this.Data)).Role = newRole;

                Master m = (Master)X.GET("/");
                PContacts p = (PContacts) m.ApplicationPage;
                ((ContactPage)p.FocusedContact).MailAddressRoleOptions.Add().Data = (IBindable)newRole;
            }
            this.Transaction.Commit();
        }

        void Handle(Input.Remove input) {
            this.Parent.Remove(this);
            this.Data.Delete();
            this.Transaction.Commit();
        }
    }

    [ContactPage_json.PhoneNumberRoleOptions]
    partial class PhoneNumberRoleOptionsObj : Json {
    }

    [ContactPage_json._PhoneNumbers]
    partial class PhoneNumbersObj : Json {
        protected override void OnData() {
            this._Countries = SQL("SELECT c FROM Country c");
        }

        [ContactPage_json._PhoneNumbers._Countries]
        partial class CountriesObj : Json {
        }

        [ContactPage_json._PhoneNumbers.Country]
        partial class CountryObj : Json {
        }

        void Handle(Input.Number input) {
            this.Number = input.Value;
            this.Transaction.Commit();
        }

        void Handle(Input.SearchRole input) {
            var role = SQL("SELECT r FROM PhoneNumberRole r WHERE Name = ?", input.Value).First;
            if (role != null) {
				((PhoneNumber)(this.Data)).Role = role;
            }
            else {
                var newRole = new PhoneNumberRole();
                newRole.Name = input.Value;

				((PhoneNumber)(this.Data)).Role = newRole;

                Master m = (Master)X.GET("/");
                PContacts p = (PContacts)m.ApplicationPage;
                ((ContactPage)p.FocusedContact).PhoneNumberRoleOptions.Add().Data = (IBindable)newRole;
            }
            this.Transaction.Commit();
        }

        void Handle(Input.SearchCountry input) {
            var selectedIndex = Convert.ToInt32(input.Value);
            this.SearchCountry = selectedIndex;
			((PhoneNumber)(this.Data)).Country = (Country)this._Countries[selectedIndex].Data;
            this.Transaction.Commit();
        }

        void Handle(Input.Remove input) {
            this.Parent.Remove(this);
            this.Data.Delete();
            this.Transaction.Commit();
        }
    }

    void Handle(Input.NamePrefix input) {
        this.NamePrefix = input.Value;
        this.Transaction.Commit();
    }

    void Handle(Input.FirstName input) {
        this.FirstName = input.Value;
        this.Transaction.Commit();
    }

    void Handle(Input.MiddleName input) {
        this.MiddleName = input.Value;
        this.Transaction.Commit();
    }

    void Handle(Input.LastName input) {
        this.LastName = input.Value;
        this.Transaction.Commit();
    }

    void Handle(Input.NameSuffix input) {
        this.NameSuffix = input.Value;
        this.Transaction.Commit();
    }

    void Handle(Input.Note input) {
        this.Note = input.Value;
        this.Transaction.Commit();
    }

    void Handle(Input.AddAddress input) {
        var address = new MailAddress();
        address.Contact = (Contact)this.Data;
        this._Addresses.Add().Data = (IBindable)address;
        this.Transaction.Commit();
    }

    void Handle(Input.AddPhoneNumber input) {
        var phoneNumber = new PhoneNumber();
        phoneNumber.Contact = (Contact)this.Data;
        this._PhoneNumbers.Add().Data = (IBindable)phoneNumber;
        this.Transaction.Commit();
    }

    void Handle(Input.Delete input) {
        this.Data.Delete();
        this.Transaction.Commit();
        ((PContacts)this.Parent).FocusedContact = null;
    }
}
