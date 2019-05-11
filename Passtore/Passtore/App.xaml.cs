using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using Passtore.Utils;
using System.Threading.Tasks;

using Passtore.Database;
using Xamarin.Essentials;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Passtore
{
    public partial class App : Application
    {
        public static Random random;

        public App()
        {
            random = new Random();

            string s = DBUtils.GetPath("TEST");

            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());

            Debug.WriteLine("DB PATH: " + AppDB.DB.DatabasePath);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }


}
