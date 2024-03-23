﻿using DevExpress.Data;
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

        public async Task SetUpExampleProject(int userID)
        {
            //Creating an Example Project as a sample

            Project exampleProject = new()
            {
                projectName = "Example Project",
                projectGoal = "Act as an example project for new users!",
                projectDescription = "This Project is to act as an example. In which this example project shows off the core functionality of this application.",
                projectStartDate = DateTime.Now,
                projectLastUsed = DateTime.Now,
                userID = userID
            };

            //adding the example project to the database
            await dbContext.AddProjectAsync(exampleProject);

            //creates new task object 
            TaskClass exampleTask = new TaskClass();

            int x = 0;
            Random rnd = new Random();

            // while x is not 5
            while (x != 5)
            {
                //edits the exampleTask object with data
                exampleTask.taskName = "Example Task " + x;
                exampleTask.taskStartDate = DateTime.Now;
                exampleTask.projectID = exampleProject.projectID;
                exampleTask.taskComplete = false;
                exampleTask.taskTimeDeadlineColour = "None";
                

                //Picks a number from 1 - 2
                int state = rnd.Next(1, 3);

                switch (state)
                {
                    //Assinging a value based off a random value
                    case 1: exampleTask.taskCatagory = "To-Do"; break;

                    case 2: exampleTask.taskCatagory = "Doing"; break;
                }

                //============
                //Task Deadline Management

                exampleTask.taskDaysLeft = "";
                int flip = rnd.Next(1, 3);
                int addDays = rnd.Next(1, 15);

                if (flip == 1) 
                {
                    exampleTask.taskDeadline = DateTime.Now.AddDays(addDays);
                    exampleTask.taskHasDeadline = true;
                }
                else
                {
                    exampleTask.taskHasDeadline = false;
                }


                //Adding the Example Task to the Database
                await dbContext.AddTaskAsync(exampleTask);

                await UpdateTaskColour(exampleTask);

                // x+1
                x++;
            }

            // creates a new task class to act as an example of a complete task
            TaskClass exampleCompleteTask = new TaskClass
            {
                taskName = "CompleteTask",
                taskStartDate = DateTime.Now,
                projectID = exampleProject.projectID,
                taskCatagory = "Done",
                taskComplete = true
        };

            await dbContext.AddTaskAsync(exampleCompleteTask);
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

            await UpdateProjectProgress(await dbContext.GetProjectAsync(projectID));

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

        public async Task UpdateProjectProgress(Project project)
        {
            double progress = 0;

            var allTasks = await dbContext.GetTasksByProjectIdAsync(project.projectID);

            var doneTasks = await GetDoneTasks(project.projectID);

            progress = (double)doneTasks.Count / (double)allTasks.Count;

            project.projectProgress = progress;

            await dbContext.UpdateProjectAsync(project);

        }

        public async Task UpdateTaskColour(TaskClass task)
        {



            if(task.taskHasDeadline == false) { return; }

            bool due = false;

            var dif = DateTime.Now - task.taskDeadline;

            int daysLeft = dif.Days * -1;

            if(daysLeft == 0) 
            {
                task.taskDaysLeft = "Due Today";
            }
            else if(daysLeft < 0) 
            {
                task.taskDaysLeft = "Overdue";
            }

            task.taskDaysLeft = "Days Left: " + daysLeft;

            if(daysLeft <= 3 || due == true)
            {
                task.taskTimeDeadlineColour = "Red";
            }
            else if(daysLeft > 3 && daysLeft <= 7) 
            {
                task.taskTimeDeadlineColour = "Yellow"; 
            } 
            else 
            {
                task.taskTimeDeadlineColour = "Green";        
            }

            await dbContext.UpdateTaskAsync(task);

        }

    }
}