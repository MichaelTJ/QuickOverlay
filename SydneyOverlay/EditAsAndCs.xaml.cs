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

namespace SydneyOverlay
{
    /// <summary>
    /// Interaction logic for EditAsAndCs.xaml
    /// </summary>
    public partial class EditAsAndCs : Window
    {
        AreasAndConditions AsAndCs;
        AreasAndConditions oldAsAndCs;
        private bool tempDisabled;
        public EditAsAndCs()
        {
            AsAndCs = new AreasAndConditions();
            oldAsAndCs = new AreasAndConditions();
            InitializeComponent();
            Title = "Unknown";

        }
        public EditAsAndCs(AreasAndConditions AAC)
        {
            AsAndCs = AAC;
            oldAsAndCs = AAC;
            InitializeComponent();
            Title = AAC.Title;

        }

        private void AreasListBox_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (string s in AsAndCs.getAreas())
            {
                AreasListBox.Items.Add(s);
            }
        }

        private void ConditionsListBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            updateConditions();
        }


        private void btnAddArea_Click(object sender, RoutedEventArgs e)
        {
            if (!(txtBoxAddArea.Text == "" || txtBoxAddArea.Text == null))
            {
                AsAndCs.addArea(txtBoxAddArea.Text);
            }
            updateAreas();
        }

        private void btnAddCondition_Click(object sender, RoutedEventArgs e)
        {
            if (!(txtBoxAddCondiition.Text == "" || txtBoxAddCondiition.Text == null))
            {
                AsAndCs.addCondition(AreasListBox.SelectedItem as string,
                    txtBoxAddCondiition.Text);
            }
            updateConditions();
        }

        private void updateAreas()
        {
            tempDisabled = true;
            AreasListBox.Items.Clear();

            foreach (string s in AsAndCs.getAreas())
            {
                AreasListBox.Items.Add(s);
            }
            tempDisabled = false;
            AreasListBox.SelectedIndex = 0;
            
        }
        private void updateConditions()
        {
            var curSelected = AreasListBox.SelectedItem as string;
            ConditionsListBox.Items.Clear();
            foreach (string s in AsAndCs.getConditions(curSelected))
            {
                ConditionsListBox.Items.Add(s);
            }
        }


        private void AreasListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tempDisabled) { return; }
            updateConditions();
        }


        private void btnDeleteArea_Click(object sender, RoutedEventArgs e)
        {
            if (AreasListBox.SelectedItem != null)
            {
                AsAndCs.removeArea(AreasListBox.SelectedItem as string);
                updateAreas();
            }
        }

        private void btnDeleteCondition_Click(object sender, RoutedEventArgs e)
        {
            if (ConditionsListBox.SelectedItem != null)
            {
                if (AreasListBox.SelectedItem != null)
                {
                    AsAndCs.removeCondition(AreasListBox.SelectedItem as string, 
                        ConditionsListBox.SelectedItem as string);
                    updateConditions();
                }
            }
        }

        private void btnEditCondition_Click(object sender, RoutedEventArgs e)
        {
            if (ConditionsListBox.SelectedItem != null)
            {
                if (AreasListBox.SelectedItem != null)
                {
                    EditText ET = new EditText(AsAndCs,
                        AreasListBox.SelectedItem as string,
                        ConditionsListBox.SelectedItem as string);
                    ET.Show();
                    updateConditions();
                }
            }
        }

        private void btnEditArea_Click(object sender, RoutedEventArgs e)
        {
            if (AreasListBox.SelectedItem != null)
            {
                EditText ET = new EditText(AsAndCs,
                    AreasListBox.SelectedItem as string);
                updateAreas();
            }

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            AsAndCs = oldAsAndCs;
            Close();
        }


    }
}
