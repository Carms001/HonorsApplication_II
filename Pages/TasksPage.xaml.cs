using HonorsApplication_II.ViewModels;

namespace HonorsApplication_II.Pages;

public partial class Tasks : ContentPage
{
	public Tasks(TasksViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    private void SwipeView_SwipeStarted(object sender, SwipeStartedEventArgs e)
    {

    }

    private void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
    {

    }
}