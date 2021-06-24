using System;
using System.Drawing;
using System.Windows.Forms;

namespace Chess
{
    public partial class Form1 : Form
    {
        class Per
        {
            public static Image chessSprites;

            public static int[,] map = new int[8, 8]
            {
            {15, 14, 13, 12, 11, 13, 14, 15},
            {16, 16, 16, 16, 16, 16, 16, 16},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {26, 26, 26, 26, 26, 26, 26, 26},
            {25, 24, 23, 22, 21, 23, 24, 25},
            };

            public static Button[,] butts = new Button[8, 8];

            public static int currPlayer;

            public static Button prevButton;

            public static bool isMoving = false;
        }
        public Form1()
        {
            InitializeComponent();

            Per.chessSprites = new Bitmap("D:\\Chess\\chess.png");

            Init();
        }

        public void Init()
        {
            Per.currPlayer = 1;
            CreateMap();
        }

        public void CreateMap()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Per.butts[i, j] = new Button();

                    Button butt = new Button();
                    butt.Size = new Size(50, 50);
                    butt.Location = new Point(j * 50, i * 50);

                    switch (Per.map[i, j] / 10)
                    {
                        case 1:
                            Image part = new Bitmap(50, 50);
                            Graphics g = Graphics.FromImage(part);
                            g.DrawImage(Per.chessSprites, new Rectangle(0, 0, 50, 50), 0 + 150 * (Per.map[i, j] % 10 - 1), 0, 150, 150, GraphicsUnit.Pixel);
                            butt.BackgroundImage = part;
                            break;

                        case 2:
                            Image part2 = new Bitmap(50, 50);
                            Graphics g1 = Graphics.FromImage(part2);
                            g1.DrawImage(Per.chessSprites, new Rectangle(0, 0, 50, 50), 0 + 150 * (Per.map[i, j] % 10 - 1), 150, 150, 150, GraphicsUnit.Pixel);
                            butt.BackgroundImage = part2;
                            break;
                    }

                    butt.BackColor = Color.White;
                    butt.Click += new EventHandler(OnFigurePress);
                    Controls.Add(butt);

