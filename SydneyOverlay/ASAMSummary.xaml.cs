using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.IO;

namespace SydneyOverlay
{
    /// <summary>
    /// Interaction logic for ASAMSummary.xaml
    /// </summary>
    public partial class ASAMSummary : Window
    {

        ObservableCollection<ReportArea> ReportAreasExt;
        ObservableCollection<ReportArea> ReportAreasInt;
        ObservableCollection<ReportArea> ReportAreasGen;

        List<string> RatingFlags;
        List<string> RatingInts;
        public ASAMSummary()
        {
            ReportAreasExt = new ObservableCollection<ReportArea>();
            ReportAreasInt = new ObservableCollection<ReportArea>();
            ReportAreasGen = new ObservableCollection<ReportArea>();
            InitReportAreas();
            InitRatings();
            InitializeComponent();
            //this.DataContext = ReportAreas;
            ASAMExternalData.ItemsSource = ReportAreasExt;
            ASAMInternalData.ItemsSource = ReportAreasInt;
            ASAMGeneralData.ItemsSource = ReportAreasGen;
            RatingFlagColExt.ItemsSource = RatingFlags;
            RatingFlagColInt.ItemsSource = RatingFlags;
            RatingFlagColGen.ItemsSource = RatingFlags;
            RatingIntColExt.ItemsSource = RatingInts;
            RatingIntColInt.ItemsSource = RatingInts;
            RatingIntColGen.ItemsSource = RatingInts;
        }
        private void InitRatings()
        {
            RatingInts = new List<string>();
            RatingInts.Add("0");
            RatingInts.Add("1");
            RatingInts.Add("2");
            RatingInts.Add("3");
            RatingInts.Add("4");
            RatingInts.Add("NA");

            RatingFlags = new List<string>();
            RatingFlags.Add("D");
            RatingFlags.Add("F");
            RatingFlags.Add("A");
            RatingFlags.Add("NA");
        }

