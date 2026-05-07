using Avalonia.Media.TextFormatting.Unicode;
using AvaloniaApplication13.Data;
using AvaloniaApplication13.Models;
using AvaloniaApplication13.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication13.Repositories
{
    public class UserRepository : BaseRepository<User>
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
        public User Login(string username, string password)
        {
            var hash = PasswordHelper.Hash(password);
            return _db.Users.FirstOrDefault(u => u.Login == username && u.Password == hash);
        }
        public List<UserWithPhone> GetContacts(int id)
        {
            if (id <= 0)
            {
                return new List<UserWithPhone>();
            }
             return _db.Users.Join(_db.Contacts,
                 user => user.Id == id,
                 contact => contact.UserId == id,
                 (user, contact) => new UserWithPhone
                 {
                     Name = $"{contact.Name} {contact.Surname}",
                     Number = contact.Phone,
                 }).ToList();
           
        }

    }
}