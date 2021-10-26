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

namespace placeholderGame
{
	/// <summary>
	/// Interaction logic for Leaderboard.xaml
	/// </summary>
	public partial class Leaderboard : Window
	{
		public Leaderboard()
		{
			InitializeComponent();
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
	}
}
