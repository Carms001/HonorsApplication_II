using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HonorsApplication.Data;
using HonorsApplication.Pages;
using HonorsApplication.ProgramClasses;
using System;
using System.Collections.ObjectModel;
using Task = System.Threading.Tasks.Task;

namespace HonorsApplication.ViewModels
{


    [QueryProperty( "userID", "userID")]

    public partial class ProjectsPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string userID;

        private readonly DatabaseContext dbContext;

        public ProjectsPageViewModel(DatabaseContext context)
        {
            dbContext = context;

            Projects = new ObservableCollection<Project>();

            LoadProjects();
            
        }

        private ObservableCollection<Project> Projects;

        [RelayCommand]
        private async Task LoadProjects()
        {
            var projects = dbContext.GetAllAsync<Project>();
            if (projects is not null && projects.Any())
            {
                if(Projects is null)
                {
                    Projects = new ObservableCollection<Project>();
                }

                foreach (var project in projects)
                {
                    if(project.UserID == Convert.ToInt32(UserID))
                    {
                        Projects.Add(project);
                        
                    }
                }
            }
        }

    }
}
