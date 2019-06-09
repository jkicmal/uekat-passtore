using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using System.Diagnostics;
using Passtore.Database;

// ogolne metody ulatwiajace prace z baza danych
namespace Passtore.Utils
{
    public static class DBUtils
    {
        // zwraca poprawną ścieżkę do bazy danych zależną od platformy
        public static string GetPath(string dbName)
        {
            string personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    return Path.Combine(personalFolder, "..", "Library", dbName);
                case Device.Android:
                    return Path.Combine(personalFolder, dbName);
                case Device.UWP:
                    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), dbName);
                default:
                    string directoryPath = Path.Combine(personalFolder, "Passtore");
                    if (!Directory.Exists(directoryPath))
                        Directory.CreateDirectory(directoryPath);
                    return Path.Combine(directoryPath, dbName);
            }
        }

        // zwraca ostatnie wpisane id wiersza dla podanej tabeli
        public static int GetLastId<T>(SQLiteConnection conn, string tableName) where T : ISQLiteModel, new()
        {
            List<T> results;
            Debug.WriteLine(tableName);
            try
            {
                results = conn.Query<T>($"select * from " + tableName + " order by Id desc limit 1");
                if (results.Count > 0)
                    return results[0].Id;
                return 0;
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine("Couldn't get last id of a database model: " + e.ToString());
                return -1;
            }
        }

        // zwraca kolejne wolne id dla podenj tabeli
        public static int GetNextId<T>(SQLiteConnection conn, string tableName) where T : ISQLiteModel, new()
        {
            int id = GetLastId<T>(conn, tableName);

            if (id == -1)
                return id;
            else
                return id + 1;
        }
    }
}
