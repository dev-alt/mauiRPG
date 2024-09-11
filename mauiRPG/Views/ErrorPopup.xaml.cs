using CommunityToolkit.Maui.Views;

namespace mauiRPG.Views
{
    public partial class ErrorPopup : Popup
    {
        public ErrorPopup(string errorMessage)
        {
            InitializeComponent();
            ErrorMessageLabel.Text = errorMessage;
        }

        private void OnCloseButtonClicked(object sender, EventArgs e)
        {
            Close();
        }
    }
}