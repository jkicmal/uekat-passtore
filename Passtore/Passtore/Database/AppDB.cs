using SQLite;
using System;
using Passtore.Utils;
using System.Diagnostics;

// interakcja z baza danych
namespace Passtore.Database
{
    public static class AppDB
    {
        const string dbName = "AppDatabase.sqlite";

        private static SQLiteConnection db;
        public static SQLiteConnection DB
        {
            get
            {
                if (db == null)
                    CreateDB();
                return db;
            }
        }

        static AppDB()
        {
            CreateDB();
        }

        public static void CreateDB()
        {
            db = new SQLiteConnection(DBUtils.GetPath(dbName));
            db.CreateTable<User>();
            db.CreateTable<Account>();
        }

        private static int ModifyDBData(Func<SQLiteConnection, int> action)
        {
            try
            {
                return action(DB);
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine("VALIDATION ERROR: " + e.ToString());
                return -1;
            }
        }

        public static int Insert(ISQLiteModel obj)
        {
            return ModifyDBData(conn => conn.Insert(obj));
        }

        public static int Update(ISQLiteModel obj)
        {
            return ModifyDBData(conn => conn.Update(obj));
        }

        public static int Remove(ISQLiteModel obj)
        {
            return ModifyDBData(conn => conn.Delete(obj));
        }
    }
}
