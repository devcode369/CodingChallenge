namespace MineSweepers
{
    public partial class Minesweeper : Form
    {
        private int numRows = 24;
        private int numCols = 24;
        private int numMines = 99;
        private int flagCount = 0;
        private int[,] gameBoard;
        private Button[,] buttons;
        private readonly int[] dRow = { -1, -1, -1, 0, 1, 1, 1, 0 };
        private readonly int[] dCol = { -1, 0, 1, 1, 1, 0, -1, -1 };
        private Button button1;
        private Button button2;
        private Panel panel1;
        private Panel panel2;
        private Panel innerPanel;
        private RichTextBox richTextBox1;
        private RichTextBox richTextBox2;
        private ComboBox cmbLevel;
        private Timer timer;
        private int secondsElapsed;
        private readonly int buttonSize = 30;

        public Minesweeper()
        {
            flagCount = numMines;
            PreRequest();
        }

        private void PreRequest()
        {
            InitializeComponent();
            InitializeBoard();
            InitializeTimer();
            AdjustWindowSize();
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
            Random rand = new();
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
                    {
                        continue;
                    }

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
            innerPanel.Controls.Clear();

            innerPanel.Size = new Size(numCols * buttonSize, numRows * buttonSize);

            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    buttons[row, col] = new Button
                    {
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(buttonSize * col, buttonSize * row),
                        Tag = new Point(row, col),
                        Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0),
                        UseVisualStyleBackColor = true,
                        BackColor = Color.DarkGray,
                        FlatStyle = FlatStyle.Popup,
                        ForeColor = Color.Green,
                        Text = ""
                    };
                    buttons[row, col].ForeColor = Color.Red;
                    // buttons[row,col].FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
                    buttons[row, col].MouseUp += new MouseEventHandler(Button_MouseUp);
                    innerPanel.Controls.Add(buttons[row, col]);
                }
            }

            CenterInnerPanel();
        }

        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            Button? btn = sender as Button;
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
            {
                return;
            }

            if (gameBoard[row, col] == -1)
            {
                buttons[row, col].BackgroundImageLayout = ImageLayout.Stretch;
                buttons[row, col].BackgroundImage = Properties.Resources.BMB2;
                button1.Image = null;
                button1.Image = Properties.Resources.Ssad;
                RevealAllMines();
                _ = MessageBox.Show("Game Over! You hit a mine.");
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
                richTextBox1.Text = flagCount.ToString();
                btn.Text = "";
                btn.Image = null;
            }
            else
            {
                flagCount--;
                richTextBox1.Text = flagCount.ToString();
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
            cmbLevel = new ComboBox();
            button2 = new Button();
            button1 = new Button();
            richTextBox2 = new RichTextBox();
            richTextBox1 = new RichTextBox();
            panel2 = new Panel();
            innerPanel = new Panel();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.Gray;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(cmbLevel);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(richTextBox2);
            panel1.Controls.Add(richTextBox1);
            panel1.Location = new Point(2, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(799, 78);
            panel1.TabIndex = 1;
            // 
            // cmbLevel
            // 
            cmbLevel.BackColor = Color.Silver;
            cmbLevel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            cmbLevel.FormattingEnabled = true;
            cmbLevel.Items.AddRange(new object[] { "----Select----", "Beginner", "Intermediate", "Advanced" });
            cmbLevel.Location = new Point(154, 21);
            cmbLevel.Name = "cmbLevel";
            cmbLevel.Size = new Size(151, 28);
            cmbLevel.TabIndex = 4;
            cmbLevel.SelectedIndexChanged += cmbLevel_SelectedIndexChanged;
            // 
            // button2
            // 
            button2.BackColor = Color.DarkGray;
            button2.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            button2.ForeColor = Color.Red;
            button2.Location = new Point(481, 17);
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
            richTextBox2.Size = new Size(100, 62);
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
            panel2.Controls.Add(innerPanel);
            panel2.Location = new Point(1, 80);
            panel2.Name = "panel2";
            panel2.Size = new Size(800, 373);
            panel2.TabIndex = 2;
            // 
            // innerPanel
            // 
            innerPanel.Location = new Point(253, 39);
            innerPanel.Name = "innerPanel";
            innerPanel.Size = new Size(250, 125);
            innerPanel.TabIndex = 0;
            // 
            // Minesweeper
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(800, 450);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Icon = Properties.Resources.BM;
            Name = "Minesweeper";
            Text = "Minesweeper";
            Load += Minesweeper_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ResetGame();
        }

        private void cmbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbLevel.SelectedItem.ToString())
            {
                case "Beginner":
                    numRows = 9;
                    numCols = 9;
                    numMines = 10;
                    break;
                case "Intermediate":
                    numRows = 16;
                    numCols = 16;
                    numMines = 40;
                    break;
                case "Advanced":
                    numRows = 24;
                    numCols = 24;
                    numMines = 99;
                    break;
                default:
                    return;
            }

            ResetGame();
        }

        private void Minesweeper_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = flagCount.ToString();
            AdjustWindowSize();
        }

        private void ResetGame()
        {
            flagCount = numMines;
            richTextBox1.Text = flagCount.ToString();
            button1.Image = null;
            button1.Image = Properties.Resources.sm;
            InitializeBoard();
            ResetTimer();
        }

        private void CenterInnerPanel()
        {
            innerPanel.Location = new Point(
                (panel2.Width - innerPanel.Width) / 2,
                (panel2.Height - innerPanel.Height) / 2
            );

            innerPanel.Anchor = AnchorStyles.None;

        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CenterInnerPanel();
        }

        private void CheckForWin()
        {
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    if (gameBoard[row, col] != -1 && buttons[row, col].Enabled)
                    {
                        return;
                    }
                }
            }

            _ = MessageBox.Show("Congratulations! You've cleared the minefield!");
        }

        private void InitializeTimer()
        {
            timer = new Timer
            {
                Interval = 1000
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            secondsElapsed++;
            richTextBox2.Text = secondsElapsed.ToString();
        }

        private void ResetTimer()
        {
            timer.Stop();
            secondsElapsed = 0;
            richTextBox2.Text = secondsElapsed.ToString();
            timer.Start();
        }

        private void AdjustWindowSize()
        {
            int windowWidth = panel2.Width + (Width - ClientSize.Width);
            int windowHeight = panel2.Height + panel1.Height + (Height - ClientSize.Height);
            Size = new Size(windowWidth, windowHeight);
        }
    }
}
