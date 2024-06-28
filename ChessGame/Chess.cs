using System;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Windows.Forms;

namespace ChessGame
{
    public partial class Chess : Form
    {
        private Panel panel1;
        private Panel panel2;
        private Button[,] buttons;
        private int buttonSize = 100;
        private Button draggedButton;
        private Image draggedImage;
        private Point originalLocation;
        private bool isDragging = false;
        private Point startPoint;
        private Point endPoint;

        private const string RIGHT = "RIGHT";
        private const string LEFT = "LEFT";
        private const string UP = "UP";
        private const string DOWN = "DOWN";

        public Chess()
        {
            try
            {
                InitializeComponent();
                CreateButtons();
                this.Resize += new EventHandler(OnFormResize);
                CenterPanel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in Chess constructor: {ex.ToString()}");
            }
        }

        private void CreateButtons()
        {
            try
            {
                panel2.Controls.Clear();
                buttons = new Button[8, 8];
                panel2.Size = new Size(8 * buttonSize, 8 * buttonSize);
                panel2.AllowDrop = true; // Enable dropping on panel2
                var color1 = Color.Wheat;
                var color2 = Color.SaddleBrown;

                for (int row = 0; row < 8; row++)
                {
                    for (int col = 0; col < 8; col++)
                    {
                        buttons[row, col] = new Button();
                        buttons[row, col].Size = new Size(buttonSize, buttonSize);
                        buttons[row, col].Location = new Point(buttonSize * col, buttonSize * row);
                        buttons[row, col].Tag = new Point(row, col);
                        // buttons[row, col].Tag = new PieceDetails();
                        buttons[row, col].UseVisualStyleBackColor = true;
                        buttons[row, col].FlatStyle = FlatStyle.Popup;

                        buttons[row, col].MouseDown += new MouseEventHandler(Button_MouseDown);
                        buttons[row, col].MouseMove += new MouseEventHandler(Button_MouseMove);
                        buttons[row, col].MouseUp += new MouseEventHandler(Button_MouseUp);

                        buttons[row, col].BackgroundImageLayout = ImageLayout.Stretch;
                        if (buttons[row, col] == buttons[0, 0] || buttons[row, col] == buttons[0, 7])
                        {
                            buttons[row, col].Image = ChessGame.Properties.Resources.WElephant;

                            // buttons[row, col].Tag = new Tuple<Point, string>((Point)(buttons[row, col].Tag), Convert.ToString(row + col) + nameof(ChessPiece.WHITEELEPHANT));
                            buttons[row, col].Image.Tag = new PieceDetails { Color = "WHITE", CurrentPoint = new Point(row, col), Name = nameof(ChessPiece.WHITEELEPHANT) };
                        }
                        if (buttons[row, col] == buttons[0, 1] || buttons[row, col] == buttons[0, 6])
                        {
                            buttons[row, col].Image = ChessGame.Properties.Resources.WHorse;
                            //  buttons[row, col].Tag = new Tuple<Point, string>((Point)(buttons[row, col].Tag), Convert.ToString(row + col) + nameof(ChessPiece.WHITEHORSE));
                            buttons[row, col].Image.Tag = new PieceDetails { Color = "WHITE", CurrentPoint = new Point(row, col), Name = nameof(ChessPiece.WHITEHORSE) };
                        }
                        if (buttons[row, col] == buttons[0, 2] || buttons[row, col] == buttons[0, 5])
                        {
                            buttons[row, col].Image = ChessGame.Properties.Resources.WMand;
                            //  buttons[row, col].Tag = new Tuple<Point, string>((Point)(buttons[row, col].Tag), Convert.ToString(row + col) + nameof(ChessPiece.WHITEBISHOP));
                            buttons[row, col].Image.Tag = new PieceDetails { Color = "WHITE", CurrentPoint = new Point(row, col), Name = nameof(ChessPiece.WHITEBISHOP) };
                        }
                        if (buttons[row, col] == buttons[0, 3])
                        {
                            buttons[row, col].Image = ChessGame.Properties.Resources.WQueen;
                            //  buttons[row, col].Tag = new Tuple<Point, string>((Point)(buttons[row, col].Tag), Convert.ToString(row + col) + nameof(ChessPiece.WHITEQUEEN));
                            buttons[row, col].Image.Tag = new PieceDetails { Color = "WHITE", CurrentPoint = new Point(row, col), Name = nameof(ChessPiece.WHITEQUEEN) };
                        }
                        if (buttons[row, col] == buttons[0, 4])
                        {
                            buttons[row, col].Image = ChessGame.Properties.Resources.Wking;
                            //  buttons[row, col].Tag = new Tuple<Point, string>((Point)(buttons[row, col].Tag), Convert.ToString(row + col) + nameof(ChessPiece.WHITEKING));
                            buttons[row, col].Image.Tag = new PieceDetails { Color = "WHITE", CurrentPoint = new Point(row, col), Name = nameof(ChessPiece.WHITEKING) };
                        }

                        if (row == 1 && col >= 0 && col <= 7)
                        {
                            buttons[row, col].Image = ChessGame.Properties.Resources.WSepoy;
                            //  buttons[row, col].Tag = new Tuple<Point, string>((Point)(buttons[row, col].Tag), Convert.ToString(row + col) + nameof(ChessPiece.WHITEPAWN));
                            buttons[row, col].Image.Tag = new PieceDetails { Color = "WHITE", CurrentPoint = new Point(row, col), Name = nameof(ChessPiece.WHITEPAWN) };
                        }
                        if (row == 6 && col >= 0 && col <= 7)
                        {
                            buttons[row, col].Image = ChessGame.Properties.Resources.BSepoy;
                            // buttons[row, col].Tag = new Tuple<Point, string>((Point)(buttons[row, col].Tag), Convert.ToString(row + col) + nameof(ChessPiece.BLACKPAWN));
                            buttons[row, col].Image.Tag = new PieceDetails { Color = "BLACK", CurrentPoint = new Point(row, col), Name = nameof(ChessPiece.BLACKPAWN) };
                        }

                        if (buttons[row, col] == buttons[7, 0] || buttons[row, col] == buttons[7, 7])
                        {
                            buttons[row, col].Image = ChessGame.Properties.Resources.BElephant;
                            //  buttons[row, col].Tag = new Tuple<Point, string>((Point)(buttons[row, col].Tag), Convert.ToString(row + col) + nameof(ChessPiece.BLACKELEPHANT));
                            buttons[row, col].Image.Tag = new PieceDetails { Color = "BLACK", CurrentPoint = new Point(row, col), Name = nameof(ChessPiece.BLACKELEPHANT) };
                        }
                        if (buttons[row, col] == buttons[7, 1] || buttons[row, col] == buttons[7, 6])
                        {
                            buttons[row, col].Image = ChessGame.Properties.Resources.BHorse;
                            // buttons[row, col].Tag = new Tuple<Point, string>((Point)(buttons[row, col].Tag), Convert.ToString(row + col) + nameof(ChessPiece.BLACKHORSE));
                            buttons[row, col].Image.Tag = new PieceDetails { Color = "BLACK", CurrentPoint = new Point(row, col), Name = nameof(ChessPiece.BLACKHORSE) };
                        }
                        if (buttons[row, col] == buttons[7, 2] || buttons[row, col] == buttons[7, 5])
                        {
                            buttons[row, col].Image = ChessGame.Properties.Resources.BMand;
                            //  buttons[row, col].Tag = new Tuple<Point, string>((Point)(buttons[row, col].Tag), Convert.ToString(row + col) + nameof(ChessPiece.BLACKBISHOP));
                            buttons[row, col].Image.Tag = new PieceDetails { Color = "BLACK", CurrentPoint = new Point(row, col), Name = nameof(ChessPiece.BLACKBISHOP) };
                        }
                        if (buttons[row, col] == buttons[7, 3])
                        {
                            buttons[row, col].Image = ChessGame.Properties.Resources.BQueen;
                            //   buttons[row, col].Tag = new Tuple<Point, string>((Point)(buttons[row, col].Tag), Convert.ToString(row + col) + nameof(ChessPiece.BLACKQUEEN));
                            buttons[row, col].Image.Tag = new PieceDetails { Color = "BLACK", CurrentPoint = new Point(row, col), Name = nameof(ChessPiece.BLACKQUEEN) };
                        }
                        if (buttons[row, col] == buttons[7, 4])
                        {
                            buttons[row, col].Image = ChessGame.Properties.Resources.BKing;
                            // buttons[row, col].Tag = new Tuple<Point, string>((Point)(buttons[row, col].Tag), Convert.ToString(row + col) + nameof(ChessPiece.BLACKKING));
                            buttons[row, col].Image.Tag = new PieceDetails { Color = "BLACK", CurrentPoint = new Point(row, col), Name = nameof(ChessPiece.BLACKKING) };
                        }

                        // Alternate colors
                        if ((row + col) % 2 == 0)
                        {
                            buttons[row, col].BackColor = color1;
                        }
                        else
                        {
                            buttons[row, col].BackColor = color2;
                        }
                        buttons[row, col].Tag = new Point(row, col);
                        panel2.Controls.Add(buttons[row, col]);
                    }
                }

                // Event handlers for panel2 to handle drag-and-drop operations
                panel2.DragEnter += Panel2_DragEnter;
                panel2.DragDrop += Panel2_DragDrop;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in CreateButtons method: {ex.ToString()}");
            }
        }

