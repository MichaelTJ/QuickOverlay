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


        //Add new Area
        //Add new Condition
        //Table text
        //Save
        //Open
        //Choose which template

        //Get areas

    }
    



}
