using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangmanGame
{
    public enum Options { IS_Secret_Word, IS_Not_Secret_Word, Undefined };

    public class MyTextBox : TextBox
    {
        public EventHandler<OptionsEventArgs> OnOptionChanched_EventHandler;
        public class OptionsEventArgs : EventArgs 
        {
            public bool IsCorrect;
            public int score;
        }
        public char SecretChar { get; private set;}

        public static int TextBox_size = 25;

        public Options WordOption { get; private set; }
        public MyTextBox(char secretChar)
        {
            SecretChar = secretChar;
            Width = Height = TextBox_size;
            this.MaxLength = 1;
            WordOption = Options.Undefined;

        }

        public void SetWordOption() 
        {
            if (this.Text == "") 
            {
                WordOption = Options.Undefined;
                return;
            }

            if (WordOption == Options.IS_Secret_Word) 
            {
                return;
            }

            var guessChar = (this.Text.ToUpper())[0];

            if (IsSecretChar(guessChar))
            {
                WordOption = Options.IS_Secret_Word;
                this.ForeColor = Color.Green;
                OnOptionChanched_EventHandler.Invoke(this, new OptionsEventArgs 
                {
                    score = 10,
                    IsCorrect = true
                });
            }
            else 
            {
                WordOption = Options.IS_Not_Secret_Word;
                this.ForeColor = Color.Red;
                OnOptionChanched_EventHandler.Invoke(this, new OptionsEventArgs
                {
                    score = -10,
                    IsCorrect = false
                });
            }


        }
        private bool IsSecretChar(char guessChar) 
        {
            return SecretChar == guessChar;
        }
        public bool IsSecretChar()
        {
            return WordOption == Options.IS_Secret_Word;
        }

    }
}
