using PluginsInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private BitmapImage baseImage;
        private BitmapSource currentVersionOfImage;
        private Dictionary<Button, IPlugin> loadedPlugins;
        public bool UndoEnable
        {
            get
            {
                return isUndoEnable;
            }
            set
            {
                isUndoEnable = value;
                NotifyPropertyChanged("UndoEnable");
            }
        }

        public bool RedoEnable
        {
            get
            {
                return isRedoEnable;
            }
            set
            {
                isRedoEnable = value;
                NotifyPropertyChanged("RedoEnable");
            }
        }
        private bool isUndoEnable;
        private bool isRedoEnable;

        private Stack<IPlugin> undoStack;
        private Stack<IPlugin> redoStack;

        public MainWindow()
        {
            InitializeComponent();

            loadedPlugins = new Dictionary<Button, IPlugin>();
            loadPlugins();

            undoStack = new Stack<IPlugin>();
            redoStack = new Stack<IPlugin>();

            undoButton.DataContext = this;
            redoButton.DataContext = this;
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

                undoStack.Clear();
                UndoEnable = false;

                redoStack.Clear();
                RedoEnable = false;
            }
        }

        private void pluginButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            IPlugin plugin;
            loadedPlugins.TryGetValue(clickedButton, out plugin);

            currentVersionOfImage = plugin.doOperation(currentVersionOfImage);
            imageView.Source = currentVersionOfImage;

            redoStack.Clear();
            undoStack.Push(plugin);

            UndoEnable = true;
            RedoEnable = false;
        }

        private void undoButton_Click(object sender, RoutedEventArgs e)
        {
            currentVersionOfImage = baseImage;

            IPlugin undoneOperation = undoStack.Pop();
            redoStack.Push(undoneOperation);

            foreach (IPlugin operation in undoStack)
            {
                currentVersionOfImage = operation.doOperation(currentVersionOfImage);
            }

            imageView.Source = currentVersionOfImage;

            if (undoStack.Count != 0)
            {
                UndoEnable = true;
            }
            else
            {
                UndoEnable = false; 
            }
            RedoEnable = true;
        }

        private void redoButton_Click(object sender, RoutedEventArgs e)
        {
            IPlugin redoneOperation = redoStack.Pop();
            undoStack.Push(redoneOperation);
            currentVersionOfImage = redoneOperation.doOperation(currentVersionOfImage);
            imageView.Source = currentVersionOfImage;

            if (undoStack.Count != 0)
            {
                UndoEnable = true;
            }
            else
            {
                UndoEnable = false;
            }
            if (redoStack.Count != 0)
            {
                RedoEnable = true;
            }
            else
            {
                RedoEnable = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


    }
}
