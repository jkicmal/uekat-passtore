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
	public partial class ViewAccountPage : ContentPage
	{
        Account account;

		public ViewAccountPage (Account account)
		{
            this.account = account;
			InitializeComponent ();
		}

        protected override async void OnAppearing()
        {
            if (account == null)
                await Navigation.PopAsync();

            applicationLabel.Text = account.Application;
            loginLabel.Text = account.Login;
            passLabel.Text = await PassUtils.GetPass(account.PasswordKey);
            emailLabel.Text = account.Email;
        }

        private async void DeleteAccountButton_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Uwaga!", "Czy na pewno chcesz usunąć to konto?", "Tak", "Nie");
            if(answer)
            {
                AppDB.Remove(account);
                await DisplayAlert("Sukces", "Udało się usunąc konto.", "OK");
                await Navigation.PopAsync();
            }
        }

        private async void EditAccountButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditAccountPage(account));
        }
    }
}