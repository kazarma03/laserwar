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
using System.Drawing;

namespace TestLaserwar
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;        
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
           download.Focus();
           download.Background = new SolidColorBrush(Colors.Blue);
           RefMainMenu(true,false,false);
        }

        private void RefMainMenu(bool dkey,bool skey,bool gkey)
        {
            if (dkey)PageDownload.Visibility = System.Windows.Visibility.Visible;
            else PageDownload.Visibility = System.Windows.Visibility.Hidden;
            if (skey) PageSound.Visibility = System.Windows.Visibility.Visible;
            else PageSound.Visibility = System.Windows.Visibility.Hidden;
            if (gkey) PageGame.Visibility = System.Windows.Visibility.Visible;
            else PageGame.Visibility = System.Windows.Visibility.Hidden;
        }

        private void download_Click(object sender, RoutedEventArgs e)
        {
            RefMainMenu(true, false, false);
        }

        private void sounds_Click(object sender, RoutedEventArgs e)
        {
            RefMainMenu(false, true, false);
        }

        private void games_Click(object sender, RoutedEventArgs e)
        {
            RefMainMenu(true, false, true);
        }

    }
}
