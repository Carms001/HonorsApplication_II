using HonorsApplication_II.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using DevExpress.Data.XtraReports.Native;
using HonorsApplication_II.ProgramClasses;

namespace HonorsApplication_II.Pages;

public partial class EditTaskPage : ContentPage
{
	public EditTaskPage(EditTaskViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}


}