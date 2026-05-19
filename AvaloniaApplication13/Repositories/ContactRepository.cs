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


        public Contact DeleteContact(Contact contact)
        {
            _db.Contacts.Remove(contact);
            _db.SaveChanges();
            return contact;
        }
        
    }
}