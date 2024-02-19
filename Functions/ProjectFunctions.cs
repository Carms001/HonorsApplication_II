using DevExpress.Data.XtraReports.Native;
using HonorsApplication_II.Data;
using HonorsApplication_II.ProgramClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Task = System.Threading.Tasks.Task;
using TaskClass = HonorsApplication_II.ProgramClasses.Task;


namespace HonorsApplication_II.Functions
{
    public partial class ProjectFunctions
    {

        DatabaseContext dbContext;

        public ProjectFunctions(DatabaseContext context)
        {
            dbContext = context;
        }
         
        public async Task<List<TaskClass>> GetToDoTasks(int projectID)
        {
            var tasks = new List<TaskClass>();

            var allTasks = await dbContext.GetTasksByProjectIdAsync(projectID);

            foreach (var task in allTasks)
            {
                if (!task.taskComplete)
                {
                    if (task.taskCatagory.Equals("To-Do"))
                    {
                        tasks.Add(task);
                    }

                }

            }

            return tasks;
        }

        public async Task<List<TaskClass>> GetDoingTasks(int projectID)
        {
            var tasks = new List<TaskClass>();

            var allTasks = await dbContext.GetTasksByProjectIdAsync(projectID);

            foreach (var task in allTasks) 
            {
                if (!task.taskComplete)
                {
                    if (task.taskCatagory.Equals("Doing"))
                    {
                        tasks.Add(task);
                    } 

                }

            }

            

            return tasks;
        }

        public async Task<List<TaskClass>> GetDoneTasks(int projectID)
        {
            var tasks = new List<TaskClass>();

            var allTasks = await dbContext.GetTasksByProjectIdAsync(projectID);

            foreach (var task in allTasks) { if (task.taskComplete == true) { tasks.Add(task); } }

            return tasks;
        }

        public async Task<ObservableRangeCollection<TaskClass>> RefreshTasks (int projectID, ObservableRangeCollection<TaskClass> tasks, string catagory, bool complete)
        {

            tasks.Clear();

            var getTasks = new List<TaskClass>();

            if (complete) { getTasks = await GetDoneTasks(projectID); } else
            {
                switch (catagory)
                {
                    case "To-DO": getTasks = await GetToDoTasks(projectID); break;

                    case "Doing": getTasks= await GetDoingTasks(projectID); break;
                }
            }

            tasks.AddRange(getTasks);

            return tasks;

        }

        public async Task ReOrderTasks(ObservableRangeCollection<TaskClass> doing, ObservableRangeCollection<TaskClass> todo, int projectID)
        {
            ObservableRangeCollection<TaskClass> done = new();

            var getDone = await GetDoneTasks(projectID);

            done.AddRange(getDone);

            List<TaskClass> allTasks = new List<TaskClass>();

            foreach (var task in doing) { allTasks.Add(task); }

            foreach (var task in todo) { allTasks.Add(task); }

            foreach (var task in done) { allTasks.Add(task); }

            await dbContext.DeleteAllTasksByProjectIDAsync(projectID);

            await dbContext.AddListOfTasksToProjectAsync(allTasks);

        }

        public async Task<TaskClass> ChangeState(TaskClass task, string state)
        {

            switch (state)
            {
                //Assinging a value based off a random value
                case "To-Do" : task.taskCatagory = "To-Do"; break;

                case "Doing" : task.taskCatagory = "Doing"; break;
            }

            await dbContext.UpdateTaskAsync(task);


            return task;

        }

    }
}
