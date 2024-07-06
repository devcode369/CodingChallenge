using System.Reflection;
using System.Windows.Forms;

namespace ChessGame
{
    public partial class Chess : Form
    {
        private Panel panel1;
        private Panel panel2;
        private Button[,] buttons;
        private readonly int buttonSize = 100;
        private Button? draggedButton;
        private Image? draggedImage;
        private Point originalLocation;
        private bool isDragging = false;
        private Point startPoint;
        private Point endPoint;

        private const string RIGHT = "RIGHT";
        private const string LEFT = "LEFT";
        private const string UP = "UP";
        private const string DOWN = "DOWN";

        private string? _Human = null;
        private string? _AI = null;
        private string? userColor = null;
        private bool isAIPlay = false;
        private Button lastTarget = null;

        private readonly int intit = 1;

        private readonly Dictionary<Point, PieceDetails?> kp = [];
        public Chess()
        {
            try
            {
                InitializeComponent();

                Resize += new EventHandler(OnFormResize);
                Load += new EventHandler(LoadButtons);
                Shown += new EventHandler(LoadPopup);

                panelWhite.AllowDrop = true;
                panelBlack.AllowDrop = true;
                panelBlack.DragEnter += panelBlack_DragEnter;
                panelBlack.DragDrop += panelBlack_DragDrop;
                panelWhite.DragEnter += panelWhite_DragEnter;
                panelWhite.DragDrop += panelWhite_DragDrop;

            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Error in Chess constructor: {ex}");
            }
        }
        private void LoadButtons(object sender, EventArgs e)
        {
            CreateButtons();
            CenterPanel();

        }
        private void LoadPopup(object sender, EventArgs e)
        {
            SetUser();
        }


        private void CreateButtons()
        {
            try
            {
                panel2.Controls.Clear();
                buttons = new Button[8, 8];
                panel2.Size = new Size(8 * buttonSize, 8 * buttonSize);
                panel2.AllowDrop = true; // Enable dropping on panel2
                Color color1 = Color.Wheat;
                Color color2 = Color.SaddleBrown;
                int i = 0;
                for (int row = 0; row < 8; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        buttons[row, col] = new Button
                        {
                            Size = new Size(buttonSize, buttonSize),
                            Location = new Point(buttonSize * col, buttonSize * row),
                            Tag = new Point(row, col),
                            UseVisualStyleBackColor = true,
                            FlatStyle = FlatStyle.Popup
                        };

                        buttons[row, col].MouseDown += new MouseEventHandler(Button_MouseDown);
                        buttons[row, col].MouseMove += new MouseEventHandler(Button_MouseMove);
                        buttons[row, col].MouseUp += new MouseEventHandler(Button_MouseUp);

                        buttons[row, col].BackgroundImageLayout = ImageLayout.Stretch;
                        if (buttons[row, col] == buttons[0, 0] || buttons[row, col] == buttons[0, 7])
                        {

                            i += 1;
                            buttons[row, col].Image = ChessGame.Properties.Resources.WHITEROOK;
                            buttons[row, col].Image.Tag = new PieceDetails { Id = nameof(ChessPiece.WHITEROOK) + i, Value = 5, Color = "WHITE", CurrentPoint = new Point(row, col), Name = nameof(ChessPiece.WHITEROOK) };
                        }
                        if (buttons[row, col] == buttons[0, 1] || buttons[row, col] == buttons[0, 6])
                        {

                            i += 1;
                            buttons[row, col].Image = ChessGame.Properties.Resources.WHITEKNIGHT;
                            buttons[row, col].Image.Tag = new PieceDetails { Id = nameof(ChessPiece.WHITEKNIGHT) + i, Value = 3, Color = "WHITE", CurrentPoint = new Point(row, col), Name = nameof(ChessPiece.WHITEKNIGHT) };
                        }
                        if (buttons[row, col] == buttons[0, 2] || buttons[row, col] == buttons[0, 5])
                        {

                            i += 1;
                            buttons[row, col].Image = ChessGame.Properties.Resources.WHITEBISHOP;
                            buttons[row, col].Image.Tag = new PieceDetails { Id = nameof(ChessPiece.WHITEBISHOP) + i, Value = 3, Color = "WHITE", CurrentPoint = new Point(row, col), Name = nameof(ChessPiece.WHITEBISHOP) };
                        }
                        if (buttons[row, col] == buttons[0, 3])
                        {
                            buttons[row, col].Image = ChessGame.Properties.Resources.WHITEQUEEN;
                            buttons[row, col].Image.Tag = new PieceDetails { Id = nameof(ChessPiece.WHITEQUEEN), Value = 9, Color = "WHITE", CurrentPoint = new Point(row, col), Name = nameof(ChessPiece.WHITEQUEEN) };
                        }
                        if (buttons[row, col] == buttons[0, 4])
                        {
                            buttons[row, col].Image = ChessGame.Properties.Resources.WHITEKING;
                            buttons[row, col].Image.Tag = new PieceDetails { Id = nameof(ChessPiece.WHITEKING), Color = "WHITE", CurrentPoint = new Point(row, col), Name = nameof(ChessPiece.WHITEKING) };
                        }

                        if (row == 1 && col >= 0 && col <= 7)
                        {
                            i += 1;
                            buttons[row, col].Image = ChessGame.Properties.Resources.WHITEPAWN;
                            buttons[row, col].Image.Tag = new PieceDetails { Id = nameof(ChessPiece.WHITEPAWN) + i, Value = 1, Color = "WHITE", CurrentPoint = new Point(row, col), Name = nameof(ChessPiece.WHITEPAWN) };
                        }
                        if (row == 6 && col >= 0 && col <= 7)
                        {

                            i += 1;
                            buttons[row, col].Image = ChessGame.Properties.Resources.BLACKPAWN;
                            buttons[row, col].Image.Tag = new PieceDetails { Id = nameof(ChessPiece.BLACKPAWN) + i, Value = 1, Color = "BLACK", CurrentPoint = new Point(row, col), Name = nameof(ChessPiece.BLACKPAWN) };
                        }

                        if (buttons[row, col] == buttons[7, 0] || buttons[row, col] == buttons[7, 7])
                        {

                            i += 1;
                            buttons[row, col].Image = ChessGame.Properties.Resources.BLACKROOK;
                            buttons[row, col].Image.Tag = new PieceDetails { Id = nameof(ChessPiece.BLACKROOK) + i, Value = 5, Color = "BLACK", CurrentPoint = new Point(row, col), Name = nameof(ChessPiece.BLACKROOK) };
                        }
                        if (buttons[row, col] == buttons[7, 1] || buttons[row, col] == buttons[7, 6])
                        {

                            i += 1;
                            buttons[row, col].Image = ChessGame.Properties.Resources.BLACKKNIGHT;
                            buttons[row, col].Image.Tag = new PieceDetails { Id = nameof(ChessPiece.BLACKKNIGHT) + i, Value = 3, Color = "BLACK", CurrentPoint = new Point(row, col), Name = nameof(ChessPiece.BLACKKNIGHT) };
                        }
                        if (buttons[row, col] == buttons[7, 2] || buttons[row, col] == buttons[7, 5])
                        {

                            i += 1;
                            buttons[row, col].Image = ChessGame.Properties.Resources.BLACKBISHOP;
                            buttons[row, col].Image.Tag = new PieceDetails { Id = nameof(ChessPiece.BLACKBISHOP) + i, Value = 3, Color = "BLACK", CurrentPoint = new Point(row, col), Name = nameof(ChessPiece.BLACKBISHOP) };
                        }
                        if (buttons[row, col] == buttons[7, 3])
                        {
                            buttons[row, col].Image = ChessGame.Properties.Resources.BLACKQUEEN;
                            buttons[row, col].Image.Tag = new PieceDetails { Color = "BLACK", Value = 3, CurrentPoint = new Point(row, col), Name = nameof(ChessPiece.BLACKQUEEN) };
                        }
                        if (buttons[row, col] == buttons[7, 4])
                        {
                            buttons[row, col].Image = ChessGame.Properties.Resources.BLACKKING;
                            buttons[row, col].Image.Tag = new PieceDetails { Color = "BLACK", CurrentPoint = new Point(row, col), Name = nameof(ChessPiece.BLACKKING) };
                        }
                        if (buttons[row, col]?.Image?.Tag is PieceDetails && ((PieceDetails)buttons[row, col]?.Image?.Tag).Color == "BLACK")
                        {
                            buttons[row, col].Enabled = false;
                        }
                        // Alternate colors
                        buttons[row, col].BackColor = (row + col) % 2 == 0 ? color1 : color2;
                        if (buttons[row, col]?.Image?.Tag is PieceDetails)
                        {
                            Point pts = (Point)buttons[row, col].Tag;
                            PieceDetails? pieceDetails = (PieceDetails)buttons[row, col].Image.Tag;
                            kp.Add(pts, pieceDetails);
                        }
                        else
                        {
                            kp.Add((Point)buttons[row, col].Tag, null);
                        }
                        panel2.Controls.Add(buttons[row, col]);


                    }
                }

                // Event handlers for panel2 to handle drag-and-drop operations
                panel2.DragEnter += Panel2_DragEnter;
                panel2.DragDrop += Panel2_DragDrop;
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Error in CreateButtons method: {ex}");
            }
        }

