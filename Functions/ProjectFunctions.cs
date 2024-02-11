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


        public async Task<List<TaskClass>> onGoingTasks(int projectID)
        {
            //Creates a new list of tasks
            List<TaskClass> onGoingTasks = new List<TaskClass>();

            //gets all the tasks assinged to the projectID
            var allTasks = await dbContext.GetTasksByProjectIdAsync(projectID);

            //If not NULL
            if (allTasks != null) 
            {
                //ForEach task in allTasks
                foreach (var task in allTasks)
                {
                    //Adds only tasks that are non complete to the onGoingTasks list
                    if (task.taskComplete == false) { onGoingTasks.Add(task); }
                }
            }

            //Returns list
            return onGoingTasks;

        }

    }
}
