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
        public Dictionary<string, List<string>> AsAndCs;
        public string Title;

        public AreasAndConditions()
        {
            AsAndCs = new Dictionary<string, List<string>>();
            Title = "Standard Tank Inspection";

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

            string[] conditions = {"Corrosion",
                                  "Weak",
                                  "Other"};
            Random RandString = new Random();
            foreach (string s in areas)
            {
                //Adding the condition and 3 random strings
                AsAndCs.Add(s, new List<string>());
                AsAndCs[s].Add(conditions[RandString.Next(0, conditions.Length)]);
                AsAndCs[s].Add(conditions[RandString.Next(0, conditions.Length)]);
                AsAndCs[s].Add(conditions[RandString.Next(0, conditions.Length)]);
            }
        }

        public AreasAndConditions(string TitleIn, Dictionary<string, List<string>> AsAndCsIn)
        {
            Title = TitleIn;
            AsAndCs = AsAndCsIn;
        }

        

        public AreasAndConditions(string name)
        {
            AsAndCs = new Dictionary<string, List<string>>();
            Title = name;
        }
        public List<string> getAreas()
        {
            return AsAndCs.Keys.ToList();
        }

        public List<string> getConditions(string area)
        {
            return AsAndCs[area];
        }

        public void addArea(string Area)
        {
            if (AsAndCs.ContainsKey(Area))
            {
                System.Windows.MessageBox.Show(String.Format("That area {0} already exists.", Area));
                return;
            }

            AsAndCs.Add(Area, new List<string>());
        }
        public void addCondition(string Area, string Condition)
        {
            //TODO: Add exceptionhandler if there's no selected key in areas
            if (AsAndCs.ContainsKey(Area))
            {
                AsAndCs[Area].Add(Condition);
            }
        }
        public void removeArea(string Area)
        {
            if (AsAndCs.ContainsKey(Area))
            {
                AsAndCs.Remove(Area);
            }
        }
        public void removeCondition(string Area, string Condition)
        {
            if (AsAndCs.ContainsKey(Area))
            {
                if (AsAndCs[Area].Contains(Condition))
                {
                    AsAndCs[Area].Remove(Condition);
                }
            }
        }

        public void updateArea(string oldArea, string newArea)
        {
            if (AsAndCs.ContainsKey(oldArea))
            {
                List<string> oldList = AsAndCs[oldArea];
                AsAndCs.Remove(oldArea);
                AsAndCs.Add(newArea, oldList);
            }
        }

        public void updateCondition(string Area, string oldCondition, string newCondition)
        {
            if (AsAndCs.ContainsKey(Area))
            {
                if (AsAndCs[Area].Contains(oldCondition))
                {
                    AsAndCs[Area].Remove(oldCondition);
                    AsAndCs[Area].Add(newCondition);
                }
            }
        }
        //Save
        //Open
        //Choose which template

    }
    



}
