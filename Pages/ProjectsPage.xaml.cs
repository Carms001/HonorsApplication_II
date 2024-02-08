
using CommunityToolkit.Mvvm.ComponentModel;
using DevExpress.Data.XtraReports.Native;
using HonorsApplication_II.ProgramClasses;
using HonorsApplication_II.ViewModels;

namespace HonorsApplication_II.Pages
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

