using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Passtore.Database;

// strona logowania, pierwsza strona jaka widzi uzytkownik
namespace Passtore
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();

            object login;
            object rememberLoginSwitchState;

            if (Application.Current.Properties.TryGetValue("rememberLoginSwitchState", out rememberLoginSwitchState))
            {
                rememberLoginSwitch.IsToggled = (bool)rememberLoginSwitchState;
            }

            if (Application.Current.Properties.TryGetValue("rememberedLogin", out login))
            {
                if(rememberLoginSwitch.IsToggled)
                {
                    loginEntry.Text = (string)login;
                }    
            }
                
		}

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            string login = loginEntry.Text;
            string pass = passEntry.Text;

            User user = new User(login);

            if (UsersManager.CheckIfAlreadyInDB(user))
            {
                bool loginSuccess = await LoginSystem.Login(user.Login, pass);
                if (loginSuccess)
                {
                    if(rememberLoginSwitch.IsToggled)
                    {
                        Application.Current.Properties.Remove("rememberedLogin");
                        Application.Current.Properties.Add("rememberedLogin", login);
                    }
                    passEntry.Text = "";
                    await Navigation.PushAsync(new MainPage());
                } else
                {
                    await DisplayAlert("Błąd", "Niepoprawne dane logowania", "Rozumiem");
                }
            } else
            {
                await DisplayAlert("Błąd", "Niepoprawne dane logowania", "Rozumiem");
            }
            
        }

        private void RememberLoginSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Application.Current.Properties.Remove("rememberLoginSwitchState");
            Application.Current.Properties.Add("rememberLoginSwitchState", rememberLoginSwitch.IsToggled);
        }
    }
}