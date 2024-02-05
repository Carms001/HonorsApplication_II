
using CommunityToolkit.Mvvm.ComponentModel;
using DevExpress.Data.XtraReports.Native;
using HonorsApplication_II.ProgramClasses;
using HonorsApplication_II.ViewModels;

namespace HonorsApplication_II.Pages
{


    //[QueryProperty("data", "key")]

    public partial class ProjectsPage : ContentPage
    {
       
        //public (LocalUser user, ObservableRangeCollection<Project> projectsCollection) data;


        public ProjectsPage(ProjectsPageViewModel vm)
        {
            InitializeComponent();

            //vm.User = data.user;
            //vm.Projects = data.projectsCollection;

            BindingContext = vm;
        }

    }
}

