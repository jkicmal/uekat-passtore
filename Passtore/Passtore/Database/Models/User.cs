using SQLite;

// uzytkownik
namespace Passtore.Database
{
    [Table("users")]
    public class User : ISQLiteModel
    {
        public static string TableName { get; } = "users";

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(250), Unique, NotNull]
        public string Login { get; set; }

        [MaxLength(250), NotNull]
        public string PasswordKey { get; set; }

        public User() { }

        public User(string login)
        {
            Login = login;
        }
    }
}