                    Per.butts[i, j] = butt;
                }
            }
        }
        

        class AllButtons
        {
            public static void DeactivateAllButtons()
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Per.butts[i, j].Enabled = false;
                    }
                }
            }

            public static void ActivateAllButtons()
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Per.butts[i, j].Enabled = true;
                    }
                }
            }

        }
        public static void OnFigurePress(object sender, EventArgs e)
        {

            if (Per.prevButton != null)
                Per.prevButton.BackColor = Color.White;

            Button pressButton = sender as Button;

            if (Per.map[pressButton.Location.Y / 50, pressButton.Location.X / 50] != 0 && Per.map[pressButton.Location.Y / 50, pressButton.Location.X / 50] / 10 == Per.currPlayer)
            {
                Other.CloseSteps();
                pressButton.BackColor = Color.Red;
                AllButtons.DeactivateAllButtons();
                pressButton.Enabled = true;
                Show.ShowSteps(pressButton.Location.Y / 50, pressButton.Location.X / 50, Per.map[pressButton.Location.Y / 50, pressButton.Location.X / 50]);

                if (Per.isMoving)
                {
                    Other.CloseSteps();
                    pressButton.BackColor = Color.White;
                    AllButtons.ActivateAllButtons();
                    Per.isMoving = false;
                }
                else
                    Per.isMoving = true;

            }
            else
            {
                if (Per.isMoving)
                {
                    Per.map[pressButton.Location.Y / 50, pressButton.Location.X / 50] = Per.map[Per.prevButton.Location.Y / 50, Per.prevButton.Location.X / 50];
                    Per.map[Per.prevButton.Location.Y / 50, Per.prevButton.Location.X / 50] = 0;
                    pressButton.BackgroundImage = Per.prevButton.BackgroundImage;
                    Per.prevButton.BackgroundImage = null;
                    Per.isMoving = false;
                    Other.CloseSteps();
                    AllButtons.ActivateAllButtons();
                    Player.SwitchPlayer();
                }
            }

            Per.prevButton = pressButton;
        }

        class Show
        {
            public static void ShowSteps(int IcurrFigure, int JcurrFigure, int currFigure)
            {
                switch (currFigure % 10)
                {
                    case 6:
                        Mov.Pawn(IcurrFigure, JcurrFigure);
                        break;

                    case 5:
                        Mov.ShowVerticalHorizontal(IcurrFigure, JcurrFigure);
                        break;

                    case 3:
                        Mov.ShowDiagonal(IcurrFigure, JcurrFigure);
                        break;

                    case 2:
                        Mov.ShowVerticalHorizontal(IcurrFigure, JcurrFigure);
                        Mov.ShowDiagonal(IcurrFigure, JcurrFigure);
                        break;

                    case 1:
                        Mov.ShowVerticalHorizontal(IcurrFigure, JcurrFigure, true);
                        Mov.ShowDiagonal(IcurrFigure, JcurrFigure, true);
                        break;

                    case 4:
                        Mov.ShowHorseSteps(IcurrFigure, JcurrFigure);
                        break;

                }
            }
        }
        class Mov
        {
            public static void Pawn(int IcurrFigure, int JcurrFigure)
            {
                int dir = Per.currPlayer == 1 ? 1 : -1;

                if (Other.InsideBorder(IcurrFigure + 2 * dir, JcurrFigure))
                {
                    if (Per.currPlayer == 1 && IcurrFigure == 1 && Per.map[IcurrFigure + 1 * dir, JcurrFigure] == 0 && Per.map[IcurrFigure + 2 * dir, JcurrFigure] == 0)
                    {
                        Per.butts[IcurrFigure + 1 * dir, JcurrFigure].BackColor = Color.Yellow;
                        Per.butts[IcurrFigure + 1 * dir, JcurrFigure].Enabled = true;

                        Per.butts[IcurrFigure + 2 * dir, JcurrFigure].BackColor = Color.Yellow;
                        Per.butts[IcurrFigure + 2 * dir, JcurrFigure].Enabled = true;
                    }
                    if (Per.currPlayer == 2 && IcurrFigure == 6 && Per.map[IcurrFigure + 1 * dir, JcurrFigure] == 0 && Per.map[IcurrFigure + 2 * dir, JcurrFigure] == 0)
                    {
                        Per.butts[IcurrFigure + 1 * dir, JcurrFigure].BackColor = Color.Yellow;
                        Per.butts[IcurrFigure + 1 * dir, JcurrFigure].Enabled = true;

                        Per.butts[IcurrFigure + 2 * dir, JcurrFigure].BackColor = Color.Yellow;
                        Per.butts[IcurrFigure + 2 * dir, JcurrFigure].Enabled = true;
                    }
                }

                if (Other.InsideBorder(IcurrFigure + 1 * dir, JcurrFigure))
                {
                    if (Per.map[IcurrFigure + 1 * dir, JcurrFigure] == 0)
                    {
                        Per.butts[IcurrFigure + 1 * dir, JcurrFigure].BackColor = Color.Yellow;
                        Per.butts[IcurrFigure + 1 * dir, JcurrFigure].Enabled = true;
                    }
                }

                if (Other.InsideBorder(IcurrFigure + 1 * dir, JcurrFigure + 1))
                {
                    if (Per.map[IcurrFigure + 1 * dir, JcurrFigure + 1] != 0 && Per.map[IcurrFigure + 1 * dir, JcurrFigure + 1] / 10 != Per.currPlayer)
                    {
                        Per.butts[IcurrFigure + 1 * dir, JcurrFigure + 1].BackColor = Color.Yellow;
                        Per.butts[IcurrFigure + 1 * dir, JcurrFigure + 1].Enabled = true;
                    }
                }

                if (Other.InsideBorder(IcurrFigure + 1 * dir, JcurrFigure - 1))
                {
                    if (Per.map[IcurrFigure + 1 * dir, JcurrFigure - 1] != 0 && Per.map[IcurrFigure + 1 * dir, JcurrFigure - 1] / 10 != Per.currPlayer)
                    {
                        Per.butts[IcurrFigure + 1 * dir, JcurrFigure - 1].BackColor = Color.Yellow;
                        Per.butts[IcurrFigure + 1 * dir, JcurrFigure - 1].Enabled = true;
                    }
                }
            }

            public static void ShowHorseSteps(int IcurrFigure, int JcurrFigure)
            {
                if (Other.InsideBorder(IcurrFigure - 2, JcurrFigure + 1))
                {
                    Other.DeterminePath(IcurrFigure - 2, JcurrFigure + 1);
                }
                if (Other.InsideBorder(IcurrFigure - 2, JcurrFigure - 1))
                {
                    Other.DeterminePath(IcurrFigure - 2, JcurrFigure - 1);
                }
                if (Other.InsideBorder(IcurrFigure + 2, JcurrFigure + 1))
                {
                    Other.DeterminePath(IcurrFigure + 2, JcurrFigure + 1);
                }
                if (Other.InsideBorder(IcurrFigure + 2, JcurrFigure - 1))
                {
                    Other.DeterminePath(IcurrFigure + 2, JcurrFigure - 1);
                }
                if (Other.InsideBorder(IcurrFigure - 1, JcurrFigure + 2))
                {
                    Other.DeterminePath(IcurrFigure - 1, JcurrFigure + 2);
                }
                if (Other.InsideBorder(IcurrFigure + 1, JcurrFigure + 2))
                {
                    Other.DeterminePath(IcurrFigure + 1, JcurrFigure + 2);
                }
                if (Other.InsideBorder(IcurrFigure - 1, JcurrFigure - 2))
                {
                    Other.DeterminePath(IcurrFigure - 1, JcurrFigure - 2);
                }
                if (Other.InsideBorder(IcurrFigure + 1, JcurrFigure - 2))
                {
                    Other.DeterminePath(IcurrFigure + 1, JcurrFigure - 2);
                }
            }

            public static void ShowDiagonal(int IcurrFigure, int JcurrFigure, bool isOneStep = false)
            {
                int j = JcurrFigure + 1;
                for (int i = IcurrFigure - 1; i >= 0; i--)
                {
                    if (Other.InsideBorder(i, j))
                    {
                        if (!Other.DeterminePath(i, j))
                            break;
                    }

                    if (j < 7)
                        j++;
                    else break;

                    if (isOneStep)
                        break;
                }

                j = JcurrFigure - 1;
                for (int i = IcurrFigure - 1; i >= 0; i--)
                {
                    if (Other.InsideBorder(i, j))
                    {
                        if (!Other.DeterminePath(i, j))
                            break;
                    }

                    if (j > 0)
                        j--;
                    else break;

                    if (isOneStep)
                        break;
                }

                j = JcurrFigure - 1;
                for (int i = IcurrFigure + 1; i < 8; i++)
                {
                    if (Other.InsideBorder(i, j))
                    {
                        if (!Other.DeterminePath(i, j))
                            break;
                    }

                    if (j > 0)
                        j--;
                    else break;

                    if (isOneStep)
                        break;
                }

                j = JcurrFigure + 1;
                for (int i = IcurrFigure + 1; i < 8; i++)
                {
                    if (Other.InsideBorder(i, j))
                    {
                        if (!Other.DeterminePath(i, j))
                            break;
                    }

                    if (j < 7)
                        j++;
                    else break;

                    if (isOneStep)
                        break;
                }

            }

            public static void ShowVerticalHorizontal(int IcurrFigure, int JcurrFigure, bool isOneStep = false)
            {
                for (int i = IcurrFigure + 1; i < 8; i++)
                {
                    if (Other.InsideBorder(i, JcurrFigure))
                    {
                        if (!Other.DeterminePath(i, JcurrFigure))
                            break;
                    }
                    if (isOneStep)
                        break;
                }
                for (int i = IcurrFigure - 1; i >= 0; i--)
                {
                    if (Other.InsideBorder(i, JcurrFigure))
                    {
                        if (!Other.DeterminePath(i, JcurrFigure))
                            break;
                    }
                    if (isOneStep)
                        break;
                }

                for (int j = JcurrFigure + 1; j < 8; j++)
                {
                    if (Other.InsideBorder(IcurrFigure, j))
                    {
                        if (!Other.DeterminePath(IcurrFigure, j))
                            break;
                    }
                    if (isOneStep)
                        break;
                }
                for (int j = JcurrFigure - 1; j >= 0; j--)
                {
                    if (Other.InsideBorder(IcurrFigure, j))
                    {
                        if (!Other.DeterminePath(IcurrFigure, j))
                            break;
                    }
                    if (isOneStep)
                        break;
                }
            }
        }
        class Other
        {
            public static bool DeterminePath(int IcurrFigurre, int j)
            {
                if (Per.map[IcurrFigurre, j] == 0)
                {
                    Per.butts[IcurrFigurre, j].BackColor = Color.Yellow;
                    Per.butts[IcurrFigurre, j].Enabled = true;
                }
                else
                {
                    if (Per.map[IcurrFigurre, j] / 10 != Per.currPlayer)
                    {
                        Per.butts[IcurrFigurre, j].BackColor = Color.Yellow;
                        Per.butts[IcurrFigurre, j].Enabled = true;
                    }
                    return false;
                }
                return true;
            }

            public static bool InsideBorder(int ti, int tj)
            {
                if (ti > 7 || tj > 7 || ti < 0 || tj < 0)
                    return false;
                return true;
            }

            public static void CloseSteps()
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Per.butts[i, j].BackColor = Color.White;
                    }
                }
            }
        }
        class Player
        {
            public static void SwitchPlayer()
            {
                Per.currPlayer = Per.currPlayer == 1 ? 2 : 1;
            }
        }
    }
}