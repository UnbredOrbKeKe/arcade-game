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
using System.Diagnostics;
using System.Windows.Threading; // add this for the timer
using System.Data.SqlClient;
using System.Data;

namespace placeholderGame
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		DispatcherTimer gameTimer = new DispatcherTimer(DispatcherPriority.Normal);
		DispatcherTimer dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
		bool moveLeft1, moveRight1, moveRight2, moveLeft2;
		List<Rectangle> itemRemover = new List<Rectangle>();

		Random rand = new Random();

		


		int player1Speed = 12;
		int player2Speed = 12;
		public int score1 = 0;
		public int score2 = 0;

		int Player1ShootDelay = 250;
		int Player2ShootDelay = 250;
		int PlayerDisableShootTime = 5000;

		int PowerUpTime = 10000;

		bool Player1CanShoot = true;
		bool Player2CanShoot = true;

		private MediaPlayer music = new MediaPlayer();


		List<Rectangle> itemstoremove = new List<Rectangle>();


		private Random Rand = new Random();
		private int EnemySpawnMax = 50;
		private int EnemySpawnCount = 100;
		private const int EnemySpeed = 10;
		private int BulletSpeed = 25;

		private int increment = 120;

		//public static string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"G:\\arcade project\\arcade-game\\placeholderGame\\placeholderGame\\Data\\Database1.mdf\";Integrated Security=True";
		public static string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Dennis\\Documents\\GitHub\\arcade-game\\placeholderGame\\placeholderGame\\Data\\Database1.mdf;Integrated Security=True;";
		public MainWindow()
		{
			InitializeComponent();
			exit.IsEnabled = false;
			exit.Visibility = Visibility.Hidden;
			Reset.IsEnabled = false;
			Reset.Visibility = Visibility.Hidden;
			ReturnToMenu.IsEnabled = false;
			ReturnToMenu.Visibility = Visibility.Hidden;

            Player1Name.Content = placeholderGame.EnterName.player1Name;
            Player2Name.Content = placeholderGame.EnterName.player2Name;

            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
			gameTimer.Tick += new EventHandler(GameEngine);
			gameTimer.Start();
			MyCanvas.Focus();

			dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
			dispatcherTimer.Tick += Dt_Tick;
			dispatcherTimer.Start();

			music.Open(new Uri("pack://siteoforigin:,,,/music/FZero_Mute_City_8bit.mp3"));
			music.Play();

			ImageBrush bg = new ImageBrush();
			bg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/space_background.png"));
			bg.TileMode = TileMode.Tile;
			bg.Viewport = new Rect(0, 0, 1, 1);
			bg.ViewboxUnits = BrushMappingMode.RelativeToBoundingBox;
			MyCanvas.Background = bg;


			ImageBrush player2Image = new ImageBrush();
			player2Image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/playerShip1_orange.png"));
			player2.Fill = player2Image;
			ImageBrush player1Image = new ImageBrush();
			player1Image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/playerShip2_blue.png"));
			player1.Fill = player1Image;

			ImageBrush PowerUp1Image = new ImageBrush();
			PowerUp1Image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/FastFire.png"));
			PowerUp1.Fill = PowerUp1Image;
			PowerUp1.Visibility = Visibility.Hidden;
			PowerUp2.Fill = PowerUp1Image;
			PowerUp2.Visibility = Visibility.Hidden;
		}
		

		#region MusicMute_button
		private void muteMusic(object sender, RoutedEventArgs e)
		{
			if (musicMute.IsChecked == true)
			{
				music.Pause();
			}
			else
			{
				music.Play();
			}
		}
		#endregion

		#region Timer
		private void Dt_Tick(object sender, EventArgs e)
		{
			if(Pause == false)
			{ 
				increment--;
				timer.Content = "timer: " + increment.ToString();
				if (increment == 0)
				{
					dispatcherTimer.Stop();
					music.Stop();
					gameTimer.Stop();
					gameover.Content = "GAME OVER";

					if (score1 > score2)
					{
						Player1Wins.Content = "PLAYER 1 WINS";
					}

					else if (score1 < score2)
					{
						Player2Wins.Content = "PLAYER 2 WINS";
					}

					else
					{
						Player1Wins.Content = "IT'S A DRAW!";
						Player2Wins.Content = "IT'S A DRAW!";
					}

					pause.Visibility = Visibility.Hidden;
					musicMute.Visibility = Visibility.Hidden;
					GameOver();
					SetHighScores();
				}
			}
		}
        #endregion

        #region setHighScores
        private void SetHighScores()
		{
			

			string query1 = "INSERT INTO [Highscores] ([PlayerName],[Score]) VALUES ('" + EnterName.player1Name + "','" + score1 + "')";
			string query2 = "INSERT INTO [Highscores] ([PlayerName],[Score]) VALUES ('" + EnterName.player2Name + "','" + score2 + "')";

			SqlConnection connection = new SqlConnection(connectionString);

			SqlCommand command = new SqlCommand();

			try
			{
				//add player1 score
				command.CommandText = query1;
				command.CommandType = CommandType.Text;
				command.Connection = connection;
				connection.Open();
				command.ExecuteNonQuery();
				connection.Close();
				//add player2 score
				command.CommandText = query2;
				command.CommandType = CommandType.Text;
				command.Connection = connection;
				connection.Open();
				command.ExecuteNonQuery();
				connection.Close();

			}
			catch (Exception e)
			{
				connection.Close();
				MessageBox.Show(e.Message);
			}
		}
        #endregion

        #region GameEngine
        public void GameEngine(object sender, EventArgs e)
		{
			scoreText1.Content = "Score: " + score1;
			scoreText2.Content = "Score: " + score2;

			Rect player2HitBox = new Rect(Canvas.GetLeft(player2), Canvas.GetTop(player2), player2.Width, player2.Height); //hitbox player rechts
			Rect player1HitBox = new Rect(Canvas.GetLeft(player1), Canvas.GetTop(player1), player1.Width, player1.Height); //hitbox player links

			if (Pause == false)
			{

				EnemyAsteroid(rand.Next(1, 51));

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

					if (x is Rectangle && (string)x.Tag == "bullet2")
					{

						Canvas.SetLeft(x, Canvas.GetLeft(x) - BulletSpeed);


						Rect bullet = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);


						if (Canvas.GetLeft(x) < 0)
						{
							itemstoremove.Add(x);
						}

						if (bullet.IntersectsWith(player1HitBox))
						{
							itemstoremove.Add(x);
							Player1CanShoot = false;
							Task.Delay(PlayerDisableShootTime).ContinueWith(_ => { Player1CanShoot = true; });

						}

						foreach (var y in MyCanvas.Children.OfType<Rectangle>())
						{

							if (y is Rectangle && ((string)y.Tag == "EnemyTop" || (string)y.Tag == "EnemyBottom"))
							{
								Rect enemy = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);

								if (bullet.IntersectsWith(enemy) && y.Width == 82)
								{
									itemstoremove.Add(x);
									itemstoremove.Add(y);
									score2 += 2;
								}

								if (bullet.IntersectsWith(enemy) && y.Width == 81)
								{
									itemstoremove.Add(x);
									itemstoremove.Add(y);
									score2 += 4;
								}

								if (bullet.IntersectsWith(enemy) && y.Width == 80)
								{
									itemstoremove.Add(x);
									itemstoremove.Add(y);
									score2 += 6;
								}

								if (bullet.IntersectsWith(enemy) && y.Width == 79)
								{
									itemstoremove.Add(x);
									itemstoremove.Add(y);
									score2 += 8;
								}

								if (bullet.IntersectsWith(enemy) && y.Width == 78)
								{
									itemstoremove.Add(x);
									itemstoremove.Add(y);
									score2 += 10;
									PlayerPowerUp2();
									Player2ShootDelay = 10;
									Task.Delay(PowerUpTime).ContinueWith(_ => { Player2ShootDelay = 250; });
								}
							}

							#region Bullet2 Intersects with asteroid
							if (y is Rectangle && ((string)y.Tag == "asteroidLinks" || (string)y.Tag == "asteroidRechts"))
							{
								Rect asteroid = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);

								if (bullet.IntersectsWith(asteroid))
								{
									itemstoremove.Add(x);
									itemstoremove.Add(y);
									score2++;
								}
							}
							#endregion
						}
					}
				}




				foreach (var x in MyCanvas.Children.OfType<Rectangle>())
				{

					if (x is Rectangle && (string)x.Tag == "bullet1")
					{

						Canvas.SetLeft(x, Canvas.GetLeft(x) + BulletSpeed);


						Rect bullet = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);


						if (Canvas.GetLeft(x) > 1280)
						{

							itemstoremove.Add(x);
						}

						if (bullet.IntersectsWith(player2HitBox))
						{
							itemstoremove.Add(x);
							Player2CanShoot = false;
							Task.Delay(PlayerDisableShootTime).ContinueWith(_ => { Player2CanShoot = true; });

						}


						foreach (var y in MyCanvas.Children.OfType<Rectangle>())
						{

							if (y is Rectangle && ((string)y.Tag == "EnemyTop" || (string)y.Tag == "EnemyBottom"))
							{
								Rect enemy = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);

								if (bullet.IntersectsWith(enemy) && y.Width == 82)
								{
									itemstoremove.Add(x);
									itemstoremove.Add(y);
									score1 += 2;
								}

								if (bullet.IntersectsWith(enemy) && y.Width == 81)
								{
									itemstoremove.Add(x);
									itemstoremove.Add(y);
									score1 += 4;
								}

								if (bullet.IntersectsWith(enemy) && y.Width == 80)
								{
									itemstoremove.Add(x);
									itemstoremove.Add(y);
									score1 += 6;
								}

								if (bullet.IntersectsWith(enemy) && y.Width == 79)
								{
									itemstoremove.Add(x);
									itemstoremove.Add(y);
									score1 += 8;
								}

								if (bullet.IntersectsWith(enemy) && y.Width == 78)
								{
									itemstoremove.Add(x);
									itemstoremove.Add(y);
									score1 += 10;
									Player1ShootDelay = 10;
									PlayerPowerUp1();
									Task.Delay(PowerUpTime).ContinueWith(_ => { Player1ShootDelay = 250; });
								}
							}
							#region Bullet1 Intersects with asteroid
							if (y is Rectangle && ((string)y.Tag == "asteroidLinks" || (string)y.Tag == "asteroidRechts"))
							{
								Rect asteroid = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);

								if (bullet.IntersectsWith(asteroid))
								{

									itemstoremove.Add(x);
									itemstoremove.Add(y);
									score1++;

								}
							}
							#endregion
						}
					}
					#region Enemy Deleted When out of frame
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
					#endregion

					#region asteroidRechts Intersects
					if (x is Rectangle && (string)x.Tag == "asteroidRechts")
					{
						Canvas.SetLeft(x, Canvas.GetLeft(x) + EnemySpeed);
						Rect asteroid = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

						if (Canvas.GetLeft(x) + 20 > 1280)
						{
							itemstoremove.Add(x);
						}

						if (asteroid.IntersectsWith(player2HitBox))
						{
							itemstoremove.Add(x);
							Player2CanShoot = false;
							Task.Delay(PlayerDisableShootTime).ContinueWith(_ => { Player2CanShoot = true; });

						}
					}
					#endregion

					#region asteroidLinks Intersects

					if (x is Rectangle && (string)x.Tag == "asteroidLinks")
					{
						Canvas.SetLeft(x, Canvas.GetLeft(x) - EnemySpeed);
						Rect asteroid = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

						if (Canvas.GetLeft(x) + 20 < 0)
						{
							itemstoremove.Add(x);
						}

						if (asteroid.IntersectsWith(player1HitBox))
						{
							itemstoremove.Add(x);
							Player1CanShoot = false;
							Task.Delay(PlayerDisableShootTime).ContinueWith(_ => { Player2CanShoot = true; });

						}
					}
					#endregion
				}

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
		}
		#endregion

		#region PowerUpMethods

		private async void PlayerPowerUp2()
		{
			PowerUp2.Visibility = Visibility.Visible;
			await Task.Delay(PowerUpTime);
			PowerUp2.Visibility = Visibility.Hidden;
		}

		private async void PlayerPowerUp1()
		{
			PowerUp1.Visibility = Visibility.Visible;
			await Task.Delay(PowerUpTime);
			PowerUp1.Visibility = Visibility.Hidden;
		}

		#endregion

		#region OnKeyDown
		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Up)
			{
				moveLeft2 = true;
			}
			if (e.Key == Key.Down)
			{
				moveRight2 = true;
			}

			if (e.Key == Key.W)
			{
				moveLeft1 = true;
			}
			if (e.Key == Key.S)
			{
				moveRight1 = true;
			}


		}
		#endregion

		#region OnKeyUp
		private void OnKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Up)
			{
				moveLeft2 = false;
			}
			if (e.Key == Key.Down)
			{
				moveRight2 = false;
			}

			if (e.Key == Key.W)
			{
				moveLeft1 = false;
			}
			if (e.Key == Key.S)
			{
				moveRight1 = false;
			}

			if (e.Key == Key.RightShift && Player2CanShoot == true)
			{
				Player2Shoot();

			}

			if (e.Key == Key.Space && Player1CanShoot == true)
			{
				Player1Shoot();
			}

		}
		#endregion

		#region MakeEnemies
		private void MakeEnemies()
		{
			ImageBrush EnemyType = new ImageBrush();
			int EnemyTypeCounter = rand.Next(1, 16);
			int Dimension = 0;

			switch (EnemyTypeCounter)
			{
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
					EnemyType.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/1.png"));
					Dimension = 82;
					break;

				case 6:
				case 7:
				case 8:
				case 9:
					EnemyType.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/2.png"));
					Dimension = 81;
					break;

				case 10:
				case 11:
				case 12:
					EnemyType.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/3.png"));
					Dimension = 80;
					break;

				case 13:
				case 14:
					EnemyType.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/4.png"));
					Dimension = 79;
					break;

				case 15:
					EnemyType.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/5.png"));
					Dimension = 78;
					break;

			}

			int EnemySpawn = rand.Next(1, 3);

			switch (EnemySpawn)
			{
				case 1:
					Rectangle NewEnemyTop = new Rectangle
					{
						Tag = "EnemyTop",
						Height = Dimension,
						Width = Dimension,
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
						Height = Dimension,
						Width = Dimension,
						Fill = EnemyType
					};

					Canvas.SetTop(NewEnemyBottom, 820);
					Canvas.SetLeft(NewEnemyBottom, rand.Next(400, 880));
					MyCanvas.Children.Add(NewEnemyBottom);
					break;

			}

			GC.Collect();

		}
        #endregion

		#region PauseMenuButtons
		private void Menu_Click(object sender, RoutedEventArgs e)
		{
			placeholderGame.MainMenu mainMenu = new placeholderGame.MainMenu
			{
				Visibility = Visibility.Visible
			};
			Close();
		}

		private void Exit_Click(object sender, RoutedEventArgs e)
        {
			Close();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
			placeholderGame.EnterName mainMenu = new placeholderGame.EnterName
			{
				Visibility = Visibility.Visible
			};
			Close();
		}

		#endregion

		#region PauseButton
		private bool Pause = false;
        private void Pause_Click(object sender, RoutedEventArgs e)
        {
			if (Pause == true)
			{
				Pause = false;
				if(musicMute.IsChecked == false)
                {
					music.Play();
                }
				PauseMenuOff();
			}

			else
            {
				Pause = true;
				if (musicMute.IsChecked == false)
				{
					music.Pause();
				}
				PauseMenuOn();
			}
        }
        #endregion

        #region Pause menu On en Off + GameOver menu
        private void PauseMenuOff()
        {
			exit.IsEnabled = false;
			exit.Visibility = Visibility.Hidden;
			Reset.IsEnabled = false;
			Reset.Visibility = Visibility.Hidden;
			ReturnToMenu.IsEnabled = false;
			ReturnToMenu.Visibility = Visibility.Hidden;
		}

		private void PauseMenuOn()
		{
			exit.IsEnabled = true;
			exit.Visibility = Visibility.Visible;
			Reset.IsEnabled = true;
			Reset.Visibility = Visibility.Visible;
			ReturnToMenu.IsEnabled = true;
			ReturnToMenu.Visibility = Visibility.Visible;
		}

		private void GameOver()
		{
			exit.IsEnabled = true;
			exit.Visibility = Visibility.Visible;
			ReturnToMenu.IsEnabled = true;
			ReturnToMenu.Visibility = Visibility.Visible;
		}
		#endregion

		#region Player1Shoot
		private async void Player1Shoot()
		{
			Player1CanShoot = false;

			Rectangle newBullet = new Rectangle
			{
				Tag = "bullet1",
				Height = 5,
				Width = 15,
				Fill = Brushes.White,
				Stroke = Brushes.Red
			};


			Canvas.SetTop(newBullet, Canvas.GetTop(player1) + player1.Height / 2);

			Canvas.SetLeft(newBullet, Canvas.GetLeft(player1) + player1.Width);

			MyCanvas.Children.Add(newBullet);

			await Task.Delay(Player1ShootDelay);

			Player1CanShoot = true;

		}
		#endregion

		#region Player2Shoot
		private async void Player2Shoot()
		{
			Player2CanShoot = false;

			Rectangle newBullet = new Rectangle
			{
				Tag = "bullet2",
				Height = 5,
				Width = 15,
				Fill = Brushes.White,
				Stroke = Brushes.Red
			};


			Canvas.SetTop(newBullet, Canvas.GetTop(player2) + player2.Height / 2);

			Canvas.SetLeft(newBullet, Canvas.GetLeft(player2) - newBullet.Width);

			MyCanvas.Children.Add(newBullet);

			await Task.Delay(Player2ShootDelay);

			Player2CanShoot = true;
		}
		#endregion

		#region Asteroid Spawning
		private void EnemyAsteroid(int Spawn)
		{
			ImageBrush Asteroid = new ImageBrush();
			Asteroid.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/asteroid.png"));

			int AsteroidDirection = rand.Next(1, 3);
			if (Spawn == 3)
			{
				switch (AsteroidDirection)
				{
					case 1:
						Rectangle AsteroidLinks = new Rectangle
						{
							Tag = "asteroidLinks",
							Height = 20,
							Width = 20,
							Fill = Asteroid
						};
						Canvas.SetTop(AsteroidLinks, rand.Next(100, 620));

						Canvas.SetLeft(AsteroidLinks, 640);

						MyCanvas.Children.Add(AsteroidLinks);
						break;


					case 2:
						Rectangle AsteroidRechts = new Rectangle
						{
							Tag = "asteroidRechts",
							Height = 20,
							Width = 20,
							Fill = Asteroid
						};
						Canvas.SetTop(AsteroidRechts, rand.Next(100, 620));

						Canvas.SetLeft(AsteroidRechts, 640);

						MyCanvas.Children.Add(AsteroidRechts);
						break;
				}
			}
		}
		#endregion

	}

}
