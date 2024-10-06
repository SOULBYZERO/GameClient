using System.IO;
using System.Text.Json;
using System.Windows;
using UserManagement.Models;

namespace UserManagement
{
    public partial class MainWindow : Window
    {
        private List<User> users;
        public MainWindow()
        {
            InitializeComponent();
            LoadUsers();
            DisplayUserList();
        }

        private void LoadUsers()
        {
            if (File.Exists("Users.txt"))
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Users.txt");
                string json = File.ReadAllText(filePath);
                users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            else
            {
                users = new List<User>();
            }
        }

        private void DisplayUserList()
        {
            UserList.ItemsSource = null;
            UserList.ItemsSource = users;
        }

        private void SaveUsers()
        {
            string json = JsonSerializer.Serialize(users);
            File.WriteAllText("Users.txt", json);
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            var addEditUser = new AddEditUser();
            if (addEditUser.ShowDialog() == true)
            {
                users.Add(addEditUser.User);
                DisplayUserList();
                SaveUsers();
            }
        }

        private void DeleteUser(User user)
        {
            users.Remove(user);
            DisplayUserList();
            SaveUsers();
        }

        private void EditUser(User user)
        {
            var addEditUser = new AddEditUser(user);
            if (addEditUser.ShowDialog() == true)
            {
                var index = users.IndexOf(user);
                if (index >= 0)
                {
                    users[index] = addEditUser.User;
                }
                DisplayUserList();
                SaveUsers();
            }
        }

        private void DeleteUserMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (UserList.SelectedItem is User selectedUser)
            {
                DeleteUser(selectedUser);
            }
        }

        private void UserList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (UserList.SelectedItem is User selectedUser)
            {
                EditUser(selectedUser);
            }
        }
    }
}