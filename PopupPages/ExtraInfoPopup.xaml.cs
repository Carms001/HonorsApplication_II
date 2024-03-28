namespace HonorsApplication_II.PopupPages;

using CommunityToolkit.Mvvm.ComponentModel;

public partial class ExtraInfoPopup
{



	public ExtraInfoPopup(string text)
	{
		InitializeComponent();

        TextLabel.Text = text;

    }
}