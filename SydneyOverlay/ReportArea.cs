using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SydneyOverlay
{
    public class ReportArea
    {
        public string Name { get; set; }
        public string RatingInt { get; set; }
        public string Comment { get; set; }
        public string RatingFlag { get; set; }

        public List<ReportArea> RelatedReports { get; set; }

        public ReportArea()
        {
            RelatedReports = new List<ReportArea>();
        }
        public ReportArea(string name, string ratingInt, string ratingFlag, string comment)
        {
            this.Name = name;
            this.RatingInt = ratingInt;
            this.RatingFlag = ratingFlag;
            this.Comment = comment;
            RelatedReports = new List<ReportArea>();
        }
        public void SetLowestReport(string newRatingFlag, string newRatingInt)
        {
            List<string> ratingFlags = new List<string>() { "A", "F", "D", "NA" };
            List<string> ratingInts = new List<string>() { "0", "1", "2", "3", "4", "NA" };
            
            int newFlagIndex = 10;
            int curFlagIndex = 10;
            int newIntIndex = 10;
            int curIntIndex = 10;
            //Check for indeces and set
            if (ratingFlags.Contains(newRatingFlag))
            {
                newFlagIndex = ratingFlags.IndexOf(newRatingFlag);
            }
            if (ratingFlags.Contains(this.RatingFlag))
            {
                curFlagIndex = ratingFlags.IndexOf(this.RatingFlag);
            }
            if (ratingInts.Contains(newRatingInt))
            {
                newIntIndex = ratingInts.IndexOf(newRatingInt);
            }
            if (ratingInts.Contains(this.RatingInt))
            {
                curIntIndex = ratingInts.IndexOf(this.RatingInt);
            }
            //if new is less than old, change
            if (newFlagIndex < curFlagIndex)
            {
                this.RatingFlag = ratingFlags[newFlagIndex];
            }
            if (newIntIndex < curIntIndex)
            {
                this.RatingInt = ratingInts[newIntIndex];
            }
        }

        public void AddComment(string newComment)
        {
            string trimmedComment = newComment.Trim();

            if (string.IsNullOrEmpty(trimmedComment) ||
                newComment.StartsWith("None")) { return; }

            if (Comment == "Appears to be in good order")
            {
                Comment = "";
            }

            StringBuilder sb = new StringBuilder();
            //if the comments section is not empty
            if (!string.IsNullOrEmpty(this.Comment))
            {
                sb.Append(this.Comment);
                sb.Append(". ");
            }
            sb.Append(trimmedComment);
            this.Comment = sb.ToString();
        }

        public void Summarize()
        {
            //run through them once
            //get the common issues
            //Set rating?

            
                //used for finding multiple instances of the same issue
                Dictionary<string,int> issues = new Dictionary<string,int>();
            //added this report to related reports so only have to summarize the related and overwrite this one
            this.RelatedReports.Add(new ReportArea(this.Name, this.RatingInt,this.RatingFlag,this.Comment));
            sortRelatedReportsByRating();
            this.Comment = "";
            foreach (ReportArea raOther in RelatedReports)
            {
                SetLowestReport(raOther.RatingFlag, raOther.RatingInt);
                //get the string up to the ':'
                if (string.IsNullOrEmpty(raOther.Comment)) { continue; }
                //Get the 
                int index;
                if ((index = raOther.Comment.IndexOf(':'))!=-1)
                {
                    string issue = raOther.Comment.Substring(0,index);
                    //get the string before it (issue)
                    if (issues.ContainsKey(issue))
                    {
                        issues[issue] += 1;
                    }
                    else
                    {
                        issues.Add(issue, 1);
                    }
                }
            }
            //go back through them by key
            foreach (string key in issues.Keys)
            {
                string issueSummary = "";
                //issues count only ever >= 1
                if (issues[key] > 1)
                {
                    issueSummary += string.Format("Multiple cases of {0} at {1}:", key, this.Name);
                    foreach (ReportArea ra in this.RelatedReports)
                    {
                        if (ra.Comment.StartsWith(key))
                        {
                            string justComment = ra.Comment.Substring(key.Length + 1).Trim();
                            //don't add empty comments or repeated comments
                            //Better verification possible here:
                            //Smaller comments may be within larger ones and skipped
                            if (!string.IsNullOrEmpty(justComment) &&
                                !issueSummary.Contains(justComment))
                            {

                                issueSummary += justComment + ". ";
                            }
                                
                        }
                    }
                }
                else
                {
                    //foreach here to catch multiples of issue
                    foreach (ReportArea ra in this.RelatedReports)
                    {
                        if (ra.Comment.StartsWith(key))
                        {
                            issueSummary += ra.Comment;
                        }
                    }
                }
                this.Comment += string.Format("({0}) {1}",this.RatingInt, issueSummary);

            }
            //else no issues
        }

        public void summarize2()
        {
            //run through them once
            //get the common issues
            //Set rating?

            
            //used for finding multiple instances of the same issue
            Dictionary<string,int> issues = new Dictionary<string,int>();
            //added this report to related reports so only have to summarize the related and overwrite this one
            //this.RelatedReports.Add(new ReportArea(this.Name, this.RatingInt,this.RatingFlag,this.Comment));
            //sortRelatedReportsByRating();
            this.Comment = "";

            List<string> unSortedComments = new List<string>();
            //two layers of related reports
            foreach (ReportArea raOther in RelatedReports)
            {
                raOther.RelatedReports.Add(raOther);
                foreach (ReportArea raSubOther in raOther.RelatedReports)
                {
                    SetLowestReport(raSubOther.RatingFlag, raSubOther.RatingInt);
                    //get the string up to the ':'
                    if (string.IsNullOrEmpty(raSubOther.Comment)) { continue; }
                    //Get the 
                    int index;
                    if ((index = raSubOther.Comment.IndexOf(':')) != -1)
                    {
                        string issue = raSubOther.Comment.Substring(0, index);
                        //get the string before it (issue)
                        if (issues.ContainsKey(issue))
                        {
                            issues[issue] += 1;
                        }
                        else
                        {
                            issues.Add(issue, 1);
                        }
                    }

                }
            }
            //go back through them by key
            foreach (string key in issues.Keys)
            {
                string issueSummary = "";
                string curRatingInt = "4";
                string curRatingFlag = "D";

                //issues count only ever >= 1
                if (issues[key] > 1)
                {
                    List<string> areas = new List<string>();
                    string justComments = "";
                    //issueSummary += string.Format("Multiple cases of {0} at {1}:", key, this.Name);
                    foreach (ReportArea ra in this.RelatedReports)
                    {
                        foreach (ReportArea raSub in ra.RelatedReports)
                        {
                            if (raSub.Comment.StartsWith(key))
                            {
                                SetLowestReport(ref curRatingInt, ref curRatingFlag, raSub.RatingFlag, raSub.RatingInt);
                                if (!areas.Contains(raSub.Name))
                                {
                                    areas.Add(raSub.Name);
                                }
                                string justComment = raSub.Comment.Substring(key.Length + 1).Trim();
                                //don't add empty comments or repeated comments
                                //Better verification possible here:
                                //Smaller comments may be within larger ones and skipped
                                if(justComment.Contains("Pit cover left o")){};
                                if (!string.IsNullOrEmpty(justComment) &&
                                    !justComments.Contains(justComment))
                                {
                                    justComment = justComment.TrimEnd('.');
                                    justComments += justComment + ". ";
                                }

                            }
                        }
                    }
                    issueSummary += string.Format("{0} at ", UppercaseFirst(key));
                    //add list of names
                    foreach(string name in areas){
                        if (areas.IndexOf(name) == areas.Count - 2)
                        {
                            issueSummary += name.ToLower() + " and ";
                        }
                        else if (areas.IndexOf(name) == areas.Count - 1)
                        {
                            issueSummary += name.ToLower();
                        }
                        else
                        {
                            issueSummary += name.ToLower() + ", ";
                        }
                    }
                    if (string.IsNullOrEmpty(justComments))
                    {
                        issueSummary += ".";
                    }
                    else
                    {
                        issueSummary += ": " + justComments;
                    }

                }
                else
                {
                    foreach (ReportArea ra in this.RelatedReports)
                    {
                        foreach (ReportArea raSub in ra.RelatedReports)
                        {
                            if (raSub.Comment.StartsWith(key))
                            {
                                issueSummary += string.Format("{0} at {1}", UppercaseFirst(key), raSub.Name.ToLower());
                                string justComment = raSub.Comment.Substring(key.Length + 1).Trim();
                                if (string.IsNullOrEmpty(justComment))
                                {
                                    issueSummary += ". ";
                                }
                                else
                                {
                                    issueSummary += ": " + justComment;
                                }


                                SetLowestReport(ref curRatingInt, ref curRatingFlag, raSub.RatingFlag, raSub.RatingInt);
                                break;
                            }
                        }

                    }
                }
                unSortedComments.Add(string.Format("({0}) {1}", curRatingInt, issueSummary));
                

            }
            //else no issues
            this.Comment = sortCommentsByRating(unSortedComments);
            if (string.IsNullOrWhiteSpace(this.Comment))
            {
                this.Comment = "Appears to be in good order.";
            }
        }

        private static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            s = s.ToLower();
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        private string sortCommentsByRating(List<string> unsortedComments)
        {

            List<string> ratingInts = new List<string>() { "0", "1", "2", "3", "4", "NA" };
            List<string> sortedComments = new List<string>();
            foreach (string rating in ratingInts)
            {
                for (int i = 0; i < unsortedComments.Count; i++)
                {
                    if (unsortedComments[i][1].ToString() == rating)
                    {
                        sortedComments.Add(unsortedComments[i]);
                        unsortedComments.RemoveAt(i);
                        i--;
                    }
                }
            }

            //add any other left over reports
            if (unsortedComments.Count > 0)
            {
                foreach (string comment in unsortedComments)
                {
                    sortedComments.Add(comment);
                }
            }
            string output = "";
            foreach (string comment in sortedComments)
            {
                output += comment.Trim() + " ";
            }
            return output;

        }
        
        public void sortRelatedReportsByRating()
        {
            //called from Summarize. related reports includes a copy of this
            List<string> ratingInts = new List<string>() { "0", "1", "2", "3", "4", "NA" };
            List<ReportArea> sortedList = new List<ReportArea>();
            foreach (string rating in ratingInts)
            {
                for(int i=0; i<RelatedReports.Count; i++)
                {
                    List<int> matchingIndexes = new List<int>();
                    if (RelatedReports[i].RatingInt == rating)
                    {
                        sortedList.Add(RelatedReports[i]);
                        RelatedReports.RemoveAt(i);
                        i--;
                    }
                }
            }

            //add any other left over reports
            if (RelatedReports.Count > 0)
            {
                foreach (ReportArea ra in RelatedReports)
                {
                    sortedList.Add(ra);
                }
            }

            this.RelatedReports = sortedList;
        }

        public void SetLowestReport(ref string curRatingInt, ref string curRatingFlag, string newRatingFlag, string newRatingInt)
        {
            List<string> ratingFlags = new List<string>() { "A", "F", "D", "NA" };
            List<string> ratingInts = new List<string>() { "0", "1", "2", "3", "4", "NA" };

            int newFlagIndex = 10;
            int curFlagIndex = 10;
            int newIntIndex = 10;
            int curIntIndex = 10;
            //Check for indeces and set
            if (ratingFlags.Contains(newRatingFlag))
            {
                newFlagIndex = ratingFlags.IndexOf(newRatingFlag);
            }
            if (ratingFlags.Contains(curRatingFlag))
            {
                curFlagIndex = ratingFlags.IndexOf(curRatingFlag);
            }
            if (ratingInts.Contains(newRatingInt))
            {
                newIntIndex = ratingInts.IndexOf(newRatingInt);
            }
            if (ratingInts.Contains(curRatingInt))
            {
                curIntIndex = ratingInts.IndexOf(curRatingInt);
            }
            //if new is less than old, change
            if (newFlagIndex < curFlagIndex)
            {
                curRatingFlag = ratingFlags[newFlagIndex];
            }
            if (newIntIndex < curIntIndex)
            {
                curRatingInt = ratingInts[newIntIndex];
            }
        }
    }
}
