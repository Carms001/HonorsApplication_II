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


        public async Task<List<taskStates>> onGoingTasks(int projectID)
        {
            //Creates a new list of tasks
            List<taskStates> onGoingTasks = new List<taskStates>();

            List<TaskClass> todoTasks = new List<TaskClass>();

            List<TaskClass> doingTasks = new List<TaskClass>();

            //gets all the tasks assinged to the projectID
            var allTasks = await dbContext.GetTasksByProjectIdAsync(projectID);

            //If not NULL
            if (allTasks != null) 
            {


                //ForEach task in allTasks
                foreach (var task in allTasks)
                {
                    //Adds only tasks that are non complete to the onGoingTasks list
                    if (task.taskComplete == false) 
                    {

                        if (task.taskState.Equals("Doing")) 
                        { 
                            
                            doingTasks.Add(task);
                        }
                        else
                        {
                            todoTasks.Add(task);
                        } 

                    }
                }
            }


            onGoingTasks.Add(new taskStates("Doing", doingTasks) { });
            onGoingTasks.Add(new taskStates("To-Do", todoTasks) { });

            //Returns list
            return onGoingTasks;

        }

    }
}
