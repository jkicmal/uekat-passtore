using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Passtore.Utils;

// zarzadzanie kontami uzytkownikow
namespace Passtore.Database
{
    public static class UsersManager
    {
        public static int GetLastId()
        {
            return DBUtils.GetLastId<User>(AppDB.DB, User.TableName);
        }

        public static int GetNextId()
        {
            return DBUtils.GetNextId<User>(AppDB.DB, User.TableName);
        }

        public static bool CheckIfAlreadyInDB(User user)
        {
            return (AppDB.DB.Table<User>().Where(u => u.Login.Equals(user.Login)).ToArray().Length != 0);
        }

        public static ObservableCollection<Account> GetUsersAccounts(User user)
        {
            List<Account> accounts = AppDB.DB.Table<Account>().Where(a => a.User_Id == user.Id).ToList();
            return new ObservableCollection<Account>(accounts);
        }

        public static string CreatePassKey()
        {
            return PassUtils.CreatePassKey("user", GetNextId());
        }

        public static async Task AddUser(User user, string pass)
        {
            user.PasswordKey = CreatePassKey();
            await PassUtils.SetPass(user.PasswordKey, pass);
            AppDB.Insert(user);
        }
    }
}
