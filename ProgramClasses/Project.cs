using HonorsApplication_II.Data;
using HonorsApplication_II.Functions;
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

        public DateTime projectEndDate { get; set; }
        public DateTime projectLastUsed { get; set; }

        //=================================================================================

        public double projectProgress { get; set; }

        //[Indexed]
        //public long userID { get; set; }

    }

}
