using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Data.XtraReports.Native;
using HonorsApplication_II.Data;
using HonorsApplication_II.Functions;
using HonorsApplication_II.ProgramClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
using TaskClass = HonorsApplication_II.ProgramClasses.Task;

namespace HonorsApplication_II.ViewModels
{

    [QueryProperty("Project", "project")]

    public partial class EditProjectViewModel : ObservableObject
    {
        [ObservableProperty]
        Project project;

        DatabaseContext dbContext;

        ProjectFunctions functions;


        public EditProjectViewModel(DatabaseContext context, ProjectFunctions functionsContext)
        {
            dbContext = context;
            functions = functionsContext;

        }

        [ObservableProperty]
        DateTime minDate = DateTime.Now;

        [ObservableProperty]
        string name;

        [ObservableProperty]
        string description;

        [ObservableProperty]
        string goal;



        [RelayCommand]

        async Task Save()
        {
            bool confrim = await App.Current.MainPage.DisplayAlert("Save", "Are you sure you want to save changes?", "Save", "Cancel");

            if (confrim) 
            {

                if (Name != null && !Name.Equals(Project.projectName)){ Project.projectName = Name;}

                if (Description != null && !Description.Equals(Project.projectDescription)) { Project.projectDescription = Description; }

                if (Goal != null && !Goal.Equals(Project.projectGoal)) { Project.projectGoal = Goal; }

                await dbContext.UpdateProjectAsync(Project);

                ObservableRangeCollection<Project> projects = new ObservableRangeCollection<Project>();

                projects.AddRange(await dbContext.GetAllProjectsAsync());

                await Shell.Current.GoToAsync("..", new Dictionary<string, object> { ["projects"] = projects});
            }
        }

        [RelayCommand]

        async Task Return()
        {
            bool confrim = await App.Current.MainPage.DisplayAlert("Back", "Are you sure you want to go back? Your changes will NOT be saved!", "Ok", "Cancel");


            if (confrim)
            {
                await Shell.Current.GoToAsync("..");
            }

        }

        [RelayCommand]
        async Task Delete()
        {
            bool confrim = await App.Current.MainPage.DisplayAlert("Back", "Are you sure you want to Delete this Project?", "Yes", "Cancel");


            if (confrim)
            {
                await dbContext.DeleteProjectAsync(Project.projectID);

                ObservableRangeCollection<Project> projects = new ObservableRangeCollection<Project>();

                projects.AddRange(await dbContext.GetAllProjectsAsync());

                await Shell.Current.GoToAsync("..", new Dictionary<string, object> { ["projects"] = projects });
            }

        }

        [RelayCommand]
        async Task Complete()
        {
            bool confrim = await App.Current.MainPage.DisplayAlert("Back", "Are you sure you want to Delete this Complete?", "Yes", "Cancel");


            if (confrim)
            {
                Project.projectIsComplete = true;
                Project.projectEndDate = DateTime.Now;

                await dbContext.UpdateProjectAsync(Project);

                ObservableRangeCollection<Project> projects = new ObservableRangeCollection<Project>();

                projects.AddRange(await dbContext.GetAllProjectsAsync());

                await Shell.Current.GoToAsync("..", new Dictionary<string, object> { ["projects"] = projects });
            }

        }
    }

}
