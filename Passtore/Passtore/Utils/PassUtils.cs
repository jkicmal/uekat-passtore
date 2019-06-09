using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

using Xamarin.Essentials;

// generowanie, sprawdzanie poprawnosci, zapis, odczyt i modyfikacja hasel
namespace Passtore.Utils
{
    public static class PassUtils
    {
        public enum PassStrength
        {
            Invalid,
            Poor,
            Average,
            Good,
            Excelent
        }

        const string alphanumericCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        const string specialCharacters = "!@#$%^&*()_+";
        const string alphanumericPlusCharacters = alphanumericCharacters + specialCharacters;

        public static string GetRandomAlphanumericString(int length)
        {
            return GetRandomString(length, alphanumericCharacters);
        }

        public static string GetRandomAlphanumericPlusString(int length)
        {
            return GetRandomString(length, alphanumericPlusCharacters);
        }

        public static string GetRandomString(int length, IEnumerable<char> characterSet)
        {
            if (length < 0)
                throw new ArgumentException("length must not be negative", "length");
            if (length > int.MaxValue / 8) // 250 million chars ought to be enough for anybody
                throw new ArgumentException("length is too big", "length");
            if (characterSet == null)
                throw new ArgumentNullException("characterSet");

            var characterArray = characterSet.Distinct().ToArray();
            if (characterArray.Length == 0)
                throw new ArgumentException("characterSet must not be empty", "characterSet");

            var bytes = new byte[length * 8];
            new RNGCryptoServiceProvider().GetBytes(bytes);
            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                ulong value = BitConverter.ToUInt64(bytes, i * 8);
                result[i] = characterArray[value % (uint)characterArray.Length];
            }
            return new string(result);
        }

        public static PassStrength GetPassStrength(string password)
        {
            if (password.Length > 7)
            {
                int str = 0;

                if (password.Any(char.IsDigit))
                    str++;

                if (password.IndexOfAny(specialCharacters.ToCharArray()) != -1)
                    str++;

                if (password.Any(char.IsUpper))
                    str++;

                if (password.Any(char.IsLower))
                    str++;

                switch (str)
                {
                    case 2:
                        return PassStrength.Average;
                    case 3:
                        return PassStrength.Good;
                    case 4:
                        return PassStrength.Excelent;
                    default:
                        return PassStrength.Poor;
                }
            }
            else
                return PassStrength.Invalid;
        }

        public static void UpdatePassBars(Grid.IGridList<View> bars, string pass)
        {
            PassStrength passStrength = GetPassStrength(pass);
            switch (passStrength)
            {
                case PassStrength.Invalid:
                    SetPassBarsColors(bars, 4, Color.Gray);
                    break;
                case PassStrength.Poor:
                    SetPassBarsColors(bars, 1, Color.Red);
                    break;
                case PassStrength.Average:
                    SetPassBarsColors(bars, 2, Color.Orange);
                    break;
                case PassStrength.Good:
                    SetPassBarsColors(bars, 3, Color.Yellow);
                    break;
                case PassStrength.Excelent:
                    SetPassBarsColors(bars, 4, Color.Green);
                    break;
            }
        }

        static void SetPassBarsColors(Grid.IGridList<View> bars, int barsCount, Color color)
        {
            for (int i = 0; i < bars.Count; i++)
            {
                if (i < barsCount)
                    bars[i].BackgroundColor = color;
                else
                    bars[i].BackgroundColor = Color.Gray;
            }
        }

        public static string CreatePassKey(string name, int id)
        {
            return string.Concat("PW", name, id);
        }

        public static async Task SetPass(string passKey, string pass)
        {
            await SecureStorage.SetAsync(passKey, pass);
        }

        public static async Task ChangePass(string passKey, string pass)
        {
            SecureStorage.Remove(passKey);
            await SetPass(passKey, pass);
        }

        public static Task<string> GetPass(string key)
        {
            return SecureStorage.GetAsync(key);
        }

        }
    }