        private void InitReportAreas()
        {
            ReportAreasExt.Add(new ReportArea() { Name = "Compound", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasExt.Add(new ReportArea() { Name = "Vandalism", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasExt.Add(new ReportArea() { Name = "Walls", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasExt.Add(new ReportArea() { Name = "Ladder External", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasExt.Add(new ReportArea() { Name = "Entry Hatch", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasExt.Add(new ReportArea() { Name = "Roof Platforms", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasExt.Add(new ReportArea() { Name = "Walkways", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasExt.Add(new ReportArea() { Name = "Roof", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasExt.Add(new ReportArea() { Name = "Roof Hatches", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasExt.Add(new ReportArea() { Name = "Handrails", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasExt.Add(new ReportArea() { Name = "Davit", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasExt.Add(new ReportArea() { Name = "Ventilation", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasExt.Add(new ReportArea() { Name = "Bird Proofing", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasExt.Add(new ReportArea() { Name = "Electrical", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasExt.Add(new ReportArea() { Name = "Level indicator", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });


            ReportAreasInt.Add(new ReportArea() { Name = "Walls", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasInt.Add(new ReportArea() { Name = "Columns", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasInt.Add(new ReportArea() { Name = "Roof Spider", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasInt.Add(new ReportArea() { Name = "Roof Framing", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasInt.Add(new ReportArea() { Name = "Floor", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasInt.Add(new ReportArea() { Name = "Inlet", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasInt.Add(new ReportArea() { Name = "Outlet", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasInt.Add(new ReportArea() { Name = "Scour", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasInt.Add(new ReportArea() { Name = "Overflow", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasInt.Add(new ReportArea() { Name = "Mixer Motor", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasInt.Add(new ReportArea() { Name = "Motor Type", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasInt.Add(new ReportArea() { Name = "Supports", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasInt.Add(new ReportArea() { Name = "Supports Type", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasInt.Add(new ReportArea() { Name = "Ladder Internal", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasInt.Add(new ReportArea() { Name = "Electrical", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });


            ReportAreasGen.Add(new ReportArea() { Name = "Security", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasGen.Add(new ReportArea() { Name = "Safety", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasGen.Add(new ReportArea() { Name = "Contamination", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasGen.Add(new ReportArea() { Name = "Confined Space", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });
            ReportAreasGen.Add(new ReportArea() { Name = "Vehicle Access", RatingInt = "4", RatingFlag = "NA", Comment = "No comments section" });
            ReportAreasGen.Add(new ReportArea() { Name = "Tanker Access", RatingInt = "4", RatingFlag = "NA", Comment = "No comments section" });
            ReportAreasGen.Add(new ReportArea() { Name = "Lifting Access", RatingInt = "4", RatingFlag = "NA", Comment = "No comments section" });
            ReportAreasGen.Add(new ReportArea() { Name = "Site Access", RatingInt = "4", RatingFlag = "NA", Comment = "No comments section" });
            ReportAreasGen.Add(new ReportArea() { Name = "Work Platform", RatingInt = "4", RatingFlag = "NA", Comment = "No comments section" });
            ReportAreasGen.Add(new ReportArea() { Name = "Ext Ladder Cond", RatingInt = "4", RatingFlag = "NA", Comment = "No comments section" });
            ReportAreasGen.Add(new ReportArea() { Name = "Int Ladder Cond", RatingInt = "4", RatingFlag = "NA", Comment = "No comments section" });
            ReportAreasGen.Add(new ReportArea() { Name = "Structure Int", RatingInt = "4", RatingFlag = "NA", Comment = "No comments section" });
            ReportAreasGen.Add(new ReportArea() { Name = "Coatings Int", RatingInt = "4", RatingFlag = "NA", Comment = "No comments section" });
            ReportAreasGen.Add(new ReportArea() { Name = "Structure Ext", RatingInt = "4", RatingFlag = "NA", Comment = "No comments section" });
            ReportAreasGen.Add(new ReportArea() { Name = "Coatings Ext", RatingInt = "4", RatingFlag = "NA", Comment = "No comments section" });
        }

        private void btnExtSum_Click(object sender, RoutedEventArgs e)
        {
            WriteComments(CommentsExt, ReportAreasExt);
        }

        private void btnIntSum_Click(object sender, RoutedEventArgs e)
        {
            WriteComments(CommentsInternal, ReportAreasInt);
        }

        private void btnGenSum_Click(object sender, RoutedEventArgs e)
        {

            WriteComments(CommentsGeneral, ReportAreasGen);
            AddInternalAndExternalToGeneral();
        }

        private void AddInternalAndExternalToGeneral()
        {
            //TODO refactor CommentsExt to CommentsExternal
            CommentsGeneral.Text += CommentsExt.Text;
            CommentsGeneral.Text += CommentsInternal.Text;
        }
        private void WriteComments(TextBox commentsSection, ObservableCollection<ReportArea> AreasLocations)
        {
            commentsSection.Text = "";
            ObservableCollection<ReportArea> problemAreas = new ObservableCollection<ReportArea>();
            foreach (ReportArea a in AreasLocations)
            {
                if(a.Comment != "Appears to be in good order" &&
                    a.Comment != "No comments section")
                {
                    problemAreas.Add(a);
                }
            }
            foreach (ReportArea a in problemAreas)
            {
                //TODO: sort by rating
                commentsSection.Text += String.Format("{0}: {1}\n", a.Name, a.Comment);
            }

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "TankSummary"; // Default file name
            dlg.DefaultExt = ".text"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                //Save
                File.WriteAllText(dlg.FileName, GetContents());
            }
            
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private string GetContents()
        {
            //should have used stringbuilder here
            string contents = "";
            contents += ObservableReportAreaToString(ReportAreasExt);
            contents += ObservableReportAreaToString(ReportAreasInt);
            contents += ObservableReportAreaToString(ReportAreasGen);
            //added newlines at start because ddon't have total control over comments sections
            contents += Environment.NewLine + "ExternalComments:" + Environment.NewLine;
            contents += TextBoxToEnvironment(CommentsExt);
            contents += Environment.NewLine + "InternalComments:" + Environment.NewLine;
            contents += TextBoxToEnvironment(CommentsInternal);
            contents += Environment.NewLine + "GeneralComments:" + Environment.NewLine;
            contents += TextBoxToEnvironment(CommentsGeneral);

            return contents;
        }

        private string ObservableReportAreaToString(ObservableCollection<ReportArea> ocra)
        {
            string output = "";
            foreach (ReportArea a in ocra)
            {
                output += string.Format("{0}: {1} {2} {3}{4}",a.Name,a.RatingInt,a.RatingFlag,a.Comment,Environment.NewLine);
            }
            return output;
        }

        private string TextBoxToEnvironment(TextBox box)
        {
            string output = "";
            string[] lines = box.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                output += line + Environment.NewLine;
            }
            return output;
        }
        
    }
    
    public class ReportArea
    {
        public string Name { get; set; }
        public string RatingInt { get; set; }
        public string Comment { get; set; }
        public string RatingFlag { get; set; }
    }
}
