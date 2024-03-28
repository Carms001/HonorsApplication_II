
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HonorsApplication_II.Data;
using HonorsApplication_II.Functions;
using HonorsApplication_II.Pages;
using HonorsApplication_II.ProgramClasses;
using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using TaskClass = HonorsApplication_II.ProgramClasses.Task;
using Task = System.Threading.Tasks.Task;
using DevExpress.Data.XtraReports.Native;

namespace HonorsApplication_II.ViewModels
{

    //Query propertys, allowing the transfer of selected data
    //[QueryProperty("User", "user")]
    [QueryProperty("Projects", "projects")]

    public partial class ProjectsPageViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {

        [ObservableProperty]
        ObservableRangeCollection<Project> projects; //RangeCollection of Projects

        private readonly DatabaseContext dbContext; //setting the DB context

        private ProjectFunctions functions;

        public ProjectsPageViewModel(DatabaseContext context, ProjectFunctions functionsContext)
        {
            dbContext = context;
            functions = functionsContext;

            if (Projects == null)
            {
                Projects = new ObservableRangeCollection<Project>();

                Task task = Startup();
            }
            
        }

        async Task Startup()
        {
            await dbContext.ResetDB();

            await functions.SetUpExampleProject();

            ObservableRangeCollection<Project> list = new ObservableRangeCollection<Project>();

            var list2 = dbContext.GetAllProjectsAsync();

            list.AddRange(await list2);

            Projects = list;

        }

        [RelayCommand]
        async Task ProjectOptions(Project project)
        {
            await Shell.Current.GoToAsync(nameof(EditProjectPage), new Dictionary<string, object> { ["project"] = project });

        }

        //========================================================================================

        [RelayCommand] //Allows the command to be seen by the page

        //Refresh page refreshes the RangeCollection 
        async Task Refresh()
        {
            
            //removes all the projects from the Projects collection
            Projects.Clear();

            //gets all projects related to the active user
            var projects = await dbContext.GetAllProjectsAsync();

            //adds all the projects to the Projects collection
            Projects.AddRange(projects);

        }

        //========================================================================================

        [RelayCommand]
        //The method to add a new project
        async Task AddProject()
        {
            //Displays a prompt to the user asking for the Project Name
            string name = await App.Current.MainPage.DisplayPromptAsync("New Project", "Whats the name of your new Project?", "Next", "Cancel");
            
            //Checks if name is Null
            if (name != null)
            {
                //Ask the user for the project goal though a prompt
                string goal = await App.Current.MainPage.DisplayPromptAsync("Project Goal", "Without too much detail. What is the goal of the Project?", "Create", "Cancel");

                //checks if goal is null
                if (goal != null)
                {
                    //creates a new project and assinges it basic data
                    Project newProject = new Project()
                    {
                        //adds the project name collected from user
                        projectName = name,
                        //adds the project goal collected from user
                        projectGoal = goal,
                        //assings the project to the active user
                        //Assings basic DateTime data and makes sure the the IsComplete veribal is false
                        projectIsComplete = false,
                        projectLastUsed = DateTime.Now,
                        projectStartDate = DateTime.Now,

                    };

                    //adds the new project to the database
                    await dbContext.AddProjectAsync(newProject);

                }
                else
                {
                    //If either value is null a error will aprear the user
                    await App.Current.MainPage.DisplayAlert("Error", "Please try again!", "OK");
                }

                //refreshes the Project Collection
                await Refresh();
            }

        }

        [RelayCommand]

        async Task ProjectDetails(Project currentProject)
        {

            ObservableRangeCollection<TaskClass> doingTasks = new();

            ObservableRangeCollection<TaskClass> todoTasks = new();

            doingTasks.AddRange(await functions.GetDoingTasks(currentProject.projectID));

            todoTasks.AddRange(await functions.GetToDoTasks(currentProject.projectID));

            //Sets the currentProject to project that was clicked on
            //Project currentProject = await dbContext.GetProjectAsync(projectID); 

            //Sends user the the TasksPage with the Project Object and a collection of non-Complete Tasks
            await Shell.Current.GoToAsync(nameof(Tasks), new Dictionary<string, object> { ["project"] = currentProject, ["doingTasks"] = doingTasks, ["todoTasks"] = todoTasks });

        }


    }
}
