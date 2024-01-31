
using HonorsApplication.ViewModels;

namespace HonorsApplication.Pages
{


    public partial class ProjectsPage : ContentPage
    {
        public ProjectsPage(ProjectsPageViewModel vm)
        {
            InitializeComponent();

            BindingContext = vm;
        }

    }
}

