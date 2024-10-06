using Common;
using System.Windows;

namespace GameClient.Templates
{
    public partial class AppInfoWindow : Window
    {
        public AppInfoWindow()
        {
            InitializeComponent();
        }

        private void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            if (DataContext is IProjectMeta project)
            {
                project.Run();
            }
        }
    }
}
