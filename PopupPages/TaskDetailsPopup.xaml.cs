namespace HonorsApplication_II.PopupPages;

using CommunityToolkit.Mvvm.ComponentModel;
using TaskClass = HonorsApplication_II.ProgramClasses.Task;

public partial class TaskDetailsPopup
{



	public TaskDetailsPopup(TaskClass task)
	{
		InitializeComponent();

		TaskName.Text = "Task: " + task.taskName;

        Goal.Text = "Goal: " + task.taskGoal;

        if(task.taskHasDeadline == true)
        {

            Deadline.Text = "Deadline: " + task.taskDeadline.ToString("dddd, dd MMMM");
        }
        else
        {
            Deadline.Text = "Deadline: NO deadline";
        }

        Catagory.Text = "Catagory: " + task.taskCatagory;




    }
}