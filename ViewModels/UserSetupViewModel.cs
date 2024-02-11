using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HonorsApplication_II.ProgramClasses;
using HonorsApplication_II.Pages;
using TaskClass = HonorsApplication_II.ProgramClasses.Task;
using HonorsApplication_II.Data;
using System.Collections.ObjectModel;
using DevExpress.Data.XtraReports.Native;
using HonorsApplication_II.Functions;
using System;

namespace HonorsApplication_II.ViewModels
{
    public partial class UserSetupViewModel : ObservableObject
    {

        private readonly DatabaseContext dbContext;



        public UserSetupViewModel(DatabaseContext context)
        {
            dbContext = context;


        }

        [ObservableProperty]
        //Obserable property Name
        string name;


        [RelayCommand]
        //Task Method Create
        async System.Threading.Tasks.Task Create()
        {
            if (Name == null) //if name is null
            {
                //display error
                await App.Current.MainPage.DisplayAlert("Error", "Please try again!", "OK");
            }
            else
            {
                //resets the database - used for testing purposes
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

                //Using the userID and a Key, it sets up examples connected to the new User.
                //Creating an Example Project as a sample

                Project exampleProject = new()
                {
                    projectName = "Example Project",
                    projectGoal = "Act as an example project for new users!",
                    projectDescription = "This Project is to act as an example. In which this example project shows off the core functionality of this application.",
                    projectStartDate = DateTime.Now,
                    projectLastUsed = DateTime.Now,
                    projectTaskCount = 0,
                    userID = newuser.userID
                };

                //adding the example project to the database
                await dbContext.AddProjectAsync(exampleProject);

                //creates new task object 
                TaskClass exampleTask = new TaskClass();

                int x = 0;
                Random rnd = new Random();

                // while x is not 5
                while (x != 5)
                {
                    //edits the exampleTask object with data
                    exampleTask.taskName = "Example Task " + x;
                    exampleTask.taskStartDate = DateTime.Now;
                    exampleTask.projectID = exampleProject.projectID;
                    exampleTask.taskComplete = false;

                    //Picks a number from 1 - 2
                    int state = rnd.Next(1,3);

                    switch (state)
                    {
                        //Assinging a value based off a random value
                        case 1: exampleTask.taskState = "To-Do"; break;

                        case 2: exampleTask.taskState = "Doing"; break;
                    }


                    //Adding the Example Task to the Database
                    await dbContext.AddTaskAsync(exampleTask);

                    // x+1
                    x++;
                }

                // creates a new task class to act as an example of a complete task
                TaskClass exampleCompleteTask = new TaskClass
                {
                    taskName = "CompleteTask",
                    taskStartDate = DateTime.Now,
                    projectID = exampleProject.projectID,
                    taskComplete = true
                };

                await dbContext.AddTaskAsync(exampleCompleteTask);

                //gets all the projects assinged to the user 
                var getUserProjects = await dbContext.GetProjectsByUserIdAsync(newuser.userID);

                //creates a new RangeCollection called userProjects
                ObservableRangeCollection<Project> userProjects = new();

                //adds the list of projects assinged to the user the the Ranger Collection
                userProjects.AddRange(getUserProjects);

                //Navigates to the Projects page passing the Active user and a RangeCollection of Projects assinged to that user
                await Shell.Current.GoToAsync(nameof(ProjectsPage), new Dictionary<string, object> { ["user"] = newuser, ["projects"] = userProjects });


            }

        }





    }
}
