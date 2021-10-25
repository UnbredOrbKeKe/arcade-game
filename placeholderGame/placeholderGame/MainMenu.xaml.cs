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
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
			placeholderGame.MainWindow gameWindow = new placeholderGame.MainWindow
			{
				Visibility = Visibility.Visible
			};
            Close();
		}

        private void Options_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Leaderboards_Click(object sender, RoutedEventArgs e)
        {
            placeholderGame.Leaderboard leaderborad = new placeholderGame.Leaderboard
            {
                Visibility = Visibility.Visible
            };
            Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
			GameHelp helpWindow = new GameHelp
			{
				Visibility = Visibility.Visible
			};
		}
    }
}
