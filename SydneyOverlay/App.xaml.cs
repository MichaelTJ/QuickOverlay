using System;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SydneyOverlay
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        void App_Startup(object sender, StartupEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            //Check args for image file
            if (e.Args.Length > 0)
            {
                mainWindow.imgPhoto.Source = new BitmapImage(new Uri(e.Args[0]));
                mainWindow.uneditedImg.Source = new BitmapImage(new Uri(e.Args[0]));
            }
            mainWindow.Show();
        }
    }
}
