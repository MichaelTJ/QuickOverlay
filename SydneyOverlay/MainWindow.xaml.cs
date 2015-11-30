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
using Microsoft.Win32;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SydneyOverlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //uneditedImg is a holder for the incoming image
        public System.Windows.Controls.Image uneditedImg;
        public MainWindow()
        {
            uneditedImg = new System.Windows.Controls.Image();   
            InitializeComponent();

        }

        private void ComboBox_Areas_Loaded(object sender, RoutedEventArgs e)
        {
            string[] areas = { "Walls Internal",
                                 "Walls External",
                                 "Entry Hatch",
                                 "Roof Platform",
                                 "Walkways",
                                 "Roof External",
                                 "Roof Internal",
                                 "Handrails",
                                 "Columns",
                                 "Roof Spider",
                                 "Roof Framing",
                                 "Floor"
                             };

            List<string> Areas = new List<string>(areas);

            var comboBox = sender as ComboBox;
            comboBox.ItemsSource = Areas;
            comboBox.SelectedIndex = 0;
        }

        private void ComboBox_Areas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            //do something with 
            //string value = comboBox.SelectedItem as string
        }
        private void ComboBox_Criterias_Loaded(object sender, RoutedEventArgs e)
        {
            string[] areas = { "Walls Internal",
                                 "Walls External",
                                 "Entry Hatch",
                                 "Roof Platform",
                                 "Walkways",
                                 "Roof External",
                                 "Roof Internal",
                                 "Handrails",
                                 "Columns",
                                 "Roof Spider",
                                 "Roof Framing",
                                 "Floor"
                             };

            List<string> Areas = new List<string>(areas);

            var comboBox = sender as ComboBox;
            comboBox.ItemsSource = Areas;
            comboBox.SelectedIndex = 0;
        }

        private void ComboBox_Criterias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            //do something with 
            //string value = comboBox.SelectedItem as string
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select an image";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png";

            if(op.ShowDialog()==true)
            {
                imgPhoto.Source = new BitmapImage(new Uri(op.FileName));
                uneditedImg.Source = new BitmapImage(new Uri(op.FileName));
            }
        }

        private void WriteComment(System.Windows.Controls.Image image)
        {
            var visual = new DrawingVisual();
            using(DrawingContext drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawImage(image.Source, 
                    new Rect(0,0,image.Source.Width,image.Source.Height));
                drawingContext.DrawText(
                    new FormattedText("Hi!",System.Globalization.CultureInfo.InvariantCulture
                        ,FlowDirection.LeftToRight,new Typeface("Arial"),20,System.Windows.Media.Brushes.Black),
                        new System.Windows.Point(0, 0));
            }
            var newImage = new DrawingImage(visual.Drawing);
            imgPhoto.Source = newImage;
        }

        private void Write_Button_Click(object sender, RoutedEventArgs e)
        {
            WriteComment(uneditedImg);
        }
        /*
        private void OnPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            SolidBrush brushWhite = new SolidBrush(System.Drawing.Color.White);
            e.Graphics.FillRectangle(brushWhite, 0, 0,
            20, 20);

            System.Drawing.FontFamily fontFamily =
                new System.Drawing.FontFamily("Arial");
            StringFormat strformat = new StringFormat();
            string szbuf = "Text Designer";

            GraphicsPath path = new GraphicsPath();
            path.AddString(szbuf, fontFamily,
            (int)System.Drawing.FontStyle.Regular, 48.0f, new System.Drawing.Point(10, 10), strformat);
            Pen pen = new System.Drawing.Pen(Color.FromArgb(234, 137, 6), 6);
            e.Graphics.DrawPath(pen, path);
            SolidBrush brush = new SolidBrush(Color.FromArgb(128, 0, 255));
            e.Graphics.FillPath(brush, path);

            brushWhite.Dispose();
            fontFamily.Dispose();
            path.Dispose();
            pen.Dispose();
            brush.Dispose();
            e.Graphics.Dispose();
        }*/

    }
}
