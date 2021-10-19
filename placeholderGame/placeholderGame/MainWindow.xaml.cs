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

		int enemySpriteCounter = 0;
		int enemyCounter = 100;
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

		public MainWindow()
		{
			InitializeComponent();


			gameTimer.Interval = TimeSpan.FromMilliseconds(20);
			gameTimer.Tick += new EventHandler(GameEngine);
			gameTimer.Start();
			MyCanvas.Focus();

			music.Open(new Uri("pack://siteoforigin:,,,/music/FZero_Mute_City_8bit.mp3"));
			music.Play();

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
				// if any rectangle has the tag bullet in it
				if (x is Rectangle && (string)x.Tag == "bullet")
				{
					// move the bullet rectangle towards top of the screen
					Canvas.SetLeft(x, Canvas.GetLeft(x) - 20);

					// make a rect class with the bullet rectangles properties
					Rect bullet = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

					// check if bullet has reached top part of the screen
					if (Canvas.GetLeft(x) < 10)
					{
						// if it has then add it to the item to remove list
						itemstoremove.Add(x);
					}

					// run another for each loop inside of the main loop this one has a local variable called y
					foreach (var y in MyCanvas.Children.OfType<Rectangle>())
					{
						// if y is a rectangle and it has a tag called enemy
						if (y is Rectangle && (string)y.Tag == "enemy")
						{
							// make a local rect called enemy and put the enemies properties into it
							Rect enemy = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);


							// now check if bullet and enemy is colliding or not
							// if the bullet is colliding with the enemy rectangle
							if (bullet.IntersectsWith(enemy))
							{

								itemstoremove.Add(x); // remove bullet
								itemstoremove.Add(y); // remove enemy
								score1++; // add one to the score
							}
						}

					}
				}
			}
			player1HitBox = new Rect(Canvas.GetLeft(player1), Canvas.GetTop(player1), player1.Width, player1.Height);

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
					Tag = "bullet",
					Height = 5,
					Width = 5,
					Fill = Brushes.White,
					Stroke = Brushes.Red
				};

				// place the bullet on top of the player location
				Canvas.SetTop(newBullet, Canvas.GetTop(player1) + player1.Height / 2);
				// place the bullet middle of the player image
				Canvas.SetLeft(newBullet, Canvas.GetLeft(player1) - newBullet.Width);
				// add the bullet to the screen
				MyCanvas.Children.Add(newBullet);
			}
		}


	}
}
