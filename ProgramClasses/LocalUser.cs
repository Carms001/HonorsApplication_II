using HonorsApplication.ProgramClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsApplication.ProgramClasses
{


    internal class LocalUser
    {
        //Local User details
        
        //Users Name
        public string username { get; set; } = null!;

        //Users Setup Date
        public DateTime signUpDate { get; set; }

        //Users Last Activity
        public DateTime lastActive { get; set; }

        //=================================================================================

        //User - Project struture

        //List of current Users Projects
        public List<Project> currentProjects { get; set; }

        //List of Users Complete Projects
        public List<Project> completedProjects { get; set; }





    }
}
