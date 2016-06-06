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

namespace PhotoEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BitmapImage baseImage;
        private BitmapSource currentVersionOfImage; 

        public MainWindow()
        {
            InitializeComponent();
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".jpg";
            dlg.Filter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                if (baseImage == null)
                {
                    
                }

                baseImage = new BitmapImage(new Uri(dlg.FileName));
                imageView.Source = baseImage;
                currentVersionOfImage = baseImage;
            }
        }
    }
}
