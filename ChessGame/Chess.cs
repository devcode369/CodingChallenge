using System;
using System.Drawing;
using System.Net;
using System.Runtime.CompilerServices;
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
            InitializeComponent();
            CreateButtons();
            this.Resize += new EventHandler(OnFormResize);
            CenterPanel();
        }

        private void CreateButtons()
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
                    buttons[row, col].UseVisualStyleBackColor = true;
                    buttons[row, col].FlatStyle = FlatStyle.Popup;

                    buttons[row, col].MouseDown += new MouseEventHandler(Button_MouseDown);
                    buttons[row, col].MouseMove += new MouseEventHandler(Button_MouseMove);
                    buttons[row, col].MouseUp += new MouseEventHandler(Button_MouseUp);

                    //  buttons[row, col].Text = $"({row},{col})";
                    buttons[row, col].BackgroundImageLayout = ImageLayout.Stretch;
                    if (buttons[row, col] == buttons[0, 0] || buttons[row, col] == buttons[0, 7])
                    {
                        buttons[row, col].Image = ChessGame.Properties.Resources.WElephant;

                    }
                    if (buttons[row, col] == buttons[0, 1] || buttons[row, col] == buttons[0, 6])
                    {
                        buttons[row, col].Image = ChessGame.Properties.Resources.WHorse;
                    }
                    if (buttons[row, col] == buttons[0, 2] || buttons[row, col] == buttons[0, 5])
                    {
                        buttons[row, col].Image = ChessGame.Properties.Resources.WMand;
                    }
                    if (buttons[row, col] == buttons[0, 3])
                    {
                        buttons[row, col].Image = ChessGame.Properties.Resources.WQueen;
                    }
                    if (buttons[row, col] == buttons[0, 4])
                    {
                        buttons[row, col].Image = ChessGame.Properties.Resources.Wking;
                    }

                    if (row == 1 && col >= 0 && col <= 7)
                    {
                        buttons[row, col].Image = ChessGame.Properties.Resources.WSepoy;
                    }
                    if (row == 6 && col >= 0 && col <= 7)
                    {
                        buttons[row, col].Image = ChessGame.Properties.Resources.BSepoy;
                        buttons[row, col].Tag = new Tuple<Point, string>((Point)(buttons[row, col].Tag), Convert.ToString(row + col) + nameof(ChessPiece.WPAWN));
                    }

                    if (buttons[row, col] == buttons[7, 0] || buttons[row, col] == buttons[7, 7])
                    {
                        buttons[row, col].Image = ChessGame.Properties.Resources.BElephant;
                    }
                    if (buttons[row, col] == buttons[7, 1] || buttons[row, col] == buttons[7, 6])
                    {
                        buttons[row, col].Image = ChessGame.Properties.Resources.BHorse;
                    }
                    if (buttons[row, col] == buttons[7, 2] || buttons[row, col] == buttons[7, 5])
                    {
                        buttons[row, col].Image = ChessGame.Properties.Resources.BMand;
                    }
                    if (buttons[row, col] == buttons[7, 3])
                    {
                        buttons[row, col].Image = ChessGame.Properties.Resources.BQueen;
                    }
                    if (buttons[row, col] == buttons[7, 4])
                    {
                        buttons[row, col].Image = ChessGame.Properties.Resources.BKing;
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

                    panel2.Controls.Add(buttons[row, col]);
                }
            }

            // Event handlers for panel2 to handle drag-and-drop operations
            panel2.DragEnter += Panel2_DragEnter;
            panel2.DragDrop += Panel2_DragDrop;
        }

        private void OnFormResize(object sender, EventArgs e)
        {
            CenterPanel();
        }

        private void CenterPanel()
        {
            int x = (this.ClientSize.Width - panel2.Width) / 2;
            int y = (this.ClientSize.Height - panel2.Height) / 2;
            panel2.Location = new Point(x, y);
        }

        private void InitializeComponent()
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

        private void Panel2_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move; // Set the drag effect to Move
        }

        private void Panel2_DragDrop(object sender, DragEventArgs e)
        {
            if (isDragging)
            {
                Point clientPoint = panel2.PointToClient(new Point(e.X, e.Y));
                Button targetButton = GetButtonFromPoint(clientPoint);

                if (targetButton != null && targetButton.Image == null && targetButton != draggedButton && ValidateMove(targetButton))
                {
                    //var isVaid = ValidateMove(targetButton);
                    //if (!isVaid)
                    //{

                    //}
                    targetButton.Image = draggedImage; // Set the image to the target button
                    targetButton.Tag = new Tuple<Point, string>((Point)targetButton.Tag, ((Tuple<Point,string>)draggedButton.Tag).Item2);
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

        private Button GetButtonFromPoint(Point clientPoint)
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

        private void Button_MouseDown(object sender, MouseEventArgs e)
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

        private void Button_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Cursor.Current = Cursors.Hand; // Change cursor to indicate dragging
            }
        }

        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && isDragging)
            {
                endPoint = e.Location;
                // Perform the drop operation in Panel2_DragDrop
            }
        }


        private bool ValidateMove(Button destPlace)
        {
            bool isValid = false;
            if (draggedImage != null)
            {
                var val = GetTagValues(draggedButton);



                if (val.Item2.Contains(nameof(ChessPiece.WPAWN)))
                {

                    if (UP == GetDirection())
                    {
                        int x = val.Item1.X;

                        var destCoordinates = (Point)destPlace.Tag;
                        int x1 = destCoordinates.X;
                        if (Math.Abs(x) - Math.Abs(x1) == 1)
                        {
                            return !isValid;
                        }
                        else
                        {
                            return isValid;
                        }
                    }
                    else
                    {
                        return isValid;
                    }

                }
            }
            return isValid;

        }


        private Tuple<Point, string?> GetTagValues(Button button)
        {

            return (Tuple<Point, string?>)button.Tag;

        }
        public string GetDirection()
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
                    //Left
                    return LEFT;
                }
            }
            else
            {
                if (deltaX > 0)
                {
                    //Down
                    return DOWN;
                }
                else
                {
                    //up
                    return UP;
                }
            }
        }
    }



    public enum ChessPiece
    {
        NONE,
        WKING,
        WQUEEN,
        WELEPHANT,
        WHORSE,
        WBISHOP,
        WPAWN,
        BKING,
        BQUEEN,
        BELEPHANT,
        BHORSE,
        BBISHOP,
        BPAWN
    }

    public enum Direction
    {
        NONE,
        UP,
        DOWN,
        RIGHT,
        LEFT
    }
}
