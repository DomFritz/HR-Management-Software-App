using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HRManagement_ServiceApplication
{
    /// <summary>
    /// The class to get a picture stack panel for a button.
    /// </summary>
    internal class ButtonImages
    {
        /// <summary>
        /// This method gets the picture, creates the stack panel and returns it.
        /// </summary>
        /// <param name="imagePath">The path to the button image.</param>
        /// <returns>The created stack panel to add it to the button.</returns>
        public static StackPanel GetButtonContent(Uri imagePath)
        {
            Image img = new Image();
            img.Source = new BitmapImage(imagePath);
            img.Stretch = Stretch.Uniform;

            StackPanel stackPnl = new StackPanel();
            stackPnl.Orientation = Orientation.Horizontal;
            stackPnl.Children.Add(img);

            return stackPnl;
        }
    }
}
