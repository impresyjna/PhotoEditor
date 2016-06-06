using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PluginsInterface
{
    public interface IPlugin
    {
        BitmapSource doOperation(BitmapSource image);
        Button getPluginButton();
    }
}
