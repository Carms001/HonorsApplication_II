using HonorsApplication.Pages;

namespace HonorsApplication
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ProjectsPage), typeof(ProjectsPage));
        }
    }
}