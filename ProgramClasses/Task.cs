﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsApplication.ProgramClasses
{
    public class Task
    {
        //Task Details

        //Task Unquie Indetifier

        [PrimaryKey, AutoIncrement]
        public int taskID { get; set; }

        //Task Title
        public String taskName { get; set; } 

        //Very Brief description of the Task
        public String taskGoal { get; set; }

        //Optional** More in depth description of the task
        public String taskDescription { get; set; }

        //True or False if task is complete to not
        public bool taskComplete { get; set; }

        //the "Story Point" Value of the Task
        public float taskPoints { get; set; }

        //The prioritisation level of the Task
        public float taskPrioLevel { get; set; }

        //the status of the task
        public String taskStatus { get; set; }


        //Task Start Date, Deadline and End Date
        public DateTime taskStartDate { get; set; }
        public DateTime taskDeadline { get; set; }
        public DateTime taskEndDate { get; set; }

        //=================================================================================

        //Task - Subtask Structure

        //public List<subTask> subTasks { get; set; }

        [Indexed]
        public int projectID { get; set; }
    }
}