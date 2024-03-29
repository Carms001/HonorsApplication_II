﻿

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Data.Controls.ExpressionEditor;
using DevExpress.Data.XtraReports.Native;
using HonorsApplication_II.Data;
using HonorsApplication_II.Functions;
using HonorsApplication_II.Pages;
using HonorsApplication_II.ProgramClasses;
using Mopups.Services;
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

        [ObservableProperty]
        bool isRefreshing = false;
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
        async Task TaskPressed (TaskClass task)
        {
            await MopupService.Instance.PushAsync(new PopupPages.TaskDetailsPopup(task), true );
        }

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

                                if(DoingTasks.Count >= 3)
                                {
                                    bool confrim = await App.Current.MainPage.DisplayAlert("Too Many Doing Tasks", "You have " + DoingTasks.Count + " Doing tasks already! You can add another but try focus on the tasks you have already started!", "Add", "Cancel");

                                    if (confrim)
                                    {
                                        task.taskCatagory = "Doing";

                                        await dbcontext.UpdateTaskAsync(task);

                                        TodoTasks.Remove(task);
                                        DoingTasks.Add(task);
                                    }
                                    else { return; }
                                }
                                else 
                                {
                                    task.taskCatagory = "Doing";

                                    await dbcontext.UpdateTaskAsync(task);

                                    TodoTasks.Remove(task);
                                    DoingTasks.Add(task);
                                }
  
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
        async Task DoingInfo()
        {
            await functions.PopInfo("Doing");
        }

        [RelayCommand]
        async Task ToDoInfo()
        {
            await functions.PopInfo("To-Do");
        }

        [RelayCommand]
        async Task Return()
        {

            await functions.UpdateProjectProgress(CurrentProject);

            ObservableRangeCollection<Project> projects = new ObservableRangeCollection<Project>();

            projects.AddRange(await dbcontext.GetAllProjectsAsync());

            //await Shell.Current.GoToAsync(nameof(ProjectsPage), new Dictionary<string, object> { ["projects"] = projects });

            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        async Task TaskOptions(TaskClass task)
        {
            await Shell.Current.GoToAsync(nameof(EditTaskPage), new Dictionary<string, object> { ["task"] = task }); 

        }  

        [RelayCommand]
        async Task NewTask()
        {

            try
            {
                string name = await App.Current.MainPage.DisplayPromptAsync("New Task", "Enter the name of your new Task, Try keep is short and sweet", "Create", "Cancel");

                //Checks if name is Null
                if (name != null)
                {

                    TaskClass task = new TaskClass();

                    task.projectID = CurrentProject.projectID;
                    task.taskName = name;
                    task.taskComplete = false;
                    task.taskCatagory = "To-Do";
                    task.taskTimeDeadlineColour = "None";
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
        public async Task Refresh()
        {
            IsRefreshing = true;

            DoingTasks.Clear();
            TodoTasks.Clear();

            var getDoing = await functions.GetDoingTasks(CurrentProject.projectID);

            var getTodo = await functions.GetToDoTasks(CurrentProject.projectID);

            foreach (var task in getDoing) { await functions.UpdateTaskColour(task); }

            foreach (var item in getTodo)
            {
                await functions.UpdateTaskColour(item);
            }

            DoingTasks.AddRange(getDoing);
            TodoTasks.AddRange(getTodo);

            IsRefreshing = false;

        }
    }
}
