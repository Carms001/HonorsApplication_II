using DevExpress.Data.XtraReports.Native;
using HonorsApplication_II.Data;
using HonorsApplication_II.ProgramClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
         
        public async Task<List<TaskClass>> getToDoTasks(int projectID)
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

        public async Task<List<TaskClass>> getDoingTasks(int projectID)
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

        public async Task<List<TaskClass>> getDoneTasks(int projectID)
        {
            var tasks = new List<TaskClass>();

            var allTasks = await dbContext.GetTasksByProjectIdAsync(projectID);

            foreach (var task in allTasks) { if (task.taskComplete == true) { tasks.Add(task); } }

            return tasks;
        }

        public async Task<ObservableRangeCollection<TaskClass>> refreshTasks (int projectID, ObservableRangeCollection<TaskClass> tasks, string catagory, bool complete)
        {

            tasks.Clear();

            var getTasks = new List<TaskClass>();

            if (complete) { getTasks = await getDoneTasks(projectID); } else
            {
                switch (catagory)
                {
                    case "To-DO": getTasks = await getToDoTasks(projectID); break;

                    case "Doing": getTasks= await getDoingTasks(projectID); break;
                }
            }

            tasks.AddRange(getTasks);

            return tasks;

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
