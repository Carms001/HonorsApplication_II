
using HonorsApplication.ViewModels;

namespace HonorsApplication.Pages
{


	public partial class UserSetupPage : ContentPage
	{
		public UserSetupPage(UserSetupViewModel vm)
		{
			InitializeComponent();

			BindingContext = vm;
		}

    }
}