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

    [QueryProperty("Task", "task")]

    public partial class EditTaskViewModel : ObservableObject
    {
        [ObservableProperty]
        TaskClass task;

        DatabaseContext dbContext;

        ProjectFunctions functions;


        public EditTaskViewModel(DatabaseContext context, ProjectFunctions functionsContext)
        {
            dbContext = context;
            functions = functionsContext;

        }

        [ObservableProperty]
        DateTime minDate = DateTime.Now;

        [ObservableProperty]
        string name;

        [ObservableProperty]
        string goal;



        [RelayCommand]

        async Task Save()
        {
            bool confrim = await App.Current.MainPage.DisplayAlert("Save", "Are you sure you want to save changes?", "Save", "Cancel");

            if (confrim) 
            {

                if (Name != null && !Name.Equals(Task.taskName)){ Task.taskName = Name;}else if(Name == null) { Task.taskName = "Unnamed Task"; }

                if (Goal != null && !Goal.Equals(Task.taskGoal)) { Task.taskGoal = Goal; }

                if (Task.taskHasDeadline) { Task.taskHasDeadline = true; } else { Task.taskHasDeadline = false; Task.taskDeadline = DateTime.MinValue; Task.taskTimeDeadlineColour = "None"; Task.taskDaysLeft = null; }

                await dbContext.UpdateTaskAsync(Task);

                Project currentProject = await dbContext.GetProjectAsync(Task.projectID);
                ObservableRangeCollection<TaskClass> doingTasks = new ObservableRangeCollection<TaskClass>();
                ObservableRangeCollection<TaskClass> todoTasks = new ObservableRangeCollection<TaskClass>();

                await functions.UpdateTaskColour(Task);

                doingTasks.AddRange(await functions.GetDoingTasks(Task.projectID));
                todoTasks.AddRange(await functions.GetToDoTasks(Task.projectID));


                //await tasksPage.Refresh();

                await Shell.Current.GoToAsync("..", new Dictionary<string, object> { ["project"] = currentProject, ["doingTasks"] = doingTasks, ["todoTasks"] = todoTasks});
            }
        }

        [RelayCommand]

        async Task Return()
        {
            bool confrim = await App.Current.MainPage.DisplayAlert("Back", "Are you sure you want to go back? Your changes will NOT be saved!", "Ok", "Cancel");

            if (confrim)
            {

                await Shell.Current.GoToAsync(".."});

            }

        }

        [RelayCommand]
        async Task Delete()
        {
            bool confrim = await App.Current.MainPage.DisplayAlert("Back", "Are you sure you want to Delete this Task?", "Yes", "Cancel");


            if (confrim)
            {
                await dbContext.DeleteTaskAsync(Task.taskID);

                Project currentProject = await dbContext.GetProjectAsync(Task.projectID);
                ObservableRangeCollection<TaskClass> doingTasks = new ObservableRangeCollection<TaskClass>();
                ObservableRangeCollection<TaskClass> todoTasks = new ObservableRangeCollection<TaskClass>();

                await functions.UpdateTaskColour(Task);

                doingTasks.AddRange(await functions.GetDoingTasks(Task.projectID));
                todoTasks.AddRange(await functions.GetToDoTasks(Task.projectID));


                //await tasksPage.Refresh();

                await Shell.Current.GoToAsync("..", new Dictionary<string, object> { ["project"] = currentProject, ["doingTasks"] = doingTasks, ["todoTasks"] = todoTasks });
            }

        }

        [RelayCommand]
        async Task Complete()
        {
            bool confrim = await App.Current.MainPage.DisplayAlert("Back", "Are you sure you want to Complete this Task?", "Yes", "Cancel");


            if (confrim)
            {
                Task.taskComplete = true;

                await dbContext.UpdateTaskAsync(Task);

                Project currentProject = await dbContext.GetProjectAsync(Task.projectID);
                ObservableRangeCollection<TaskClass> doingTasks = new ObservableRangeCollection<TaskClass>();
                ObservableRangeCollection<TaskClass> todoTasks = new ObservableRangeCollection<TaskClass>();

                await functions.UpdateTaskColour(Task);

                doingTasks.AddRange(await functions.GetDoingTasks(Task.projectID));
                todoTasks.AddRange(await functions.GetToDoTasks(Task.projectID));


                //await tasksPage.Refresh();

                await Shell.Current.GoToAsync("..", new Dictionary<string, object> { ["project"] = currentProject, ["doingTasks"] = doingTasks, ["todoTasks"] = todoTasks });
            }

        }

        [RelayCommand]
        async Task TaskNameInfo()
        {
            await functions.PopInfo("TaskName");
        }

        [RelayCommand]
        async Task GoalInfo()
        {
            await functions.PopInfo("Goal");
        }

        [RelayCommand]
        async Task DeadlineInfo()
        {
            await functions.PopInfo("Deadline");
        }

    }

}
