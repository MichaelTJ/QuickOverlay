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
using System.IO;

namespace SydneyOverlay
{
    /// <summary>
    /// Interaction logic for ASAMLoad.xaml
    /// </summary>
    public partial class ASAMLoad : Window
    {
        public ASAMLoad()
        {
            InitializeComponent();
            txtSuperFolder.Text = "";

        }

        private void btnBrowseSuperFolder_Click(object sender, RoutedEventArgs e)
        {
            SetBrowseText(txtSuperFolder);
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            string externalLabelledPath = "";
            string internalLabelledPath = "";

            //go into each folder
            foreach (string IntsExtsPath in Directory.GetDirectories(txtSuperFolder.Text))
            {
                //get the Labelled Images folder
                foreach (string labelledImagePath in Directory.GetDirectories(IntsExtsPath, "Labelled Images"))
                {

                    if (IntsExtsPath.Contains("External"))
                    {
                        externalLabelledPath = labelledImagePath;
                    }
                    else if (IntsExtsPath.Contains("Internal"))
                    {
                        internalLabelledPath = labelledImagePath;
                    }
                }
            }
            if (string.IsNullOrEmpty(externalLabelledPath) ||
                string.IsNullOrEmpty(internalLabelledPath))
            {
                System.Windows.MessageBox.Show("Either internal or external folder paths in empty");
            }
            ASAMSummary summary = new ASAMSummary(externalLabelledPath, internalLabelledPath);
            summary.ShowDialog();

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
