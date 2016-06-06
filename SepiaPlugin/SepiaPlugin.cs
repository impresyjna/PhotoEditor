using PluginsInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SepiaPlugin
{
    public class SepiaPlugin: IPlugin
    {
        public BitmapSource doOperation(BitmapSource image)
        {
           return new FormatConvertedBitmap(image, PixelFormats.Gray2, BitmapPalettes.Gray256, 0.0);
        }

        public Button getPluginButton()
        {
            Button pluginButton = new Button();
            Label label = new Label();
            label.Content = "Czarno-biały szum";
            pluginButton.Content = label;

            return pluginButton;
        }
    }
}
