using AvaloniaApplication13.Commands;
using AvaloniaApplication13.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AvaloniaApplication13.Models;
using AvaloniaApplication13.Data;
using AvaloniaApplication13.Scripts;
using System.Collections.ObjectModel;
using Microsoft.Identity.Client;

namespace AvaloniaApplication13.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged

    {
        private List<UserWithPhone> _contacts = new List<UserWithPhone>();

        private readonly UserRepository userRepository;
        private readonly ContactRepository _contactRepository;

        private UserWithPhone _selectedContact;

        private int _currentUserId;
        private string _firstName = "";
        private string _secondName = "";
        private string login = "";
        private string password = "";
        private string _status = "";
        private bool _statusVisible = false;
        private bool _isRegisterVisible = false;
        private bool _isLoginVisible = true;
        private bool _isCabinetVisible = false;

        private string _newContactName = "";
        private string _newPhoneNumber = "";
        private string _newContactSurname = "";
        public string NewContactSurname
        {
            get => _newContactSurname;
            set
            {
                _newContactSurname = value;
                OnPropertyChanged();
            }
        }


        public UserWithPhone SelectedContact
        {
            get => _selectedContact;
            set
            {
                _selectedContact = value;
                OnPropertyChanged();
                (DeleteContactCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }
        public string NewPhoneNumber
        {
            get => _newPhoneNumber;
            set
            {
                _newPhoneNumber = value;
                OnPropertyChanged();

            }
        }
        public string NewContactName
        {
            get => _newContactName;
            set
            {
                _newContactName = value;
                OnPropertyChanged();
            }
        }

        public List<UserWithPhone> Contacts
        {
            get => _contacts;
            set
            {
                _contacts = value;
                OnPropertyChanged(nameof(Contacts));
            }
        }
        public bool IsCabinetVisible
        {
            get => _isCabinetVisible;
            set
            {
                _isCabinetVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsRegisterVisible
        {
            get => _isRegisterVisible;
            set
            {
                _isRegisterVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoginVisible
        {
            get => _isLoginVisible;
            set
            {
                _isLoginVisible = value;
                OnPropertyChanged();
            }
        }

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value; OnPropertyChanged();
            }
        }


        public bool StatusVisible
        {
            get
            {
                return _statusVisible;
            }
            set
            {
                _statusVisible = value;
                OnPropertyChanged();
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FullName));
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }
        public string SecondName
        {
            get { return _secondName; }
            set
            {
                _secondName = value;
                OnPropertyChanged(nameof(FullName));
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }
        public string FullName
        {
            get => $"{FirstName} {SecondName}";

        }
        public string Login
        {
            get => login;
            set
            {
                login = value;
                OnPropertyChanged();
                LoginCommand.RaiseCanExecuteChanged();
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged();
                LoginCommand.RaiseCanExecuteChanged();
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        public string Greeting => string.IsNullOrWhiteSpace(FullName) ? "Введите имя чтобы увидеть приветствие" : $"Привет {FullName}";
        public string FullContactName => $"{NewContactName} {NewContactSurname}";
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public bool CanLogin => !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password);
        public bool CanRegister()
        {
            return !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(SecondName) && !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password);
        }
        public RelayCommand LoginCommand { get; }
        public RelayCommand RegisterCommand { get; }
        public RelayCommand ShowRegisterCommand { get; }
        public RelayCommand BackToLoginCommand { get; }
        public RelayCommand LogoutCommand { get; }
        public RelayCommand AddContactComand { get; }
        public MainViewModel()
        {
            _contactRepository = new ContactRepository();
            userRepository = new UserRepository();

            LoginCommand = new RelayCommand(OnLogin, () => CanLogin);
            RegisterCommand = new RelayCommand(OnRegister, () => CanRegister());
            ShowRegisterCommand = new RelayCommand(OnShowRegister);
            BackToLoginCommand = new RelayCommand(OnBackToLogin);
            LogoutCommand = new RelayCommand(OnLogout);
            DeleteContactCommand = new RelayCommand(OnDeleteContact, () => SelectedContact != null);
            AddContactComand = new RelayCommand(OnAddContact);
        }
        public void OnLogin()
        {
            var user = userRepository.Login(Login, Password);
            if (user != null)
            {
                _currentUserId = user.Id;
                var existingContacts = userRepository.GetContacts(_currentUserId);

                Contacts = userRepository.GetContacts(_currentUserId);
                IsLoginVisible = false;
                IsRegisterVisible = false;
                IsCabinetVisible = true;

            }
            else
            {
                Status = "Неверный логин или пароль";
                StatusVisible = true;
            }

        }
        public void OnRegister()
        {
            var newUser = new User
            {
                Name = FirstName,
                Surname = SecondName,
                Login = Login,
                Password = Password,
                IsLogin = false
            };
            userRepository.Register(FirstName, SecondName, Login, Password);
            OnBackToLogin();

            FirstName = "";
            SecondName = "";
            Login = "";
            Password = "";
            Status = "создан";
            StatusVisible = true;
        }
        public void OnAddContact()
        {
            var contact = new Contact
            {
                Name = NewContactName,   
                Surname = NewContactSurname,
                Phone = NewPhoneNumber,     
                UserId = _currentUserId
            };
            _contactRepository.AddContact(contact);
            Contacts = userRepository.GetContacts(_currentUserId);
            NewContactName = "";
            NewPhoneNumber = "";

            Status = "Контакт успешно добавлен";
            StatusVisible = true;
        }

        public void OnShowRegister()
        {
            IsLoginVisible = false;
            IsRegisterVisible = true;
            IsCabinetVisible = false;
            Status = "";


            FirstName = "";
            SecondName = "";
            Login = "";
            Password = "";
        }
        private void OnBackToLogin()
        {
            IsLoginVisible = true;
            IsRegisterVisible = false;
            IsCabinetVisible = false;
            Status = "";

            FirstName = "";
            SecondName = "";
            Login = "";
            Password = "";
        }
        public void OnLogout()
        {
            _currentUserId = 0;
            Contacts = new List<UserWithPhone>();
            IsLoginVisible = true;
            IsRegisterVisible = false;
            IsCabinetVisible = false;

        }
        private void AddTestContacts(int userid)
        {

            List<Contact> testContacts = new List<Contact>
            {
        new Contact { UserId = userid, Phone = "+767" },
        new Contact { UserId = userid, Phone = "+78" },
        new Contact { UserId = userid, Phone = "+89" }
    };
            foreach (var contact in testContacts)
            {
                _contactRepository.AddContact(contact);
            }
            OnPropertyChanged(nameof(Contacts));
        }
        public RelayCommand DeleteContactCommand { get; }
        private void OnDeleteContact()
        {
            if (SelectedContact != null)
            {
                _contactRepository.DleteContactByPhone(SelectedContact.Number, _currentUserId);
            }

        }
    }
}