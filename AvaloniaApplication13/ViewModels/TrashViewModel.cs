using AvaloniaApplication13.Commands;
using AvaloniaApplication13.Models;
using AvaloniaApplication13.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication13.ViewModels
{
    public class TrashViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        private readonly ContactRepository _contactRepository;
        private readonly UserRepository _userRepository; private int _currentUserId;
        private List<UserWithPhone> _deletedContacts;
        private UserWithPhone _selectedContact;
        public TrashViewModel(int userId)
        {
            _currentUserId = userId;
            _contactRepository = new ContactRepository();
            _userRepository = new UserRepository();
            RestoreContactCommand = new RelayCommand(OnRestoreContact, () => SelectedContact != null);
            PermanentDeleteCommand = new RelayCommand(OnPermanentDelete, () => SelectedContact != null);
            ClearAllTrashCommand = new RelayCommand(OnClearAllTrash);
            CloseCommand = new RelayCommand(OnClose);
            LoadDeletedContacts();

        }

        public List<UserWithPhone> DeletedContacts
        {
            get => _deletedContacts;
            set
            {
                _deletedContacts = value;
                OnPropertyChanged(nameof(DeletedContacts));
            }
        }
        public UserWithPhone SelectedContact
        {
            get => _selectedContact;
            set
            {
                _selectedContact = value;
                OnPropertyChanged(nameof(SelectedContact));
            }
        }
        private void LoadDeletedContacts()
        {
            DeletedContacts = _userRepository.GetTrashedContacts(_currentUserId);
        }
        public RelayCommand RestoreContactCommand { get; }
        public RelayCommand PermanentDeleteCommand { get; }
        public RelayCommand ClearAllTrashCommand { get; }
        public RelayCommand CloseCommand { get; }
        private void OnRestoreContact()
        {
            if (SelectedContact != null)
            {
                _contactRepository.RestoreContact(SelectedContact.ContactId);
                LoadDeletedContacts();
            }
        }
        private void OnPermanentDelete()
        {
            if (SelectedContact != null)
            {
                _contactRepository.PermanentDeleteContact(SelectedContact.ContactId);
                LoadDeletedContacts() ;
            }
        }
        private void OnClearAllTrash()
        {
            var count = _contactRepository.ClearTrash(_currentUserId);
            LoadDeletedContacts();
           
        }
        public event Action? CloseRequested;

        private void OnClose()
        {
            CloseRequested?.Invoke();
        }
    }
}
