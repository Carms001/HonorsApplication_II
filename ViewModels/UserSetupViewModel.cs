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
using Task = System.Threading.Tasks.Task;

namespace HonorsApplication_II.ViewModels
{
    public partial class UserSetupViewModel : ObservableObject
    {

        private readonly DatabaseContext dbContext;

        private ProjectFunctions functions;

        public UserSetupViewModel(DatabaseContext context, ProjectFunctions functioncontext)
        {
            dbContext = context;
            functions = functioncontext;

        }

        [ObservableProperty]
        //Obserable property Name
        string name;


        [RelayCommand]
        //Task Method Create
        public async Task Create()
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

                await functions.SetUpExampleProject(newuser.userID);


                //gets all the projects assinged to the user 
                var getUserProjects = await dbContext.GetProjectsByUserIdAsync(newuser.userID);

                //creates a new RangeCollection called userProjects
                ObservableRangeCollection<Project> userProjects = new();

                //adds the list of projects assinged to the user the the Ranger Collection
                userProjects.AddRange(getUserProjects);

                foreach(var project in userProjects)
                {
                    await functions.UpdateProjectProgress(project);
                }

                //Navigates to the Projects page passing the Active user and a RangeCollection of Projects assinged to that user
                await Shell.Current.GoToAsync(nameof(ProjectsPage), new Dictionary<string, object> { ["user"] = newuser, ["projects"] = userProjects });


            }

        }





    }
}
