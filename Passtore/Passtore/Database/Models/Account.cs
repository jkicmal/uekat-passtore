using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

using Passtore.Utils;

namespace Passtore.Database
{
    [Table("accounts")]
    public class Account : ISQLiteModel
    {
        public static string TableName { get; } = "accounts";

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(250), NotNull]
        public string Login { get; set; }

        [MaxLength(250), NotNull]
        public string PasswordKey { get; set; }

        [MaxLength(250), NotNull]
        public string Email { get; set; }

        [MaxLength(250), NotNull]
        public string Application { get; set; }

        [MaxLength(250)]
        public string SecretAnswer { get; set; }

        [NotNull]
        public int User_Id { get; set; }

        public Account() { }

        public Account(string login, string email, string application, User user, string secretAnswer = "")
        {
            Login = login;
            Email = email;
            Application = application;
            User_Id = user.Id;
            SecretAnswer = secretAnswer;
        }
    }
}
