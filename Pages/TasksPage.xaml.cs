using HonorsApplication_II.ViewModels;

namespace HonorsApplication_II.Pages;

public partial class Tasks : ContentPage
{
	public Tasks(TasksViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}