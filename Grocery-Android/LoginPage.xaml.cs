using CommunityToolkit.Maui.Alerts;
using GroceryAndroid.Networking;

namespace GroceryAndroid
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            Loaded += async (s, e) =>
            {
                AuthController auth = new AuthController();
                bool result = await auth.CheckToken();
                if (result)
                    await Navigation.PushAsync(new MakeListPage());
            };
            BindingContext = new Components.SelectionItem();
            InitializeComponent();
        }

        public async void OnLoginClicked(object sender, EventArgs e)
        {
            LoginBtn.IsEnabled = false;
#if DEBUG
            if (usernameField.Text == "override" && passwordField.Text == "Password_123?")
            {
                ShowToast("Welcome aboard, captain!");
                await Navigation.PushAsync(new MakeListPage());
            }
            else
            {
#endif
                AuthController auth = new();
                HttpResult loginResult = await auth.AttemptLogin(
                    usernameField.Text,
                    passwordField.Text
                );
                switch (loginResult)
                {
                    case HttpResult.Success:
                        await Navigation.PushAsync(new MakeListPage());
                        break;
                    case HttpResult.ConnectionError:
                        ShowToast("Failed to connect to server");
                        break;
                    case HttpResult.AuthError:
                        ShowToast("Invalid login credentials");
                        break;
                }
#if DEBUG
            }
#endif
            LoginBtn.IsEnabled = true;
        }

        private void ShowToast(string message)
        {
            CommunityToolkit.Maui.Core.IToast toast = Toast.Make(message);
            toast.Show();
        }
    }
}
