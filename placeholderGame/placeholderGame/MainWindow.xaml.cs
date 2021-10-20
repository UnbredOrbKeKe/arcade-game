using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Media;
using Microsoft.Win32;

using System.Windows.Threading; // add this for the timer

namespace PlaceholderGame
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		DispatcherTimer gameTimer = new DispatcherTimer(DispatcherPriority.Normal);
		bool moveLeft1, moveRight1, moveRight2, moveLeft2;
		List<Rectangle> itemRemover = new List<Rectangle>();

		Random rand = new Random();

		
		int player1Speed = 10;
		int player2Speed = 10;
		int limit = 50;
		int score1 = 0;
		int coins1 = 0;
		int coins2 = 0;
		int score2 = 0;

		Rect player1HitBox;
		Rect player2HitBox;

		private MediaPlayer music = new MediaPlayer();


		List<Rectangle> itemstoremove = new List<Rectangle>();


		private Random Rand = new Random();
		private int EnemySpawnMax = 50;
		private int EnemySpawnCount = 100;
		private const int EnemySpeed = 10;
		


		public MainWindow()
		{
			InitializeComponent();


			gameTimer.Interval = TimeSpan.FromMilliseconds(20);
			gameTimer.Tick += new EventHandler(GameEngine);
			gameTimer.Start();
			MyCanvas.Focus();

			//music.Open(new Uri("pack://siteoforigin:,,,/music/FZero_Mute_City_8bit.mp3"));
			//music.Play();
			
			ImageBrush bg = new ImageBrush();
			bg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/space_background.png"));
			bg.TileMode = TileMode.Tile;
			bg.Viewport = new Rect(0, 0, 1, 1);
			bg.ViewboxUnits = BrushMappingMode.RelativeToBoundingBox;
			MyCanvas.Background = bg;


			ImageBrush player1Image = new ImageBrush();
			player1Image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/playerShip1_orange.png"));
			player1.Fill = player1Image;
			ImageBrush player2Image = new ImageBrush();
			player2Image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/playerShip2_blue.png"));
			player2.Fill = player2Image;

		}


		public void GameEngine(object sender, EventArgs e)
		{
			scoreText1.Content = "Score: " + score1;
			scoreText2.Content = "Score: " + score2;


			if (moveLeft1 == true && Canvas.GetTop(player1) > 0)
			{
				Canvas.SetTop(player1, Canvas.GetTop(player1) - player1Speed);
			}
			if (moveRight1 == true && Canvas.GetTop(player1) < 700)
			{
				Canvas.SetTop(player1, Canvas.GetTop(player1) + player1Speed);
			}

			if (moveLeft2 == true && Canvas.GetTop(player2) > 0)
			{
				Canvas.SetTop(player2, Canvas.GetTop(player2) - player2Speed);
			}
			if (moveRight2 == true && Canvas.GetTop(player2) < 700)
			{
				Canvas.SetTop(player2, Canvas.GetTop(player2) + player2Speed);
			}

			foreach (var x in MyCanvas.Children.OfType<Rectangle>())
			{

				if (x is Rectangle && (string)x.Tag == "bullet1")
				{

					Canvas.SetLeft(x, Canvas.GetLeft(x) + 20);


					Rect bullet = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);


					if (Canvas.GetLeft(x) < 10)
					{

						itemstoremove.Add(x);
					}


					foreach (var y in MyCanvas.Children.OfType<Rectangle>())
					{

						if (y is Rectangle && ((string)y.Tag == "EnemyTop" || (string)y.Tag == "EnemyBottom"))
						{
							Rect enemy = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);

							if (bullet.IntersectsWith(enemy))
							{

								itemstoremove.Add(x);
								itemstoremove.Add(y);
								score1++;
							}
						}


					}
				}
			}


			

				foreach (var x in MyCanvas.Children.OfType<Rectangle>())
				{

					if (x is Rectangle && (string)x.Tag == "bullet2")
					{

						Canvas.SetLeft(x, Canvas.GetLeft(x) - 20);


						Rect bullet = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);


						if (Canvas.GetLeft(x) < 10)
						{

							itemstoremove.Add(x);
						}


						foreach (var y in MyCanvas.Children.OfType<Rectangle>())
						{

							if (y is Rectangle && ((string)y.Tag == "EnemyTop" || (string)y.Tag == "EnemyBottom"))
							{
								Rect enemy = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);

								if (bullet.IntersectsWith(enemy))
								{

									itemstoremove.Add(x);
									itemstoremove.Add(y);
									score2++;
								}
							}


						}
					}

					if ((string)x.Tag == "EnemyTop")
					{
						Canvas.SetTop(x, Canvas.GetTop(x) + EnemySpeed);
						Rect enemy = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

						if (Canvas.GetTop(x) + 20 > 720)
						{
							itemstoremove.Add(x);
						}
					}

					if ((string)x.Tag == "EnemyBottom")
					{
						Canvas.SetTop(x, Canvas.GetTop(x) - EnemySpeed);
						Rect enemy = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

						if (Canvas.GetTop(x) + 20 < 0)
						{
							itemstoremove.Add(x);
						}
					}
				}
				player1HitBox = new Rect(Canvas.GetLeft(player1), Canvas.GetTop(player1), player1.Width, player1.Height);


				foreach (Rectangle r in itemstoremove)
				{
					MyCanvas.Children.Remove(r);
				}

				EnemySpawnCount--;
				if (EnemySpawnCount < 0)
				{
					MakeEnemies();
					EnemySpawnCount = EnemySpawnMax;
				}
			}
		

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Up)
			{
				moveLeft1 = true;
			}
			if (e.Key == Key.Down)
			{
				moveRight1 = true;
			}

			if (e.Key == Key.W)
			{
				moveLeft2 = true;
			}
			if (e.Key == Key.S)
			{
				moveRight2 = true;
			}
		}

		private void OnKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Up)
			{
				moveLeft1 = false;
			}
			if (e.Key == Key.Down)
			{
				moveRight1 = false;
			}

			if (e.Key == Key.W)
			{
				moveLeft2 = false;
			}
			if (e.Key == Key.S)
			{
				moveRight2 = false;
			}

			if (e.Key == Key.RightShift)
			{
				Rectangle newBullet = new Rectangle
				{
					Tag = "bullet2",
					Height = 5,
					Width = 5,
					Fill = Brushes.White,
					Stroke = Brushes.Red
				};

				
				Canvas.SetTop(newBullet, Canvas.GetTop(player1) + player1.Height / 2);
				
				Canvas.SetLeft(newBullet, Canvas.GetLeft(player1) - newBullet.Width);
				
				MyCanvas.Children.Add(newBullet);
			}

			if (e.Key == Key.LeftShift)
			{
				Rectangle newBullet = new Rectangle
				{
					Tag = "bullet1",
					Height = 5,
					Width = 5,
					Fill = Brushes.White,
					Stroke = Brushes.Red
				};


				Canvas.SetTop(newBullet, Canvas.GetTop(player2) + player2.Height / 2);

				Canvas.SetLeft(newBullet, newBullet.Width + Canvas.GetLeft(player2) + player2.Width);

				MyCanvas.Children.Add(newBullet);
			}
		}


		private void MakeEnemies()
        {
			ImageBrush EnemyType = new ImageBrush();
			int EnemyTypeCounter = rand.Next(1, 16);

			switch (EnemyTypeCounter)
            {
				case 1: case 2: case 3: case 4: case 5:
					EnemyType.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/1.png"));
					break;

				case 6: case 7: case 8: case 9:
					EnemyType.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/2.png"));
					break;

				case 10: case 11: case 12:
					EnemyType.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/3.png"));
					break;

				case 13: case 14:
					EnemyType.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/4.png"));
					break;

				case 15:
					EnemyType.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/5.png"));
					break;

			}

			int EnemySpawn = rand.Next(1, 3);
			
			switch (EnemySpawn)
            {
               case 1:
					Rectangle NewEnemyTop = new Rectangle
					{
						Tag = "EnemyTop",
						Height = 80,
						Width = 80,
						Fill = EnemyType
					};

					Canvas.SetTop(NewEnemyTop, -100);
					Canvas.SetLeft(NewEnemyTop, rand.Next(400, 880));
					MyCanvas.Children.Add(NewEnemyTop);
                    break;

				case 2:
					Rectangle NewEnemyBottom = new Rectangle
					{
						Tag = "EnemyBottom",
						Height = 80,
						Width = 80,
						Fill = EnemyType
					};

					Canvas.SetTop(NewEnemyBottom, 820);
					Canvas.SetLeft(NewEnemyBottom, rand.Next(400, 880));
					MyCanvas.Children.Add(NewEnemyBottom);
					break;

            }

			GC.Collect();

        }

	}
}
