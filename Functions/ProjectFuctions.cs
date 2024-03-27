using DevExpress.Data;
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

        //Settings up the database context to allow for its use.
        public ProjectFunctions(DatabaseContext context)
        {
            dbContext = context;
        }

        //This method setups an example project for first time users
        public async Task SetUpExampleProject()
        {
            //Creating an Example Project as a sample

            Project exampleProject = new()
            {
                projectName = "Example Project",
                projectGoal = "Act as an example project for new users!",
                projectDescription = "This Project is to act as an example. In which this example project shows off the core functionality of this application.",
                projectStartDate = DateTime.Now,
                projectLastUsed = DateTime.Now,
                //userID = userID
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

            await UpdateProjectProgress(exampleProject);

        } 

        //========================================================================================
        //Gets the tasks that are labled as To-Do
        public async Task<List<TaskClass>> GetToDoTasks(int projectID)
        {
            //creates a list of type task
            var tasks = new List<TaskClass>();

            //gets all the tasks associated with that project ID
            var allTasks = await dbContext.GetTasksByProjectIdAsync(projectID);

            //Loops though each task in the list of all tasks
            foreach (var task in allTasks)
            {
                //if the task doesnt have taskComplete as true
                if (!task.taskComplete)
                {
                    //If the task catagory = To-Do
                    if (task.taskCatagory.Equals("To-Do"))
                    {
                        //adds the task to the list of tasks
                        tasks.Add(task);
                    }

                }

            }
            //Returns the list of tasks with only those with the catagory of "To-Do"
            return tasks;
        }

        //========================================================================================
        //Gets the tasks that are labled as To-Do
        public async Task<List<TaskClass>> GetDoingTasks(int projectID)
        {
            //creates a list of type task
            var tasks = new List<TaskClass>();

            //gets all the tasks associated with that project ID
            var allTasks = await dbContext.GetTasksByProjectIdAsync(projectID);

            //Loops though each task in the list of all tasks
            foreach (var task in allTasks) 
            {
                //if the task doesnt have taskComplete as true
                if (!task.taskComplete)
                {
                    //If the task catagory = Doing
                    if (task.taskCatagory.Equals("Doing"))
                    {
                        //adds the task to the list of tasks
                        tasks.Add(task);
                    } 

                }

            }

            //Returns the list of tasks with only those with the catagory of "Doing"
            return tasks;
        }

        //========================================================================================
        //Gets the tasks that are labled as To-Do
        public async Task<List<TaskClass>> GetDoneTasks(int projectID)
        {
            //creates a list of type task
            var tasks = new List<TaskClass>();

            //gets all the tasks associated with that project ID
            var allTasks = await dbContext.GetTasksByProjectIdAsync(projectID);

            //Loops though each task in the list of all tasks if the task has taskComplete = true, it adds it to the list
            foreach (var task in allTasks) { if (task.taskComplete == true) { tasks.Add(task); } }

            //Returns a list of tasks that are marked as complete
            return tasks;
        }

        //========================================================================================
        //Method that reOrdersTasks
        public async Task ReOrderTasks(ObservableRangeCollection<TaskClass> doing, ObservableRangeCollection<TaskClass> todo, int projectID)
        {
            //Creates a Range of type TaskClass
            ObservableRangeCollection<TaskClass> done = new();

            //gets a list of of done Tasks
            var getDone = await GetDoneTasks(projectID);

            //Adds the list of done tasks to the range
            done.AddRange(getDone);

            //creates a new List of type TaskClass
            List<TaskClass> allTasks = new List<TaskClass>();


            //adds all the range of tasks into one big list of tasks in order
            foreach (var task in doing) { allTasks.Add(task); }

            foreach (var task in todo) { allTasks.Add(task); }

            foreach (var task in done) { allTasks.Add(task); }

            //Removes all the tasks related to the project
            await dbContext.DeleteAllTasksByProjectIDAsync(projectID);
            //adds the task back into the project 
            await dbContext.AddListOfTasksToProjectAsync(allTasks);

            //By removing then adding the task back within the database they are now in the order that the user wants

        }

        //========================================================================================
        //Method that Changes the state/Catafory of the Task
        public async Task<TaskClass> ChangeState(TaskClass task, string state)
        {
            //Sets the task catagory based of the parameter states value
            switch (state)
            {
                
                case "To-Do" : task.taskCatagory = "To-Do"; break;

                case "Doing" : task.taskCatagory = "Doing"; break;
            }

            //updates the task in the database
            await dbContext.UpdateTaskAsync(task);

            //returns the current task
            return task;

        }

        //========================================================================================
        //Method that Updates the Projects Progress 
        public async Task UpdateProjectProgress(Project project)
        {
           
            double progress = 0;

            //gets all the tasks associated with the project
            var allTasks = await dbContext.GetTasksByProjectIdAsync(project.projectID);

            //gets all the done tasks 
            var doneTasks = await GetDoneTasks(project.projectID);

            //devides the amount of done tasks by the amount of all tasks to find the percentage that are done
            progress = (double)doneTasks.Count / (double)allTasks.Count;

            //sets the project progress to the value calcualted
            project.projectProgress = progress;

            //updates the project in the database with the new progress value
            await dbContext.UpdateProjectAsync(project);

        }

        //========================================================================================
        //Method that Updates the background of the task based of teh deadline
        public async Task UpdateTaskColour(TaskClass task)
        {
            //if the task has no deadline 
            if(task.taskHasDeadline == false) { return; }

            bool due = false;

            //finds the diffrence in DateTime bettween now and the deadline
            var dif = DateTime.Now - task.taskDeadline;

            //Calculates the amount of whole days left
            int daysLeft = dif.Days * -1;

            //assinged diffrent values to taskDaysLeft depending the value of daysLeft
            if(daysLeft == 0) 
            {
                task.taskDaysLeft = "Due Today";
            }
            else if(daysLeft < 0) 
            {
                task.taskDaysLeft = "Overdue";
            }

            //Assinges a value to the daysleft string
            task.taskDaysLeft = "Days Left: " + daysLeft;

            //if the amounr of days are above or below a certain about of time a diffrent colour will be set
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

            //updates the database
            await dbContext.UpdateTaskAsync(task);

        }

    }
}
