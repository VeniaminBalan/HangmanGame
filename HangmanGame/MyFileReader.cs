using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame
{
    internal class MyFileReader
    {
        private const string PATH = "Data.txt";

        public GameWord GetGameWord() 
        {
            string caleFisier = Path.GetFullPath(Path.Combine(Application.StartupPath, @"../../"));

            if (File.Exists(caleFisier + PATH))
            {
                string[] lines = File.ReadAllLines(caleFisier + PATH);
                Random random = new Random();

                int line_index = random.Next(0, lines.Length);

                string[] fileGameWords = lines[line_index].Split(',');

                GameWord gameWord = new GameWord();
                gameWord.SecretWord = fileGameWords[0];
                gameWord.Hint = fileGameWords[1];

                return gameWord;

            }
            else 
            {
                MessageBox.Show("File does not exist");
            }

            return null;
        }
    }
}
