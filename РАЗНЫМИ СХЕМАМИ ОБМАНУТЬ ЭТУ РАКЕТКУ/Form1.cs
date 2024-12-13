using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace РАЗНЫМИ_СХЕМАМИ_ОБМАНУТЬ_ЭТУ_РАКЕТКУ
{
    public partial class Form1 : Form
    {
        private Random random;
        private double multiplier;
        private bool isGameRunning;
        private int rocketYPosition;
        private List<(string, bool)> multiplierHistory;

        public Form1()
        {

            // Form properties
            this.Text = "Rocket Game";
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Black;


            // Multiplier label
            multiplierLabel = new Label
            {
                Text = "Multiplier: x1.00",
                ForeColor = Color.White,
                Font = new Font("Arial", 16, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(150, 50)
            };
            this.Controls.Add(multiplierLabel);

            // Result label
            resultLabel = new Label
            {
                Text = "",
                ForeColor = Color.Yellow,
                Font = new Font("Arial", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(100, 400)
            };
            this.Controls.Add(resultLabel);

            // History list box
            historyListBox = new ListBox
            {
                Location = new Point(10, 250),
                Size = new Size(360, 130),
                ForeColor = Color.White,
                BackColor = Color.Black,
                Font = new Font("Arial", 10)
            };
            this.Controls.Add(historyListBox);

            // Rocket picture box
            rocketPictureBox = new PictureBox
            {
                Size = new Size(30, 60),
                Location = new Point((this.ClientSize.Width - 30) / 2, 200),
                Image = Image.FromFile("rocket.png"), // Ensure you have "rocket.png" in the executable directory
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };
            this.Controls.Add(rocketPictureBox);

            // Start button
            startButton = new Button
            {
                Text = "Start",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Size = new Size(100, 40),
                Location = new Point(50, 150)
            };
            startButton.Click += StartButton_Click;
            this.Controls.Add(startButton);

            // Stop button
            stopButton = new Button
            {
                Text = "Stop",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Size = new Size(100, 40),
                Location = new Point(200, 150),
                Enabled = false
            };
            stopButton.Click += StopButton_Click;
            this.Controls.Add(stopButton);

            // Timer setup
            gameTimer = new Timer
            {
                Interval = 100 // 100 ms
            };
            gameTimer.Tick += GameTimer_Tick;

            random = new Random();
            multiplier = 1.0;
            isGameRunning = false;
            rocketYPosition = rocketPictureBox.Top;
            multiplierHistory = new List<(string, bool)>();

        }
        
        private void StartButton_Click(object sender, EventArgs e)
        {
            if (isGameRunning)
                return;

            multiplier = 1.0;
            rocketYPosition = 200;
            rocketPictureBox.Top = rocketYPosition;
            multiplierLabel.Text = $"Multiplier: x{multiplier:F2}";
            resultLabel.Text = "";
            startButton.Enabled = false;
            stopButton.Enabled = true;
            isGameRunning = true;
            gameTimer.Start();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            if (!isGameRunning)
                return;

            gameTimer.Stop();
            isGameRunning = false;
            startButton.Enabled = true;
            stopButton.Enabled = false;

            string result = $"Cashed out at x{multiplier:F2}";
            resultLabel.Text = result;
            multiplierHistory.Add((result, true));
            UpdateHistory();
        }
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (random.NextDouble() < 0.02) // 2% chance of crash per tick
            {
                gameTimer.Stop();
                isGameRunning = false;
                startButton.Enabled = true;
                stopButton.Enabled = false;
                resultLabel.Text = "Rocket crashed! You lose!";
                multiplierHistory.Add(("Rocket crashed at x" + multiplier.ToString("F2"), false));
                UpdateHistory();
                return;
            }

            multiplier += 0.05; // Increase multiplier
            multiplierLabel.Text = $"Multiplier: x{multiplier:F2}";

            // Animate rocket
            if (rocketPictureBox.Top > 50)
            {
                rocketYPosition -= 5;
                rocketPictureBox.Top = rocketYPosition;
            }
        }

        private void UpdateHistory()
        {
            historyListBox.Items.Clear();
            foreach (var (text, isWin) in multiplierHistory)
            {
                historyListBox.Items.Add(new ListViewItem
                {
                    Text = text,
                    ForeColor = isWin ? Color.Green : Color.Red
                });
            }
        }
        [STAThread]
        public static void Form()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }


    }
}
