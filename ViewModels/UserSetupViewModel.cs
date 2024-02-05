﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HonorsApplication_II.ProgramClasses;
using HonorsApplication_II.Pages;
using TaskClass = HonorsApplication_II.ProgramClasses.Task;
using HonorsApplication_II.Data;
using System.Collections.ObjectModel;

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
                //Pending Implementation of a error messgie
            }
            else
            {

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
                        projectStartDate = DateTime.Now,
                        projectLastUsed = DateTime.Now,
                        userID = newuser.userID
                };

                ///
                //Project exampleProject2 = new()
                //{
                    //projectName = "Example Project 2",
                    //projectStartDate = DateTime.Now,
                    //projectLastUsed = DateTime.Now,
                    //userID = newuser.userID
                //};
                //
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

                    //Adding the Example Task to the Database
                    await dbContext.AddTaskAsync(exampleTask);

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

                var userProjects = await dbContext.GetProjectsByUserIdAsync(newuser.userID);

                ObservableCollection<Project> userProjects2 = new ObservableCollection<Project>(userProjects);

                //await Shell.Current.GoToAsync($"{nameof(ProjectsPage)}?name={newuser.username}", new Dictionary<string, object> { ["projects"] = userProjects });

                //await Shell.Current.GoToAsync(nameof(ProjectsPage), new Dictionary<string, object> { ["key"] = (newuser, userProjects) });

                await Shell.Current.GoToAsync(nameof(ProjectsPage), new Dictionary<string, object> { ["key"] = newuser, ["key2"] = userProjects2 });


            }

        }





    }
}
