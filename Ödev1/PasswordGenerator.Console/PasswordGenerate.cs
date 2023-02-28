using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGenerator.Console
{
    public class PasswordGenerate
    {
        private string Password { get; set; }
        private string PasswordBase { get; set; }

        private Random _random;

        public PasswordGenerate()
        {
            Password = string.Empty;
            PasswordBase = string.Empty;
            _random = new Random();
        }
        public void GeneratePassword(int length)
        {
            if (PasswordBase.Length == 0)
            {
                System.Console.WriteLine("You said no to all questions, password cannot be created!");
                return;
            }
            for (int i = 0; i < length; i++)
            {
                var randomIndex = _random.Next(PasswordBase.Length - 1);
                Password += PasswordBase[randomIndex];
            }

        }

        public void AddPasswordBase(string answer, string charList)
        {
            if (answer == "Y" || answer == "y")
            {
                PasswordBase += charList;

            }
        }

        public string GetPassword()
        {
            return Password;
        }
    }
}
