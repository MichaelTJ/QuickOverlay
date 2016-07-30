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
using System.Threading;
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

        Dictionary<string, string> InternalMapping;
        Dictionary<string, string> ExternalMapping;

        UnusedSummaries us;
        public bool LoadUnusedSummaries = false;
        List<ReportArea> InternalSummaries;
        List<ReportArea> ExternalSummaries;

        List<string> RatingFlags;
        List<string> RatingInts;
        public ASAMSummary()
        {
            InitASAMSummary();
        }
        public ASAMSummary(string externalFolderPath, string internalFolderPath)
        {
            InitASAMSummary();
            InitMapping();
            string[] intFilePaths = Directory.GetFiles(internalFolderPath, "*.txt");
            string[] extFilePaths = Directory.GetFiles(externalFolderPath, "*.txt");

            StringBuilder sb = new StringBuilder();
            sb.Append("Internal summaries:");
            sb.AppendLine();
            InternalSummaries = GetSummaries(intFilePaths);
            ExternalSummaries = GetSummaries(extFilePaths);

            //Process summaries
            processSummaries(ref InternalSummaries, ref ReportAreasInt, InternalMapping);
            processSummaries(ref ExternalSummaries, ref ReportAreasExt, ExternalMapping);

            //this window has to be open before can make new one
            LoadUnusedSummaries = true;

        }
        /// <summary>
        /// For the list of summaries, see if they map to something in the observable collection. If
        /// it does: add the report summary comment, set the lowest report rating and remove it from the
        /// summaries list.
        /// </summary>
        /// <param name="summaries"></param>
        /// <param name="relatedCollection"></param>
        /// <param name="mapping"></param>
        private void processSummaries(ref List<ReportArea> summaries, ref ObservableCollection<ReportArea> relatedCollection, Dictionary<string,string> mapping)
        {
            //For each summary
            for (int i = 0; i < summaries.Count; i++)
            {
                //If the summary has a matching ASAM name
                if (mapping.ContainsKey(summaries[i].Name))
                {
                    //Find the name
                    foreach (ReportArea ra in relatedCollection)
                    {
                        if (ra.Name == mapping[summaries[i].Name])
                        {
                            ra.RelatedReports.Add(summaries[i]);
                            //ra.AddComment(string.Format("{0}: {1}", summaries[i].Name, summaries[i].Comment));
                            //ra.SetLowestReport(summaries[i].RatingFlag,summaries[i].RatingInt);
                            summaries.RemoveAt(i);
                            i--;
                            break;
                        }
                    }
                }
            }


            foreach (ReportArea ra in relatedCollection)
            {
                ra.summarize2();
            }
        }

        //Summaries dont summarize atm
        private List<ReportArea> GetSummaries(string[] filePaths)
        {
            List<ReportArea> reportAreasSummary = new List<ReportArea>();
            //for each txt file
            foreach (string path in filePaths)
            {
                //Get the report
                ReportArea newReport = GetReportArea(path);
                //Learn Linq
                //check to see if it is existing
                bool IsExisting = false;
                foreach(ReportArea existingReport in reportAreasSummary){
                    if (newReport.Name == existingReport.Name)
                    {
                        existingReport.RelatedReports.Add(newReport);
                        IsExisting = true;
                        break;
                    }
                }
                if (!IsExisting)
                {
                    reportAreasSummary.Add(newReport);
                }
            }
            foreach (ReportArea ra in reportAreasSummary)
            {
                //ra.Summarize();
            }
            return reportAreasSummary;
        }

        private ReportArea GetReportArea(string filePath)
        {
            //This could be a function of the ReportArea class (constructor even)
            string[] lines = File.ReadAllLines(filePath);
            ReportArea a = new ReportArea();
            string issue = "";
            foreach (string line in lines)
            {
                if (line.StartsWith("Area: "))
                {
                    a.Name = line.Substring(6);
                }
                else if (line.StartsWith("Rating: "))
                {
                    a.RatingInt = line.Substring(8);
                }
                else if (line.StartsWith("Issue: "))
                {
                    //issue should always come before comments
                    if (issue.StartsWith("None") || issue.StartsWith(@"N/A"))
                    {
                        issue = "";
                    }
                    issue = line.Substring(7) + ": ";
                }
                else if (line.StartsWith("Comment/s:"))
                {
                    int curIndex = Array.IndexOf(lines,line);
                    //parsing for comment
                    string newComment = issue;
                    //if the string is long enough
                    if (lines[curIndex].Length > 10 ||
                        !string.IsNullOrEmpty(lines[curIndex+1].Trim()))
                    {
                        //Get first line of comments
                        newComment += lines[curIndex].Substring(10).Trim();
                        //if there's more than one line
                        if (lines.Length > curIndex + 1)
                        {
                            //add the rest of the lines
                            for (int i = curIndex + 1; i < lines.Length; i++)
                            {
                                newComment += lines[i].Trim() + " ";
                            }
                            newComment = newComment.Trim() + ". ";
                        }
                    }
                    if(newComment.StartsWith("No Defect Visible:") ||
                        newComment.StartsWith("None:"))
                    {
                        a.Comment = "";
                    }
                    else
                    {
                        a.Comment = newComment;
                    }
                    
                }
            }
            if (string.IsNullOrEmpty(a.RatingInt)) { a.RatingInt = "NA"; }
            //set the rating
            switch (a.RatingInt)
            {
                case "0":
                case "1":
                    {
                        a.RatingFlag = "A";
                        break;
                    }
                case "2":
                case "3":
                    {
                        a.RatingFlag = "F";
                        break;
                    }
                case "4":
                    {
                        a.RatingFlag = "D";
                        break;
                    }
                default:
                    {
                        //TODO check this is the right NA for ASAM upload
                        a.RatingFlag = "NA";
                        break;
                    }
            }
            return a;
        }
        private void InitASAMSummary()
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

        private void InitMapping()
        {
            //TODO: JSON this goddamn

            //Mapping list for AUSROV custom areas to ASAM areas
            ExternalMapping = new Dictionary<string, string>(){
                {"Chlorination Hatch","Roof Hatches"},
                {"Electrical","Electrical"},
                {"External Coating","Walls"},
                {"External Ladder","Ladder External"},
                {"External Wall","Walls"},
                {"Floor Seal","Walls"},
                {"Footpath","Compound"},
                {"Handrail","Handrails"},
                {"Hold down clamp","Walls"},
                {"Level Indicator","Level Indicator"},
                {"Reservoir Profile","Compound"},
                {"Roof Platform","Roof Platforms"},
                {"Platform Area","Roof Platforms"},
                {"Roof Sheets","Roof"},
                {"Rooftop Entry Hatch","Entry Hatch"},
                {"Rooftop Equipment Hatch","Roof Hatches"},
                {"Security","Compound"},
                {"Side Access Gate","Compound"},
                {"Top Ring Beam","Handrails"},
                {"Vent","Ventilation"},
                {"Walkway","Walkways"},
                {"Davit","Davit"},
                {"Anchor Point","Walkways"},
                {"Annular","Walls"},
                {"Cathodic Protection","Electrical"},
                {"Compound","Compound"},
                {"Inlet","Walls"},
                {"Kick Plate","Walkways"},
                {"Mixer Frame","Compound"},
                {"Outlet","Walls"},
                {"Overflow","Walls"},
                {"Roof","Roof"},
                {"Roof Flashing","Roof"},
                {"Sample Point","Walls"},
                {"Stairs","Ladder External"},
                {"Emergency Hatch","Roof Hatches"},
                {"Instrument Hatch","Roof Hatches"},
                {"Pressure Valve","Walls"},
                {"Roof Beam","Walls"},
                {"External Structure","Walls"},
                {"Purlin","Roof"},
                {"Roof Spider","Roof"},
                {"Tap","Walls"},
                {"Chlorine Residual Analyser","Walls"},
                {"Foundation","Walls"},
                {"Valve Pit","Compound"},
                {"Vermin Proofing","Bird Proofing"},
                {"Depth Gauge", "Walls"},
                {"Dosing Point","Walls"},
                {"Drain","Walkways"},
                {"Leak Detection","Walls"},
                {"Flashing","Roof"},
                {"Roof Hatch","Roof"},
                {"External Pipework","Compound"},
                {"Mixer Support Frame","Roof"},
                {"Rain Guage","Compound"},
                {"Rafter","Roof"},
                {"Re-Chlorination Line","Walls"},
                {"Side Access Hatch","Walls"},
                {"Tank Base","Walls"},
                {"Vandalism","Vandalism"},
                {"Ventilation Hatch","Roof Hatches"},
                {"Base Plate","Walls"},
                {"Access Door","Walls"},
                {"Internal Roof","Roof"},
                {"Internal Stairs","Ladder External"},
                {"Internal Wall","Walls"},
                {"Walls","Walls"},
                {"Dives Outlet","Walls"},
                {"Pressure Transmitter","Electrical"},
                {"Mixer Mast","Supports"},
                {"Bottom Ring Beam","Walkways"},
                {"Internal Ladder","Ladder External"},
                {"Rooftop Hatch","Roof Hatches"},
                {"Support Legs","Walls"},
                {"Support Structure","Walls"},
                {"Ventilation","Ventilation"}
            };


            InternalMapping = new Dictionary<string, string>(){
                {"Column","Columns"},
                {"Column Base","Columns"},
                {"Entry Hatch","Roof Framing"},
                {"Floor","Floor"},
                {"Floor Joint","Floor"},
                {"Inlet","Inlet"},
                {"Internal Ladder","Ladder Internal"},
                {"Internal Roof","Roof Framing"},
                {"Internal Wall","Walls"},
                {"Outlet","Outlet"},
                {"Overflow","Overflow"},
                {"Platform Area","Roof Framing"},
                {"Roof Framing","Roof Framing"},
                {"Roof Spider","Roof Spider"},
                {"Sample Point","Outlet"},
                {"Scour","Scour"},
                {"Wall Floor Interface","Walls"},
                {"Chlorine Hatch","Roof Framing"},
                {"Cross Bracing","Roof Framing"},
                {"Mixer Hatch","Roof Framing"},
                {"Mixer Motor","Mixer Motor"},
                {"Mixer Support Frame","Supports"},
                {"Rafter","Roof Framing"},
                {"Re-Chlorination Inlet","Inlet"},
                {"Side Access Hatch","Walls"},
                {"Ventilation","Roof Framing"},
                {"Wall Roof Interface","Walls"},
                {"Emergency Hatch","Roof Framing"},
                {"Pressure Guage","Walls"},
                {"Tap","Walls"},
                {"Instrument Hatch","Roof Framing"},
                {"Sample or Dosing Point","Walls"},
                {"Cathodic Protection","Electrical"},
                {"Chlorine Cannister","Supports"},
                {"Level Indicator","Supports"},
                {"Mixer Mast","Supports"},
                {"Davit","Supports"},
                {"Chlorination Hatch","Roof Framing"},
                {"Depth Guage","Walls"},
                {"Internal Coating","Walls"},
                {"Internal Hydrant","Floor"},
                {"Mixer Base","Floor"},
                {"Dosing Line","Floor"},
                {"Internal Stairs","Ladder Internal"},
                {"Mixer Blades","Mixer Motor"},
                {"Purlin","Roof Framing"},
                {"Floor Seal","Floor"},
                {"Redundant Pipework","Walls"},
                {"Re-Chlorination Framing","Supports"},
                {"Chlorine Dosing Line","Supports"},
                {"Dives Outlet","Walls"},
                {"Chlorine Residual Return","Inlet"},
                {"Flow Meter","Electrical"},
                {"Equipment Hatch","Roof Framing"},
                {"Instruments","Supports"}

            };

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
            ReportAreasExt.Add(new ReportArea() { Name = "Level Indicator", RatingInt = "4", RatingFlag = "D", Comment = "Appears to be in good order" });


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

            ReportArea usedForSorting = new ReportArea();
            foreach (ReportArea a in AreasLocations)
            {
                usedForSorting.RelatedReports.Add(a);
            }
            usedForSorting.sortRelatedReportsByRating();
            foreach (ReportArea a in usedForSorting.RelatedReports)
            {
                //TODO: sort by rating
                string betterString = "";
                if (a.RatingInt == "0" || a.RatingInt == "1" || a.RatingInt == "2")
                {
                    int index;
                    
                    if((index = a.Comment.IndexOf("(3)")) != -1){
                        betterString = a.Comment.Substring(0, index);
                    }
                    else
                    {
                        betterString = a.Comment;
                    }
                    commentsSection.Text += String.Format("{0}: {1}\n", a.Name, betterString);
                }
                
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
            contents += Environment.NewLine + "External:" + Environment.NewLine;
            contents += ObservableReportAreaToString(ReportAreasExt);
            //added newlines at start because ddon't have total control over comments sections
            contents += Environment.NewLine + "ExternalComments:" + Environment.NewLine;
            contents += TextBoxToEnvironment(CommentsExt);

            contents += Environment.NewLine + "Internal:" + Environment.NewLine;
            contents += ObservableReportAreaToString(ReportAreasInt);
            contents += Environment.NewLine + "InternalComments:" + Environment.NewLine;
            contents += TextBoxToEnvironment(CommentsInternal);

            contents += Environment.NewLine + "General:" + Environment.NewLine;
            contents += ObservableReportAreaToString(ReportAreasGen);
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(LoadUnusedSummaries){
                Thread thread = new Thread(() =>
                {
                    us = new UnusedSummaries(InternalSummaries, ExternalSummaries);
                    us.Show();

                    us.Closed += (sender2, e2) =>
                        us.Dispatcher.InvokeShutdown();

                    System.Windows.Threading.Dispatcher.Run();

                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();

                //piggybacking LoadUnusedSummaries trigger
                WriteComments(CommentsExt, ReportAreasExt);
                WriteComments(CommentsInternal, ReportAreasInt);
                WriteComments(CommentsGeneral, ReportAreasGen);
                AddInternalAndExternalToGeneral();
            }
        }


    }
    
}
