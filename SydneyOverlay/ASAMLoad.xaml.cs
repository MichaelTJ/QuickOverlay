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
using System.Windows.Forms;

namespace SydneyOverlay
{
    /// <summary>
    /// Interaction logic for ASAMLoad.xaml
    /// </summary>
    public partial class ASAMLoad : Window
    {
        private bool isTesting = true;
        public ASAMLoad()
        {
            InitializeComponent();
            if (isTesting)
            {
                txtBoxExternal.Text = @"C:\Users\Sandy\Documents\AUS-ROV\014.WS0269 Minchinbury\WS0269 Externals\Labelled Images";
                txtBoxInternal.Text = @"C:\Users\Sandy\Documents\AUS-ROV\014.WS0269 Minchinbury\WS0269 Internals\Labelled Images";
            }
        }

        private void btnBrowseInternal_Click(object sender, RoutedEventArgs e)
        {
            SetBrowseText(txtBoxInternal);
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(txtBoxExternal.Text) ||
                string.IsNullOrEmpty(txtBoxInternal.Text))
            {
                System.Windows.MessageBox.Show("Either internal or external folder paths in empty");
            }
            else
            {
                ASAMSummary summary = new ASAMSummary(txtBoxExternal.Text,txtBoxInternal.Text);
                summary.ShowDialog();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnBrowseExternal_Click(object sender, RoutedEventArgs e)
        {
            SetBrowseText(txtBoxExternal);
        }

        private void SetBrowseText(System.Windows.Controls.TextBox txtBox)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if(result == System.Windows.Forms.DialogResult.OK){
                txtBox.Text = dialog.SelectedPath;
            }
        }
        
    }
}