        private void OnFormResize(object sender, EventArgs e)
        {
            try
            {
                CenterPanel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in OnFormResize method: {ex.ToString()}");
            }
        }

        private void CenterPanel()
        {
            try
            {
                int x = (this.ClientSize.Width - panel2.Width) / 2;
                int y = (this.ClientSize.Height - panel2.Height) / 2;
                panel2.Location = new Point(x, y);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in CenterPanel method: {ex.ToString()}");
            }
        }

        private void InitializeComponent()
        {
            try
            {
                panel1 = new Panel();
                panel2 = new Panel();
                this.button1 = new Button();
                panel1.SuspendLayout();
                SuspendLayout();
                // 
                // panel1
                // 
                panel1.AutoSize = true;
                panel1.Controls.Add(this.button1);
                panel1.Location = new Point(1, 1);
                panel1.Name = "panel1";
                panel1.Size = new Size(982, 94);
                panel1.TabIndex = 0;
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
                // button1
                // 
                this.button1.Location = new Point(433, 33);
                this.button1.Name = "button1";
                this.button1.Size = new Size(94, 29);
                this.button1.TabIndex = 0;
                this.button1.Text = "button1";
                this.button1.UseVisualStyleBackColor = true;
                // 
                // Chess
                // 
                AutoScaleDimensions = new SizeF(8F, 20F);
                AutoScaleMode = AutoScaleMode.Font;
                ClientSize = new Size(982, 953);
                Controls.Add(panel2);
                Controls.Add(panel1);
                Name = "Chess";
                Text = "Chess";
                panel1.ResumeLayout(false);
                ResumeLayout(false);
                PerformLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in InitializeComponent method: {ex.ToString()}");
            }
        }

