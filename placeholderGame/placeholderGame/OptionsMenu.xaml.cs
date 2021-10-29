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
        }

        private void Slider_Volume(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
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
