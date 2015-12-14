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
    /// Interaction logic for EditText.xaml
    /// </summary>
    public partial class EditText : Window
    {
        private AreasAndConditions AsAndCs;
        private bool isArea = false;
        private bool isCondition = false;
        private string areaIn;
        private string conditionIn;
        public EditText()
        {
            InitializeComponent();
        }
        public EditText(AreasAndConditions AsAndCsIn)
        {
            InitializeComponent();
            AsAndCs = AsAndCsIn;
        }
        public EditText(AreasAndConditions AsAndCsIn, string area)
        {
            InitializeComponent();
            AsAndCs = AsAndCsIn;
            isArea = true;
            areaIn = area;
            txtBlockOld.Text = area;
            txtBoxNew.Text = area;
        }
        public EditText(AreasAndConditions AsAndCsIn, string area, string condition)
        {
            InitializeComponent();
            AsAndCs = AsAndCsIn;
            isCondition = true;
            areaIn = area;
            conditionIn = condition;
            txtBlockOld.Text = condition;
            txtBoxNew.Text = condition;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (isArea)
            {
                AsAndCs.updateArea(areaIn, txtBoxNew.Text);
            }
            if (isCondition)
            {
                AsAndCs.updateCondition(areaIn,conditionIn, txtBoxNew.Text);
            }
            Close();
        }
    }
}
