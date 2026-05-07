using AvaloniaApplication13.Data;
using AvaloniaApplication13.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication13.Repositories
{
    public class ContactRepository
    {

        private readonly DataBase _db;
        public ContactRepository()
        {
            _db = new DataBase();
        }
        public Contact AddContact(Contact contact)
        {
            _db.Contacts.Add(contact);
            _db.SaveChanges();
            return contact;
        }
        public void DleteContactByPhone(string phone, int userId)
        {
            var contact = _db.Contacts.FirstOrDefault(c => c.Phone == phone && c.UserId == userId);
            if (contact != null)
            {
                _db.Contacts.Remove(contact);
                _db.SaveChanges();
            }
        }
        public Contact DeleteContact(Contact contact)
        {
            _db.Contacts.Remove(contact);
            _db.SaveChanges();
            return contact;
        }
    }
}