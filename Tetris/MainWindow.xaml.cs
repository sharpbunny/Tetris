using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Tetris
{
	public class Board
	{
		private int Rows;
		private int Cols;
		private int Score;
		private int LinesFilled;
		private Tetramino currTetramino;
		private Label[,] BlockControls;

		static private Brush NoBrush = Brushes.Transparent;
		static private Brush SilverBrush = Brushes.Gray;

		public Board(Grid TetrisGrid)
		{
			Rows = TetrisGrid.RowDefinitions.Count;
			Cols = TetrisGrid.ColumnDefinitions.Count;
			Score = 0;
			LinesFilled = 0;

			BlockControls = new Label[Cols, Rows];

			for (int i = 0; i < Cols; i++)
			{
				for (int j = 0; j < Rows; j++)
				{
					BlockControls[i, j] = new Label();
					BlockControls[i, j].Background = NoBrush;
					BlockControls[i, j].BorderBrush = SilverBrush;
					BlockControls[i, j].BorderThickness = new Thickness(1, 1, 1, 1);
					Grid.SetRow(BlockControls[i, j], j);//Affichage du plateau
					Grid.SetColumn(BlockControls[i, j], i);//Affichage du plateau
					TetrisGrid.Children.Add(BlockControls[i, j]);
				}
			}
			currTetramino = new Tetramino();
			currTetraminoDraw();
		}
		public int getScore()
		{
			return Score;
		}
		public int getLines()
		{
			return LinesFilled;
		}
		private void currTetraminoDraw()
		{
			//WO MALEN
			Point Position = currTetramino.getCurrPosition();
			//WAS MALEN
			Point[] Shape = currTetramino.getCurrShape();
			//Farbe
			Brush Color = currTetramino.getCurrColor();
			foreach (Point S in Shape)
			{
				BlockControls[(int)(S.X + Position.X) + ((Cols / 2) - 1),
				(int)(S.Y + Position.Y) + 2].Background = Color;
			}


		}
		private void currTetraminoErase()
		{
			//WO MALEN
			Point position = currTetramino.getCurrPosition();
			//WAS MALEN
			Point[] Shape = currTetramino.getCurrShape();
			foreach (Point S in Shape)
			{
				BlockControls[(int)(S.X + position.X) + ((Cols / 2) - 1),
				(int)(S.Y + position.Y) + 2].Background = NoBrush;
			}

		}
		private void CheckRows()
		{
			bool full;
			for (int i = Rows - 1; i > 0; i--)
			{
				full = true;
				for (int j = 0; j < Cols; j++)
				{
					if (BlockControls[j, i].Background == NoBrush)
					{
						full = false;
					}
				}
				if (full)
				{
					RemoveRow(i);
					Score += 100;
					LinesFilled += 1;
				}
			}
		}
		private void RemoveRow(int row)
		{
			for (int i = row; i > 2; i--)
			{
				for (int j = 0; j < Cols; j++)
				{
					BlockControls[j, i].Background = BlockControls[j, i - 1].Background;
				}
			}
		}

		public void CurrTetraminoMovLeft()
		{
			Point Position = currTetramino.getCurrPosition();
			Point[] Shape = currTetramino.getCurrShape();
			bool move = true;
			currTetraminoErase();
			foreach (Point S in Shape)
			{
				if (((int)(S.X + Position.X) + ((Cols / 2) - 1) - 1) < 0)
				{
					move = false;
				}
				else if (BlockControls[((int)(S.X + Position.X) + ((Cols / 2) - 1) - 1),
				 (int)(S.Y + Position.Y) + 2].Background != NoBrush)
				{
					move = false;
				}
			}
			if (move)
			{
				currTetramino.movLeft();
				currTetraminoDraw();
			}
			else
			{
				currTetraminoDraw();
			}

		}

		public void CurrTetraminoMovRight()
		{
			Point Position = currTetramino.getCurrPosition();
			Point[] Shape = currTetramino.getCurrShape();
			bool move = true;

			currTetraminoErase();

			foreach (Point S in Shape)
			{
				if (((int)(S.X + Position.X) + ((Cols / 2) - 1) + 1) >= Cols)
				{
					move = false;
				}
				else if (BlockControls[((int)(S.X + Position.X) + ((Cols / 2) - 1) + 1),
				 (int)(S.Y + Position.Y) + 2].Background != NoBrush)
				{
					move = false;
				}
			}
			if (move)
			{
				currTetramino.movRight();
				currTetraminoDraw();
			}
			else
			{
				currTetraminoDraw();
			}
		}

		public void CurrTetraminoMovDown()
		{
			Point Position = currTetramino.getCurrPosition();
			Point[] Shape = currTetramino.getCurrShape();
			bool move = true;
			currTetraminoErase();//fait tomber les briques
			foreach (Point S in Shape)
			{
				if (((int)(S.Y + Position.Y) + 2 + 1) >= Rows)
				{
					move = false;
				}
				else if (BlockControls[((int)(S.X + Position.X) + ((Cols / 2) - 1)),
				 (int)(S.Y + Position.Y) + 2 + 1].Background != NoBrush)
				{
					move = false;
				}
			}
			if (move)
			{
				currTetramino.movDown();
				currTetraminoDraw();
			}
			else
			{
				currTetraminoDraw();
				CheckRows();
				currTetramino = new Tetramino();
			}
		}
		public void CurrTetraminoMovRotate()
		{
			Point Position = currTetramino.getCurrPosition();
			Point[] S = new Point[4];
			Point[] Shape = currTetramino.getCurrShape();
			bool move = true;
			Shape.CopyTo(S, 0);
			currTetraminoErase();
			for (int i = 0; i < S.Length; i++)
			{
				double x = S[i].X;
				S[i].X = S[i].Y * -1;
				S[i].Y = x;
				if (((int)((S[i].Y + Position.Y) + 2)) >= Rows)
				{
					move = false;
				}
				else if (((int)(S[i].X + Position.X) + ((Cols / 2) - 1)) < 0)
				{
					move = false;
				}
				else if (((int)(S[i].X + Position.X) + ((Cols / 2) - 1)) >= Rows)
				{
					move = false;
				}
				else if (BlockControls[((int)(S[i].X + Position.X) + ((Cols / 2) - 1)),
						(int)(S[i].Y + Position.Y) + 2].Background != NoBrush)
				{
					move = false;
				}

			}
			if (move)
			{
				currTetramino.movRotate();
				currTetraminoDraw();
			}
			else
			{
				currTetraminoDraw();
			}
		}
	}
	//###############################################################################################
	public class Tetramino
	{
		private Point currPosition;
		private Point[] currShape;
		private Brush currColor;
		private bool rotate;
		public Tetramino()
		{
			currPosition = new Point(0, 0);
			currColor = Brushes.Transparent;
			currShape = setRandomShape();
		}
		public Brush getCurrColor()
		{
			return currColor;
		}
		public Point getCurrPosition()
		{
			return currPosition;
		}
		public Point[] getCurrShape()
		{
			return currShape;
		}
		public void movLeft()
		{
			currPosition.X -= 1;
		}
		public void movRight()
		{
			currPosition.X += 1;
		}
		public void movDown()
		{
			currPosition.Y += 1;
		}
		public void movRotate()
		{
			if (rotate)
				for (int i = 0; i < currShape.Length; i++)
				{
					double x = currShape[i].X;
					currShape[i].X = currShape[i].Y * -1;
					currShape[i].Y = x;
				}
		}
		private Point[] setRandomShape()
		{
			Random rand = new Random();
			switch (rand.Next() % 7)
			{
				case 0:// I
					rotate = true;
					currColor = Brushes.Cyan;
					return new Point[] {
						new Point(0,0),
						new Point(-1,0),
						new Point(1,0),
						new Point(2,0),
					};
				case 1://J
					rotate = true;
					currColor = Brushes.Blue;
					return new Point[] {
						new Point(1,-1),
						new Point(-1,0),
						new Point(0,0),
						new Point(1,0)
					};
				case 2://L
					rotate = true;
					currColor = Brushes.Orange;
					return new Point[] {
						new Point(0,0),
						new Point(-1,0),
						new Point(1,0),
						new Point(1,-1)
					};
				case 3:
					rotate = false;
					currColor = Brushes.Yellow;
					return new Point[] {
						new Point(0,0),
						new Point(0,1),
						new Point(1,0),
						new Point(1,1)
					};
				case 4:
					rotate = true;
					currColor = Brushes.Green;
					return new Point[] {
						new Point(0,0),
						new Point(-1,0),
						new Point(0,-1),
						new Point(1,0)
					};
				case 5:
					rotate = true;
					currColor = Brushes.Purple;
					return new Point[] {
						new Point(0,0),
						new Point(-1,0),
						new Point(0,-1),
						new Point(1,0)
					};
				case 6:
					rotate = true;
					currColor = Brushes.Red;
					return new Point[] {
						new Point(0,0),
						new Point(-1,0),
						new Point(0,1),
						new Point(1,1)
					};

				default:
					return null;
			}
		}
	}
	/// <summary>
	/// Logique d'interaction pour MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		DispatcherTimer Timer;
		Board myBoard;
		public MainWindow()
		{
			InitializeComponent();
		}
		void MainWindow_Initialized(object sender, EventArgs e)
		{
			Timer = new DispatcherTimer();//Surligner en Rouge Pour résoudre:click Droit Resoudre=>using.system.windows.Threading
			Timer.Tick += new EventHandler(GameTick);
			Timer.Interval = new TimeSpan(0, 0, 0, 0, 400);
			GameStart();
		}
		private void GameStart()
		{
			MainGrid.Children.Clear();
			myBoard = new Board(MainGrid);
			Timer.Start();
		}
		void GameTick(object sender, EventArgs e)
		{
			Score.Content = myBoard.getScore().ToString("00000000000");
			Lines.Content = myBoard.getLines().ToString("00000000000");
			myBoard.CurrTetraminoMovDown();
		}
		private void GamePause()
		{
			if (Timer.IsEnabled) Timer.Stop();
			else Timer.Start();
		}
		private void HandleKeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Left:
					if (Timer.IsEnabled) myBoard.CurrTetraminoMovLeft();//Bouger avec la flèche Gauche
					break;
				case Key.Right:
					if (Timer.IsEnabled) myBoard.CurrTetraminoMovRight();//Bouger avec la flèche Droite
					break;
				case Key.Down:
					if (Timer.IsEnabled) myBoard.CurrTetraminoMovDown();//Bouger avec la flèche Droite
					break;
				case Key.Up:
					if (Timer.IsEnabled) myBoard.CurrTetraminoMovRotate();//Bouger avec la flèche Droite
					break;
				case Key.F2:
					GameStart();
					break;
				case Key.F3:
					GamePause();
					break;
				default:
					break;
			}
		}
	}
}
