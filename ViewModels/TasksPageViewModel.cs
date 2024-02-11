using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Data.XtraReports.Native;
using HonorsApplication_II.Data;
using HonorsApplication_II.ProgramClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
using taskClass = HonorsApplication_II.ProgramClasses.Task;

namespace HonorsApplication_II.ViewModels
{

    [QueryProperty("CurrentProject", "project")]
    [QueryProperty("Tasks", "tasks")]

    public partial class TasksViewModel : ObservableObject
    {

        private readonly DatabaseContext dbcontext;

        [ObservableProperty]
        Project currentProject;

        [ObservableProperty]
        ObservableRangeCollection<taskClass> tasks;

        public TasksViewModel(DatabaseContext context)
        {
            dbcontext = context;
        }

        [RelayCommand]
        async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");//This is how you navigate backwords
        }

    }
}
