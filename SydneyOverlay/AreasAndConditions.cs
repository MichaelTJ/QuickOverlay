using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SydneyOverlay
{
    public class AreasAndConditions
    {
        Dictionary<string, List<string>> AsAndCs;

        public AreasAndConditions()
        {
            AsAndCs = new Dictionary<string, List<string>>();

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
                                  "Too strong",
                                  "Human error",
                                  "Nuclear",
                                  "Needs Help",
                                  "Bad Reception",
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
            AsAndCs.Add(Area, new List<string>());
        }
        public void addCondition(string Area, string Condition)
        {
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
