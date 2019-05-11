using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Passtore.Database;
using Passtore.Utils;
using System.Threading.Tasks;

namespace Passtore
{
    public static class LoginSystem
    {
        public static User LoggedUser { get; private set; }

        public static async Task<bool> Login(string username, string pass)
        {
            User user = await MatchUserWithCredentials(username, pass);

            if(user != null)
            {
                LoggedUser = user;
                return true;
            }

            return false;
        }

        public static async Task<User> MatchUserWithCredentials(string username, string pass)
        {
            List<User> users = AppDB.DB.Table<User>().Where(u => u.Login.Equals(username)).ToList();

            if (users.Count > 0)
            {
                string storedPass = await PassUtils.GetPass(users[0].PasswordKey);

                if (pass == storedPass)
                    return users[0];
                return null;
            }

            return null;
        }

        public static void Logout()
        {
            LoggedUser = null;
        }

        public static async void PerformLoginTest()
        {
            User user = new User("Jakubosik");

            if (!UsersManager.CheckIfAlreadyInDB(user))
            {
                Debug.WriteLine("Adding User");
                await UsersManager.AddUser(user, "1234");
            }
            else
            {
                Debug.WriteLine("User already exists");
            }

            bool loginSuccess = await LoginSystem.Login(user.Login, "1234");

            if (loginSuccess)
            {
                Debug.WriteLine("Successfully logged in");
            }
            else
            {
                Debug.WriteLine("Wrong Credentials");
            }
        }
    }
}
