//Michael jensen 2015
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
namespace SydneyOverlay
{
    public class AreasAndConditions
    {

        //Made public only so that the JsonWriter can read variables
        public List<string> Areas;
        public List<string> Conditions;
            
            //Dictionary<string, List<string>> AsAndCs;
        public string Title{get;set;}

        public AreasAndConditions()
        {
            //AsAndCs = new Dictionary<string, List<string>>();
            this.Title = "Standard Tank Inspection";

            this.Areas = new List<string>{ "Walls Internal",
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

            this.Conditions = new List<string>{"Corrosion",
                                  "Paint Cracking",
                                  "Other"};
            /*
            Random RandString = new Random();
            foreach (string s in areas)
            {
                //Adding the condition and 3 random strings
                AsAndCs.Add(s, new List<string>());
                AsAndCs[s].Add(conditions[RandString.Next(0, conditions.Length)]);
                AsAndCs[s].Add(conditions[RandString.Next(0, conditions.Length)]);
                AsAndCs[s].Add(conditions[RandString.Next(0, conditions.Length)]);
            }*/
            sortAreasAndConditions();
        }

        public void sortAreasAndConditions()
        {
            Areas = Areas.OrderBy(a => a).ToList();
            Conditions = Conditions.OrderBy(a => a).ToList();
        }

        public AreasAndConditions(string titleIn, List<string> areas, List<string> conditions)
        {
            this.Title = titleIn;
            this.Areas = areas;
            this.Conditions = conditions;
            //AsAndCs = AsAndCsIn;
            sortAreasAndConditions();
        }

        

        public AreasAndConditions(string title)
        {
            //AsAndCs = new Dictionary<string, List<string>>();
            this.Areas = new List<string>();
            this.Conditions = new List<string>();
            this.Title = title;
        }
        public List<string> GetAreas()
        {
            //return AsAndCs.Keys.ToList();
            return Areas;
        }

        public List<string> GetConditions(string area)
        {
            //return AsAndCs[area];
            return Conditions;
        }

        //
        public void AddArea(string area)
        {
            /*
            if (AsAndCs.ContainsKey(Area))
            {
                System.Windows.MessageBox.Show(String.Format("That area {0} already exists.", Area));
                return;
            }

            AsAndCs.Add(Area, new List<string>());
             * */
            if (Areas.Contains(area))
            {
                System.Windows.MessageBox.Show(String.Format("That area {0} already exists.", area));
            }
            else { Areas.Add(area); }

        }
        public void AddCondition(string condition)
        {
            /*
            //TODO: Add exceptionhandler if there's no selected key in areas
            if (AsAndCs.ContainsKey(Area))
            {
                AsAndCs[Area].Add(Condition);
            }*/

            if (Conditions.Contains(condition))
            {
                System.Windows.MessageBox.Show(String.Format("The condition {0} already exists.", condition));
            }
            else { Conditions.Add(condition); }
            
        }
        public void RemoveArea(string area)
        {
            /*
            if (AsAndCs.ContainsKey(Area))
            {
                AsAndCs.Remove(Area);
            }*/
            if (Areas.Contains(area))
            {
                Areas.Remove(area);
            }
            else { System.Windows.MessageBox.Show("That area does not exist."); }
        }
        public void RemoveCondition(string condition)
        {
            /*
            if (AsAndCs.ContainsKey(Area))
            {
                if (AsAndCs[Area].Contains(Condition))
                {
                    AsAndCs[Area].Remove(Condition);
                }
            }*/
            if (Conditions.Contains(condition))
            {
                Conditions.Remove(condition);
            }
            else { System.Windows.MessageBox.Show("That condition does not exist."); }

        }

        public void UpdateArea(string oldArea, string newArea)
        {
            /*
            if (AsAndCs.ContainsKey(oldArea))
            {
                List<string> oldList = AsAndCs[oldArea];
                AsAndCs.Remove(oldArea);
                AsAndCs.Add(newArea, oldList);
            }*/
            if (Areas.Contains(oldArea))
            {
                Areas.Remove(oldArea);
                Areas.Add(newArea);
            }
        }

        public void UpdateCondition(string oldCondition, string newCondition)
        {
            /*
            if (AsAndCs.ContainsKey(Area))
            {
                if (AsAndCs[Area].Contains(oldCondition))
                {
                    AsAndCs[Area].Remove(oldCondition);
                    AsAndCs[Area].Add(newCondition);
                }
            }*/
            if (Conditions.Contains(oldCondition))
            {
                Conditions.Remove(oldCondition);
                Conditions.Add(newCondition);
            }
        }
    }
    



}
