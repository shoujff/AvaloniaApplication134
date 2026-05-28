using Avalonia.Controls;
using AvaloniaApplication13.ViewModels;
using Avalonia.Interactivity;

namespace AvaloniaApplication13
{
    public partial class TrashWindow : Window
    {
        public TrashWindow(int userId)
        {
            InitializeComponent();
            var vm = new TrashViewModel(userId);
            vm.CloseRequested += () => Close();
            DataContext = vm;
        }
        public TrashWindow()
        {
            InitializeComponent();
        }

        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}