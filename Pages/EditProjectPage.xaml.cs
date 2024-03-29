using HonorsApplication_II.ViewModels;

namespace HonorsApplication_II.Pages;

public partial class EditProjectPage : ContentPage
{
	public EditProjectPage(EditProjectViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}