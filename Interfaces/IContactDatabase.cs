using MauiContactManager.Models;

namespace MauiContactManager.Interfaces
{
    public interface IContactDatabase
    {
        List<ContactModel> GetContacts();
        ContactModel GetContact(int id);
        void SaveContact(ContactModel contact);
        void DeleteContact(int id);

    }
}
