using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Passtore.Database;
using Passtore.Utils;

namespace Passtore
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditAccountPage : ContentPage
	{
        Account account;

		public EditAccountPage (Account account)
		{
            this.account = account;

			InitializeComponent ();

            Title = $"Edytuj '{account.Application}'";

            FillEntries();
		}

        private async void FillEntries()
        {
            applicationEntry.Text = account.Application;
            loginEntry.Text = account.Login;
            passEntry.Text = await PassUtils.GetPass(account.PasswordKey);
            emailEntry.Text = account.Email;
        }

        private void GeneratePassButton_Clicked(object sender, EventArgs e)
        {
            passEntry.Text = PassUtils.GetRandomAlphanumericPlusString(App.random.Next(8, 12));
        }

        private async void UpdateAccountButton_Clicked(object sender, EventArgs e)
        {
            bool validation = await AccountsManager.Validate( this,
                new Account(loginEntry.Text, emailEntry.Text, applicationEntry.Text, LoginSystem.LoggedUser), 
                passEntry.Text);

            if(validation)
            {
                account.Application = applicationEntry.Text;
                account.Login = loginEntry.Text;
                account.Email = emailEntry.Text;

                AccountsManager.UpdateAccount(account, passEntry.Text);

                await DisplayAlert("Sukces", "Udało się pomyślnie zaktualizować konto.", "OK");
                await Navigation.PopAsync();
            }
        }

        private void PassEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            PassUtils.UpdatePassBars(passGrid.Children, passEntry.Text);
        }
    }
}