
using HonorsApplication_II.ViewModels;

namespace HonorsApplication_II.Pages
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