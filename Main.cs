using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Tetris
{
    public partial class Main : Form
    {
        // The display area
        PictureBox Display = new PictureBox();

        Random rng = new Random();

        // Grid settings
        int gridSize = 32;
        int gridPadding = 20;

        // Game settings
        int score = 0;
        bool paused = false;
        bool game_over = false;
        int[,] grid = new int[10, 20];

        int game_ticks = 0;

        Piece piece;

        public Main()
        {
            InitializeComponent();

            // Dock the PictureBox to the form and set its background to white.
            Display.Dock = DockStyle.Fill;
            Display.BackColor = Color.White;

            // Connect the Paint event of the PictureBox to the event handler method.
            Display.Paint += new PaintEventHandler(this.Draw);

            // Add the PictureBox control to the Form.
            this.Controls.Add(Display);

            // Begin gameplay
            GameLoop();
        }

        // Handle keyboard input
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!game_over)
            {
                SoundPlayer player = new SoundPlayer();                

                if (keyData == Keys.Escape)
                {
                    paused = !paused;
                    return true;
                }
                if (keyData == Keys.Right && piece != null)
                {
                    piece.move(grid, 1);
                    return true;
                }
                if (keyData == Keys.Left && piece != null)
                {
                    piece.move(grid, -1);
                    return true;
                }
                if (keyData == Keys.Up && piece != null)
                {
                    if (piece.rotate(grid))
                        player.SoundLocation = "../../sfx/SFX_PieceRotateLR.wav";
                    else
                        player.SoundLocation = "../../sfx/SFX_PieceRotateFail.wav";

                    player.Play();

                    return true;
                }
                if (keyData == Keys.Down && piece != null)
                {
                    game_ticks = 3;
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        async void GameLoop()
        {
            game_ticks += 1;

            // Cycle from 0 through 3
            if (game_ticks > 3)
                game_ticks = 0;

            // Don't apply gravity every frame
            if (!paused && !game_over && game_ticks == 0)
                GameAction();

            // Update display
            Display.Refresh();

            // Wait for next tick
            await Task.Delay(50);
            GameLoop();
        }

        void GameAction()
        {
            SoundPlayer player = new SoundPlayer();

            // Clear rows
            for (int y = 0; y < 20; y++)
            {
                bool cleared = true;
                for (int x = 0; x < 10; x++)
                {
                    if (grid[x, y] == 0)
                    {
                        cleared = false;
                        break;
                    }
                }
                if (cleared)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        grid[x, y] = 0;
                        // Shift above tiles down
                        for (int y2 = y; y2 >= 0; y2--)
                        {
                            if (grid[x, y2] != 0 && grid[x, y2 + 1] == 0)
                            {
                                grid[x, y2 + 1] = grid[x, y2];
                                grid[x, y2] = 0;
                            }
                        }
                    }
                    score += 10;

                    player.SoundLocation = "../../sfx/SFX_SpecialLineClearSingle.wav";
                    player.Play();
                }
            }

            // Drop piece
            if (piece == null || !piece.moveDown(grid))
            {
                switch (rng.Next(7))
                {
                    case 0: piece = new O(); break;
                    case 1: piece = new I(); break;
                    case 2: piece = new S(); break;
                    case 3: piece = new Z(); break;
                    case 4: piece = new L(); break;
                    case 5: piece = new J(); break;
                    case 6: piece = new T(); break;
                }
                player.SoundLocation = "../../sfx/SFX_PieceTouchDown.wav";
                player.Play();

                // If the new piece immediately has a collision, it's game over
                if (piece.collidesWith(grid))
                    game_over = true;
            }
        }

        void Draw(object sender, PaintEventArgs e)
        {
            // Create a local version of the graphics object for the PictureBox.
            Graphics g = e.Graphics;

            Rectangle background = new Rectangle(0, 0, this.Width, this.Height);
            Rectangle board = new Rectangle(gridPadding, gridPadding, gridSize * 10, gridSize * 20);
            Rectangle boardBorder = new Rectangle(gridPadding - 2, gridPadding - 2, gridSize * 10 + 4, gridSize * 20 + 4);

            g.FillRectangle(Brushes.Black, background);
            g.FillRectangle(Brushes.Black, board);

            g.DrawRectangle(Pens.Gray, boardBorder);

            // Draw gridlines
            for (int i = 0; i <= 20; i++)
            {
                // Vertical lines
                if (i <= 10)
                    g.DrawLine(Pens.DimGray, new Point(gridPadding + gridSize * i, gridPadding), new Point(gridPadding + gridSize * i, gridPadding + gridSize * 20));
                // Horizontal lines
                g.DrawLine(Pens.DimGray, new Point(gridPadding, gridPadding + gridSize * i), new Point(gridPadding + gridSize * 10, gridPadding + gridSize * i));
            }

            // Display placed pieces
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    // Don't redraw blank squares
                    if (grid[x, y] != 0)
                    {
                        Rectangle square = new Rectangle(gridPadding + gridSize * x, gridPadding + gridSize * y, gridSize, gridSize);
                        Brush color = getColor(grid[x, y]);

                        if (color != null)
                        {
                            g.FillRectangle(color, square);
                            g.DrawRectangle(Pens.Gray, square);
                        }
                    }
                }
            }

            // Display active piece
            if (piece != null)
            {
                int[,] subGrid = piece.rotations[piece.index];

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (subGrid[j, i] != 0)
                        {
                            Rectangle square = new Rectangle(gridPadding + gridSize * (piece.x + i), gridPadding + gridSize * (piece.y + j), gridSize, gridSize);
                            Brush color = getColor(piece.type);

                            if (color != null)
                            {
                                g.FillRectangle(color, square);
                                g.DrawRectangle(Pens.Gray, square);
                            }
                        }
                    }
                }
            }

            // Display score
            g.DrawString("Score: " + score, new Font("Arial", 10), Brushes.White, new Point(30, 675));
        }

        private Brush getColor(int v)
        {
            switch (v)
            {
                case 1: return Brushes.Cyan;
                case 2: return Brushes.DodgerBlue;
                case 3: return Brushes.Orange;
                case 4: return Brushes.Yellow;
                case 5: return Brushes.Green; 
                case 6: return Brushes.Violet;
                case 7: return Brushes.Red;
            }
            return null;
        }
    }
}
