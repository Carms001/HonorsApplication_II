using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsApplication_II.ProgramClasses
{
    public class Project
    {
        //Project Details

        [PrimaryKey, AutoIncrement]
        public int projectID { get; set; }
        //Project Title
        public string projectName { get; set; }

        //Very brief description of the Project Ideally one sentance
        public string projectGoal { get; set; }

        //Optional** More in depth descrition of the project
        public string projectDescription { get; set; }

        //True/False is the project complete?
        public bool projectIsComplete { get; set; }

        //Progress
        //public double Progress { get { return getProgress(assginedTasks.Count, completedTasksCount); } set { } }


        //Project Start Date, Deadline and End Date
        public DateTime projectStartDate { get; set; }
        public DateTime projectDeadLine { get; set; }
        public DateTime projectEndDate { get; set; }
        public DateTime projectLastUsed { get; set; }

        //=================================================================================

        //Project - Task Structure

        //Get No. Assinged Tasks

        public int projectAssingedTasks 
        { 
            get 
            {
                return 0; 
            } 

            
            set { } }



        //List of Completed Tasks
        //public List<Task> completedTasks { get; set; }

        //public int completedTasksCount { get { if (completedTasks == null || completedTasks.Count == 0) { return 0; } else { return completedTasks.Count; } } set { } }

        //public int UserID { get => userID; set => userID = value; }

        //public double getProgress(double at, double ct)
        //{

            //double tt = at + ct;

            //double progress = (ct / tt);

           // return progress;
        //}

        [Indexed]
        public long userID { get; set; }


    }
}
