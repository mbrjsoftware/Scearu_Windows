using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;

namespace ProjectShare
{
    public partial class Recieve : Form
    {
        bool nofinish = false;
        public Recieve()
        {
            StartData.endsudden = false;
            //backgroundWorker2.RunWorkerAsync();
            InitializeComponent();
            nofinish = false;
            string ip = GetIPAddress();
            string[] alphabet = new string[] { "A","B","C","D","E","F","G","H","I","J","K"};
            string newip = "";
            int i = 0;
            foreach(char c in GetIPAddress())
            {
                if (c.ToString() == ".")
                {
                    newip = newip + "-";
                }
                else
                {
                    while (i != Convert.ToInt32(c.ToString()))
                    {
                        i++;
                    }
                    newip = newip + alphabet[i];
                }
                i = 0;
            }
            label2.Text = newip;
            backgroundWorker1.RunWorkerAsync();
            label5.Text = "Ready to Recieve";
        }
       
        static string IPAddress1 = GetIPAddress();
        public static string GetIPAddress()
        {
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress1 = Convert.ToString(IP);
                }
            }
            return IPAddress1;
        }
        TcpListener a = new TcpListener(IPAddress.Parse(GetIPAddress()), 3369);
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            StartData.waitforyes = false;
            
            string hostName = Dns.GetHostName();
            string myIP = GetIPAddress();
            Console.WriteLine(myIP);
            
         
            
            string info = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            bool stop = false;
            Console.WriteLine("dbg1");
            try
            {
                a.Start();
            }catch (Exception x)
            {
                Console.WriteLine(x);
            }
            Console.WriteLine("dbg2");
            while (stop == false)
            {
                if(backgroundWorker1.CancellationPending == true)
                {
                    a.Stop();
                }
                Console.WriteLine("error");
                using (var client = a.AcceptTcpClient())
                using (var stream = client.GetStream())
                using (var output = File.Create(info + @"\AppData\Local\Temp\ProjectShare.zip"))
                {
                    Console.WriteLine("part2");
                    var networkStream = client.GetStream();
                    var pi = networkStream.GetType().GetProperty("Socket", BindingFlags.NonPublic | BindingFlags.Instance);
                    var socketIp = ((Socket)pi.GetValue(networkStream, null)).RemoteEndPoint.ToString();
                    StartData.Connection_true = socketIp;
                    backgroundWorker1.ReportProgress(-1);
                    while (StartData.waitforyes == false)
                    {

                    }


                    //  Console.WriteLine(socketIp);
                    Console.WriteLine("Client connected. Starting to receive the file");

                    backgroundWorker1.ReportProgress(1);
                    // read the file in chunks of 1KB
                    var buffer = new byte[8192];
                    long total = 0;
                    int bytesRead;
                    int reset = 0;
                    stop = true;
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        output.Write(buffer, 0, bytesRead);

                        total = total + 1024;
                        if (reset > 2500)
                        {
                            StartData.bytes = total;
                            backgroundWorker1.ReportProgress(5);
                            reset = 0;
                        }
                        else
                        {
                            reset++;
                        }

                        //  Console.WriteLine("read");
                        if (StartData.endsudden == true)
                        {
                            networkStream.Close();
                            return;
                        }
                    }
                    // Console.WriteLine("Finished");
                    networkStream.Close();
                    client.Close();
                    a.Stop();
                }
                Console.Write("Finished sharing");
               
            }
            StartData.wait_for_password = false;
            backgroundWorker1.ReportProgress(-2);
            while (StartData.wait_for_password == false)
            {

            }

            // using (ZipFile zip = ZipFile.Read(info + @"\AppData\Local\Temp\ProjectShare.zip"))
            // {
            //   foreach (ZipEntry a in zip)
            // {
            //   a.ExtractWithPassword(System.Environment.SpecialFolder.UserProfile + @"\Downloads\", StartData.encrypt_pass);
            // }
            // }
            try
            {
                Directory.Delete(info + @"\AppData\Local\Temp\ProjectShare_Un\", true);
            }
            catch
            {

            }
                Console.WriteLine(1);
            //Encrypt.DecryptFile(info + @"\AppData\Local\Temp\ProjectShare.zip", info + @"\AppData\Local\Temp\ProjectShare_Un.zip", StartData.encrypt_pass);
            // ZipFile.ExtractToDirectory(info + @"\AppData\Local\Temp\ProjectShare.zip", info + @"\AppData\Local\Temp\ProjectShare_Un\");

            FastZip fastZip = new FastZip();
            
            // Will always overwrite if target filenames already exist
            fastZip.ExtractZip(info + @"\AppData\Local\Temp\ProjectShare.zip", info + @"\AppData\Local\Temp\ProjectShare_Un\", null);
            Console.WriteLine(2);
            System.IO.File.Delete(info + @"\AppData\Local\Temp\ProjectShare.zip");
            // System.IO.File.Delete(info + @"\AppData\Local\Temp\ProjectShare_Un.zip");
            Console.WriteLine(3);
            DirectoryCopy(info + @"\AppData\Local\Temp\ProjectShare_Un\",info + @"\Downloads\",false);
          //  Directory.Delete(info + @"\AppData\Local\Temp\ProjectShare_Un\",true);
        }
        public static void DirectoryCopy(string sourceDirName, string destDirName,
                                    bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
        private void Recieve_Load(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (nofinish == false)
            {
                label5.Text = "Finished";
                label3.Text = "We've Finished";
                label4.Text = "Thank You for using our software";
                label1.Text = "If you want to share another file, exit and reopen recieve";
                label2.Hide();
                button1.Show();
                MessageBox.Show("Done", "Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
            if (e.ProgressPercentage == 1)
            {
                label5.Text = "Connected";
            } else if (e.ProgressPercentage == 5)
            {
                if (StartData.bytes < 1000000)
                {
                    label5.Text = "Recieved " + StartData.bytes / 1000 + " KB";
                }else if (StartData.bytes > 1000000){
                    label5.Text = "Recieved " + StartData.bytes / 1000000 + " MB";
                }else if (StartData.bytes > 1000000000)
                {
                    label5.Text = "Recieved " + StartData.bytes / 1000000000 + " GB";
                }
            }else if (e.ProgressPercentage == -1)
            {
                DialogResult result2 = MessageBox.Show("Allow Connection?",
            "Accept connection from: " + StartData.Connection_true + " ?",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Question);
                if (result2 == DialogResult.Yes)
                {
                    StartData.waitforyes = true;
                    label3.Text = "Transfering";
                    label4.Hide();
                }
                else if (
                    result2== DialogResult.Cancel)
                {
                    backgroundWorker1.CancelAsync();
                } 
            }
            else if (e.ProgressPercentage == -2)
            {

              //  Password frm2 = new Password();
             //   DialogResult dr = frm2.ShowDialog(this);
              //  if (dr == DialogResult.OK)
               // {
                    StartData.wait_for_password = true;
                label4.Text = "Nearly there!";
                label4.Show(); 
                label5.Text = "Unpacking file";
                label1.Text = "Shouldn't be long";
                //    frm2.Close();
                //}
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            while (StartData.endsudden == false)
            {

            }

        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StartData.endsudden = true;
            
        }

        private void Recieve_FormClosing(object sender, FormClosingEventArgs e)
        {
            a.Stop();
            nofinish = true;
          //  Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private void panel8_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {
            pictureBox4.BackColor = Color.Orange;
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.BackColor = Color.Transparent;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
