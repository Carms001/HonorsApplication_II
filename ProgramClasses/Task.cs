using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsApplication_II.ProgramClasses
{
    public class Task
    {
        //Task Details

        //Task Unquie Indetifier

        [PrimaryKey, AutoIncrement]
        public int taskID { get; set; }

        //Task Title
        public string taskName { get; set; } 

        //Very Brief description of the Task
        public string taskGoal { get; set; }

        //True or False if task is complete to not
        public bool taskComplete { get; set; }

        //The prioritisation level of the Task
        public string taskCatagory { get; set; }

        //Each task is in 1 of 3 catagorys
        // 1 = ToDo
        // 2 = Doing or InProgress
        // 3 = Done or Complete

        //Task Start Date, Deadline and End Date

        public bool taskHasDeadline {  get; set; }

        public DateTime taskStartDate { get; set; }
        public DateTime taskDeadline { get; set; }
        public DateTime taskCompleteDate { get; set; }

        public string taskDaysLeft { get; set; }
        public string taskTimeDeadlineColour { get; set; }

        //================================================================================

        //Task - Subtask Structure

        //public List<subTask> subTasks { get; set; }

        [Indexed]
        public int projectID { get; set; }

    }

}
