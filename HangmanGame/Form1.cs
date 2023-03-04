using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static HangmanGame.MyTextBox;

namespace HangmanGame
{
    public partial class Form1 : Form
    {
        MyFileReader myFileReader;
        List<MyTextBox> textBoxes;

        int tries = 0;
        int score = 0;

        private Graphics graphics;
        public Form1()
        {
            InitializeComponent();

            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);
            graphics = Graphics.FromImage(pictureBox.Image);

            SetGame(NewGameWord());
        }
        private void SetGame(GameWord gameWord) 
        {
            ClearControls();
            SetTries(0);
            SetScore(0);

            label_Hint.Text = gameWord.Hint;
            textBoxes = new List<MyTextBox>();



            for (int i = 0; i < gameWord.SecretWord.Length; i++)
            {
                if (gameWord.SecretWord[i] == ' ') continue;
                var textBox = new MyTextBox(gameWord.SecretWord[i]);

                textBox.Location = new Point(171 + i * 30, 143);
                textBox.KeyPress += MyTextBox_OnKeyPress;
                textBox.OnOptionChanched_EventHandler += OnOptionChanched;

                textBoxes.Add(textBox);
                Controls.Add(textBox);
                
            }
        }

        private void OnOptionChanched(object sender, OptionsEventArgs e)
        {
            if (e.IsCorrect)
                SetTries(0);
            AddScore(e.score);
        }

        private void SetTries(int tries)
        {
            this.tries = tries;

            DrawOnMistake();

            if (tries == 3)
            {
                MessageBox.Show($"Game Over !\n Your Score:  {score}");
                SetGame(NewGameWord());

            }
        }
        private void SetScore(int score) 
        {
            this.score = score;
            label_Score.Text = this.score.ToString();
        }
        private void AddScore(int score)
        {
            this.score += score;
            label_Score.Text = this.score.ToString();
        }
        private void button_Submit_Click(object sender, EventArgs e)
        {
            ValidateWord();
        }

        private void ValidateWord() 
        {
            
            foreach (MyTextBox textBox in textBoxes) 
            {
                textBox.SetWordOption();

                if (textBox.WordOption == Options.Undefined)
                    continue;

                if ( !textBox.IsSecretChar()) 
                    SetTries(tries + 1);
            }

            foreach (MyTextBox textBox in textBoxes) 
            {
                if ( !textBox.IsSecretChar() ) return;
            }

            MessageBox.Show($"You Win !\nYour Score:  {score}");
            SetGame(NewGameWord());


        }

        private void MyTextBox_OnKeyPress(object sender, KeyPressEventArgs e)
        {
            var textBox = (MyTextBox)sender;

            if ( textBox.WordOption == Options.IS_Secret_Word )
                e.Handled = true;

            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Back )
            {
                if(!textBox.IsSecretChar())
                    textBox.ForeColor = Color.Black;
            }

            if (e.KeyChar == (char)Keys.Enter)
            {
                button_Submit_Click(new object(), EventArgs.Empty);
            }
        }
        private GameWord NewGameWord() 
        {
            myFileReader = new MyFileReader();

            return myFileReader.GetGameWord();
        }

        private void ClearControls() 
        {
            if (textBoxes is null) return;
            foreach (MyTextBox textBox in textBoxes) 
            {
                textBox.Dispose();
                
            }
            textBoxes = null;

        }

        private void DrawOnMistake() 
        {
            graphics.Clear(Color.Transparent);
            graphics.FillRectangle(Brushes.Red, 0,0,(pictureBox.Width / 3) * tries, pictureBox.Height);
            pictureBox.Refresh();
        }
    }
}