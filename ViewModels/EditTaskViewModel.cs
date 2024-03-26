using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

            task = new TaskClass();

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
                Task.taskName = Name;
                Task.taskDescription = Description;
                Task.taskGoal = Goal;

                if (Task.taskHasDeadline) 
                { 
                    Task.taskHasDeadline = true;

                }
                else
                {
                    Task.taskHasDeadline = false;
                    
                }

                await dbContext.UpdateTaskAsync(Task);

                await Shell.Current.GoToAsync("..");
            }
        }
     }

}
