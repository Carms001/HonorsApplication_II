
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

        [ObservableProperty]
        bool isRefreshing = false;

        public ProjectsPageViewModel(DatabaseContext context, ProjectFunctions functionsContext)
        {
            dbContext = context;
            functions = functionsContext;

            Task task = Startup();

        }

        async Task Startup()
        {
            bool noProject = true;
            bool seenExample = true;

            bool needReset = false;

            try
            {

                ObservableRangeCollection<Project> list = new ObservableRangeCollection<Project>();

                var list2 = dbContext.GetAllProjectsAsync();

                list.AddRange(await list2);

                Projects = list;

                foreach (var project in Projects) 
                { 
                    if(project.projectName.Equals("Example Project")) { seenExample = true; }
                    if (!project.projectIsComplete) { noProject = false; }
                
                }


            }catch(Exception ex) { await App.Current.MainPage.DisplayAlert("Error", ex.ToString(), "OK"); }

            if(Projects.Count == 0 || needReset)
            {
                await dbContext.ResetDB();

                await functions.SetUpExampleProject();

                seenExample = true;
            }

            if(noProject && !seenExample) { await functions.SetUpExampleProject(); seenExample = true; }


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

            IsRefreshing = true;


            //removes all the projects from the Projects collection
            Projects.Clear();

            var listOfProjects = await dbContext.GetAllProjectsAsync();

            foreach (var project in listOfProjects) { await functions.UpdateProjectProgress(project); }


            //adds all the projects to the Projects collection
            Projects.AddRange(listOfProjects);

            IsRefreshing = false;

        }

        //========================================================================================

        [RelayCommand]
        //The method to add a new project
        async Task AddProject()
        {
            //Displays a prompt to the user asking for the Project Name
            string name = await App.Current.MainPage.DisplayPromptAsync("New Project", "Whats the name of your new Project?\n\nTry keep is short and sweet.", "Next", "Cancel");
            
            //Checks if name is Null
            if (name != null && name != string.Empty)
            {
                //Ask the user for the project goal though a prompt
                string goal = await App.Current.MainPage.DisplayPromptAsync("Project Goal", "Without too much detail. What is the goal of the Project?", "Create", "Cancel");

                //checks if goal is null
                if (goal != null )
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
                Projects.Clear();

                var listOfProjects = await dbContext.GetAllProjectsAsync();

                //adds all the projects to the Projects collection
                Projects.AddRange(listOfProjects);
            }
            else
            {
                //If either value is null a error will aprear the user
                await App.Current.MainPage.DisplayAlert("Error", "Please try again!", "OK");
            }

        }

        [RelayCommand]

        async Task ProjectDetails(Project currentProject)
        {

            ObservableRangeCollection<TaskClass> doingTasks = new();

            ObservableRangeCollection<TaskClass> todoTasks = new();

            ObservableRangeCollection<TaskClass> doneTasks = new();

            doingTasks.AddRange(await functions.GetDoingTasks(currentProject.projectID));

            todoTasks.AddRange(await functions.GetToDoTasks(currentProject.projectID));

            doneTasks.AddRange(await functions.GetDoneTasks(currentProject.projectID));

            //Sets the currentProject to project that was clicked on
            //Project currentProject = await dbContext.GetProjectAsync(projectID); 

            //Sends user the the TasksPage with the Project Object and a collection of non-Complete Tasks
            await Shell.Current.GoToAsync(nameof(Tasks), new Dictionary<string, object> { ["project"] = currentProject, ["doingTasks"] = doingTasks, ["todoTasks"] = todoTasks, ["doneTasks"] = doneTasks });

        }


    }
}
