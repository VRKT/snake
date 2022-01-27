using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        public Form1()
        {
            InitializeComponent();

            new Settings();

            gameTimer.Interval = 1000 / Settings.Speed;
            //gameTimer.Tick += UpdateScreen;
            gameTimer.Tick += navigateToFood;
            gameTimer.Start();

            StartGame();
        }

        private void StartGame()
        {
            lblGameOver.Visible = false;

            new Settings();

            Snake.Clear();
            Circle head = new Circle { X = 10, Y = 5 };
            Snake.Add(head);


            lblScore.Text = Settings.Score.ToString();
            GenerateFood();

        }

        private void GenerateFood()
        {
            int maxXPos = pbCanvas.Size.Width / Settings.Width;
            int maxYPos = pbCanvas.Size.Height / Settings.Height;

            Random random = new Random();
            food = new Circle { X = random.Next(0, maxXPos), Y = random.Next(0, maxYPos) };
        }

        private void navigateToFood(object sender, EventArgs e)
        {
            int snakeX = Snake[0].X;
            int snakeY = Snake[0].Y;
            int foodX = food.X;
            int foodY = food.Y;
            int maxXPos = pbCanvas.Size.Width / Settings.Width;
            int maxYPos = pbCanvas.Size.Height / Settings.Height;

            if (Settings.GameOver)
            {
                if (Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {

                if (snakeX != foodX)
                {
                    do
                    {
                        if (snakeX < foodX)
                        {
                            if (Settings.direction != Direction.Left)
                            {
                                Settings.direction = checkNext(Direction.Right);
                                MovePlayer();
                            }
                        }
                        else
                        {
                            if (Settings.direction != Direction.Right)
                            {
                                Settings.direction = checkNext(Direction.Left);
                                MovePlayer();
                            }
                        }
                        snakeX = Snake[0].X;
                        pbCanvas.Invalidate();
                    } while (snakeX != foodX);
                }
                if (snakeY != foodY)
                {
                    do
                    {
                        if (snakeY < foodY)
                        {
                            if (Settings.direction != Direction.Up)
                            {
                                Settings.direction = checkNext(Direction.Down);
                                MovePlayer();
                            }
                        }
                        else
                        {
                            if (Settings.direction != Direction.Down)
                            {
                                Settings.direction = checkNext(Direction.Up);
                                MovePlayer();
                            }
                        }
                        snakeY = Snake[0].Y;
                        pbCanvas.Invalidate();
                    } while (snakeY != foodY);
                }
            }
        }
        private Direction checkNext(Direction d)
        {
            int x = Snake[0].X;
            int y = Snake[0].Y;

            switch (d)
            {
                case Direction.Up:
                    if (isClear(d))
                    {
                        return d;
                    }
                    else if (isClear(Direction.Left))
                    {
                        return Direction.Left;
                    }
                    else if(isClear(Direction.Right))
                    {
                        return Direction.Right;
                    }
                    break;
                case Direction.Down:
                    if (isClear(d))
                    {
                        return d;
                    }
                    else if (isClear(Direction.Left))
                    {
                        return Direction.Left;
                    }
                    else if (isClear(Direction.Right))
                    {
                        return Direction.Right;
                    }
                    break;
                case Direction.Left:
                    if (isClear(d))
                    {
                        return d;
                    }
                    else if (isClear(Direction.Up))
                    {
                        return Direction.Up;
                    }
                    else if (isClear(Direction.Down))
                    {
                        return Direction.Down;
                    }
                    break;
                case Direction.Right:
                    if (isClear(d))
                    {
                        return d;
                    }
                    else if (isClear(Direction.Up))
                    {
                        return Direction.Up;
                    }
                    else if (isClear(Direction.Down))
                    {
                        return Direction.Down;
                    }
                    break;
                default:
                    break;
            }
            return d;
        }
        private bool isClear(Direction d)
        {
            int x = Snake[0].X;
            int y = Snake[0].Y;
            int maxXPos = pbCanvas.Size.Width / Settings.Width;
            int maxYPos = pbCanvas.Size.Height / Settings.Height;

            switch (d)
            {
                case Direction.Up:
                    if (y != 0)
                    {
                        y--;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case Direction.Down:
                    if (y != maxYPos)
                    {
                        y++;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case Direction.Left:
                    if (x != 0)
                    {
                        x--;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case Direction.Right:
                    if (x != maxXPos)
                    {
                        x++;
                    }
                    else
                    {
                        return false;
                    }
                    break;
                default:
                    break;
            }


            for (int j = 1; j < Snake.Count; j++)
            {
                if (x == Snake[j].X && y == Snake[j].Y)
                {
                    return false;
                }
            }
            return true;
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            if (Settings.GameOver)
            {
                if (Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (Input.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                    Settings.direction = Direction.Right;
                else if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                    Settings.direction = Direction.Left;
                else if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                    Settings.direction = Direction.Up;
                else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                    Settings.direction = Direction.Down;

                MovePlayer();
            }

            pbCanvas.Invalidate();

        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (!Settings.GameOver)
            {
                for (int i = 0; i < Snake.Count; i++)
                {
                    Brush snakeColour;
                    if (i == 0)
                        snakeColour = Brushes.Black;     //Kopf
                    else
                        snakeColour = Brushes.Green;    //schwanz

                    //Snake
                    canvas.FillEllipse(snakeColour,
                        new Rectangle(Snake[i].X * Settings.Width,
                                      Snake[i].Y * Settings.Height,
                                      Settings.Width, Settings.Height));

                    //Essen
                    canvas.FillEllipse(Brushes.Red,
                        new Rectangle(food.X * Settings.Width,
                             food.Y * Settings.Height, Settings.Width, Settings.Height));

                }
            }
            else
            {
                string gameOver = "Game over \nPunkte: " + Settings.Score + "\nEnter zum neustart";
                lblGameOver.Text = gameOver;
                lblGameOver.Visible = true;
            }
        }


        private void MovePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                //bewegen
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                    }


                    //maximum X and Y Pos
                    int maxXPos = pbCanvas.Size.Width / Settings.Width;
                    int maxYPos = pbCanvas.Size.Height / Settings.Height;

                    //wandkollision
                    if (Snake[i].X < 0 || Snake[i].Y < 0 || Snake[i].X >= maxXPos || Snake[i].Y >= maxYPos)
                    {
                        Die();
                    }


                    //schwanzkollision
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }

                    //essen essen
                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        Eat();
                    }

                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }

        private void Eat()
        {
            //schwanz verlängern
            Circle circle = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(circle);

            //punkte aktualisieren
            Settings.Score += Settings.Points;
            lblScore.Text = Settings.Score.ToString();

            GenerateFood();
        }

        private void Die()
        {
            Settings.GameOver = true;
        }
    }
}
