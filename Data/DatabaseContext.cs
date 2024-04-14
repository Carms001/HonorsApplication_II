using DevExpress.Data.XtraReports.Native;
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

                await db.CreateTableAsync<Project>();
                await db.CreateTableAsync<TaskClass>();


            }

        }

        //If the Database is needed to be removed for testing purposes
        public  async Task ResetDB()
        {

            await Init();

            //Checks if the the database has been created
            if(db != null)
            {
                //Drops tables

                await db.DropTableAsync<TaskClass>();
                await db.DropTableAsync<Project>();


                //Removes the Database Connection
                db = null;

            }

        }

        //=================================================================================
       
        //Projects

        // Create a new project asynchronously
        public async Task AddProjectAsync(Project project)
        {
            await Init();

            await db.InsertAsync(project);
        }

        // Retrieve all projects asynchronously
        public async Task<List<Project>> GetAllProjectsAsync()
        {
            await Init();

            return await db.Table<Project>().Where(Project => !Project.projectIsComplete).ToListAsync();
        }

        // Retrieve a project by ID asynchronously
        public async Task<Project> GetProjectAsync(int projectId)
        {
            await Init();

            return await db.Table<Project>().FirstOrDefaultAsync(p => p.projectID == projectId);
        }

        // Retrieve all projects for a specific user asynchronously
        //public async Task<List<Project>> GetProjectsByUserIdAsync(int userId)
        //{
            //await Init();

            //return await db.Table<Project>().Where(p => p.userID == userId).ToListAsync();
        //}

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

    }
}