        public void SetUser()
        {
            DialogResult res = MessageBox.Show("Select side YES - White Or NO - Black..", "Chess", MessageBoxButtons.YesNoCancel);

            if (res == DialogResult.Yes)
            {
                userColor = "WHITE";

                _Human = "WHITE";
                _AI = "BLACK";
                lblBlack.Text = "AI";
                lblWhite.Text = "You";
            }
            else if (res == DialogResult.No)
            {
                userColor = "BLACK";
                _AI = "WHITE";
                _Human = "BLACK";
                lblBlack.Text = "You";
                lblWhite.Text = "AI";
            }
            else if (res == DialogResult.Cancel)
            {
                Close();
            }
            if (_AI == "WHITE")
            {
                isAIPlay = true;
                AIPlays(_AI);
            }
        }

        private void AIPlays(string color)
        {
            int possibleMoveCount = 0;
            try
            {

                Dictionary<int, Point> blackColor = [];
                Dictionary<int, Point> whiteColor = [];
                Dictionary<int, Point> targetEmptyPoints = [];
                Dictionary<string, int> movementCount = [];
                List<(string, ((int, int), (int, int)))> lis = [];



                int ctn = 1;
                foreach (Button button in buttons)
                {
                    Point pt = (Point)button.Tag;
                    if (button?.Image?.Tag is PieceDetails)
                    {
                        PieceDetails? pis = button.Image.Tag as PieceDetails;

                        if (((PieceDetails)button.Image.Tag).Color == "WHITE")
                        {
                            whiteColor.Add(ctn, new Point(pt.X, pt.Y));
                        }
                        else
                        {
                            blackColor.Add(ctn, new Point(pt.X, pt.Y));
                        }
                    }
                    else
                    {
                        targetEmptyPoints.Add(ctn, new Point(pt.X, pt.Y));

                    }
                    ctn++;

                }

            // possibleMoveCount = blackColor.Count + targetEmptyPoints.Count;




            DragPoints:

                Random fromRandom = new();
                Button targetBtn = null;

                Point points = new();
                if (_AI == "WHITE")
                {
                    int fromRow = fromRandom.Next(whiteColor.Keys.Min(), whiteColor.Keys.Max());
                    _ = whiteColor.TryGetValue(fromRow, out points);
                    possibleMoveCount = blackColor.Count + targetEmptyPoints.Count;
                }
                else
                {
                    int fromRow = fromRandom.Next(blackColor.Keys.Min(), blackColor.Keys.Max());
                    _ = blackColor.TryGetValue(fromRow, out points);
                    possibleMoveCount = whiteColor.Count + targetEmptyPoints.Count;
                }


                draggedButton = buttons[points.X, points.Y];

                PieceDetails? drgButton = (PieceDetails)draggedButton.Image.Tag;
                _ = movementCount.TryGetValue(drgButton.Id, out int movPiece);

                if (movPiece >= possibleMoveCount)
                {
                    goto DragPoints;
                }

            TargetEmptyPoints:

                Random toRandom = new();

                Dictionary<int, Point> tgt = [];
                tgt = _AI == "WHITE"
                    ? blackColor.Concat(targetEmptyPoints).GroupBy(kv => kv.Key).ToDictionary(g => g.Key, g => g.First().Value)
                    : whiteColor.Concat(targetEmptyPoints).GroupBy(kv => kv.Key).ToDictionary(g => g.Key, g => g.First().Value);

                int fromCol = toRandom.Next(tgt.Keys.Min(), tgt.Keys.Max());
                _ = tgt.TryGetValue(fromCol, out Point tgtPtn);

                //if (_AI=="WHITE" && AreAllValues(movementCount, blackColor.Count, possibleMoveCount))
                //{
                //    lastTarget = null;
                //}
                //else if (_AI == "BLACK" && AreAllValues(movementCount, whiteColor.Count, possibleMoveCount))
                //{
                //    lastTarget = null;
                //}
                //if (lastTarget != null)
                //{
                //    var lastTargetPoint=(Point)lastTarget.Tag;

                //    targetBtn = buttons[lastTargetPoint.X, lastTargetPoint.Y];

                //}
                //else
                //{
                //    targetBtn = buttons[tgtPtn.X, tgtPtn.Y];
                //}

                targetBtn = buttons[tgtPtn.X, tgtPtn.Y];
                draggedImage = draggedButton?.Image;

                lis.Add((drgButton.Id, ((points.X, points.Y), (tgtPtn.X, tgtPtn.Y))));

                if (ValidateMove(targetBtn))
                {

                    Thread.Sleep(3000);
                    ((PieceDetails)draggedImage.Tag).CurrentPoint = (Point)targetBtn.Tag;

                    if (targetBtn.Image != null)
                    {

                        var pis = targetBtn.Image.Tag as PieceDetails;

                        DataObject dataObject = new DataObject();
                        dataObject.SetData(DataFormats.Bitmap, targetBtn.Image);
                        if ((pis.Color == "WHITE"))
                        {
                            CalculateScore("WHITE", pis.Value);
                            DragEventArgs dragEnterArgs = new DragEventArgs(dataObject, 0, 0, 0, DragDropEffects.Copy, DragDropEffects.Copy);
                            panelWhite_DragEnter(panelWhite, dragEnterArgs);


                            DragEventArgs dragDropArgs = new DragEventArgs(dataObject, 0, 0, 0, DragDropEffects.Copy, DragDropEffects.Copy);
                            panelWhite_DragDrop(panelWhite, dragDropArgs);
                        }
                        else
                        {
                            CalculateScore("BLACK", pis.Value);
                            DragEventArgs dragEnterArgs = new DragEventArgs(dataObject, 0, 0, 0, DragDropEffects.Copy, DragDropEffects.Copy);
                            panelBlack_DragEnter(panelBlack, dragEnterArgs);


                            DragEventArgs dragDropArgs = new DragEventArgs(dataObject, 0, 0, 0, DragDropEffects.Copy, DragDropEffects.Copy);
                            panelBlack_DragDrop(panelBlack, dragDropArgs);
                        }

                    }

                    targetBtn.Image = draggedImage;

                    if (draggedImage.Tag is PieceDetails)
                    {
                        isAIPlay = false;
                        DisableSide(((PieceDetails)draggedImage.Tag).Color);

                    }
                    draggedButton = null;
                    draggedImage = null;
                    buttons[points.X, points.Y].Image = null;
                    return;

                }
                else
                {
                    PieceDetails? drgPt = draggedButton.Image.Tag as PieceDetails;

                    if (movementCount.ContainsKey(drgPt.Id))
                    {
                        _ = movementCount.TryGetValue(drgPt.Id, out int num);
                        movementCount[drgPt.Id] += 1;
                    }
                    else
                    {
                        _ = movementCount.TryAdd(drgPt.Id, 1);
                    }
                    if (movementCount.ContainsKey(drgPt.Id))
                    {
                        _ = movementCount.TryGetValue(drgPt.Id, out int num);

                        if (num < possibleMoveCount)
                        {

                            goto TargetEmptyPoints;
                        }
                        else
                        {

                            goto DragPoints;
                        }
                    }
                }
            }
            catch (Exception)

            {



            }


        }


