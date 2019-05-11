using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Passtore.Database;

namespace Passtore
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Title = LoginSystem.LoggedUser.Login;
            accountsList.ItemTapped += AccountsList_ItemTapped;
        }

        private async void AccountsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var account = (Account)e.Item;
            await Navigation.PushAsync(new ViewAccountPage(account));
        }

        private async void AddAccountButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddAccountPage());
        }

        protected override void OnAppearing()
        {
            accountsList.ItemsSource = UsersManager.GetUsersAccounts(LoginSystem.LoggedUser);
        }
    }
}