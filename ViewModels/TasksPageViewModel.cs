
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

    [QueryProperty("CurrentProject", "project")]
    [QueryProperty("DoingTasks", "doingTasks")]
    [QueryProperty("TodoTasks", "todoTasks")]
    

    public partial class TasksViewModel : ObservableObject
    {

        private readonly DatabaseContext dbcontext;

        [ObservableProperty]
        Project currentProject;

        [ObservableProperty]
        ObservableRangeCollection<TaskClass> doingTasks;

        [ObservableProperty]
        ObservableRangeCollection<TaskClass> todoTasks;

        public ProjectFunctions functions;

        //========================================================================
        //Propertys & Methods used to reorder collection

        public TasksViewModel(DatabaseContext context)
        {
            dbcontext = context;

        }

        [RelayCommand]
        async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");//This is how you navigate backwords
        }

        [RelayCommand] //Allows the command to be seen by the page

        //Refresh page refreshes the RangeCollection 
        async Task Refresh()
        {

            TodoTasks = await functions.refreshTasks(CurrentProject.projectID, TodoTasks, "To-Do", false);

            DoingTasks = await functions.refreshTasks(CurrentProject.projectID, DoingTasks, "Doing", false);


        }
    }
}
