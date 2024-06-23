
namespace MineSweepers
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;


    public partial class MinesSweeper : Form
    {
        private const int numRows = 24;
        private const int numCols = 24;
        private const int numMines = 99;
        private int flagCount = numMines;
        private int[,] gameBoard;
        private Button[,] buttons;
        private readonly int[] dRow = { -1, -1, -1, 0, 1, 1, 1, 0 };
        private Button button2;
        private readonly int[] dCol = { -1, 0, 1, 1, 1, 0, -1, -1 };
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
        private Timer timer;
        private int secondsElapsed;
        private ComboBox comboBox1;
        public MinesSweeper()
        {
            PreRequest();
        }

        private void PreRequest()
        {
            InitializeComponent();
            InitializeBoard();
            InitializeTimer();
        }
        private void InitializeBoard()
        {
            gameBoard = new int[numRows, numCols];
            buttons = new Button[numRows, numCols];

            PlaceMines();
            CalculateNumbers();
            CreateButtons();
        }



        private void PlaceMines()
        {
            Random rand = new Random();
            int minesPlaced = 0;

            while (minesPlaced < numMines)
            {
                int row = rand.Next(numRows);
                int col = rand.Next(numCols);
                if (gameBoard[row, col] != -1)
                {
                    gameBoard[row, col] = -1;
                    minesPlaced++;
                }
            }
        }

        private void CalculateNumbers()
        {

            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {

                    if (gameBoard[row, col] == -1)
                        continue;
                    int mineCount = 0;

                    for (int i = 0; i < 8; i++)
                    {
                        int newRow = row + dRow[i];
                        int newCol = col + dCol[i];

                        if (IsInBounds(newRow, newCol) && gameBoard[newRow, newCol] == -1)
                        {
                            mineCount++;
                        }
                    }

                    gameBoard[row, col] = mineCount;
                }
            }
        }

        private bool IsInBounds(int row, int col)
        {
            return row >= 0 && row < numRows && col >= 0 && col < numCols;
        }


        private void CreateButtons()
        {
            panel2.Controls.Clear();

            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    buttons[row, col] = new Button();

                    buttons[row, col].Size = new Size(30, 30);
                    buttons[row, col].Location = new Point(30 * col, 30 * row);
                    buttons[row, col].Tag = new Point(row, col);
                    buttons[row, col].MouseUp += new MouseEventHandler(Button_MouseUp);
                    buttons[row, col].Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
                    buttons[row, col].UseVisualStyleBackColor = true;
                   // buttons[row, col].BackColor = Color.FromArgb(64, 64, 64);
                   // buttons[row, col].ForeColor = Color.Red;
                    buttons[row, col].BackColor = Color.DarkGray;
                    buttons[row, col].FlatStyle = FlatStyle.Popup;
                    buttons[row, col].Text = "";
                    panel2.Controls.Add(buttons[row, col]);
                }
            }
        }


        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            Point point = (Point)btn.Tag;
            int row = point.X;
            int col = point.Y;

            if (e.Button == MouseButtons.Left)
            {
                RevealCell(row, col);
            }
            else if (e.Button == MouseButtons.Right)
            {
                ToggleFlag(btn);
            }
        }


        private void RevealCell(int row, int col)
        {

            if (!IsInBounds(row, col) || buttons[row, col].Enabled == false)
                return;


            if (gameBoard[row, col] == -1)
            {
                buttons[row, col].BackgroundImageLayout = ImageLayout.Stretch;
                buttons[row, col].BackgroundImage = Properties.Resources.BMB2;
                button1.Image = null;
                button1.Image = Properties.Resources.Ssad;
                RevealAllMines();
                MessageBox.Show("Game Over! You hit a mine.");
                return;
            }

            buttons[row, col].Enabled = false;


            buttons[row, col].Text = gameBoard[row, col].ToString();


            if (gameBoard[row, col] == 0)
            {

                for (int i = 0; i < 8; i++)
                {
                    int newRow = row + dRow[i];
                    int newCol = col + dCol[i];
                    RevealCell(newRow, newCol);
                }
            }


            CheckForWin();
        }

        private void ToggleFlag(Button btn)
        {
            if (btn.Text == "F")
            {
                flagCount++;
                this.richTextBox1.Text = flagCount.ToString();
                btn.Text = "";
                btn.Image = null;
            }
            else
            {
                flagCount--;
                this.richTextBox1.Text = flagCount.ToString();
                btn.Text = "F";
                btn.Image = Properties.Resources.flag;
                btn.BackgroundImageLayout = ImageLayout.Stretch;
            }

            CheckForWin();
        }


        private void RevealAllMines()
        {

            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {

                    if (gameBoard[row, col] == -1 && buttons[row, col].Enabled == true)
                    {
                        buttons[row, col].Image = Properties.Resources.BM1;
                        buttons[row, col].Enabled = false;
                    }
                }
            }
        }


        private void InitializeComponent()
        {
            panel1 = new Panel();
            button2 = new Button();
            button1 = new Button();
            richTextBox2 = new RichTextBox();
            richTextBox1 = new RichTextBox();
            panel2 = new Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.Gray;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(richTextBox2);
            panel1.Controls.Add(richTextBox1);
            panel1.Location = new Point(2, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(799, 78);
            panel1.TabIndex = 1;
            // 
            // button2
            // 
            button2.BackColor = Color.DarkGray;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            button2.ForeColor = Color.Brown;
            button2.Location = new Point(178, 17);
            button2.Name = "button2";
            button2.Size = new Size(76, 32);
            button2.TabIndex = 3;
            button2.Text = "Reset";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Image = Properties.Resources.sm;
            button1.Location = new Point(340, 3);
            button1.Name = "button1";
            button1.Size = new Size(85, 62);
            button1.TabIndex = 2;
            button1.UseVisualStyleBackColor = true;
            // 
            // richTextBox2
            // 
            richTextBox2.BackColor = Color.FromArgb(64, 64, 64);
            richTextBox2.BorderStyle = BorderStyle.FixedSingle;
            richTextBox2.Font = new Font("Segoe UI", 25F, FontStyle.Bold, GraphicsUnit.Point, 1, true);
            richTextBox2.ForeColor = Color.Red;
            richTextBox2.Location = new Point(630, 10);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.Size = new Size(79, 62);
            richTextBox2.TabIndex = 1;
            richTextBox2.Text = "";
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = Color.FromArgb(64, 64, 64);
            richTextBox1.BorderStyle = BorderStyle.FixedSingle;
            richTextBox1.Font = new Font("Segoe UI", 25F, FontStyle.Bold);
            richTextBox1.ForeColor = Color.Red;
            richTextBox1.Location = new Point(42, 11);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(79, 62);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = flagCount.ToString();
            // 
            // panel2
            // 
            panel2.AutoSize = true;
            panel2.BackColor = Color.Gray;
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Location = new Point(1, 80);
            panel2.Name = "panel2";
            panel2.Size = new Size(800, 373);
            panel2.TabIndex = 2;
            // 
            // MinesSweeper
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "MinesSweeper";
            Text = "MinesSweeper";
            Load += TestMinesSweeper_Load;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private void InitializeTimer()
        {
            timer = new Timer();
            timer.Interval = 1000; // 1 second
            timer.Tick += Timer_Tick;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            secondsElapsed = 0;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            secondsElapsed++;
            UpdateRichTextBox(secondsElapsed);
        }

        private void UpdateRichTextBox(int seconds)
        {
            this.richTextBox2.Text = $"{seconds}";
        }
       

        private void TestMinesSweeper_Load(object sender, EventArgs e)
        {
            secondsElapsed = 0;
            timer.Start();

        }


        private void ResetGameBoard()
        {

            gameBoard = new int[numRows, numCols];
            buttons = new Button[numRows, numCols];

            PlaceMines();
            CalculateNumbers();

            flagCount = numMines;
            richTextBox1.Text = flagCount.ToString();

            panel2.Controls.Clear();
            CreateButtons();
            button1.Image = null;
            button1.Image = Properties.Resources.sm;
            secondsElapsed = 0;
            richTextBox2.Text = "0";
            timer.Stop();
            timer.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ResetGameBoard();
        }
        private void CheckForWin()
        {
            bool hasWon = true;
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    if (gameBoard[row, col] == -1 && buttons[row, col].Text != "F")
                    {
                        hasWon = false;
                        break;
                    }

                    else if (gameBoard[row, col] != -1 && buttons[row, col].Enabled)
                    {
                        hasWon = false;
                        break;
                    }
                }
            }


            if (hasWon)
            {
                MessageBox.Show("Congratulations! You have won the game!");
                timer.Stop();
            }
        }

    }




}
