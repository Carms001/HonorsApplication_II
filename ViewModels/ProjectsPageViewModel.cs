using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Data.XtraReports.Native;
using HonorsApplication_II.Data;
using HonorsApplication_II.Pages;
using HonorsApplication_II.ProgramClasses;
using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Task = System.Threading.Tasks.Task;

namespace HonorsApplication_II.ViewModels
{
    [QueryProperty("Thingy", "key")]


    public partial class ProjectsPageViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        [ObservableProperty]
        public (LocalUser user2, List<Project> projects2) thingy;

        [ObservableProperty]
        bool isBusy = false;

        [ObservableProperty]
        LocalUser user;

        [ObservableProperty]
        ObservableRangeCollection<Project> projects;

        private readonly DataBaseService dbContext;

        public ProjectsPageViewModel(DataBaseService context)
        {
            dbContext = context;

            projects = new ObservableRangeCollection<Project>();

        }


        [RelayCommand]
        async Task Refresh()
        {
            IsBusy = true;

            Projects.Clear();

            var pro = await dbContext.GetProjectsByUserIdAsync(User.userID);

            Projects.AddRange(pro);

            IsBusy = false;

        }


    }
}
