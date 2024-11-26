using CommunityToolkit.Maui.Alerts;
using Grocery_Android.Networking;

namespace Grocery_Android
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            Loaded += async (s, e) =>
            {
                AuthController auth = new AuthController();
                bool result = await auth.CheckToken();
                if (result)
                    await Navigation.PushAsync(new Home());
            };
            InitializeComponent();
        }


        public async void OnLoginClicked(object sender, EventArgs e)
        {
            LoginBtn.IsEnabled = false;
            AuthController auth = new();
            HttpResult loginResult = await auth.AttemptLogin(usernameField.Text, passwordField.Text);
            switch (loginResult)
            {
                case HttpResult.Success:
                    await Navigation.PushAsync(new Home());
                    break;
                case HttpResult.ConnectionError:
                    ShowToast("Failed to connect to server");
                    break;
                case HttpResult.AuthError:
                    ShowToast("Invalid login credentials");
                    break;
            }
            LoginBtn.IsEnabled = true;
        }

        private void ShowToast(string message)
        {
            CommunityToolkit.Maui.Core.IToast toast = Toast.Make(message);
            toast.Show();
        }
    }
}
