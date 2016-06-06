using System;
using System.Collections.Generic;
using System.Linq;
using PluginsInterface;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BlackAndWhitePlugin
{
    public class BlackAndWhite: IPlugin
    {
        public BitmapSource doOperation(BitmapSource image)
        {
            return new FormatConvertedBitmap(image, PixelFormats.Gray8, BitmapPalettes.Gray256, 0.0);
        }

        public Button getPluginButton()
        {
            Button pluginButton = new Button();
            Label label = new Label();
            label.Content = "Czarno-białe";
            pluginButton.Content = label;

            return pluginButton;
        }
    }
}
