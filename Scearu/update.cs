using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace ProjectShare
{
    public partial class update : Form
    {
        public update()
        {
            InitializeComponent();
            backgroundWorker1.RunWorkerAsync();
        }

        private void update_Load(object sender, EventArgs e)
        {
            

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string info = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                Console.WriteLine(info);

                string targetPath = info + @"\AppData\Local\Temp\";
                System.IO.File.Delete(targetPath + "setup.exe");
                using (var client = new WebClient())
                {
                    client.DownloadFile("http://download.mbrjsoftware.com/scearusetup.exe", targetPath + "setup.exe");
                }
                backgroundWorker1.ReportProgress(-1);
                Process.Start(targetPath + "setup.exe");
            }
            catch
            {
                MessageBox.Show("We could not download the installer." + Environment.NewLine + "This is usually due to a network connection issue." + Environment.NewLine + "Please try again later.", "Failed to download installer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if(e.ProgressPercentage == -1)
            {
                label1.Text = "launching installer";
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Environment.Exit(1);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
