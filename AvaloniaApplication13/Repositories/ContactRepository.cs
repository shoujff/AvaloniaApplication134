using AvaloniaApplication13.Data;
using AvaloniaApplication13.Models;
using Microsoft.EntityFrameworkCore;
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
            if(contact.Groups !=null && contact.Groups.Any())
            {
                var groups = contact.Groups.ToList();
                contact.Groups.Clear();
                _db.Contacts.Add(contact);
                _db.SaveChanges();
                foreach (var group in groups)
                {
                    var existingGroup = _db.Groups.Find(group.Id);
                    if (existingGroup != null)
                    {
                        contact.Groups.Add(existingGroup);
                    }
                }
                _db.SaveChanges();
            } 
            
            return contact;
        }
        public Contact AddContactWithGroup(Contact contact,List<int> groupIds) 
        {
           
            _db.Contacts.Add(contact);
            _db.SaveChanges();
            foreach (var groupId in groupIds)
            {
                var group = _db.Groups.Find(groupId);
                if (group != null)
                {
                    contact.Groups.Add(group);
                }
            }
            _db.SaveChanges(); 
            return contact;

        }
        public void UpdateContactGroups(int contactId, List<int> groupIds)
        {
            var contact = _db.Contacts.Include(c => c.Groups).FirstOrDefault(c => c.Id == contactId);

            if(contact != null)
            {
                contact.Groups.Clear();
                foreach (var groupId in groupIds)
                {
                    var group = _db.Groups.Find(groupId);
                    if (group != null)
                    {
                        contact.Groups.Add(group);
                    }
                }
                _db.SaveChanges();
            }
        }
        public int ClearTrash(int userId)
        {
            var deletedContacts = _db.Contacts
                .Where(c => c.UserId == userId && c.IsDeleted)
                .ToList();

            _db.Contacts.RemoveRange(deletedContacts);
            _db.SaveChanges();
            return deletedContacts.Count;
        }



        public bool SoftDeleteContact(int contactId)
        {
            var contact = GetContactById(contactId);
            if (contact != null && !contact.IsDeleted)
            {
                contact.IsDeleted = true;
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool RestoreContact(int contactId)
        {
            var contact = GetContactById(contactId);
            if (contact != null && contact.IsDeleted)
            {
                contact.IsDeleted = false;
                _db.SaveChanges();
                return true;
            }
            return false;
        }
        public bool PermanentDeleteContact(int contactId)
        {
            var contact = GetContactById(contactId);
            if (contact != null)
            {
                _db.Contacts.Remove(contact);
                _db.SaveChanges();
                return true;
            }
            return false;
        }
        
        public Contact GetContactById(int id)
        {
            return _db.Contacts
                .Include(c => c.Groups)
                .FirstOrDefault(c => c.Id == id);
        }

    }
}