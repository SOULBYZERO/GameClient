using System.Security.Policy;
using System.Windows;
using UserManagement.Models;

namespace UserManagement
{
    public partial class AddEditUser : Window
    {
        public User User { get; private set; }
        public AddEditUser()
        {
            InitializeComponent();
        }

        public AddEditUser(User user) : this()
        {
            User = new User
            {
                Username = user.Username,
                Description = user.Description,
                URL = user.URL
            };
            UsernameText.Text = user.Username;
            DescriptionText.Text = user.Description;
            URLText.Text = "pack://application:,,,/UserManagement;component/Assets/profile.png";
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            User = new User
            {
                Username = UsernameText.Text,
                Description = DescriptionText.Text,
                URL = URLText.Text
            };
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
