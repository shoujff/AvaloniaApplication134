using Avalonia.Media.TextFormatting.Unicode;
using AvaloniaApplication13.Data;
using AvaloniaApplication13.Models;
using AvaloniaApplication13.Scripts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication13.Repositories
{
    public class UserRepository 
    {
        private readonly DataBase _db = new DataBase();

        public UserRepository()
        {
            _db = new DataBase();
        }
        public User Register(string firstname, string lastname, string username, string password)
        {
            var existingUser = _db.Users.FirstOrDefault(u => u.Login == username);
            if (existingUser != null)
            {
                throw new System.Exception("Пользователь с таким логином уже существует");

            }
            var user = new User { Name = firstname, Surname = lastname, Login = username, Password = PasswordHelper.Hash(password), IsLogin = false };
            _db.Users.Add(user);
            _db.SaveChanges();
            return user;
        }
        public async Task<User>Login(string username, string password)
        {
            var hash = PasswordHelper.Hash(password);
            return await _db.Users.FirstOrDefaultAsync(u => u.Login == username && u.Password == hash);
        }
        public async Task< List<UserWithPhone>> GetContacts(int id)
        {
            if (id <= 0)
            {
                return new List<UserWithPhone>();
            }
            
            return await _db.Contacts
         .Include(c => c.Groups) 
         .Where(c => c.UserId == id && !c.IsDeleted)
         .Select(contact => new UserWithPhone
         {
             Name = $"{contact.Name} {contact.Surname}",
             Number = contact.Phone,
             Groups = contact.Groups.ToList(), 
             ContactId = contact.Id,
             contact = contact
         }).ToListAsync();
        }
        public  List<UserWithPhone> GetContactsA(int id)
        {
            if (id <= 0)
            {
                return new List<UserWithPhone>();
            }
           
            return  _db.Contacts
         .Include(c => c.Groups)
         .Where(c => c.UserId == id && !c.IsDeleted)
         .Select(contact => new UserWithPhone
         {
             Name = $"{contact.Name} {contact.Surname}",
             Number = contact.Phone,
             Groups = contact.Groups.ToList(),
             ContactId = contact.Id,
             contact = contact
         }).ToList();
        }
        public List<UserWithPhone> GetTrashedContacts(int id)
        {
            if (id <= 0)
            {
                return new List<UserWithPhone>();
            }

            return _db.Contacts
           .Include(c => c.Groups)
         .Where(c => c.UserId == id && c.IsDeleted)
         .Select(contact => new UserWithPhone
         {
             Name = $"{contact.Name} {contact.Surname}",
             Number = contact.Phone,
             Groups = contact.Groups.ToList(),
             ContactId = contact.Id,
             contact = contact
         }).ToList();
        }

    }
}