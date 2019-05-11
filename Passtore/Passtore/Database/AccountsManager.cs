using System;
using System.Collections.Generic;
using System.Text;

using Passtore.Database;
using Passtore.Utils;
using Xamarin.Forms;

using System.Threading.Tasks;

namespace Passtore.Database
{
    public static class AccountsManager
    {
        public static int GetLastId()
        {
            return DBUtils.GetLastId<Account>(AppDB.DB, Account.TableName);
        }

        public static int GetNextId()
        {
            return DBUtils.GetNextId<Account>(AppDB.DB, Account.TableName);
        }

        public static string CreatePassKey()
        {
            return PassUtils.CreatePassKey("account", GetNextId());
        }

        public static async void AddAccount(Account account, string pass)
        {
            account.PasswordKey = CreatePassKey();
            await PassUtils.SetPass(account.PasswordKey, pass);
            AppDB.Insert(account);
        }

        public static async void UpdateAccount(Account account, string pass)
        {
            await PassUtils.ChangePass(account.PasswordKey, pass);
            AppDB.Update(account);
        }

        public static async Task<bool> Validate(Page page, Account account, string pass)
        {
            if (account.Application == null || account.Application.Length <= 0)
            {
                await page.DisplayAlert("Błąd", "Niepoprawna nazwa aplikacji", "Rozumiem");
                return false;
            }

            if (account.Login == null || account.Login.Length <= 0 || !ValidationUtils.IsUsername(account.Login))
            {
                await page.DisplayAlert("Błąd", "Niepoprawna nazwa użytkownika", "Rozumiem");
                return false;
            }

            if (pass == null || pass.Length <= 0)
            {
                await page.DisplayAlert("Błąd", "Niepoprawna długość hasła", "Rozumiem");
                return false;
            }

            if (account.Email == null || account.Email.Length <= 0 || !ValidationUtils.IsEmail(account.Email))
            {
                await page.DisplayAlert("Błąd", "Niepoprawny adres email", "Rozumiem");
                return false;
            }

            return true;
        }
    }
}
