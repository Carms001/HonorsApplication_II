using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsApplication.ProgramClasses
{
    class Project
    {
        //Project Details

        //Project  Unquie Identifer
        public string projectID {  get; set; }

        //Project Title
        public string projectName { get; set; } = null!;

        //Very brief description of the Project Ideally one sentance
        public string projectGoal { get; set; } = null!;

        //Optional** More in depth descrition of the project
        public string projectDescription { get; set; }

        //True/False is the project complete?
        public bool projectIsComplete { get; set; }

        //Project Start Date, Deadline and End Date
        public DateTime projectStartDate { get; set; }
        public DateTime projectDeadLine { get; set; }
        public DateTime projectEndDate { get; set; }

        //=================================================================================

        //Project - Task Structure

        //List of assinged Tasks
        public List<Task> assginedTasks { get; set; }

        //List of Completed Tasks
        public List<Task> completedTasks { get; set; }

        
    }
}
