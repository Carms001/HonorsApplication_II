

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Data.Controls.ExpressionEditor;
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

        //Page Constructor
        public TasksViewModel(DatabaseContext context, ProjectFunctions functionsContext)
        {
            dbcontext = context;
            functions = functionsContext;
        }


        //========================================================================
        //Propertys & Methods used to reorder collection

        private TaskClass taskDragged;

        [ObservableProperty]
        private string catagory;

        [RelayCommand]
        public void TaskDragged(TaskClass task)
        {

            taskDragged = task;

        }

        [RelayCommand]
        async Task TaskDraggedOver(TaskClass task)
        {
            try
            {
                var taskToMove = taskDragged;
                var taskToInsertBefore = task;
                string catagory = task.taskCatagory;

                if (taskToMove == taskToInsertBefore || taskToMove == null || taskToInsertBefore == null || taskToMove.taskCatagory != taskToInsertBefore.taskCatagory) { return; }

                int insertAtIndex;

                switch (catagory)
                {
                    case "Doing":
                        insertAtIndex = DoingTasks.IndexOf(taskToInsertBefore);

                        if (insertAtIndex >= 0 && insertAtIndex < DoingTasks.Count)
                        {
                            DoingTasks.Remove(taskToMove);
                            DoingTasks.Insert(insertAtIndex, taskToMove);

                        }

                        break;

                    case "To-Do":
                        insertAtIndex = TodoTasks.IndexOf(taskToInsertBefore);

                        if (insertAtIndex >= 0 && insertAtIndex < DoingTasks.Count)
                        {
                            TodoTasks.Remove(taskToMove);
                            TodoTasks.Insert(insertAtIndex, taskToMove);

                        }

                        break;
                }

                await functions.ReOrderTasks(DoingTasks, TodoTasks, CurrentProject.projectID);



            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }

        }


        [RelayCommand]
        async Task TaskDraggedOverCatagory(string catagory)
        {
            try
            {

                var task = taskDragged;

                Catagory = catagory;


                if (Catagory != null)
                {

                    if ((Catagory.Equals("Doing") && task.taskCatagory.Equals("Doing")) || (Catagory.Equals("To-Do") && task.taskCatagory.Equals("To-Do")))
                    {

                        await App.Current.MainPage.DisplayAlert("Error", "Something went wrong", "OK");

                    }
                    else
                    {

                        switch (Catagory)
                        {
                            case "Doing":

                                task.taskCatagory = "Doing";

                                await dbcontext.UpdateTaskAsync(task);

                                TodoTasks.Remove(task);
                                DoingTasks.Add(task);


                                break;

                            case "To-Do":

                                task.taskCatagory = "To-Do";

                                await dbcontext.UpdateTaskAsync(task);

                                DoingTasks.Remove(task);
                                TodoTasks.Add(task);

                                break;
                        }

                        await functions.ReOrderTasks(DoingTasks, TodoTasks, CurrentProject.projectID);

                    }

                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Catagory = Null", "OK");
                }

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }



        [RelayCommand]
        async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");//This is how you navigate backwords
        }

        [RelayCommand]
        async Task TaskOptions(TaskClass task)
        {

            string action = await App.Current.MainPage.DisplayActionSheet("Task Options", "Cancel", null, "Edit", "Delete", "Complete");


            if (action == null) { return; }


            switch (action)
            {
                case "Edit": break;


                //=========================================================

                case "Delete":

                    switch (task.taskCatagory)
                    {

                        case "Doing": DoingTasks.Remove(task); break;

                        case "To-Do": TodoTasks.Remove(task); break;


                    }

                    await dbcontext.DeleteTaskAsync(task.taskID);

                    await functions.UpdateProjectProgress(CurrentProject); 

                    break;

                //=========================================================

                case "Complete":

                    switch (task.taskCatagory)
                    {

                        case "Doing": DoingTasks.Remove(task); break;

                        case "To-Do": TodoTasks.Remove(task); break;

                    }

                    task.taskCatagory = "Done";
                    task.taskComplete = true;

                    await functions.UpdateProjectProgress(CurrentProject);

                    break;

                case "Cancel": return; 

                default: await App.Current.MainPage.DisplayAlert("Error", "Something went wrong!", "OK");  break;
            }

        }  

        [RelayCommand]
        async Task NewTask()
        {

            try
            {
                string name = await App.Current.MainPage.DisplayPromptAsync("New Task", "Enter the name of your new Task", "Create", "Cancel");

                //Checks if name is Null
                if (name != null)
                {

                    TaskClass task = new TaskClass();

                    task.projectID = CurrentProject.projectID;
                    task.taskName = name;
                    task.taskComplete = false;
                    task.taskCatagory = "To-Do";

                    await dbcontext.AddTaskAsync(task);

                    TodoTasks.Add(task);

                    //await Refresh();

                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Something went wrong!", "OK");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }

        }



        [RelayCommand] //Allows the command to be seen by the page
        //Refresh page refreshes the RangeCollection 
        async Task Refresh()
        {

            DoingTasks.Clear();
            TodoTasks.Clear();

            var getDoing = await functions.GetDoingTasks(CurrentProject.projectID);

            var getTodo = await functions.GetToDoTasks(CurrentProject.projectID);

            DoingTasks.AddRange(getDoing);
            TodoTasks.AddRange(getTodo);


        }
    }
}
