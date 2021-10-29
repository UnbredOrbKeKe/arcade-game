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
    /// Interaction logic for OptionsMenu.xaml
    /// </summary>
    public partial class OptionsMenu : Window
    {
        public OptionsMenu()
        {
            InitializeComponent();
            ImageBrush bg = new ImageBrush();

            //initialize the background
            bg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/space_background.png"));
            bg.TileMode = TileMode.Tile;
            bg.Viewport = new Rect(0, 0, 1, 1);
            bg.ViewboxUnits = BrushMappingMode.RelativeToBoundingBox;
            MyCanvas.Background = bg;
        }

        private void Slider_Volume(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //couldn't get the volume to work in time
        }

        /// <summary>
        /// on click opens the mainmenu window and closes the options menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BacktoMenu_Click(object sender, RoutedEventArgs e)
        {
            placeholderGame.MainMenu mainMenu = new placeholderGame.MainMenu
            {
                Visibility = Visibility.Visible
            };
            Close();
        }
    }
}