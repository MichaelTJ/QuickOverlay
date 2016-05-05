//Michael Jensen 2015

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
using System.Drawing.Imaging;
using System.IO;
using System.Speech.Recognition;
using System.Xml;
using System.Xml.Serialization;
using System.ServiceModel.Web;
using System.ServiceModel;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using BMDMediaExpressInterfaceWPF;
using Paloma;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Diagnostics;

namespace SydneyOverlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables
        //uneditedImg is a holder for the incoming image
        public System.Windows.Controls.Image uneditedImg;
        public List<string> filesList;
        private ObservableCollection<AreasAndConditions> AandCList;
        public AreasAndConditions AandC;
        public SpeechSifter SpeechS;
        private ComboBox curComboBox;
        private bool dontRunSelectionChanged = false;

        private BMDMediaExpressInterfaceWPF.MainWindow bmdm;
        

        //Used for switching windows
        private Process MediaExpress;

        private System.Windows.Shapes.Rectangle curRect;
        
        
        #endregion


        public MainWindow()
        {
            uneditedImg = new System.Windows.Controls.Image();
            filesList = new List<string>();

            //AandC = new AreasAndConditions();
            SpeechS = new SpeechSifter();
            InitializeComponent();
            SpeechS.sre.SpeechRecognized +=
                new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);
            SpeechS.sre.LoadGrammarCompleted +=
                new EventHandler<LoadGrammarCompletedEventArgs>(LoadGrammarCompletedHandler);
            //TODO

            SpeechS.controlWords = new string[] { "Write", "Save", "Discard", "Take" };

            //LoadLocalJSON creates now AandCList if there's none
            LoadLocalJSON();
            UpdateOverlayCombobox();
            AandCList.CollectionChanged += AandCList_CollectionChanged;

            //Triggeres OverlayComboBOx_CHanged and sets the res of the boxes
            //Sets up the rest of the ComboCoxes
            OverlayComboBox.SelectedIndex = 0;

            btnOptionsVoiceEnabled.IsChecked = false;

            bmdm = new BMDMediaExpressInterfaceWPF.MainWindow();
            bmdm.Show();

            //Add handler for new bmd images
            bmdm.NewImage += bmdm_NewImage;

            Process[] prc = Process.GetProcessesByName("MediaExpress");
            if (prc.Length > 0)
            {
                MediaExpress = prc[0];
                btnMediaExpressEnabled.IsChecked = true;
                //checkbox in the options menu
                
            }
            else { btnMediaExpressEnabled.IsChecked = false; }



        }

        private void LoadGrammarCompletedHandler(object sender, LoadGrammarCompletedEventArgs e)
        {
            if (btnOptionsVoiceEnabled.IsChecked)
            {
                if (curRect != null)
                {
                    curRect.Fill = new SolidColorBrush(System.Windows
                        .Media.Colors.Green);
                }
            }
        }
        private void ResetAllRects()
        {
            List<System.Windows.Shapes.Rectangle> rects = new List<System.Windows.Shapes.Rectangle>();
            rects.Add(RectArea);
            rects.Add(RectIssue);
            rects.Add(RectLocation);
            rects.Add(RectRating);
            if (btnOptionsVoiceEnabled.IsChecked)
            {
                foreach (System.Windows.Shapes.Rectangle rectangle in rects)
                {
                    rectangle.Fill = new SolidColorBrush(System.Windows
                    .Media.Colors.Red);
                }
            }
            else
            {
                foreach (System.Windows.Shapes.Rectangle rectangle in rects)
                {
                    rectangle.Fill = new SolidColorBrush(System.Windows
                    .Media.Colors.Gray);
                }
            }
        }

        void bmdm_NewImage(object sender, NewImageEventArgs e)
        {

            //needed as can be triggered by event from BMD
            //Updates the UI in another thread - throws errors without
            this.Dispatcher.Invoke((Action)(() =>
                {
                    foreach (string imagePath in e.GetPaths)
                    {
                        filesList.Add(imagePath);
                    }
                    setNextImage();

                    if (btnMediaExpressEnabled.IsChecked)
                    {
                        this.Activate();
                    }
                }));
        }


        private void AandCList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //here to update the current A&C options
            //when a new one is added
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                AandC = AandCList[AandCList.Count-1];
            }
        }


        #region Initializers
        private void ComboBox_Areas_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            comboBox.ItemsSource = AandC.GetAreas();

            //comboBox.SelectedIndex = 0;
            CriteriasComboBox.IsDropDownOpen = false;
        }
        private void ComboBox_Criterias_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            comboBox.ItemsSource = new string[] { "Select an Area" };
            //required for friendly UI on startup
            CriteriasComboBox.IsDropDownOpen = false;
        }
        private void DateText_Loaded(object sender, RoutedEventArgs e)
        {
            DateText.Text = string.Format(DateTime.Today.ToString("dd/MM/yyyy"));
        }

        private void RatingComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            string[] areas = { "0",
                                 "1",
                                 "2",
                                 "3",
                                 "4"
                             };

            List<string> Areas = new List<string>(areas);
            var comboBox = sender as ComboBox;
            comboBox.ItemsSource = Areas;
            //comboBox.SelectedIndex = 0;
            CriteriasComboBox.IsDropDownOpen = false;

        }
        private void LocationComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            string[] locations = {"1",
                                 "2",
                                 "3",
                                 "4",
                                 "5",
                                 "6",
                                 "7",
                                 "8",
                                 "9",
                                 "10",
                                 "11",
                                 "12",
                                 "Central"
                             };

            List<string> Locations = new List<string>(locations);
            var comboBox = sender as ComboBox;
            comboBox.ItemsSource = Locations;
            //comboBox.SelectedIndex = 0;

        }
        #endregion


        #region UserInput
        private void ComboBox_Areas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dontRunSelectionChanged) { return; }
            var comboBox = sender as ComboBox;
            //get the selected item
            string value = comboBox.SelectedItem as string;
            //set the conditions to selecteditem
            CriteriasComboBox.ItemsSource = AandC.GetConditions(value);

            //Necessary for freindly UI on startup
            //  SelectionChanged triggers ComboCox_Criterias.SelectionChanged
            //CriteriasComboBox.SelectedIndex = 0;
            curRect = RectIssue;
            setCurComboBox(CriteriasComboBox);
        }
        private void ComboBox_Criterias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dontRunSelectionChanged) { return; }
            var comboBox = sender as ComboBox;
            curRect = RectRating;
            setCurComboBox(RatingComboBox);
            //do something with 
            //string value = comboBox.SelectedItem as string
        }
        private void RatingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dontRunSelectionChanged) { return; }
            var comboBox = sender as ComboBox;
            curRect = RectLocation;

            setCurComboBox(LocationComboBox);
        }
        private void LocationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dontRunSelectionChanged) { return; }
            var comboBox = sender as ComboBox;
            curRect = null;
            setCurComboBox(null);
            comboBox.IsDropDownOpen = false;
            WriteComment(uneditedImg);
        }
        private void Write_Button_Click(object sender, RoutedEventArgs e)
        {
            WriteComment(uneditedImg);
        }
        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            if (filesList.Count == 0) { return; }
            
            SaveImageToFile(imgPhoto, filesList[0]);
            SaveLabelsToText(filesList[0]);
            
            //Discard and set over the image
            filesList.RemoveAt(0);
            setNextImage();
            if (btnMediaExpressEnabled.IsChecked)
            {
                SetForegroundWindow(MediaExpress.MainWindowHandle);
            }
        }
        private void Discard_Button_Click(object sender, RoutedEventArgs e)
        {
            if (filesList.Count == 0) { return; }
            //Skip over the image
            filesList.RemoveAt(0);
            setNextImage();
            if (btnMediaExpressEnabled.IsChecked)
            {
                SetForegroundWindow(MediaExpress.MainWindowHandle);
            }
        }
        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select an image";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png";

            if (op.ShowDialog() == true)
            {
                //add it to files list
                filesList.Add(op.FileName);
                setNextImage();
            }
        }
        private void Image_Drop(object sender, DragEventArgs e)
        {
            string[] droppedFiles = null;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                droppedFiles = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            }

            if ((null == droppedFiles) || (!droppedFiles.Any())) { return; }

            //listFiles.Items.Clear();

            foreach (string s in droppedFiles)
            {
                filesList.Add(s);
            }
            setNextImage();

        }
        #endregion


        #region Functions
        private void WriteComment(System.Windows.Controls.Image image)
        {

            if (image.Source == null) { return; }
            //TextContent is the text container with writing style for 
            //the black with white outline
            TextContent.Text = string.Format("ID: {0} ", IDText.Text);
            TextContent.Text += string.Format("Date: {0}\n", DateText.Text);
            TextContent.Text += string.Format("Area: {0}\n", AreasComboBox.SelectedItem as string);
            TextContent.Text += string.Format("Issue: {0}\n", CriteriasComboBox.SelectedItem as string);
            TextContent.Text += string.Format("Rating: {0}\n", RatingComboBox.Text);
            TextContent.Text += string.Format("Location: {0}:00\n", LocationComboBox.SelectedItem as string);
            TextContent.Text += string.Format("Comment/s:\n{0}\n", CommentsBox.Text);
            var visual = new DrawingVisual();
            System.Windows.Media.Pen pen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Black, 1);
            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                int textLeftMargin = -5;
                int textTopMargin = -5;
                drawingContext.DrawImage(image.Source,
                    new Rect(textLeftMargin, textTopMargin, image.Source.Width, image.Source.Height));
                drawingContext.DrawGeometry(System.Windows.Media.Brushes.White, pen, TextContent._textGeometry);
                /*
                drawingContext.DrawText(
                    new FormattedText("Hi!",System.Globalization.CultureInfo.InvariantCulture
                        ,FlowDirection.LeftToRight,new Typeface("Arial"),20,System.Windows.Media.Brushes.Black),
                        new System.Windows.Point(0, 0));
                 */
            }
            var newImage = new DrawingImage(visual.Drawing);
            imgPhoto.Source = newImage;

        }
        private void SaveImageToFile(System.Windows.Controls.Image imageIn, string filePath)
        {
            if (imageIn.Source == null) { return; }

            RenderTargetBitmap rtBmp = new RenderTargetBitmap((int)imageIn.Source.Width, (int)imageIn.Source.Height,
                96.0, 96.0, PixelFormats.Pbgra32);

            imageIn.Measure(new System.Windows.Size((int)imageIn.Source.Width, (int)imageIn.Source.Height));
            imageIn.Arrange(new Rect(new System.Windows.Size((int)imageIn.Source.Width, (int)imageIn.Source.Height)));

            rtBmp.Render(imageIn);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            MemoryStream stream = new MemoryStream();
            encoder.Frames.Add(BitmapFrame.Create(rtBmp));

            // Save to memory stream and create Bitamp from stream
            encoder.Save(stream);
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(stream);

            // Demonstrate that we can do something with the Bitmap
            bitmap.Save(GetSaveToFilePath(filePath), ImageFormat.Png);



            // Optionally, if we didn't need Bitmap object, but
            // just wanted to render to file, we could:
            //encoder.Save(new FileStream(GetSaveToFilePath(filePath), FileMode.Create));

        }
        private void SaveLabelsToText(string filePath)
        {
            string[] combinedText = new string[7];
            combinedText[0] = string.Format("ID: {0} ", IDText.Text);
            combinedText[1] = string.Format("Date: {0}\n", DateText.Text);
            combinedText[2] = string.Format("Area: {0}\n", AreasComboBox.SelectedItem as string);
            combinedText[3] = string.Format("Issue: {0}\n", CriteriasComboBox.SelectedItem as string);
            combinedText[4] = string.Format("Rating: {0}\n", RatingComboBox.Text);
            combinedText[5] = string.Format("Location: {0}:00\n", LocationComboBox.SelectedItem as string);
            combinedText[6] = string.Format("Comment/s:\n{0}\n", CommentsBox.Text);

            string newPath = GetSaveToFilePath(filePath);
            string newPathGoodExtension = System.IO.Path.ChangeExtension(newPath,".txt");

            File.WriteAllLines(newPathGoodExtension, combinedText);

        }
        private string GetSaveToFilePath(string imgFilePath)
        {
            //checks for 'labelled images' directory in cur folder
            string directory = System.IO.Path.GetDirectoryName(imgFilePath);
            string[] labelsDir = Directory.GetDirectories(directory, "Labelled Images", SearchOption.TopDirectoryOnly);
            string newDir = System.IO.Path.Combine(directory, "Labelled Images");
            if (labelsDir.Length == 0)
            {
                //create a new folder
                Directory.CreateDirectory(newDir);
            }
            //Change name of file here
            string newName = string.Format("{0} {1} {2:dd-MM-yyyy}.png",
                IDText.Text,
                AreasComboBox.SelectedItem as string,
                DateTime.Now);

            return System.IO.Path.Combine(newDir, newName);

            //
            //return System.IO.Path.Combine(newDir, System.IO.Path.GetFileName(imgFilePath));
            //save to filpath
            //check for existing subfolder
            //check for existing image
        }
        public void setNextImage()
        {
            //if there no next image empty
            if (!filesList.Any())
            {
                //make the image empty
                imgPhoto.Source = null;
            }
            else
            {
                //Used for dealing with .tga images
                string extension = System.IO.Path.GetExtension(filesList[0]);

                if (string.Compare(extension, ".tga") == 0)
                {
                    using (MemoryStream memory = new MemoryStream())
                    {
                        Paloma.TargaImage.LoadTargaImage(filesList[0]).Save(memory, ImageFormat.Bmp);
                        memory.Position = 0;
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = memory;
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.EndInit();
                        imgPhoto.Source = bitmapImage;
                        uneditedImg.Source = bitmapImage;
                    }
                }
                else
                {
                    imgPhoto.Source = new BitmapImage(new Uri(filesList[0]));
                    uneditedImg.Source = new BitmapImage(new Uri(filesList[0]));
                }
                imgPhoto.Stretch = Stretch.Uniform;
            }

            curRect = RectArea;
            setCurComboBox(AreasComboBox);
            
        }
        public void setCurComboBox(ComboBox nextBox)
        {
            //CurComboBox is the identifyer for the currently selected Cbox
            //It's items are passed to the grammar recognizer
            //And its options are made visible through IsDropDown=true
            ResetAllRects();
            if (nextBox == null)
            {
                curComboBox = new ComboBox();
                if (btnOptionsVoiceEnabled.IsChecked)
                {
                    SpeechS.updateChoices(new string[] { });
                }
            }
            else
            {
                //update curComboBox
                curComboBox = nextBox;

                //update speech choices
                List<string> nextBoxItems = new List<string>();
                foreach (string s in nextBox.Items)
                {
                    nextBoxItems.Add(s);
                }
                //check to make sure that the voice options command are turned on
                if (btnOptionsVoiceEnabled.IsChecked)
                {
                    SpeechS.updateChoices(nextBoxItems.ToArray());
                }

                //dropdown menu = true
                nextBox.IsDropDownOpen = true;
            }
        }
        private void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            //match for comboBox
            if (curComboBox.Items.Contains(e.Result.Text))
            {
                //if they don't match then selectedItemChange is triggered
                if (curComboBox.SelectedItem as string != e.Result.Text)
                {
                    curComboBox.SelectedItem = e.Result.Text;
                }
                else
                {

                    dontRunSelectionChanged = true;
                    curComboBox.SelectedItem = null;
                    dontRunSelectionChanged = false;
                    curComboBox.SelectedItem = e.Result.Text;
                }
            }
            else
            {
                switch (e.Result.Text)
                {
                    //update with SpeechSifter.ControlWords
                    case "Write":
                        WriteComment(uneditedImg);
                        break;
                    case "Save":
                        if (filesList.Count == 0) { return; }
                        SaveImageToFile(imgPhoto, filesList[0]);
                        SaveLabelsToText(filesList[0]);
                        filesList.RemoveAt(0);
                        setNextImage();
                        if (btnMediaExpressEnabled.IsChecked)
                        {
                            SetForegroundWindow(MediaExpress.MainWindowHandle);
                        }
                        break;
                    case "Discard":
                        if (filesList.Count == 0) { return; }
                        filesList.RemoveAt(0);
                        setNextImage();
                        if (btnMediaExpressEnabled.IsChecked)
                        {
                            SetForegroundWindow(MediaExpress.MainWindowHandle);
                        }
                        break;
                    case "Take":
                        if (btnMediaExpressEnabled.IsChecked)
                        {
                            SetForegroundWindow(MediaExpress.MainWindowHandle);
                            bmdmHack.PressCtrlG();
                        }

                        break;
                    default:
                        //do nothin
                        break;
                }
            }


        }

        private void UpdateOverlayCombobox()
        {
            List<string> overlayTitles = new List<string>();
            foreach (var overlay in AandCList)
            {
                overlayTitles.Add(overlay.Title);
            }
            OverlayComboBox.ItemsSource = overlayTitles;

        }

        #endregion


        private void btnOverEdit_Click(object sender, RoutedEventArgs e)
        {
            EditAsAndCs eaas = new EditAsAndCs(AandC);
            eaas.ShowDialog();
            ResetComboBoxes();
        }

        private void btnOverNew_Click(object sender, RoutedEventArgs e)
        {
            int preNewCount = AandCList.Count;
            NewAAC newWindow = new NewAAC(AandCList);
            newWindow.ShowDialog();
            int postNewCount = AandCList.Count;
            //If a new overlay type was added then make it the selected one
            if (preNewCount != postNewCount)
            {
                UpdateOverlayCombobox();
                OverlayComboBox.SelectedIndex = AandCList.Count - 1;
            }
            //Else dont do anything
            ResetComboBoxes();
        }

        private void btnOverLoad_Click(object sender, RoutedEventArgs e)
        {

        }


        #region SavingAndLoading
        private void saveLocalJSON()
        {

            
            var fileName = System.IO.Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), "AUSROV", "AreasAndConditions.txt");
            MemoryStream ms = new System.IO.MemoryStream();
            JsonSerializer ser = new JsonSerializer();
            //false implies overwrite don't append
            using (StreamWriter sw = new System.IO.StreamWriter(fileName,false))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                ser.Serialize(writer, AandCList);
            }

        }
        private void LoadLocalJSON()
        {
            AandCList = new ObservableCollection<AreasAndConditions>();
            //Deserializes the local list of Areas and Conditions
            JsonSerializer ser = new JsonSerializer();

            var fileName = System.IO.Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), "AUSROV", "AreasAndConditions.txt");
            if (File.Exists(fileName))
            {
                try
                {
                    using (StreamReader sr = new System.IO.StreamReader(fileName))
                    using (JsonReader reader = new JsonTextReader(sr))
                    {

                        JArray a = (JArray)ser.Deserialize(reader);
                        //Get the individual Areas and conditions
                        foreach (var aac in a)
                        {
                            //Get the title
                            string title = (string)aac.SelectToken("Title");

                            //Pre refactored asandcs
                            /*
                            //create an empty ASANDCS dictionary for
                            Dictionary<string, List<string>> AsAndCsDictTemp = new Dictionary<string, List<string>>();
                            //Convert JSON to
                            IDictionary<string, JToken> AsAndCsTokens = (IDictionary<string, JToken>)aac.SelectToken("AsAndCs");
                            foreach (KeyValuePair<string, JToken> s in AsAndCsTokens)
                            {
                                //Add each element of the dictionay to the temp dictionary
                                AsAndCsDictTemp.Add(s.Key, (List<string>)JsonConvert.DeserializeObject(s.Value.ToString(), typeof(List<string>)));
                            }

                            AandCList.Add(new AreasAndConditions(title, AsAndCsDictTemp));*/
                            //No straight method for Jarray to List

                            List<string> areas = aac.SelectToken("Areas").ToObject<string[]>().ToList<string>();
                            List<string> conditions = aac.SelectToken("Conditions").ToObject<string[]>().ToList<string>();
                            
                            AandCList.Add(new AreasAndConditions(title, areas, conditions));
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(fileName)))
                {
                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fileName));
                }
                File.Create(fileName);
            }
            if (AandCList.Count == 0)
            {
                //Adds the default areas and conditions to the pprogram
                AandCList.Add(new AreasAndConditions());
            }
            
        }
        #endregion


        private void Window_Closed(object sender, EventArgs e)
        {
            saveLocalJSON();
        }

        private void OverlayComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetComboBoxes();
        }

        private void ResetComboBoxes()
        {
            dontRunSelectionChanged = true;
            AandC = AandCList[OverlayComboBox.SelectedIndex];
            AreasComboBox.ItemsSource = AandC.GetAreas();
            CriteriasComboBox.ItemsSource = new string[] { "Select an Area" };
            //comboBox.SelectedIndex = 0;
            //AreasComboBox.SelectedItem = null;
            dontRunSelectionChanged = false;
            curRect = RectArea;
            setCurComboBox(AreasComboBox);
        }

        private void btnOverDeleteCurrent_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Do you want to delete the current overlay set?",
                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                //TODO: Deal with removing the last element in the AandCList
                AandCList.Remove(AandC);
                OverlayComboBox.SelectedIndex = 0;
                UpdateOverlayCombobox();
            }
        }

        private void btnOptionsVoiceEnabled_Click(object sender, RoutedEventArgs e)
        {
            //Turn of sre.recognize if the voice options are disabled
            if (!btnOptionsVoiceEnabled.IsChecked)
            {
                curRect = null;
                SpeechS.StopVoiceCommands();
            }
            else
            {
                curRect = null;
                SpeechS.updateChoices(new string[]{});
            }
        }

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(
            IntPtr hWnd,
            IntPtr hWndInsertAfter,
            int X,
            int Y,
            int cx,
            int cy,
            uint uFlags);

        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;

        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private void btnMediaExpressEnabled_Click(object sender, RoutedEventArgs e)
        {
            if (btnMediaExpressEnabled.IsChecked)
            {
                //if is checked then search for bmme process
                Process[] prc = Process.GetProcessesByName("MediaExpress");
                if (prc.Length > 0)
                {
                    MediaExpress = prc[0];
                    btnMediaExpressEnabled.IsChecked = true;
                }
                else {
                    btnMediaExpressEnabled.IsChecked = false;
                    MessageBox.Show("Blackmagic Media Express could not be found running in the background");
                }
            }
            else
            {
                btnMediaExpressEnabled.IsChecked = false;
            }
        }

    }
}
