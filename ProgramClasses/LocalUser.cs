using HonorsApplication_II.ProgramClasses;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsApplication_II.ProgramClasses
{


    public class LocalUser
    {

        public LocalUser()
        {
            
        }
        //Local User details
        [PrimaryKey, AutoIncrement]
        public int userID { get; set; }

        //Users Name
        public string username { get; set; }

        //Users Setup Date
        public DateTime signUpDate { get; set; }

        //Users Last Activity
        public DateTime lastActive { get; set; }

        //=================================================================================

        //User - Project struture

        //List of current Users Projects
        //public List<Project> currentProjects { get; set; }

        //List of Users Complete Projects
        //public List<Project> completedProjects { get; set; }

    }
}