        private void OnFormResize(object sender, EventArgs e)
        {
            try
            {
                CenterPanel();

                SetUser();
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Error in OnFormResize method: {ex}");
            }
        }

        private void CenterPanel()
        {
            try
            {
                int x = (ClientSize.Width - panel2.Width) / 2;
                int y = (ClientSize.Height - panel2.Height) / 2;
                panel2.Location = new Point(x, y);

            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Error in CenterPanel method: {ex}");
            }
        }

        private void AssignImagesToButtons(Panel panel, string pieceName)
        {
            var buttons = panel.Controls.OfType<Button>().ToList();
            buttons.Sort((b1, b2) => b1.Name.CompareTo(b2.Name));
            var resourceProperties = typeof(ChessGame.Properties.Resources).GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            var defaultImageProperty = resourceProperties.FirstOrDefault(prop => prop.Name.Contains(pieceName));
            foreach (var button in buttons)
            {
                button.Visible = true;

                if (button?.Image == null)
                {
                    button.Visible = true;
                    button.BackColor = Color.Wheat;
                    button.BackgroundImageLayout = ImageLayout.Stretch;
                    button.Image = (Bitmap)defaultImageProperty.GetValue(null, null);
                    break;
                }

            }
        }


        private Button GetEmptyButtonFromPanel(Panel panel)
        {
            var buttons = panel.Controls.OfType<Button>().ToList();
            buttons.Sort((b1, b2) => b1.Name.CompareTo(b2.Name));
            foreach (var button in buttons)
            {
                button.Visible = true;

                if (button?.Image == null)
                {
                    return button;
                }
            }
            return new Button();
        }
        public void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            panel1 = new Panel();
            lblWhite = new Label();
            lblBlack = new Label();
            txtWhite = new RichTextBox();
            txtBlack = new RichTextBox();
            button1 = new Button();
            panel2 = new Panel();
            panelBlack = new Panel();
            button26 = new Button();
            button27 = new Button();
            button28 = new Button();
            button29 = new Button();
            button30 = new Button();
            button31 = new Button();
            button32 = new Button();
            button33 = new Button();
            button25 = new Button();
            button24 = new Button();
            button23 = new Button();
            button22 = new Button();
            button21 = new Button();
            button20 = new Button();
            button19 = new Button();
            button18 = new Button();
            panelWhite = new Panel();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            button7 = new Button();
            button8 = new Button();
            button9 = new Button();
            button10 = new Button();
            button11 = new Button();
            button12 = new Button();
            button13 = new Button();
            button14 = new Button();
            button15 = new Button();
            button16 = new Button();
            button17 = new Button();
            contextMenuStrip1 = new ContextMenuStrip(components);
            panel1.SuspendLayout();
            panelBlack.SuspendLayout();
            panelWhite.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.AutoSize = true;
            panel1.BackColor = Color.Chocolate;
            panel1.Controls.Add(lblWhite);
            panel1.Controls.Add(lblBlack);
            panel1.Controls.Add(txtWhite);
            panel1.Controls.Add(txtBlack);
            panel1.Controls.Add(button1);
            panel1.Location = new Point(-3, 1);
            panel1.Name = "panel1";
            panel1.Size = new Size(1098, 82);
            panel1.TabIndex = 0;
            // 
            // lblWhite
            // 
            lblWhite.AutoSize = true;
            lblWhite.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblWhite.ForeColor = Color.DarkGreen;
            lblWhite.Location = new Point(718, 20);
            lblWhite.Name = "lblWhite";
            lblWhite.Size = new Size(63, 35);
            lblWhite.TabIndex = 4;
            lblWhite.Text = "Test";
            // 
            // lblBlack
            // 
            lblBlack.AutoSize = true;
            lblBlack.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblBlack.ForeColor = Color.DarkGreen;
            lblBlack.Location = new Point(130, 22);
            lblBlack.Name = "lblBlack";
            lblBlack.Size = new Size(63, 35);
            lblBlack.TabIndex = 3;
            lblBlack.Text = "Test";
            // 
            // txtWhite
            // 
            txtWhite.BackColor = Color.Wheat;
            txtWhite.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            txtWhite.Location = new Point(803, 8);
            txtWhite.Name = "txtWhite";
            txtWhite.Size = new Size(71, 57);
            txtWhite.TabIndex = 2;
            txtWhite.Text = "00";
            // 
            // txtBlack
            // 
            txtBlack.BackColor = Color.Wheat;
            txtBlack.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            txtBlack.Location = new Point(253, 11);
            txtBlack.Name = "txtBlack";
            txtBlack.RightToLeft = RightToLeft.No;
            txtBlack.Size = new Size(71, 57);
            txtBlack.TabIndex = 1;
            txtBlack.Text = "00";
            // 
            // button1
            // 
            button1.BackColor = Color.DarkGreen;
            button1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button1.ForeColor = Color.Snow;
            button1.Location = new Point(498, 11);
            button1.Name = "button1";
            button1.Size = new Size(94, 57);
            button1.TabIndex = 0;
            button1.Text = "Reset";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // panel2
            // 
            panel2.AutoSize = true;
            panel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel2.Location = new Point(0, 98);
            panel2.Name = "panel2";
            panel2.Size = new Size(0, 0);
            panel2.TabIndex = 1;
            // 
            // panelBlack
            // 
            panelBlack.BackColor = Color.Chocolate;
            panelBlack.Controls.Add(button26);
            panelBlack.Controls.Add(button27);
            panelBlack.Controls.Add(button28);
            panelBlack.Controls.Add(button29);
            panelBlack.Controls.Add(button30);
            panelBlack.Controls.Add(button31);
            panelBlack.Controls.Add(button32);
            panelBlack.Controls.Add(button33);
            panelBlack.Controls.Add(button25);
            panelBlack.Controls.Add(button24);
            panelBlack.Controls.Add(button23);
            panelBlack.Controls.Add(button22);
            panelBlack.Controls.Add(button21);
            panelBlack.Controls.Add(button20);
            panelBlack.Controls.Add(button19);
            panelBlack.Controls.Add(button18);
            panelBlack.Location = new Point(-3, 89);
            panelBlack.Name = "panelBlack";
            panelBlack.Size = new Size(93, 967);
            panelBlack.TabIndex = 2;
            panelBlack.DragDrop += panelBlack_DragDrop;
            panelBlack.DragEnter += panelBlack_DragEnter;
            // 
            // button26
            // 
            button26.BackColor = Color.Chocolate;
            button26.Location = new Point(11, 861);
            button26.Name = "button26";
            button26.Size = new Size(76, 51);
            button26.TabIndex = 58;
            button26.UseVisualStyleBackColor = false;
            button26.Visible = false;
            // 
            // button27
            // 
            button27.BackColor = Color.Chocolate;
            button27.Location = new Point(11, 804);
            button27.Name = "button27";
            button27.Size = new Size(76, 51);
            button27.TabIndex = 57;
            button27.UseVisualStyleBackColor = false;
            button27.Visible = false;
            // 
            // button28
            // 
            button28.BackColor = Color.Chocolate;
            button28.Location = new Point(11, 747);
            button28.Name = "button28";
            button28.Size = new Size(76, 51);
            button28.TabIndex = 56;
            button28.UseVisualStyleBackColor = false;
            button28.Visible = false;
            // 
            // button29
            // 
            button29.BackColor = Color.Chocolate;
            button29.Location = new Point(11, 690);
            button29.Name = "button29";
            button29.Size = new Size(76, 52);
            button29.TabIndex = 55;
            button29.UseVisualStyleBackColor = false;
            button29.Visible = false;
            // 
            // button30
            // 
            button30.BackColor = Color.Chocolate;
            button30.Location = new Point(11, 634);
            button30.Name = "button30";
            button30.Size = new Size(76, 52);
            button30.TabIndex = 54;
            button30.UseVisualStyleBackColor = false;
            button30.Visible = false;
            // 
            // button31
            // 
            button31.BackColor = Color.Chocolate;
            button31.Location = new Point(11, 577);
            button31.Name = "button31";
            button31.Size = new Size(76, 52);
            button31.TabIndex = 53;
            button31.UseVisualStyleBackColor = false;
            button31.Visible = false;
            // 
            // button32
            // 
            button32.BackColor = Color.Chocolate;
            button32.Location = new Point(11, 520);
            button32.Name = "button32";
            button32.Size = new Size(76, 52);
            button32.TabIndex = 52;
            button32.UseVisualStyleBackColor = false;
            button32.Visible = false;
            // 
            // button33
            // 
            button33.BackColor = Color.Chocolate;
            button33.Location = new Point(11, 463);
            button33.Name = "button33";
            button33.Size = new Size(76, 52);
            button33.TabIndex = 51;
            button33.UseVisualStyleBackColor = false;
            button33.Visible = false;
            // 
            // button25
            // 
            button25.BackColor = Color.Chocolate;
            button25.Location = new Point(11, 404);
            button25.Name = "button25";
            button25.Size = new Size(76, 52);
            button25.TabIndex = 50;
            button25.UseVisualStyleBackColor = false;
            button25.Visible = false;
            // 
            // button24
            // 
            button24.BackColor = Color.Chocolate;
            button24.Location = new Point(11, 347);
            button24.Name = "button24";
            button24.Size = new Size(76, 52);
            button24.TabIndex = 49;
            button24.UseVisualStyleBackColor = false;
            button24.Visible = false;
            // 
            // button23
            // 
            button23.BackColor = Color.Chocolate;
            button23.Location = new Point(11, 290);
            button23.Name = "button23";
            button23.Size = new Size(76, 52);
            button23.TabIndex = 5;
            button23.UseVisualStyleBackColor = false;
            button23.Visible = false;
            // 
            // button22
            // 
            button22.BackColor = Color.Chocolate;
            button22.Location = new Point(11, 233);
            button22.Name = "button22";
            button22.Size = new Size(76, 52);
            button22.TabIndex = 4;
            button22.UseVisualStyleBackColor = false;
            button22.Visible = false;
            // 
            // button21
            // 
            button21.BackColor = Color.Chocolate;
            button21.Location = new Point(11, 177);
            button21.Name = "button21";
            button21.Size = new Size(76, 52);
            button21.TabIndex = 3;
            button21.UseVisualStyleBackColor = false;
            button21.Visible = false;
            // 
            // button20
            // 
            button20.BackColor = Color.Chocolate;
            button20.Location = new Point(11, 120);
            button20.Name = "button20";
            button20.Size = new Size(76, 52);
            button20.TabIndex = 2;
            button20.UseVisualStyleBackColor = false;
            button20.Visible = false;
            // 
            // button19
            // 
            button19.BackColor = Color.Chocolate;
            button19.Location = new Point(11, 63);
            button19.Name = "button19";
            button19.Size = new Size(76, 52);
            button19.TabIndex = 1;
            button19.UseVisualStyleBackColor = false;
            button19.Visible = false;
            // 
            // button18
            // 
            button18.BackColor = Color.Chocolate;
            button18.Location = new Point(11, 6);
            button18.Name = "button18";
            button18.Size = new Size(76, 53);
            button18.TabIndex = 0;
            button18.UseVisualStyleBackColor = false;
            button18.Visible = false;
            // 
            // panelWhite
            // 
            panelWhite.BackColor = Color.Chocolate;
            panelWhite.Controls.Add(button2);
            panelWhite.Controls.Add(button3);
            panelWhite.Controls.Add(button4);
            panelWhite.Controls.Add(button5);
            panelWhite.Controls.Add(button6);
            panelWhite.Controls.Add(button7);
            panelWhite.Controls.Add(button8);
            panelWhite.Controls.Add(button9);
            panelWhite.Controls.Add(button10);
            panelWhite.Controls.Add(button11);
            panelWhite.Controls.Add(button12);
            panelWhite.Controls.Add(button13);
            panelWhite.Controls.Add(button14);
            panelWhite.Controls.Add(button15);
            panelWhite.Controls.Add(button16);
            panelWhite.Controls.Add(button17);
            panelWhite.Location = new Point(1004, 89);
            panelWhite.Name = "panelWhite";
            panelWhite.Size = new Size(91, 967);
            panelWhite.TabIndex = 59;
            panelWhite.DragDrop += panelWhite_DragDrop;
            panelWhite.DragEnter += panelWhite_DragEnter;
            // 
            // button2
            // 
            button2.BackColor = Color.Chocolate;
            button2.Location = new Point(11, 861);
            button2.Name = "button2";
            button2.Size = new Size(76, 51);
            button2.TabIndex = 58;
            button2.UseVisualStyleBackColor = false;
            button2.Visible = false;
            // 
            // button3
            // 
            button3.BackColor = Color.Chocolate;
            button3.Location = new Point(11, 804);
            button3.Name = "button3";
            button3.Size = new Size(76, 51);
            button3.TabIndex = 57;
            button3.UseVisualStyleBackColor = false;
            button3.Visible = false;
            // 
            // button4
            // 
            button4.BackColor = Color.Chocolate;
            button4.Location = new Point(11, 747);
            button4.Name = "button4";
            button4.Size = new Size(76, 51);
            button4.TabIndex = 56;
            button4.UseVisualStyleBackColor = false;
            button4.Visible = false;
            // 
            // button5
            // 
            button5.BackColor = Color.Chocolate;
            button5.Location = new Point(11, 690);
            button5.Name = "button5";
            button5.Size = new Size(76, 51);
            button5.TabIndex = 55;
            button5.UseVisualStyleBackColor = false;
            button5.Visible = false;
            // 
            // button6
            // 
            button6.BackColor = Color.Chocolate;
            button6.Location = new Point(11, 634);
            button6.Name = "button6";
            button6.Size = new Size(76, 51);
            button6.TabIndex = 54;
            button6.UseVisualStyleBackColor = false;
            button6.Visible = false;
            // 
            // button7
            // 
            button7.BackColor = Color.Chocolate;
            button7.Location = new Point(11, 577);
            button7.Name = "button7";
            button7.Size = new Size(76, 51);
            button7.TabIndex = 53;
            button7.UseVisualStyleBackColor = false;
            button7.Visible = false;
            // 
            // button8
            // 
            button8.BackColor = Color.Chocolate;
            button8.Location = new Point(11, 520);
            button8.Name = "button8";
            button8.Size = new Size(76, 51);
            button8.TabIndex = 52;
            button8.UseVisualStyleBackColor = false;
            button8.Visible = false;
            // 
            // button9
            // 
            button9.BackColor = Color.Chocolate;
            button9.Location = new Point(11, 463);
            button9.Name = "button9";
            button9.Size = new Size(76, 51);
            button9.TabIndex = 51;
            button9.UseVisualStyleBackColor = false;
            button9.Visible = false;
            // 
            // button10
            // 
            button10.BackColor = Color.Chocolate;
            button10.Location = new Point(11, 404);
            button10.Name = "button10";
            button10.Size = new Size(76, 51);
            button10.TabIndex = 50;
            button10.UseVisualStyleBackColor = false;
            button10.Visible = false;
            // 
            // button11
            // 
            button11.BackColor = Color.Chocolate;
            button11.Location = new Point(11, 347);
            button11.Name = "button11";
            button11.Size = new Size(76, 51);
            button11.TabIndex = 49;
            button11.UseVisualStyleBackColor = false;
            button11.Visible = false;
            // 
            // button12
            // 
            button12.BackColor = Color.Chocolate;
            button12.Location = new Point(11, 290);
            button12.Name = "button12";
            button12.Size = new Size(76, 51);
            button12.TabIndex = 5;
            button12.UseVisualStyleBackColor = false;
            button12.Visible = false;
            // 
            // button13
            // 
            button13.BackColor = Color.Chocolate;
            button13.Location = new Point(11, 233);
            button13.Name = "button13";
            button13.Size = new Size(76, 51);
            button13.TabIndex = 4;
            button13.UseVisualStyleBackColor = false;
            button13.Visible = false;
            // 
            // button14
            // 
            button14.BackColor = Color.Chocolate;
            button14.Location = new Point(11, 177);
            button14.Name = "button14";
            button14.Size = new Size(76, 51);
            button14.TabIndex = 3;
            button14.UseVisualStyleBackColor = false;
            button14.Visible = false;
            // 
            // button15
            // 
            button15.BackColor = Color.Chocolate;
            button15.Location = new Point(11, 120);
            button15.Name = "button15";
            button15.Size = new Size(76, 51);
            button15.TabIndex = 2;
            button15.UseVisualStyleBackColor = false;
            button15.Visible = false;
            // 
            // button16
            // 
            button16.BackColor = Color.Chocolate;
            button16.Location = new Point(11, 63);
            button16.Name = "button16";
            button16.Size = new Size(76, 51);
            button16.TabIndex = 1;
            button16.UseVisualStyleBackColor = false;
            button16.Visible = false;
            // 
            // button17
            // 
            button17.BackColor = Color.Chocolate;
            button17.Location = new Point(11, 6);
            button17.Name = "button17";
            button17.Size = new Size(76, 51);
            button17.TabIndex = 0;
            button17.UseVisualStyleBackColor = false;
            button17.Visible = false;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // Chess
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.PeachPuff;
            ClientSize = new Size(1094, 1055);
            Controls.Add(panelWhite);
            Controls.Add(panelBlack);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "Chess";
            Text = "Chess";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panelBlack.ResumeLayout(false);
            panelWhite.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private void Panel2_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                e.Effect = DragDropEffects.Move; // Set the drag effect to Move

            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Error in Panel2_DragEnter method: {ex}");
            }
        }
        private bool IsValidPosition(int row, int col)
        {
            //To Check target within board
            return row >= 0 && row < 8 && col >= 0 && col < 8;
        }

        private void DisableSide(string color)
        {
            foreach (Button button in buttons)
            {
                if (button?.Image?.Tag is PieceDetails)
                {
                    button.Enabled = ((PieceDetails)button?.Image?.Tag).Color != color;
                }
            }
            // Reset the dragging state
            isDragging = false;
            draggedButton = null;
            draggedImage = null;
            Cursor.Current = Cursors.Default; // Reset cursor
            if (isAIPlay)
            {
                AIPlays(_AI);
            }
        }

        private void Panel2_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (isDragging)
                {
                    Point clientPoint = panel2.PointToClient(new Point(e.X, e.Y));
                    Button targetButton = GetButtonFromPoint(clientPoint);



                    if (targetButton != null && targetButton != draggedButton && ValidateMove(targetButton))
                    {
                        Point? currentPoint = ((PieceDetails)draggedImage.Tag).CurrentPoint;
                        ((PieceDetails)draggedImage.Tag).CurrentPoint = (Point)targetButton.Tag;
                        if (targetButton?.Image != null)
                        {
                            var pis = targetButton.Image.Tag as PieceDetails;
                            DataObject dataObject = new DataObject();
                            dataObject.SetData(DataFormats.Bitmap, targetButton.Image);
                            if ((pis.Color == "WHITE"))
                            {
                                CalculateScore("WHITE", pis.Value);
                                DragEventArgs dragEnterArgs = new DragEventArgs(dataObject, 0, 0, 0, DragDropEffects.Copy, DragDropEffects.Copy);
                                panelWhite_DragEnter(panelWhite, dragEnterArgs);


                                DragEventArgs dragDropArgs = new DragEventArgs(dataObject, 0, 0, 0, DragDropEffects.Copy, DragDropEffects.Copy);
                                panelWhite_DragDrop(panelWhite, dragDropArgs);
                            }
                            else
                            {
                                CalculateScore("BLACK", pis.Value);
                                DragEventArgs dragEnterArgs = new DragEventArgs(dataObject, 0, 0, 0, DragDropEffects.Copy, DragDropEffects.Copy);
                                panelBlack_DragEnter(panelBlack, dragEnterArgs);


                                DragEventArgs dragDropArgs = new DragEventArgs(dataObject, 0, 0, 0, DragDropEffects.Copy, DragDropEffects.Copy);
                                panelBlack_DragDrop(panelBlack, dragDropArgs);
                            }
                        }
                        targetButton.Image = draggedImage;

                        if (kp.ContainsKey((Point)(draggedImage.Tag as PieceDetails).CurrentPoint))
                        {
                            kp[(Point)(draggedImage.Tag as PieceDetails).CurrentPoint] = (PieceDetails)draggedImage.Tag;
                            kp[currentPoint.Value] = null;
                        }


                        if (draggedImage.Tag is PieceDetails)
                        {
                            isAIPlay = true;
                            lastTarget = targetButton;
                            DisableSide(((PieceDetails)draggedImage.Tag).Color);
                            return;
                        }

                    }
                    else
                    {
                        // Restore the image to the original button if the drop is not valid
                        draggedButton.Image = draggedImage;
                    }

                    //// Reset the dragging state
                    isDragging = false;
                    draggedButton = null;
                    draggedImage = null;
                    Cursor.Current = Cursors.Default; // Reset cursor
                }
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Error in Panel2_DragDrop method: {ex}");
            }
        }

        private Button? GetButtonFromPoint(Point clientPoint)
        {
            try
            {
                foreach (Button btn in panel2.Controls)
                {
                    if (btn.Bounds.Contains(clientPoint))
                    {
                        return btn;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Error in GetButtonFromPoint method: {ex}");
                return null;
            }
        }

        private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (sender is Button button && button.Image != null)
                    {
                        draggedButton = button;
                        draggedImage = button.Image;
                        originalLocation = button.Location;
                        isDragging = true;
                        button.Image = null; // Clear the image from the original button
                        _ = panel2.DoDragDrop(button, DragDropEffects.Move); // Start drag-and-drop operation

                    }
                    startPoint = e.Location;
                }
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Error in Button_MouseDown method: {ex}");
            }
        }

        private void Button_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (isDragging)
                {
                    Cursor.Current = Cursors.Hand; // Change cursor to indicate dragging
                }
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Error in Button_MouseMove method: {ex}");
            }
        }

        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left && isDragging)
                {
                    endPoint = e.Location;
                }
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Error in Button_MouseUp method: {ex}");
            }
        }

        private bool ValidateMove(Button destPlace)
        {
            try
            {
                bool isValid = false;
                int stX = 0;
                int stY = 0;
                int eX = 0;
                int eY = 0;
                bool isImage = false;
                PieceDetails targetDetials = new();
                if (!IsValidPosition(eX, eY))
                {
                    return false;
                }
                if (draggedImage != null)
                {

                    if (destPlace?.Image != null && destPlace?.Image?.Tag != null && destPlace?.Image?.Tag is PieceDetails)
                    {
                        targetDetials = (PieceDetails)destPlace?.Image?.Tag;
                        isImage = true;

                    }
                    if (destPlace?.Tag is Point)
                    {
                        Point tgtPoint = (Point)destPlace.Tag;
                        eX = tgtPoint.X;
                        eY = tgtPoint.Y;
                    }
                    PieceDetails? draggedDetails = draggedImage.Tag as PieceDetails;

                    stX = draggedDetails.CurrentPoint.Value.X;
                    stY = draggedDetails.CurrentPoint.Value.Y;
                    (string primaryColor, string secondaryColor) colors = FindColor(draggedDetails.Name);

                    if (draggedDetails.Name.Contains(nameof(ChessPiece.WHITEPAWN)))
                    {
                        if ((eX > stX && Math.Abs(eX - stX) == 1 && stY == eY && isImage == false) ||
                             (stX == 1 && Math.Abs(eX - stX) == 2 && stY == eY && isImage == false) ||
                             (eX > stX && Math.Abs(eX - stX) == 1 && Math.Abs(stY - eY) == 1 && isImage && targetDetials.Color.Equals("BLACK")))
                        {
                            isValid = true;

                        }
                    }
                    else if (draggedDetails.Name.Contains(nameof(ChessPiece.BLACKPAWN)))
                    {

                        if ((stX > eX && Math.Abs(stX - eX) == 1 && stY == eY && isImage == false) ||
                              (stX == 6 && eX - stX == -2 && stY == eY && isImage == false) ||
                              (stX > eX && Math.Abs(stX - eX) == 1 && Math.Abs(stY - eY) == 1 && isImage && targetDetials.Color.Equals("WHITE")))
                        {
                            isValid = true;

                        }

                    }
                    else if (draggedDetails.Name.Contains(nameof(ChessPiece.BLACKROOK)) || draggedDetails.Name.Contains(nameof(ChessPiece.WHITEROOK)))
                    {


                        return CheckStraightPos(stX, stY, eX, eY) != false
&& ((buttons[((Point)destPlace.Tag).X, ((Point)destPlace.Tag).Y]?.Image != null &&
                            ((PieceDetails)buttons[((Point)destPlace.Tag).X, ((Point)destPlace.Tag).Y]?.Image?.Tag).Color.Equals(colors.primaryColor) && (stX == eX || stY == eY))
                              || (buttons[((Point)destPlace.Tag).X, ((Point)destPlace.Tag).Y]?.Image == null && (stX == eX || stY == eY)));
                    }

                    else if (draggedDetails.Name.Contains(nameof(ChessPiece.BLACKQUEEN)) || draggedDetails.Name.Contains(nameof(ChessPiece.WHITEQUEEN)))
                    {
                        List<(int, int)> diagPoints = GetDiagonalPoints((stX, stY), (eX, eY));

                        return diagPoints?.Any() == true
                            ? ValidateDiagonalMoves(eX, eY, colors, diagPoints)
                            : CheckStraightPos(stX, stY, eX, eY) != false
&& ((buttons[((Point)destPlace.Tag).X, ((Point)destPlace.Tag).Y]?.Image != null &&
                            ((PieceDetails)buttons[((Point)destPlace.Tag).X, ((Point)destPlace.Tag).Y]?.Image?.Tag).Color.Equals(colors.primaryColor) && (stX == eX || stY == eY))
                              || (buttons[((Point)destPlace.Tag).X, ((Point)destPlace.Tag).Y]?.Image == null && (stX == eX || stY == eY)));
                    }

                    else if (draggedDetails.Name.Contains(nameof(ChessPiece.BLACKBISHOP)) || draggedDetails.Name.Contains(nameof(ChessPiece.WHITEBISHOP)))
                    {
                        List<(int, int)> diagPoints = GetDiagonalPoints((stX, stY), (eX, eY));

                        if (diagPoints?.Any() == true)
                        {

                            return ValidateDiagonalMoves(eX, eY, colors, diagPoints);

                        }

                    }
                    else if (draggedDetails.Name.Contains(nameof(ChessPiece.BLACKKNIGHT)) || draggedDetails.Name.Contains(nameof(ChessPiece.WHITEKNIGHT)))
                    {

                        int[,] moveOffsets = new int[,]
                             {
                                  { 2, 1 }, { 2, -1 }, { -2, 1 }, { -2, -1 },
                                  { 1, 2 }, { 1, -2 }, { -1, 2 }, { -1, -2 }
                             };

                        // Check each possible move
                        for (int i = 0; i < moveOffsets.GetLength(0); i++)
                        {
                            int newX = stX + moveOffsets[i, 0];
                            int newY = stY + moveOffsets[i, 1];

                            if (newX == eX && newY == eY && IsValidPosition(newX, newY))
                            {

                                return (buttons[((Point)destPlace.Tag).X, ((Point)destPlace.Tag).Y]?.Image != null &&
                            ((PieceDetails)buttons[((Point)destPlace.Tag).X, ((Point)destPlace.Tag).Y]?.Image?.Tag).Color.Equals(colors.primaryColor)) ||
                            buttons[((Point)destPlace.Tag).X, ((Point)destPlace.Tag).Y]?.Image == null;
                            }
                        }

                        return false;


                    }

                    else if (draggedDetails.Name.Contains(nameof(ChessPiece.WHITEKING)) || draggedDetails.Name.Contains(nameof(ChessPiece.BLACKKING)))
                    {

                        int[,] kingMoves = new int[,]
                            {
                              { -1, -1 }, { -1, 0 }, { -1, 1 },
                              { 0, -1 }, { 0, 1 },
                              { 1, -1 }, { 1, 0 }, { 1, 1 }
                            };
                        (int, int)? king2L = FindKing(colors.primaryColor);
                        int kingRow = king2L.Value.Item1;
                        int kingCol = king2L.Value.Item2;
                        bool isValids = KingValidMove(stX, stY, eX, eY, kingMoves) &&
                                      !IsKingCLose(eX, eY, kingRow, kingCol, kingMoves);


                        if (isValids)
                        {

                            if ((buttons[((Point)destPlace.Tag).X, ((Point)destPlace.Tag).Y]?.Image != null &&
                              ((PieceDetails)buttons[((Point)destPlace.Tag).X, ((Point)destPlace.Tag).Y]?.Image?.Tag).Color.Equals(colors.primaryColor)) ||
                              buttons[((Point)destPlace.Tag).X, ((Point)destPlace.Tag).Y]?.Image == null)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return false;
                        }

                        return false;

                    }
                }

                return isValid;
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Error in ValidateMove method: {ex}");
                return false;
            }
        }

        private bool ValidateDiagonalMoves(int eX, int eY, (string primaryColor, string secondaryColor) colors, List<(int, int)> diagPoints)
        {
            if (diagPoints?.Any() == true)
            {
                for (int i = 1; i <= diagPoints.Count - 1; i++)
                {
                    Image? btnImage = buttons[diagPoints[i].Item1, diagPoints[i].Item2]?.Image;

                    if (buttons[diagPoints[i].Item1, diagPoints[i].Item2]?.Image?.Tag is PieceDetails pieceDetails)
                    {
                        if (btnImage != null && pieceDetails.Color.Equals(colors.primaryColor))
                        {
                            return buttons[diagPoints[i].Item1, diagPoints[i].Item2] == buttons[eX, eY];
                        }
                        else if (btnImage != null && pieceDetails.Color.Equals(colors.secondaryColor))
                        {
                            return false;
                        }

                    }
                    else if (btnImage == null && buttons[diagPoints[i].Item1, diagPoints[i].Item2] == buttons[eX, eY])
                    {
                        return true;
                    }


                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public bool KingValidMove(int currRow, int currCol, int newRow, int newCol, int[,] kingMoves)
        {
            for (int i = 0; i < kingMoves.GetLength(0); i++)
            {
                int newRowCheck = currRow + kingMoves[i, 0];
                int newColCheck = currCol + kingMoves[i, 1];

                if (newRow == newRowCheck && newCol == newColCheck)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsKingCLose(int king1Row, int king1Col, int king2Row, int king2Col, int[,] kingMoves)
        {
            for (int i = 0; i < kingMoves.GetLength(0); i++)
            {
                int checkRow = king1Row + kingMoves[i, 0];
                int checkCol = king1Col + kingMoves[i, 1];

                if (king2Row == checkRow && king2Col == checkCol)
                {
                    return true;
                }
            }

            return king1Row == king2Row && king1Col == king2Col;
        }
        private bool CheckStraightPos(int stX, int stY, int eX, int eY)
        {
            bool isValid = true;
            int max = 0;
            int min = 0;
            int? x1 = null;
            int? y1 = null;

            if (stX != eX)
            {
                max = Math.Max(stX, eX);
                min = Math.Min(stX, eX);
                y1 = eY;

            }
            if (stY != eY)
            {
                max = Math.Max(stY, eY);
                min = Math.Min(stY, eY);
                x1 = eX;

            }

            for (int i = min; i <= max; i++)
            {
                int? x = x1 == null ? i : x1;
                int? y = y1 == null ? i : y1;

                if (buttons[(int)x, (int)y].Image != null && buttons[(int)x, (int)y] != buttons[eX, eY] && (stX == eX || stY == eY))
                {
                    isValid = false;

                }
            }
            return isValid;
        }

        private (string primaryColor, string secondaryColor) FindColor(string piece)
        {
            return piece.Contains("WHITE") ? ("BLACK", "WHITE") : ("WHITE", "BLACK");

        }


        private (int, int)? FindKing(string color)
        {

            foreach (Button button in buttons)
            {
                if (button?.Image?.Tag is PieceDetails)
                {

                    PieceDetails piDetails = (PieceDetails)button.Image.Tag;
                    if (piDetails.Color == color && piDetails.Name.Contains("KING"))
                    {
                        return new(piDetails.CurrentPoint.Value.X, piDetails.CurrentPoint.Value.Y);
                    }
                }

            }
            return null;
        }

        private List<(int, int)> GetDiagonalPoints((int, int) startPoint, (int, int) endPoint)
        {
            int sX = startPoint.Item1;
            int sY = startPoint.Item2;
            int eX = endPoint.Item1;
            int eY = endPoint.Item2;

            List<(int, int)> diagPoints = [];

            if (Math.Abs(eX - sX) != Math.Abs(eY - sY))
            {

                return diagPoints;
            }

            //find (steps direction) slope
            int xSteps = sX < eX ? 1 : -1;
            int ySteps = sY < eY ? 1 : -1;

            int x = sX;
            int y = sY;

            // x steps from start  to end till reaches end point
            while (x != (eX + xSteps) && y != (eY + ySteps))
            {
                diagPoints.Add((x, y));

                x += xSteps;
                y += ySteps;
            }

            return diagPoints;
        }

        private void PriorityMoves(string lastDragged, string targetBtn)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            CreateButtons();
            Resize += new EventHandler(OnFormResize);
            CenterPanel();
        }

        private void panelBlack_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                Bitmap droppedImage = (Bitmap)e.Data.GetData(DataFormats.Bitmap);
                var button = GetEmptyButtonFromPanel(panelBlack);

                button.Image = droppedImage;
                button.Visible = true;
                // button.BackColor = Color.Wheat;
                button.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        private void panelBlack_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void panelWhite_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {

                Bitmap droppedImage = (Bitmap)e.Data.GetData(DataFormats.Bitmap);
                var button = GetEmptyButtonFromPanel(panelWhite);
                button.Image = droppedImage;
                button.Visible = true;
                //  button.BackColor = Color.Wheat;
                button.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        private void panelWhite_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private bool AreAllValues(Dictionary<string, int> dict, int colorPieceCount, int possibleMove)
        {
            int totalCount = 0;
            foreach (var kvp in dict)
            {
                if (kvp.Value != possibleMove)
                {
                    return false;
                }
                totalCount++;
            }

            return totalCount == colorPieceCount;
        }

        private void CalculateScore(string color, int point)
        {
            if(color=="WHITE")
            {
                if(int.TryParse(txtWhite.Text,out var result))
                {
                    txtWhite.Text = Convert.ToString(result + point);
                }               
            }
            else if(color == "BLACK")
            {
                if (int.TryParse(txtBlack.Text, out var result))
                {
                    txtBlack.Text = Convert.ToString(result + point);
                }             
            }
        }
    }
}