        private void Panel2_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                e.Effect = DragDropEffects.Move; // Set the drag effect to Move
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in Panel2_DragEnter method: {ex.ToString()}");
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

                    //if (targetButton.Tag is not Tuple<Point, string>)
                    //{
                    //    targetButton.Tag = new Tuple<Point, string>((Point)targetButton.Tag, ((Tuple<Point, string>)draggedButton.Tag).Item2);
                    //}
                    if (targetButton != null && targetButton != draggedButton && ValidateMove(targetButton))
                    {

                        ((PieceDetails)draggedImage.Tag).CurrentPoint = (Point)targetButton.Tag;
                        targetButton.Image = draggedImage;
                        //targetButton.Image.Tag = draggedButton.Image.Tag;


                        //if (targetButton.Tag is Tuple<Point, string>)
                        //{
                        //    var vlas = (Tuple<Point, string>)targetButton.Tag;

                        //    targetButton.Tag = new Tuple<Point, string>(vlas.Item1, ((Tuple<Point, string>)draggedButton.Tag).Item2);

                        //}
                        //else
                        //{
                        //    targetButton.Tag = new Tuple<Point, string>((Point)targetButton.Tag, ((Tuple<Point, string>)draggedButton.Tag).Item2);
                        //}

                    }
                    else
                    {
                        // Restore the image to the original button if the drop is not valid
                        draggedButton.Image = draggedImage;
                    }

