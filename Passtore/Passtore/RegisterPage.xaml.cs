using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Passtore.Utils;
using Passtore.Database;

// formularz rejestracji uzytkownika
namespace Passtore
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterPage : ContentPage
	{
		public RegisterPage ()
		{
            InitializeComponent ();
		}

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            string login = loginEntry.Text;
            string pass1 = passEntry.Text;
            string pass2 = passRepeatEntry.Text;

            bool flag = true;

            if(!ValidationUtils.IsUsername(login))
            {
                flag = false;
                await DisplayAlert("Błąd", "Niepoprawna nazwa użytkownika", "Rozumiem");
            }
            if (pass1.Length < 8)
            {
                flag = false;
                await DisplayAlert("Błąd", "Hasło powinno mieć przynajmniej 8 znaków", "Rozumiem");
            }
            if (pass1 != pass2)
            {
                flag = false;
                await DisplayAlert("Błąd", "Hasła nie pasują do siebie", "Rozumiem");
            }

            User user = new User(login); 
            if(UsersManager.CheckIfAlreadyInDB(user))
            {
                flag = false;
                await DisplayAlert("Błąd", "Podany login jest zajęty", "Rozumiem");
            }

            if(flag)
            {
                await UsersManager.AddUser(user, pass1);
                await DisplayAlert("Sukces!", "Pomyślnie zarejestrowano", "OK");
                await Navigation.PopAsync();
            }
        }

        void UpdatePassBars()
        {
            PassUtils.UpdatePassBars(passGrid.Children, passEntry.Text);
        }

        private void PassEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePassBars();
        }
    }
}