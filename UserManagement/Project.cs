using Common;
using System.Windows.Media.Imaging;

namespace UserManagement
{
    public class Project : IProjectMeta
    {
        public string Name { get; set; } = "User Management";
        public BitmapImage Image => new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}/Assets/UserManagement.jpg"));
        public string AppInfo { get; set; } = "User Management Application" +
            "\r\n\r\nFunctionality:" +
            "\r\n- User List: Displays a list of users with their names and email addresses. Double-click on a user to edit their details." +
            "\r\n- Add User Button: Click this button to add a new user. Fill in the details in the popup window and click \"\"Save\"\"." +
            "\r\n- Edit User: Double-click on a user in the list to edit their information. Update the details in the popup window and click \"\"Save\"\"." +
            "\r\n- Delete User: Right-click on a user in the list and select \"\"Delete\"\" to remove them from the user list." +
            "\r\n\r\nUsage:" +
            "\r\n1. Start the application." +
            "\r\n2. The main window displays a list of users. You can see each user's name and email address." +
            "\r\n3. To add a new user, click the \"\"Add User\"\" button. Fill in the user details and click \"\"Save\"\"." +
            "\r\n4. To edit a user's details, double-click on the user in the list. Modify the information and click \"\"Save\"\"." +
            "\r\n5. To delete a user, right-click on the user in the list and select \"\"Delete\"\".";
        public void Run()
        {
            MainWindow window = new MainWindow();
            window.ShowDialog();
        }
    }
}
