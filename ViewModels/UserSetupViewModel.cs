using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HonorsApplication_II.ProgramClasses;
using HonorsApplication_II.Pages;
using TaskClass = HonorsApplication_II.ProgramClasses.Task;
using HonorsApplication_II.Data;
using System.Collections.ObjectModel;
using DevExpress.Data.XtraReports.Native;

namespace HonorsApplication_II.ViewModels
{
    public partial class UserSetupViewModel : ObservableObject
    {

        private readonly DataBaseService dbContext;

        public UserSetupViewModel(DataBaseService context)
        {
            dbContext = context;
        }

        [ObservableProperty]
        string name;


        [RelayCommand]
        async System.Threading.Tasks.Task Create()
        {
            if (Name == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please try again!", "OK");
            }
            else
            {

                await dbContext.ResetDB();

                //Creating the new user and assiging them some data
                LocalUser newuser = new()
                {
                    username = Name,
                    signUpDate = DateTime.Now,
                    lastActive = DateTime.Now
                };

                //Adding the new user to the database
                await dbContext.AddUserAsync(newuser);

                //Creating an Example Project as a sample

                Project exampleProject = new()
                {
                    projectName = "Example Project",
                    projectGoal = "Act as an example project for new users!",
                    projectDescription = "This Project is to act as an example. In which this example project shows off the core functionality of this application.",
                    projectStartDate = DateTime.Now,
                    projectLastUsed = DateTime.Now,
                    projectTaskCount =  0,
                        userID = newuser.userID
                };


                await dbContext.AddProjectAsync(exampleProject);
               // await dbContext.AddProjectAsync(exampleProject2);

                TaskClass exampleTask = new TaskClass();

                int x = 0;

                while (x != 5)
                {
                    exampleTask.taskName = "Example Task " + x;
                    exampleTask.taskStartDate = DateTime.Now;
                    exampleTask.projectID = exampleProject.projectID;
                    exampleTask.taskComplete = false;

                    Project updateProject = new() { projectTaskCount = +1 };

                    //Adding the Example Task to the Database
                    await dbContext.AddTaskAsync(exampleTask);
                    await dbContext.UpdateProjectAsync(updateProject);

                    x++;
                }

                    //Creating an example Task that is complete

                    //to be properly Implemented Later

                TaskClass exampleCompleteTask = new TaskClass
                {
                    taskName = "DummyTask",
                    taskStartDate = DateTime.Now,
                    projectID = exampleProject.projectID,
                    taskComplete = true
                };

                var getUserProjects = await dbContext.GetProjectsByUserIdAsync(newuser.userID);

                

                foreach (var userProject in getUserProjects)
                {

                    int count = 0;

                    var allTasks = await dbContext.GetTasksByProjectIdAsync(userProject.projectID);

                    count = allTasks.Count();

                    Project update = new() { projectTaskCount= count };

                    await dbContext.UpdateProjectAsync(update);
                }


                ObservableRangeCollection<Project> userProjects = new();

                userProjects.AddRange(getUserProjects);

                await Shell.Current.GoToAsync(nameof(ProjectsPage), new Dictionary<string, object> { ["key"] = newuser, ["key2"] = userProjects });


            }

        }





    }
}
