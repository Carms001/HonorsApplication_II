using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsApplication.ProgramClasses
{
     public class subTask
    {

        //subTask Details

        //subTask Unquie Identifiyer
        [PrimaryKey, AutoIncrement]
        public int subTaskID { get; set; }

        //subTask Title
        public String subTaskName { get; set; } = null!;

        //Very Brief description of the subTask
        public String subTaskGoal { get; set; } = null!;

        //Optional** More in depth description of the subTask
        public String subTaskDescription { get; set; }

        //True or False if subTask is complete to not
        public bool subTaskComplete { get; set; }

        //the "Story Point" Value of the subTask
        public float subTaskPoints { get; set; }

        //The prioritisation level of the subTask
        public float subTaskPrioLevel { get; set; }

        //the status of the subTask
        public String subTaskStatus { get; set; }

        //subTask Start Date, Deadline and End Date
        public DateTime subTaskStartDate { get; set; }
        public DateTime subTaskDeadline { get; set; }
        public DateTime subTaskEndDate { get; set; }

        [Indexed]
        public int taskID { get; set; }

    }
}
