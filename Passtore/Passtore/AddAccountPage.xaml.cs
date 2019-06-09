using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Passtore.Database;
using Passtore.Utils;

// formularz dodawania konta
namespace Passtore
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddAccountPage : ContentPage
	{
		public AddAccountPage ()
		{
			InitializeComponent ();
		}

        private async void CreateAccountButton_Clicked(object sender, EventArgs e)
        {
            string application = applicationEntry.Text;
            string login = loginEntry.Text;
            string pass = passEntry.Text;
            string email = emailEntry.Text;

            Account account = new Account(login, email, application, LoginSystem.LoggedUser);

            bool validation = await AccountsManager.Validate(this, account, pass);

            if (validation)
            {
                AccountsManager.AddAccount(account, pass);

                await Navigation.PopAsync();
            }
        }

        private void GeneratePassButton_Clicked(object sender, EventArgs e)
        {
            passEntry.Text = PassUtils.GetRandomAlphanumericPlusString(App.random.Next(8, 12));
        }

        private void PassEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            PassUtils.UpdatePassBars(passGrid.Children, passEntry.Text);
        }
    }
}