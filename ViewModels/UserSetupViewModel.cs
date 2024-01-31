using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HonorsApplication.ProgramClasses;
using HonorsApplication.Pages;
using TaskClass = HonorsApplication.ProgramClasses.Task;
using HonorsApplication.Data;

namespace HonorsApplication.ViewModels
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

        [ObservableProperty]

        private LocalUser user;

        [ObservableProperty]

        private Project dummyProject;


        [RelayCommand]
        async System.Threading.Tasks.Task Create()
        {
            if (Name == null)
            {
                //Pending Implementation of a error messgie
            }
            else
            {


                User.username = Name;
                User.signUpDate = DateTime.Now;
                User.lastActive = DateTime.Now;

                await dbContext.AddUserAsync<LocalUser>(User);

                


                //LocalUser newUser = new LocalUser(); //Creates a new LocalUser called newUser


                //Sets the Propertys of newUser
                //newUser.username = Name;
                //newUser.signUpDate = DateTime.Now;
                //newUser.lastActive = DateTime.Now;
                //newUser.currentProjects = new List<Project>();//Creates a new list of Projects for currectProjects

                //Project dummyProject = new Project();//Creates a new DummyProject as DummyData

                //Assinges the dummy data attributes 
                DummyProject.projectName = "DummyProject";
                DummyProject.projectStartDate = DateTime.Now;
                DummyProject.projectLastUsed = DateTime.Now;
                DummyProject.completedTasksCount = 4;

                //Creates and Assigned Values to a DummyTask

                DummyProject.assginedTasks = new List<TaskClass>();

                TaskClass dummyTask = new TaskClass();

                int x = 0;

                while (x != 5)
                {
                    dummyTask.taskName = "DummyTask" + x;
                    dummyTask.taskStartDate = DateTime.Now;
                    DummyProject.assginedTasks.Add(dummyTask);
                    x++;
                }

                TaskClass dummyCompleteTask = new TaskClass();

                dummyCompleteTask.taskName = "DummyTask" + x;
                dummyCompleteTask.taskStartDate = DateTime.Now;

                DummyProject.completedTasks = new List<TaskClass>();

                DummyProject.completedTasks.Add(dummyCompleteTask);

                //adds the dummyProject to the currentProjects
                //newUser.currentProjects.Add(dummyProject);

                string outString = User.userID.ToString();

                await Shell.Current.GoToAsync($"{nameof(ProjectsPage)}?userID={outString}");
                //await Shell.Current.GoToAsync(nameof(ProjectsPage), new Dictionary<string, object> { ["newUser"] = newUser });//Passing though newUser Object usign the newUser Key
                //await Shell.Current.GoToAsync(nameof(ProjectsPage));

            }

        }





    }
}
