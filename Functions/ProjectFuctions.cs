using DevExpress.Data;
using DevExpress.Data.XtraReports.Native;
using HonorsApplication_II.Data;
using HonorsApplication_II.ProgramClasses;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            var tasks = new List<TaskClass>();


            var allTasks = await dbContext.GetTasksByProjectIdAsync(projectID);


            foreach (var task in allTasks)
            {

                if (!task.taskComplete){

                    if (task.taskCatagory.Equals("To-Do"))
                    {

                        tasks.Add(task);
                    }

                }

            }

            return tasks;
        }

        //========================================================================================
        //Gets the tasks that are labled as To-Do
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

        //========================================================================================
        //Gets the tasks that are labled as To-Do
        public async Task<List<TaskClass>> GetDoneTasks(int projectID)
        {

            var tasks = new List<TaskClass>();


            var allTasks = await dbContext.GetTasksByProjectIdAsync(projectID);


            foreach (var task in allTasks) { if (task.taskComplete == true) { tasks.Add(task); } }


            return tasks;
        }

        //========================================================================================
        //Method that reOrdersTasks
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

        //========================================================================================
        //Method that Changes the state/Catafory of the Task
        public async Task<TaskClass> ChangeState(TaskClass task, string state)
        {

            switch (state)
            {
                
                case "To-Do" : task.taskCatagory = "To-Do"; break;

                case "Doing" : task.taskCatagory = "Doing"; break;
            }


            await dbContext.UpdateTaskAsync(task);

            return task;

        }

        //========================================================================================
        //Method that Updates the Projects Progress 
        public async Task UpdateProjectProgress(Project project)
        {
           
            double progress = 0;


            var allTasks = await dbContext.GetTasksByProjectIdAsync(project.projectID);


            var doneTasks = await GetDoneTasks(project.projectID);

            progress = (double)doneTasks.Count / (double)allTasks.Count;


            project.projectProgress = progress;

            await dbContext.UpdateProjectAsync(project);

        }

        //========================================================================================
        //Method that Updates the background of the task based of teh deadline
        public async Task UpdateTaskColour(TaskClass task)
        {

            if(task.taskHasDeadline == false) { return; }

            bool due = false;


            var dif = DateTime.Now - task.taskDeadline;


            int daysLeft = dif.Days * -1;

            //daysLeft = daysLeft + 1;

            var v1 = DateTime.Now.DayOfYear;
            var v2 = task.taskDeadline.DayOfYear;
            
   
            if(v1 == v2) 
            {
                task.taskDaysLeft = "Due Today";
            }
            else if(v2 < v1) 
            {
                task.taskDaysLeft = "Overdue";
            }
            else if(v2-v1 == 1)
            {
                task.taskDaysLeft = "Due Tommorow";
            }
            else
            {
                task.taskDaysLeft = "Days Left: " + daysLeft;
            }

            
  
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
        //========================================================================================
        //Method that Sets the Text for the ExtraInfo popup
        public async Task PopInfo(string choice)
        {

            string text;

            switch (choice)
            {
                case "Doing":

                    text = "The 'Doing' column shows the tasks that are activly being worked on!\n\n" +
                        "You can add to this by dragging a task over the header. ";

                    await MopupService.Instance.PushAsync(new PopupPages.ExtraInfoPopup(text), true);

                    break;
                case "To-Do": 

                    text = "The 'To-Do' column shows the tasks that are in backlog and what need to be done AFTER all those in the 'Doing' pile are done.\n\n" +
                        "Like the 'Doing' Header if you drag a task over this header you can deprioritise that task and put it in the backlog or 'To-Do'.";

                    await MopupService.Instance.PushAsync(new PopupPages.ExtraInfoPopup(text), true);

                    break;

                case "Goal":

                    text = "The Goal is a very short sentence about the overall objective and is NOT a descriptions!\n\n" +
                        "An Example: if that objective is to get milk form the store, the goal would be: Go get milk!";

                    await MopupService.Instance.PushAsync(new PopupPages.ExtraInfoPopup(text), true);

                    break;

                case "Decription":

                    text = "Unlike the Goal, Description is a OPTIONAL place to jot down what the Objective is in detail.";

                    await MopupService.Instance.PushAsync(new PopupPages.ExtraInfoPopup(text), true);

                    break;

                case "Deadline":

                    text = "You can give the task a Deadline by Checking or UnChecking the checkbox.";

                    await MopupService.Instance.PushAsync(new PopupPages.ExtraInfoPopup(text), true);

                    break;

                case "TaskName":

                    text = "Task Name: is the name you give to the task and is what is displayed on the Task List. Try naming the task as relatable to its purpose as you can\n\n" +
                        "Example: 'Get Milk' if the task is to get milk.";

                    await MopupService.Instance.PushAsync(new PopupPages.ExtraInfoPopup(text), true);

                    break;

                case "ProjectName":

                    text = "Project Name: is the name you give to the task and is what is displayed on the Project List. Try naming the Project as relatable to its purpose as you can\n\n" +
                        "Example: 'Write Essay' if the task is to write an Essay";

                    await MopupService.Instance.PushAsync(new PopupPages.ExtraInfoPopup(text), true);

                    break;


            }
        }

    }
}
