using System.Collections.Generic;

namespace Tetris
{
    public class Piece
    {
        public int x = 3, y = -1, index = 0, type = 1;

        public List<int[,]> rotations;

        // Attempt to move piece down by 1
        public bool moveDown(int[,] grid)
        {
            y += 1;

            if (collidesWith(grid))
            {
                y -= 1;
                project(grid);
                return false;
            }

            return true;
        }

        // Attempt to move piece left or right by 1
        public void move(int[,] grid, int dir)
        {
            x += dir;

            if (collidesWith(grid))
                x -= dir;
        }

        // Check if the piece and grid collide
        public bool collidesWith(int[,] grid)
        {
            int[,] subGrid = rotations[index];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (subGrid[j, i] != 0 && (x + i < 0 || x + i > 9 || y + j < 0 || y + j > 19 || grid[x + i, y + j] != 0))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // Attempt to rotate the piece
        public bool rotate(int[,] grid)
        {
            int tmpIndex = index;

            index -= 1;

            if (index < 0)
                index = rotations.Count - 1;

            if (!collidesWith(grid))                
                return true;

            // Restore index
            index = tmpIndex;
            return false;
        }

        // Project the piece onto the grid
        public void project(int[,] grid)
        {
            int[,] subGrid = rotations[index];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (subGrid[j, i] != 0 && (x + i < 10 && y + j < 20 && grid[x + i, y + j] == 0))
                    {
                        grid[x + i, y + j] = type;
                    }
                }
            }
        }
    }

    public class O : Piece
    {
        public O()
        {
            type = 4;

            rotations = new List<int[,]>
            {
                new int[,] {
                    { 0, 0, 0, 0 },
                    { 0, 1, 1, 0 },
                    { 0, 1, 1, 0 },
                    { 0, 0, 0, 0 }
                }
            };
        }

    }

    public class I : Piece
    {
        public I()
        {
            type = 1;

            rotations = new List<int[,]>
            {
                new int[,] {
                    { 0, 0, 0, 0 },
                    { 1, 1, 1, 1 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 }
                },
                new int[,] {
                    { 0, 0, 1, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 1, 0 }
                }
            };
        }

    }

    public class S : Piece
    {
        public S()
        {
            type = 5;

            rotations = new List<int[,]>
            {
                new int[,] {
                    { 0, 0, 0, 0 },
                    { 0, 0, 1, 1 },
                    { 0, 1, 1, 0 },
                    { 0, 0, 0, 0 }
                },
                new int[,] {
                    { 0, 0, 1, 0 },
                    { 0, 0, 1, 1 },
                    { 0, 0, 0, 1 },
                    { 0, 0, 0, 0 }
                }
            };
        }

    }

    public class Z : Piece
    {
        public Z()
        {
            type = 7;

            rotations = new List<int[,]>
            {
                new int[,] {
                    { 0, 0, 0, 0 },
                    { 0, 1, 1, 0 },
                    { 0, 0, 1, 1 },
                    { 0, 0, 0, 0 }
                },
                new int[,] {
                    { 0, 0, 0, 1 },
                    { 0, 0, 1, 1 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 0 }
                }
            };
        }

    }

    public class L : Piece
    {
        public L()
        {
            type = 3;

            rotations = new List<int[,]>
            {
                new int[,] {
                    { 0, 0, 0, 0 },
                    { 0, 1, 1, 1 },
                    { 0, 1, 0, 0 },
                    { 0, 0, 0, 0 }
                },
                new int[,] {
                    { 0, 0, 1, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 1, 1 },
                    { 0, 0, 0, 0 }
                },
                new int[,] {
                    { 0, 0, 0, 1 },
                    { 0, 1, 1, 1 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 }
                },
                new int[,] {
                    { 0, 1, 1, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 0 }
                }
            };
        }

    }

    public class J : Piece
    {
        public J()
        {
            type = 2;

            rotations = new List<int[,]>
            {
                new int[,] {
                    { 0, 0, 0, 0 },
                    { 0, 1, 1, 1 },
                    { 0, 0, 0, 1 },
                    { 0, 0, 0, 0 }
                },
                new int[,] {
                    { 0, 0, 1, 1 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 0 }
                },
                new int[,] {
                    { 0, 1, 0, 0 },
                    { 0, 1, 1, 1 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 }
                },
                new int[,] {
                    { 0, 0, 1, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 1, 1, 0 },
                    { 0, 0, 0, 0 }
                }
            };
        }

    }

    public class T : Piece
    {
        public T()
        {
            type = 6;

            rotations = new List<int[,]>
            {
                new int[,] {
                    { 0, 0, 0, 0 },
                    { 0, 1, 1, 1 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 0 }
                },
                new int[,] {
                    { 0, 0, 1, 0 },
                    { 0, 0, 1, 1 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 0 }
                },
                new int[,] {
                    { 0, 0, 1, 0 },
                    { 0, 1, 1, 1 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 }
                },
                new int[,] {
                    { 0, 0, 1, 0 },
                    { 0, 1, 1, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 0, 0 }
                }
            };
        }

    }

}
