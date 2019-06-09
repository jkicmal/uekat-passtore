using System.Collections.Generic;
using Passtore.Database;
using Passtore.Utils;
using System.Threading.Tasks;

// logowanie i przechowywanie stanu logowania
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
    }
}
