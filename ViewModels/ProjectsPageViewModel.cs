
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
    [QueryProperty("User", "key")]
    [QueryProperty("Projects", "key2")]

    //[QueryProperty("Thingy", "key")]


    public partial class ProjectsPageViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {

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

        }

        [RelayCommand]
        async Task Refresh()
        {
            IsBusy = true;

            Projects.Clear();

            var projects = await dbContext.GetProjectsByUserIdAsync(User.userID);

            Projects.AddRange(projects);

            IsBusy = false;

        }
        [RelayCommand]
        async Task AddProject()
        {
            string name = await App.Current.MainPage.DisplayPromptAsync("New Project", "Whats the name of your new Project?", "Next", "Cancel");

            if (name != null)
            {

                string goal = await App.Current.MainPage.DisplayPromptAsync("Project Goal", "Without too much detail. What is the goal of the Project?", "Create", "Cancel");

                if (goal != null)
                {
                    Project newProject = new Project()
                    {
                    projectName = name,
                    projectGoal = goal,
                    userID = User.userID,
                    projectIsComplete = false,
                    projectLastUsed = DateTime.Now,
                    projectStartDate = DateTime.Now,

                    };

                    await dbContext.AddProjectAsync(newProject);

                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Please try again!", "OK");
                }

                await Refresh();
            }

        }


    }
}
