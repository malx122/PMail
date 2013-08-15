using Starcounter;
using System;

partial class ContactPage : Json<Contact> {
    [ContactPage.json._Addresses]
    partial class AddressesObj : Json<MailAddress> {
        void Handle(Input._Addresses.Address input) {
            this.Address = input.Value;
            this.Transaction.Commit();
        }

        void Handle(Input._Addresses.Remove input) {
            this.Parent.Remove(this);
            this.Data.Delete();
            this.Transaction.Commit();
        }
    }

    void Handle(Input.FirstName input) {
        this.FirstName = input.Value;
        this.Transaction.Commit();
    }

    void Handle(Input.LastName input) {
        this.LastName = input.Value;
        this.Transaction.Commit();
    }

    void Handle(Input.AddAddress input) {
        var address = new MailAddress();
        address.Contact = this.Data;
        this._Addresses.Add().Data = address;
        this.Transaction.Commit();
    }

    void Handle(Input.Delete input) {
        this.Data.Delete();
        this.Transaction.Commit();
        ((PContacts)this.Parent).FocusedContact = null;
    }
}
