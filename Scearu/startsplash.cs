using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Xml;
using System.IO;
using System.Xml.Linq;

namespace ProjectShare
{
    public partial class startsplash : UserControl
    {
        public startsplash()
        {
            InitializeComponent();
        }

        private void startsplash_Load(object sender, EventArgs e)
        {



            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(100);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            label6.Text = "Reading settings file";
            string firstrun = string.Empty;
            try
            {
                string xmlfile = File.ReadAllText(@"C:\Program Files (x86)\Scearu\configs\settings.xml");
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.LoadXml(xmlfile);
                XmlNodeList nodelsit = xmldoc.GetElementsByTagName("firstrun");
                 firstrun = string.Empty;
                foreach (XmlNode node in nodelsit)
                {
                    firstrun = node.InnerText;
                }
            }
            catch
            {
               DialogResult corrupted_error =  MessageBox.Show( "The settings file is missing or corrupted" + Environment.NewLine + "We will asume default settings.","Corrupted File", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                if (corrupted_error == DialogResult.Cancel)
                {
                    Application.Exit();
                }
            }
            if (firstrun == "true")
            {

                MessageBox.Show("Thank you for Downloading Scearu" + Environment.NewLine + "Before we begin, you should know Scearu is completely free," + Environment.NewLine + "as said in the license." + Environment.NewLine + "if you paid for this software, YOU SHOULD DEMAND YOUR MONEY BACK.", "A small note", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                XmlDocument doc = new XmlDocument();
                doc.Load(@"C:\Program Files (x86)\Scearu\configs\settings.xml");

                XmlNodeList aDateNodes = doc.SelectNodes("startup/firstrun");
                foreach (XmlNode aDateNode in aDateNodes)
                {
                    XmlAttribute DateAttribute = aDateNode.Attributes["firstrun"];
                    aDateNode.InnerText = "false";
                }
                doc.Save(@"C:\Program Files (x86)\Scearu\configs\settings.xml"); 
            }
                
                try
                {
                    var result = string.Empty;
                    label6.Text = "Checking for updates";
                    using (var webClient = new System.Net.WebClient())
                    {
                        result = webClient.DownloadString("http://download.mbrjsoftware.com/version.txt");
                    }
                    if (result != "1.0.1")
                    {
                        DialogResult wantupdate = MessageBox.Show("An update is available." + Environment.NewLine + "Do you want to install it?", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        if (wantupdate == DialogResult.Yes)
                        {
                            update NewCatForm = new update();
                            var dialogResult = NewCatForm.ShowDialog();
                        }
                        else if (wantupdate == DialogResult.No)
                        {
                            MessageBox.Show("You canceled the update." + Environment.NewLine + "Please note different versions of Scearu are incompatible.", "User canceled update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch
                {
                    label6.Text = "failed to reach update server";
                    MessageBox.Show("We could not reach the update server" + Environment.NewLine + "This could mean your software is out of date" + Environment.NewLine + "keep in mind different versions of project share are incompatible" + Environment.NewLine + "check the info panel for more information", "Could not connect to update Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                StartData.endsplash = true;
            }
        }
    }

