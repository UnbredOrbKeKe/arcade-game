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
    /// Interaction logic for GameRules.xaml
    /// </summary>
    public partial class GameHelp : Window
    {
        public GameHelp()
        {
            InitializeComponent();
        }

        private void BacktoMenu_Click(object sender, RoutedEventArgs e)
        {
            {
                placeholderGame.MainMenu mainMenu = new placeholderGame.MainMenu
                {
                    Visibility = Visibility.Visible
                };
                Close();
            }
        }
    }
}
