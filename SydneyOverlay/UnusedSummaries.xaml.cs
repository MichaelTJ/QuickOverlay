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

namespace SydneyOverlay
{
    /// <summary>C:\Users\Sandy\Documents\Visual Studio 2013\Projects\QuickOverlay\SydneyOverlay\UnusedSummaries.xaml
    /// Interaction logic for UnusedSummaries.xaml
    /// </summary>
    public partial class UnusedSummaries : Window
    {
        ObservableCollection<ReportArea> ReportAreasExt;
        ObservableCollection<ReportArea> ReportAreasInt;

        public List<string> RatingInts { get; set; }
        public List<string> RatingFlags { get; set; }

        public UnusedSummaries(List<ReportArea> internalReports, List<ReportArea> externalReports)
        {
            InitializeComponent();
            InitRatings();
            ReportAreasExt = new ObservableCollection<ReportArea>(externalReports);
            ReportAreasInt = new ObservableCollection<ReportArea>(internalReports);

            RatingFlagColExt.ItemsSource = RatingFlags;
            RatingFlagColInt.ItemsSource = RatingFlags;

            RatingIntColExt.ItemsSource = RatingInts;
            RatingIntColInt.ItemsSource = RatingInts;

            ASAMExternalData.ItemsSource = ReportAreasExt;
            ASAMInternalData.ItemsSource = ReportAreasInt;

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

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnExtSum_Click(object sender, RoutedEventArgs e)
        {
            WriteComments(CommentsExt, ReportAreasExt);
        }

        private void btnIntSum_Click(object sender, RoutedEventArgs e)
        {
            WriteComments(CommentsInternal, ReportAreasInt);
        }

        private void WriteComments(TextBox commentsSection, ObservableCollection<ReportArea> AreasLocations)
        {
            //repeated code from ASAMSummmary
            commentsSection.Text = "";
            ObservableCollection<ReportArea> problemAreas = new ObservableCollection<ReportArea>();
            foreach (ReportArea a in AreasLocations)
            {
                if (a.Comment != "Appears to be in good order" &&
                    a.Comment != "No comments section" &&
                    a.Comment != "")
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
    }
}
