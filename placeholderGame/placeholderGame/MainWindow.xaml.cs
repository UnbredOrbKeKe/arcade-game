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

using System.Windows.Threading;

namespace placeholderGame
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		public MainWindow()

		{
            {
                InitializeComponent();
            }
            //make a game timer
            DispatcherTimer gameTimer = new DispatcherTimer();
            // move left and move right boolean declaration
            bool moveLeft, moveRight;
            // make a new items remove list
            List<Rectangle> itemstoremove = new List<Rectangle>();
            // make a new random class to generate random numbers from
            Random rand = new Random();


            int enemySpriteCounter; // int to help change enemy images
            int enemyCounter = 100; // enemy spawn time
            int playerSpeed = 10; // player movement speed
            int limit = 50; // limit of enemy spawns
            int score = 0; // default score
            int damage = 0; // default damage

            Rect playerHitBox; // player hit box to check for collision against enemy
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{

		}

		private void OnKeyUp(object sender, KeyEventArgs e)
		{

        }

}
}
