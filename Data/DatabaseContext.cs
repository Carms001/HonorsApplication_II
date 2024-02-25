using HonorsApplication_II.Functions;
using HonorsApplication_II.ProgramClasses;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using Task = System.Threading.Tasks.Task;
using TaskClass = HonorsApplication_II.ProgramClasses.Task;

namespace HonorsApplication_II.Data
{
    public class DatabaseContext
    {
        static SQLiteAsyncConnection db;


        static async Task Init()
        {
            // If the DB path is null
            if (db != null) 
            { 
                //If not Null
                return; 
            }
            else 
            {
                //If the DB path is Null

                //Create a new database path
                var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyData.db");

                //Assign a new SQLite Connection to the database path
                db = new SQLiteAsyncConnection(databasePath);

                //Create Tables
                await db.CreateTableAsync<LocalUser>();
                await db.CreateTableAsync<Project>();
                await db.CreateTableAsync<TaskClass>();
                await db.CreateTableAsync<subTask>();

            }

        }

        //If the Database is needed to be removed for testing purposes
        public async Task ResetDB()
        {
            //Checks if the the database has been created
            if(db != null)
            {
                //Drops tables
                await db.DropTableAsync<subTask>();
                await db.DropTableAsync<TaskClass>();
                await db.DropTableAsync<Project>();
                await db.DropTableAsync<LocalUser>();

                //Removes the Database Connection
                db = null;

            }

        }

        //=================================================================================
        //LocalUser

        // Method to add a new user
        public async Task AddUserAsync(LocalUser user)
        {
            //Checks the database is initalised
            await Init();

            //Adds new user to the database
            await db.InsertAsync(user);

            
        }

        // Retrieve a user by ID asynchronously
        public async Task<LocalUser> GetUserAsync(int userId)
        {
            await Init();

            return await db.Table<LocalUser>().FirstOrDefaultAsync(u => u.userID == userId);
        }

        // Update a user asynchronously
        public  async Task UpdateUserAsync(LocalUser user)
        {
            await Init();

            await db.UpdateAsync(user);
        }

        // Delete a user by ID asynchronously
        public  async Task DeleteUserAsync(int userId)
        {
            await Init();

            var user = await GetUserAsync(userId);
            if (user != null)
                await db.DeleteAsync(user);
        }

        //=================================================================================
        //Projects

        // Create a new project asynchronously
        public async Task AddProjectAsync(Project project)
        {
            await Init();

            await db.InsertAsync(project);
        }

        // Retrieve a project by ID asynchronously
        public async Task<Project> GetProjectAsync(int projectId)
        {
            await Init();

            return await db.Table<Project>().FirstOrDefaultAsync(p => p.projectID == projectId);
        }

        // Retrieve all projects for a specific user asynchronously
        public async Task<List<Project>> GetProjectsByUserIdAsync(int userId)
        {
            await Init();

            return await db.Table<Project>().Where(p => p.userID == userId).ToListAsync();
        }

        // Update a project asynchronously
        public async Task UpdateProjectAsync(Project project)
        {
            await Init();

            await db.UpdateAsync(project);
        }

        // Delete a project by ID asynchronously
        public async Task DeleteProjectAsync(int projectId)
        {
            await Init();

            var project = await GetProjectAsync(projectId);
            if (project != null)
                await db.DeleteAsync(project);
        }

        //=================================================================================
        //TaskClass aka Task

        // Create a new task asynchronously
        public async Task AddTaskAsync(TaskClass task)
        {
            await Init();

            await db.InsertAsync(task);
        }

        // Retrieve a task by ID asynchronously
        public async Task<TaskClass> GetTaskAsync(int taskId)
        {
            await Init();

            return await db.Table<TaskClass>().FirstOrDefaultAsync(t => t.taskID == taskId);
        }

        // Retrieve all tasks for a specific project asynchronously
        public async Task<List<TaskClass>> GetTasksByProjectIdAsync(int projectId)
        {
            await Init();

            return await db.Table<TaskClass>().Where(t => t.projectID == projectId).ToListAsync();

        }

        // Update a task asynchronously
        public async Task UpdateTaskAsync(TaskClass task)
        {
            await Init();

            await db.UpdateAsync(task);
        }

        // Delete a task by ID asynchronously
        public async Task DeleteTaskAsync(int taskId)
        {
            await Init();

            var task = await GetTaskAsync(taskId);
            if (task != null)
                await db.DeleteAsync(task);
        }

        public async Task AddListOfTasksToProjectAsync(List<TaskClass> tasks)
        {
            await Init();

            foreach (var task in tasks)
            {
                await db.InsertAsync(task);
            }
        }

        public async Task DeleteAllTasksByProjectIDAsync(int projectID)
        {
            await Init();

            var allTasks = await GetTasksByProjectIdAsync(projectID);

            foreach (var task in allTasks)
            {
                await db.DeleteAsync(task);
            }
        }

        // Counts total amount of tasks asinged to that project
        public async Task<int> GetTotalAmountOfTasksAsync(int projectId)
        {
            var tasks = await GetTasksByProjectIdAsync(projectId);
            int count = tasks.Count();
            return count;
        }

        //=================================================================================
        //SubTask

        // Create a new subTask asynchronously
        public async Task AddSubTaskAsync(subTask subTask)
        {
            await Init();

            await db.InsertAsync(subTask);
        }

        // Retrieve a subTask by ID asynchronously
        public  async Task<subTask> GetSubTaskAsync(int subTaskId)
        {
            await Init();

            return await db.Table<subTask>().FirstOrDefaultAsync(st => st.subTaskID == subTaskId);
        }

        // Retrieve all subTasks for a specific task asynchronously
        public async Task<List<subTask>> GetSubTasksByTaskIdAsync(int taskId)
        {
            await Init();

            return await db.Table<subTask>().Where(st => st.taskID == taskId).ToListAsync();
        }

        // Retrieve all subTasks asynchronously
        public async Task<List<subTask>> GetAllSubTasksAsync()
        {
            await Init();

            return await db.Table<subTask>().ToListAsync();
        }

        // Update a subTask asynchronously
        public async Task UpdateSubTaskAsync(subTask subTask)
        {
            await Init();

            await db.UpdateAsync(subTask);
        }

        // Delete a subTask by ID asynchronously
        public async Task DeleteSubTaskAsync(int subTaskId)
        {
            await Init();

            var subTask = await GetSubTaskAsync(subTaskId);
            if (subTask != null)
                await db.DeleteAsync(subTask);
        }


    }
}