                    // Reset the dragging state
                    isDragging = false;
                    draggedButton = null;
                    draggedImage = null;
                    Cursor.Current = Cursors.Default; // Reset cursor
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in Panel2_DragDrop method: {ex.ToString()}");
            }
        }

        private Button GetButtonFromPoint(Point clientPoint)
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
                MessageBox.Show($"Error in GetButtonFromPoint method: {ex.ToString()}");
                return null;
            }
        }

        private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    Button button = sender as Button;
                    if (button != null && button.Image != null)
                    {
                        draggedButton = button;
                        draggedImage = button.Image;
                        originalLocation = button.Location;
                        isDragging = true;
                        button.Image = null; // Clear the image from the original button
                        panel2.DoDragDrop(button, DragDropEffects.Move); // Start drag-and-drop operation
                    }
                    startPoint = e.Location;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in Button_MouseDown method: {ex.ToString()}");
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
                MessageBox.Show($"Error in Button_MouseMove method: {ex.ToString()}");
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
                MessageBox.Show($"Error in Button_MouseUp method: {ex.ToString()}");
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
                PieceDetails targetDetials = new PieceDetails();

                if (draggedImage != null)
                {

                    if (destPlace?.Image != null && destPlace?.Image?.Tag != null && destPlace?.Image?.Tag is PieceDetails)
                    {
                        targetDetials = (PieceDetails)destPlace?.Image?.Tag;
                        isImage = true;

                    }
                    if (destPlace?.Tag is Point)
                    {
                        var tgtPoint = (Point)destPlace.Tag;
                        eX = tgtPoint.X;
                        eY = tgtPoint.Y;
                    }
                    var draggedDetails = draggedImage.Tag as PieceDetails;

                    stX = draggedDetails.CurrentPoint.Value.X;
                    stY = draggedDetails.CurrentPoint.Value.Y;


                    if (draggedDetails.Name.Equals(nameof(ChessPiece.WHITEPAWN)))
                    {
                        if (((eX > stX && Math.Abs(eX - stX) == 1) && stY == eY && isImage == false) ||
                             (stX == 1 && Math.Abs(eX - stX) == 2 && stY == eY && isImage == false) ||
                             ((eX > stX && Math.Abs(eX - stX) == 1) && Math.Abs(stY - eY) == 1) && isImage && targetDetials.Color.Equals("BLACK"))
                        {
                            isValid = true;

                        }
                    }
                    else if (draggedDetails.Name.Equals(nameof(ChessPiece.BLACKPAWN)))
                    {

                        if (((stX > eX && Math.Abs(stX - eX) == 1) && stY == eY && isImage == false) ||
                              (stX == 6 && eX - stX == -2 && stY == eY && isImage == false) ||
                              ((stX > eX && Math.Abs(stX - eX) == 1) && Math.Abs(stY - eY) == 1) && isImage && targetDetials.Color.Equals("WHITE"))
                        {
                            isValid = true;

                        }

                    }


                    if (draggedDetails.Name.Equals(nameof(ChessPiece.BLACKELEPHANT)) || draggedDetails.Name.Equals(nameof(ChessPiece.WHITEELEPHANT)))
                    {
                        int max = 0;
                        int min = 0;
                        int cons = 0;
                        bool isXVar = true;

                        int? x1 = null;
                        int? y1 = null;

                        if (stX != eX)
                        {
                            max = Math.Max(stX, eX);
                            min = Math.Min(stX, eX);
                            y1 = eY;
                            isXVar = false;
                        }
                        if (stY != eY)
                        {
                            max = Math.Max(stY, eY);
                            min = Math.Min(stY, eY);
                            cons = eX;
                            x1 = eX;

                        }

                        for (int i = min; i <= max; i++)
                        {
                            int? x = x1 == null ? i : x1;
                            int? y = y1 == null ? i : y1;

                            if (buttons[(int)x, (int)y].Image != null && buttons[(int)x, (int)y] != buttons[eX, eY] && (stX == eX || stY == eY))
                            {
                                return false;

                            }
                        }

                        if (draggedDetails.Name.Equals(nameof(ChessPiece.BLACKELEPHANT)))
                        {
                            if (
                               (buttons[((Point)destPlace.Tag).X, ((Point)destPlace.Tag).Y]?.Image != null &&
                                ((PieceDetails)buttons[((Point)destPlace.Tag).X, ((Point)destPlace.Tag).Y]?.Image?.Tag).Color.Equals("WHITE") && (stX == eX || stY == eY))
                                  || (buttons[((Point)destPlace.Tag).X, ((Point)destPlace.Tag).Y]?.Image == null && (stX == eX || stY == eY))
                                )
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        if (draggedDetails.Name.Equals(nameof(ChessPiece.WHITEELEPHANT)))
                        {
                            if (
                               (buttons[((Point)destPlace.Tag).X, ((Point)destPlace.Tag).Y]?.Image != null &&
                                ((PieceDetails)buttons[((Point)destPlace.Tag).X, ((Point)destPlace.Tag).Y]?.Image?.Tag).Color.Equals("BLACK") && (stX == eX || stY == eY))
                                  || (buttons[((Point)destPlace.Tag).X, ((Point)destPlace.Tag).Y]?.Image == null && (stX == eX || stY == eY))
                                )
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                      
                    }

                }

                return isValid;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in ValidateMove method: {ex.ToString()}");
                return false;
            }
        }

        private Tuple<Point, string?> GetTagValues(Button button)
        {
            try
            {
                return (Tuple<Point, string?>)button.Tag;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in GetTagValues method: {ex.ToString()}");
                return null;
            }
        }



        public string GetDirection()
        {
            try
            {
                int deltaX = endPoint.X - startPoint.X;
                int deltaY = endPoint.Y - startPoint.Y;

                if (Math.Abs(deltaX) > Math.Abs(deltaY))
                {
                    if (deltaX > 0)
                    {
                        return RIGHT;
                    }
                    else
                    {
                        return LEFT;
                    }
                }
                else
                {
                    if (deltaY > 0)
                    {
                        return DOWN;
                    }
                    else
                    {
                        return UP;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in GetDirection method: {ex.ToString()}");
                return null;
            }
        }
    }

    public enum ChessPiece
    {
        NONE,
        WHITEKING,
        WHITEQUEEN,
        WHITEELEPHANT,
        WHITEHORSE,
        WHITEBISHOP,
        WHITEPAWN,
        BLACKKING,
        BLACKQUEEN,
        BLACKELEPHANT,
        BLACKHORSE,
        BLACKBISHOP,
        BLACKPAWN
    }

    public enum Direction
    {
        NONE,
        UP,
        DOWN,
        RIGHT,
        LEFT
    }



    public class Board
    {
        public string? Direction { get; set; }
    }

    public class PieceDetails
    {
        public PieceDetails()
        {
            LinkedPoint = new LinkedList<string>();

        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }
        public Point? CurrentPoint { get; set; }
        public Image? Img { get; set; }
        public LinkedList<string> LinkedPoint { get; set; }

    }
}
