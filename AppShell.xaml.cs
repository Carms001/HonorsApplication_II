using HonorsApplication_II.Pages;

namespace HonorsApplication_II
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ProjectsPage), typeof(ProjectsPage));
            Routing.RegisterRoute(nameof(Tasks), typeof(Tasks));
            Routing.RegisterRoute(nameof(EditTaskPage), typeof(EditTaskPage));

        }
    }
}