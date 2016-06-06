using PluginsInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private Dictionary<Button, IPlugin> loadedPlugins;

        public MainWindow()
        {
            InitializeComponent();

            loadedPlugins = new Dictionary<Button, IPlugin>();
            loadPlugins();
        }

        private void loadPlugins()
        {
            foreach (string dll in Directory.GetFiles("./plugins", "*.dll"))
            {
                Assembly assembly = Assembly.LoadFrom(dll);
                foreach (Type type in assembly.GetTypes())
                {
                    toolBar.Items.Add(new Separator());
                    if (type.IsClass && type.IsPublic && typeof(IPlugin).IsAssignableFrom(type))
                    {
                        Object obj = Activator.CreateInstance(type);
                        IPlugin plugin = (IPlugin)obj;

                        Button pluginButton = plugin.getPluginButton();
                        pluginButton.IsEnabled = false;
                        pluginButton.Click += pluginButton_Click;

                        loadedPlugins.Add(pluginButton, plugin);
                        toolBar.Items.Add(pluginButton);
                    }
                }
            }
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
                    foreach (Button button in loadedPlugins.Keys)
                    {
                        button.IsEnabled = true;
                    }
                }

                baseImage = new BitmapImage(new Uri(dlg.FileName));
                imageView.Source = baseImage;
                currentVersionOfImage = baseImage;
            }
        }

        private void pluginButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            IPlugin plugin;
            loadedPlugins.TryGetValue(clickedButton, out plugin);

            currentVersionOfImage = plugin.doOperation(currentVersionOfImage);
            imageView.Source = currentVersionOfImage;
        }
    }
}
