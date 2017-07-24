using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Device.Location;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Xml;
using VkNet;
using System.IO;
using VkNet.Enums.Filters;
using VkNet.Model.RequestParams;
using System.Text;
using System.Windows.Media.Imaging;

namespace TestLaserwar
{
    static class ScreenShotElement
    {
        public static void SaveToPNG(FrameworkElement frameworkElement, string fileName)
        {
            using (FileStream stream = new FileStream(string.Format("{0}.png", fileName), FileMode.Create))
            {
                SaveToPNG(frameworkElement, stream);
            }
        }

        private static void SaveToPNG(FrameworkElement frameworkElement, Stream stream)
        {
            Size size = frameworkElement.RenderSize;
            Transform transform = frameworkElement.LayoutTransform;
            frameworkElement.LayoutTransform = null;
            Thickness margin = frameworkElement.Margin;
            frameworkElement.Margin = new Thickness(0, 0, margin.Right - margin.Left, margin.Bottom - margin.Top);
            frameworkElement.Measure(size);
            frameworkElement.Arrange(new Rect(size));
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(frameworkElement);
            frameworkElement.LayoutTransform = transform;
            frameworkElement.Margin = margin;
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Interlace = PngInterlaceOption.On;
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            encoder.Save(stream);
        }
    }
}
