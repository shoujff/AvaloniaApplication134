using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaApplication13.Data;
using AvaloniaApplication13.Models;
using AvaloniaApplication13.Repositories;
using AvaloniaApplication13.ViewModels;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AvaloniaApplication13
{
    public partial class MainWindow : Window
    {
        private readonly GroupRepository _gRepo = new GroupRepository("Server=(localdb)\\mssqllocaldb;Database=users;Trusted_Connection=True");
        public MainWindow()
        {
            InitializeComponent();
            
            DataContext = new MainViewModel();
        }
        public void AddContactClick(object sender, RoutedEventArgs eventArgs)
        {
            var family = new Group { Name = "Семья" };
            var work = new Group { Name = "Работа" };
            var contact = new Contact
            {
                Name = "masha",
                Surname = "Maskova",
                Phone = "dfsfs",
                UserId = 2,
                Groups = new List<Group> { family, work }
            };
            var contact2 = new Contact
            {
                Name = "ivan",
                Surname = "ivanov",
                Phone = "fdsfdsfsd",
                UserId = 3,
                Groups = new List<Group> { family }
            };
            var context = new DataBase();
            context.AddRange(contact, contact2);
            context.SaveChanges();

        }
        public void Load(object sender, RoutedEventArgs eventArgs)
        {
            MainViewModel vm = new MainViewModel();
            vm.LoadContacts();
               vm.FilterContactsByGroup();
            Debug.WriteLine($"Контакт ID");

        }

        public void Show_Data(object sender, RoutedEventArgs e)
        {
            var context = new DataBase();

            // Получаем контакты с группой ID = 1
            var contactsWithGroup1 = _gRepo.GetContactsByGroup(13);

            Debug.WriteLine("=== Контакты с группой ID=1 ===");
            Debug.WriteLine($"Найдено контактов: {contactsWithGroup1.Count}");
            Debug.WriteLine("");

            foreach (var contact in contactsWithGroup1)
            {
                Debug.WriteLine($"Контакт ID: {contact.Id}");
                Debug.WriteLine($"Имя: {contact.Name}");
                Debug.WriteLine($"Фамилия: {contact.Surname}");
                Debug.WriteLine($"Телефон: {contact.Phone}");
                Debug.WriteLine($"User ID: {contact.UserId}");
                Debug.WriteLine("---");
            }
        
    }
        public void Show_All_Groups(object sender, RoutedEventArgs e)
        {
            var context = new DataBase();
            var groups = context.Groups.ToList();

            Debug.WriteLine("\n========== ВСЕ ГРУППЫ ==========");
            Debug.WriteLine($"Всего групп: {groups.Count}");
            Debug.WriteLine("");

            foreach (var group in groups)
            {
                Debug.WriteLine($"ID: {group.Id}");
                Debug.WriteLine($"Название: {group.Name}");

                // Выводим количество контактов в группе
                var contactsCount = context.Contacts.Count(c => c.Groups.Any(g => g.Id == group.Id));
                Debug.WriteLine($"Количество контактов: {contactsCount}");
                Debug.WriteLine("---");
            }

            Debug.WriteLine("========== КОНЕЦ СПИСКА ==========\n");
        }
        private void AddGroupButton_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).IsGroupsVisible = true;
        }
    }
}