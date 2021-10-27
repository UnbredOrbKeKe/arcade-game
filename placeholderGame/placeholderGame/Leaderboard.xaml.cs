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
using System.Windows.Shapes;
using System.Diagnostics;

namespace placeholderGame
{
	/// <summary>
	/// Interaction logic for Leaderboard.xaml
	/// </summary>
	public partial class Leaderboard : Window
	{

		Dictionary<string, int> highScores = new Dictionary<string, int>();
		Random rand = new Random();

		public Leaderboard()
		{
			InitializeComponent();
			
			highScores.Add("asdf", rand.Next(1, 700));
			highScores.Add("fghj", rand.Next(1, 700));
			highScores.Add("assertdf", rand.Next(1, 700));
			highScores.Add("asjosdirtdf", rand.Next(1, 700));
			highScores.Add("ahuwersdf", rand.Next(1, 700));
			highScores.Add("ezsdf", rand.Next(1, 700));
			highScores.Add("pz", rand.Next(1, 700));
			highScores.Add("asdzdf", rand.Next(1, 700));

			GetHighscores();
			CreateLables();
			ImageBrush bg = new ImageBrush();
			bg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/space_background.png"));
			bg.TileMode = TileMode.Tile;
			bg.Viewport = new Rect(0, 0, 1, 1);
			bg.ViewboxUnits = BrushMappingMode.RelativeToBoundingBox;
			leaderboard.Background = bg;
		}

		private void Exit_click(object sender, RoutedEventArgs e)
		{
			placeholderGame.MainMenu mainMenu = new placeholderGame.MainMenu
			{
				Visibility = Visibility.Visible
			};
			Close();
		}

		private void Refresh_Click(object sender, RoutedEventArgs e)
		{
			GetHighscores();
			CreateLables();
			Debug.WriteLine("refreshed");
		}

		private void GetHighscores()
		{
			//get highscores from database
		}
		private void CreateLables()
		{
			HighScoresPanel1.Children.Clear();
			var sortedHighscores = from score in highScores orderby score.Value descending select score;
			foreach (KeyValuePair<string, int> highscore in sortedHighscores)
			{
				Label labelPlayerName = new Label();
				Label labelPlayerScore = new Label();
				labelPlayerName.Content = highscore.Key;
				labelPlayerScore.Content = highscore.Value;

				labelPlayerName.Foreground = new SolidColorBrush(Colors.Gray);
				labelPlayerName.FontSize = 30;

				labelPlayerScore.Foreground = new SolidColorBrush(Colors.Gray);
				labelPlayerScore.FontSize = 30;

				labelPlayerName.HorizontalAlignment = HorizontalAlignment.Center;
				labelPlayerScore.HorizontalAlignment = HorizontalAlignment.Center;
				HighScoresPanel1.Children.Add(labelPlayerName);
				HighScoresPanel2.Children.Add(labelPlayerScore);
				Debug.WriteLine(highscore.Key + " " + highscore.Value);
			}
		}
	}
}
