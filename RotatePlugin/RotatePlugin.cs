using PluginsInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RotatePlugin
{
    public class RotatePlugin: IPlugin
    {
        public BitmapSource doOperation(BitmapSource image)
        {
            return new TransformedBitmap(image, new RotateTransform(90));
        }

        public Button getPluginButton()
        {
            Button result = new Button();
            Label label = new Label();
            label.Content = "Obrót w prawo";
            result.Content = label;

            return result;
        }
    }
}
