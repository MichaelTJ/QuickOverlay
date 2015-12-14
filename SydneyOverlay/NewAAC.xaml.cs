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
    /// Interaction logic for NewAAC.xaml
    /// </summary>
    public partial class NewAAC : Window
    {
        List<AreasAndConditions> AACList;
        public NewAAC()
        {
            AACList = new List<AreasAndConditions>();
            InitializeComponent();
        }

        public NewAAC(List<AreasAndConditions> AACListIn)
        {
            AACList = AACListIn;
            InitializeComponent();
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            AreasAndConditions newAAC = new AreasAndConditions(txtBoxName.Text);
            AACList.Add(newAAC);
            EditAsAndCs EAAC = new EditAsAndCs(newAAC);
            EAAC.Show();
            Close();
        }
    }
}
