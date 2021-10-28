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
    /// Interaction logic for EnterName.xaml
    /// </summary>
    public partial class EnterName : Window
    {
        public static string player1Name;
        public static string player2Name;

        public EnterName()
        {
            InitializeComponent();

            ImageBrush bg = new ImageBrush();
            bg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/space_background.png"));
            bg.TileMode = TileMode.Tile;
            bg.Viewport = new Rect(0, 0, 1, 1);
            bg.ViewboxUnits = BrushMappingMode.RelativeToBoundingBox;
            MyCanvas.Background = bg;

            EnterAName.Visibility = Visibility.Hidden;

            ImageBrush player1Image = new ImageBrush();
            player1Image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/playerShip1_orange.png"));
            player1.Fill = player1Image;
            ImageBrush player2Image = new ImageBrush();
            player2Image.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/playerShip2_blue.png"));
            player2.Fill = player2Image;
        }
             
      

        public async void Start_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(PlayerName1.Text) || String.IsNullOrEmpty(PlayerName2.Text))
            {
                EnterAName.Visibility = Visibility.Visible;
                await Task.Delay(2000);
                EnterAName.Visibility = Visibility.Hidden;
            }

            else 
            {
                player1Name = PlayerName1.Text;
                player2Name = PlayerName2.Text;

                placeholderGame.MainWindow gameWindow = new placeholderGame.MainWindow
                {
                    Visibility = Visibility.Visible
                };
                Close();
            }
        }
    }
}
