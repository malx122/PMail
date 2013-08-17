using Starcounter;
using System;

partial class ContactPage : Json<Contact> {
    protected override void Init() {
        this.MailAddressRoleOptions = SQL("SELECT r FROM MailAddressRole r");
        this.PhoneNumberRoleOptions = SQL("SELECT r FROM PhoneNumberRole r");
    }

    [ContactPage.json.MailAddressRoleOptions]
    partial class MailAddressRoleOptionsObj : Json<MailAddressRole> {
    }
    
    [ContactPage.json._Addresses]
    partial class AddressesObj : Json<MailAddress> {
        void Handle(Input._Addresses.Address input) {
            this.Address = input.Value;
            this.Transaction.Commit();
        }

        void Handle(Input._Addresses.SearchRole input) {
            var role = SQL("SELECT r FROM MailAddressRole r WHERE Name = ?", input.Value).First;
            if (role != null) {
                this.Data.Role = role;
            }
            else {
                var newRole = new MailAddressRole();
                newRole.Name = input.Value;
                
                this.Data.Role = newRole;

                Master m = (Master)X.GET("/");
                PContacts p = (PContacts) m.ApplicationPage;
                ((ContactPage)p.FocusedContact).MailAddressRoleOptions.Add().Data = newRole;
            }
            this.Transaction.Commit();
        }

        void Handle(Input._Addresses.Remove input) {
            this.Parent.Remove(this);
            this.Data.Delete();
            this.Transaction.Commit();
        }
    }

    [ContactPage.json.PhoneNumberRoleOptions]
    partial class PhoneNumberRoleOptionsObj : Json<PhoneNumberRole> {
    }

    [ContactPage.json._PhoneNumbers]
    partial class PhoneNumbersObj : Json<PhoneNumber> {
        protected override void Init() {
            this._Countries = SQL("SELECT c FROM Country c");
        }

        [ContactPage.json._PhoneNumbers._Countries]
        partial class CountriesObj : Json<Country> {
        }

        [ContactPage.json._PhoneNumbers.Country]
        partial class CountryObj : Json<Country> {
        }

        void Handle(Input._PhoneNumbers.Number input) {
            this.Number = input.Value;
            this.Transaction.Commit();
        }

        void Handle(Input._PhoneNumbers.SearchRole input) {
            var role = SQL("SELECT r FROM PhoneNumberRole r WHERE Name = ?", input.Value).First;
            if (role != null) {
                this.Data.Role = role;
            }
            else {
                var newRole = new PhoneNumberRole();
                newRole.Name = input.Value;

                this.Data.Role = newRole;

                Master m = (Master)X.GET("/");
                PContacts p = (PContacts)m.ApplicationPage;
                ((ContactPage)p.FocusedContact).PhoneNumberRoleOptions.Add().Data = newRole;
            }
            this.Transaction.Commit();
        }

        void Handle(Input._PhoneNumbers.SearchCountry input) {
            var selectedIndex = Convert.ToInt32(input.Value);
            this.SearchCountry = selectedIndex;
            this.Data.Country = (Country)this._Countries[selectedIndex].Data;
            this.Transaction.Commit();
        }

        void Handle(Input._PhoneNumbers.Remove input) {
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
        address.Contact = this.Data;
        this._Addresses.Add().Data = address;
        this.Transaction.Commit();
    }

    void Handle(Input.AddPhoneNumber input) {
        var phoneNumber = new PhoneNumber();
        phoneNumber.Contact = this.Data;
        this._PhoneNumbers.Add().Data = phoneNumber;
        this.Transaction.Commit();
    }

    void Handle(Input.Delete input) {
        this.Data.Delete();
        this.Transaction.Commit();
        ((PContacts)this.Parent).FocusedContact = null;
    }
}
