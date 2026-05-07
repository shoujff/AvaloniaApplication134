using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaApplication13.Models;
using AvaloniaApplication13.Repositories;
using AvaloniaApplication13.ViewModels;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;

using AvaloniaApplication13.Data;

namespace AvaloniaApplication13
{
    public partial class MainWindow : Window
    {
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
    }
}