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
    /// <summary>
    /// Interaction logic for NewAAC.xaml
    /// </summary>
    public partial class NewAAC : Window
    {
        ObservableCollection<AreasAndConditions> AACList;
        public NewAAC()
        {
            AACList = new ObservableCollection<AreasAndConditions>();
            InitializeComponent();
        }

        public NewAAC(ObservableCollection<AreasAndConditions> AACListIn)
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
            EAAC.ShowDialog();
            Close();
        }
    }
}
