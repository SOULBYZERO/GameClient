using Common;
using System.Windows.Controls;

namespace GameClient.Templates
{
    public partial class AppCard : UserControl
    {
        public AppCard(IProjectMeta project)
        {
            InitializeComponent();
            DataContext = project;
            AppButton.Click += (sender, e) =>
            {
                var appInfoWindow = new AppInfoWindow(); 
                appInfoWindow.DataContext = project;   
                appInfoWindow.ShowDialog();             
            };
        }

    }
}
