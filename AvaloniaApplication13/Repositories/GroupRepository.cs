using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvaloniaApplication13.Data;
using AvaloniaApplication13.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Dapper;

namespace AvaloniaApplication13.Repositories
{
    public class GroupRepository
    {
        private string conn;
        private SqlConnection _dapper;
        private readonly DataBase _db;
        public GroupRepository(string conn)
        {
            _db = new DataBase();
            conn = conn;
            _dapper = new(conn);
        }
        public Group AddGroup(Group group)

        {
            _db.Groups.Add(group);
            _db.SaveChanges();
            return group;
        }
        public void AddContactToGroup(int contactId, int groupId)
        {
            var contact = _db.Contacts.
                Include(C => C.Groups).
                FirstOrDefault(c => c.Id == contactId);
            var group = _db.Groups
              .Include(g => g.Contacts)
              .FirstOrDefault(g => g.Id == groupId);

            if (contact != null && group != null && !contact.Groups.Contains(group) )
            {
                contact.Groups.Add(group);
                _db.SaveChanges();
            }

        }
        public void RemoveContactFromGroup(int contactId, int groupId)
        {
            var contact = _db.Contacts
                .Include(c => c.Groups)
                .FirstOrDefault(c => c.Id == contactId);

            var group = _db.Groups.FirstOrDefault(g => g.Id == groupId);

            if (contact != null && group != null)
            {
                contact.Groups.Remove(group);
                _db.SaveChanges();
            }
        }
        public List<Group> GetAllGroups()
        {
            return _db.Groups.ToList();

        }
        public Group GetGroupById(int id)
        {
            return _db.Groups.FirstOrDefault(g => g.Id == id);
        }
        public List<Contact> GetContactsByGroup(int groupId)
        {
            var groups = _dapper.Query<Contact>(@"SELECT c.* FROM Contacts c JOIN ContactGroup cg ON c.Id = cg.ContactsId WHERE cg.GroupsId = @groupId",new { groupId }).ToList();
           
           
            return groups.ToList();
        }
    }
}
