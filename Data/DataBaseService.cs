using HonorsApplication.ProgramClasses;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
using TaskClass = HonorsApplication.ProgramClasses.Task;

namespace HonorsApplication.Data
{
    public class DataBaseService
    {
        static SQLiteAsyncConnection db;
        static async Task Init()
        {
            if (db != null) { return; } else //If database already created dont create it again
            {

                var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyData.db");
                db = new SQLiteAsyncConnection(databasePath);

                await db.CreateTablesAsync<LocalUser, Project, TaskClass, subTask>();


            }

            
        }

        //=================================================================================
        //LocalUser

        // Create a new user asynchronously
        public async Task AddUserAsync(LocalUser user)
        {
            await Init();

            await db.InsertAsync(user);
        }

        // Retrieve a user by ID asynchronously
        public static async Task<LocalUser> GetUserAsync(int userId)
        {
            await Init();

            return await db.Table<LocalUser>().FirstOrDefaultAsync(u => u.userID == userId);
        }

        // Update a user asynchronously
        public static async Task UpdateUserAsync(LocalUser user)
        {
            await Init();

            await db.UpdateAsync(user);
        }

        // Delete a user by ID asynchronously
        public static async Task DeleteUserAsync(int userId)
        {
            await Init();

            var user = await GetUserAsync(userId);
            if (user != null)
                await db.DeleteAsync(user);
        }

        //=================================================================================
        //Projects

        // Create a new project asynchronously
        public static async Task AddProjectAsync(Project project)
        {
            await Init();

            await db.InsertAsync(project);
        }

        // Retrieve a project by ID asynchronously
        public static async Task<Project> GetProjectAsync(int projectId)
        {
            await Init();

            return await db.Table<Project>().FirstOrDefaultAsync(p => p.projectID == projectId);
        }

        // Retrieve all projects for a specific user asynchronously
        public static async Task<List<Project>> GetProjectsByUserIdAsync(int userId)
        {
            await Init();

            return await db.Table<Project>().Where(p => p.userID == userId).ToListAsync();
        }

        // Update a project asynchronously
        public static async Task UpdateProjectAsync(Project project)
        {
            await Init();

            await db.UpdateAsync(project);
        }

        // Delete a project by ID asynchronously
        public static async Task DeleteProjectAsync(int projectId)
        {
            await Init();

            var project = await DataBaseService.GetProjectAsync(projectId);
            if (project != null)
                await db.DeleteAsync(project);
        }

        //=================================================================================
        //TaskClass aka Task

        // Create a new task asynchronously
        public static async Task AddTaskAsync(TaskClass task)
        {
            await Init();

            await db.InsertAsync(task);
        }

        // Retrieve a task by ID asynchronously
        public static async Task<TaskClass> GetTaskAsync(int taskId)
        {
            await Init();

            return await db.Table<TaskClass>().FirstOrDefaultAsync(t => t.taskID == taskId);
        }

        // Retrieve all tasks for a specific project asynchronously
        public static async Task<List<TaskClass>> GetTasksByProjectIdAsync(int projectId)
        {
            await Init();

            return await db.Table<TaskClass>().Where(t => t.projectID == projectId).ToListAsync();
        }

        // Update a task asynchronously
        public static async Task UpdateTaskAsync(TaskClass task)
        {
            await Init();

            await db.UpdateAsync(task);
        }

        // Delete a task by ID asynchronously
        public static async Task DeleteTaskAsync(int taskId)
        {
            await Init();

            var task = await GetTaskAsync(taskId);
            if (task != null)
                await db.DeleteAsync(task);
        }

        //=================================================================================
        //SubTask

        // Create a new subTask asynchronously
        public static async Task AddSubTaskAsync(subTask subTask)
        {
            await Init();

            await db.InsertAsync(subTask);
        }

        // Retrieve a subTask by ID asynchronously
        public static async Task<subTask> GetSubTaskAsync(int subTaskId)
        {
            await Init();

            return await db.Table<subTask>().FirstOrDefaultAsync(st => st.subTaskID == subTaskId);
        }

        // Retrieve all subTasks for a specific task asynchronously
        public static async Task<List<subTask>> GetSubTasksByTaskIdAsync(int taskId)
        {
            await Init();

            return await db.Table<subTask>().Where(st => st.taskID == taskId).ToListAsync();
        }

        // Retrieve all subTasks asynchronously
        public static async Task<List<subTask>> GetAllSubTasksAsync()
        {
            await Init();

            return await db.Table<subTask>().ToListAsync();
        }

        // Update a subTask asynchronously
        public static async Task UpdateSubTaskAsync(subTask subTask)
        {
            await Init();

            await db.UpdateAsync(subTask);
        }

        // Delete a subTask by ID asynchronously
        public static async Task DeleteSubTaskAsync(int subTaskId)
        {
            await Init();

            var subTask = await GetSubTaskAsync(subTaskId);
            if (subTask != null)
                await db.DeleteAsync(subTask);
        }


    }
}
