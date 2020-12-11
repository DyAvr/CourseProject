using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Xceed.Words.NET;

namespace CourseProject.Models
{
    public class Encryption
    {
        private static readonly Dictionary<string, string> Languages = new Dictionary<string, string>();

        static Encryption()
        {
            Languages.Add("Rus", "абвгдеёжзийклмнопрстуфхцчшщъыьэюя");
            Languages.Add("Eng", "abcdefghijklmnopqrstuvwxyz");
        }

        public static string ParseTextFromFile(IFormFile file)
        {
            string result = "";
            if (Path.GetExtension(file.FileName)?.ToLower() == ".docx")
            {
                using (Stream fs = file.OpenReadStream())
                {
                    var doc = DocX.Load(fs);
                    for (int i = 0; i < doc.Paragraphs.Count; i++)
                    {
                        result += doc.Paragraphs[i].Text;
                        if (i != doc.Paragraphs.Count - 1) result += "\r\n";
                    }
                }
            }
            else if (Path.GetExtension(file.FileName)?.ToLower() == ".txt")
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using (var fs = new StreamReader(file.OpenReadStream(),Encoding.GetEncoding(1251)))
                {
                    var bytes = Encoding.GetEncoding(1251).GetBytes(fs.ReadToEnd());
                    result += Encoding.UTF8.GetString(Encoding.Convert(Encoding.GetEncoding(1251),Encoding.UTF8,bytes));
                }
            }
            else throw new Exceptions.WrongFileException();
            return result;
        }

        public static string Encoder(string stringToEncode, string key, string alph)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new Exceptions.WrongKeyException();
            string alphabet = Languages[alph];
            key = key.ToLower();
            foreach (var item in key)
            {
                if (!alphabet.Contains(item)) throw new Exceptions.WrongKeyException();
            }
            StringBuilder res = new StringBuilder();
            int i = 0;
            foreach (var symbol in stringToEncode)
            {
                if (alphabet.Contains(char.ToLower(symbol)))
                {
                    if (char.IsUpper(symbol))
                    {
                        res.Append(char.ToUpper(alphabet[(alphabet.IndexOf(symbol.ToString().ToLower()) + alphabet.IndexOf(key[i % key.Length])) % alphabet.Length]));
                    }
                    else
                    {
                        res.Append(char.ToLower(alphabet[(alphabet.IndexOf(symbol.ToString().ToLower()) + alphabet.IndexOf(key[i % key.Length])) % alphabet.Length]));
                    }
                    i++;
                }
                else
                {
                    res.Append(symbol);
                }
            }
            return res.ToString();
        }

        public static string Decoder(string stringToDecode, string key, string alph)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new Exceptions.WrongKeyException();
            if (string.IsNullOrWhiteSpace(stringToDecode)) throw new Exceptions.WrongFileException();
            string alphabet = Languages[alph];
            key = key.ToLower();
            foreach (var item in key)
            {
                if (!alphabet.Contains(item)) throw new Exceptions.WrongKeyException();
            }
            StringBuilder res = new StringBuilder();
            int i = 0;
            foreach (var symbol in stringToDecode)
            {
                if (alphabet.Contains(char.ToLower(symbol)))
                {
                    if (char.IsUpper(symbol))
                    {
                        res.Append(char.ToUpper(alphabet[(alphabet.IndexOf(symbol.ToString().ToLower()) - alphabet.IndexOf(key[i % key.Length]) + alphabet.Length) % alphabet.Length]));
                    }
                    else
                    {
                        res.Append(char.ToLower(alphabet[(alphabet.IndexOf(symbol.ToString().ToLower()) - alphabet.IndexOf(key[i % key.Length]) + alphabet.Length) % alphabet.Length]));
                    }
                    i++;
                }
                else
                {
                    res.Append(symbol);
                }
            }
            return res.ToString();
        }
    }
}
