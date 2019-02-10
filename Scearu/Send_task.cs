using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;
using System.IO.Compression;
using System.Net;
using ICSharpCode.SharpZipLib.Zip;

namespace ProjectShare
{
    public partial class Send_task : UserControl
    {
        public Send_task()
        {
            InitializeComponent();
        }

        private void Send_task_Load(object sender, EventArgs e)
        {
            label1.Text = "Preparing your Files";
            label3.Hide();
            label2.Text = "This could take up to five minutes";
            backgroundWorker2.RunWorkerAsync();
        }
        bool finsihed = false;
        long length;
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
             
            string info = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            length = new System.IO.FileInfo(info + @"\AppData\Local\Temp\ProjectShareSend.zip").Length;
            Console.WriteLine(length);
            SendTCP(info + @"\AppData\Local\Temp\ProjectShareSend.zip", StartData.ip,3369);
            
            
        }
        long sent = 0;
        public void SendTCP(string M, string IPA, Int32 PortN)
        {
            byte[] SendingBuffer = null;
            TcpClient client = null;
            const int BufferSize = 8 * 1024;
          //  label1.Text = "";
            NetworkStream netstream = null;
            try
            {
                Console.Write("starting");
                client = new TcpClient(IPA, PortN);
              //  label1.Text = "Connected to the Server...\n";
                netstream = client.GetStream();
                
                FileStream Fs = new FileStream(M, FileMode.Open, FileAccess.Read);
                int NoOfPackets = Convert.ToInt32
             (Math.Ceiling(Convert.ToDouble(Fs.Length) / Convert.ToDouble(BufferSize)));

                int TotalLength = (int)Fs.Length, CurrentPacketLength, counter = 0;
                for (int i = 0; i < NoOfPackets; i++)
                {
                    if (TotalLength > BufferSize)
                    {
                        CurrentPacketLength = BufferSize;
                        TotalLength = TotalLength - CurrentPacketLength;
                         sent ++;
                    }
                    else
                        CurrentPacketLength = TotalLength;
                     StartData.important = NoOfPackets;
                    backgroundWorker1.ReportProgress(i);
                    SendingBuffer = new byte[CurrentPacketLength];
                    Fs.Read(SendingBuffer, 0, CurrentPacketLength);
                    netstream.Write(SendingBuffer, 0, (int)SendingBuffer.Length);

                   
                }

                Console.WriteLine("End");
                netstream.Close();
                Fs.Close();
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                netstream.Close();
                client.Close();
                Console.WriteLine("end2");

            }
        }

        bool connected = false;
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            if(e.ProgressPercentage > 2 && connected == false)
            {
                connected = true;
                label1.Text = "Connected";
            }
            // label1.Text = Convert.ToString(e.ProgressPercentage / length);
            //Console.WriteLine(length);
             double num = (e.ProgressPercentage) / StartData.important;
            //double num = StartData.important;
             progressBar1.Value = Convert.ToInt32(num * 100);
            // Console.WriteLine(num * 100000000000000000);
  
        }
        Random rnd = new Random();
        string digits;
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            while (StartData.close == false)
            {

            }
            try
            {
                Environment.CurrentDirectory = Environment.GetEnvironmentVariable("USERPROFILE");
                DirectoryInfo info2 = new DirectoryInfo(".");
                System.IO.File.Delete(info2 + @"\AppData\Local\Temp\ProjectShareSend.zip");
            }
            catch
            {
                Console.WriteLine("Clean");
            }
            Console.WriteLine("starting zip");
            

               
                digits = rnd.Next(1, 9).ToString() + rnd.Next(1, 9).ToString() + rnd.Next(1, 9).ToString() + rnd.Next(1, 9).ToString();
            // add this map file into the "images" directory in the zip archive

            string info =  Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            Console.WriteLine(info);
            string targetPath =  info + @"\AppData\Local\Temp\ProjectShareSend\";
      
            System.IO.DirectoryInfo di = new DirectoryInfo(targetPath);
            try
            {
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
            catch {
                Console.WriteLine("empty");

                  }

            if (!System.IO.Directory.Exists(targetPath))
                {
                    System.IO.Directory.CreateDirectory(targetPath);
                }
            
            System.IO.File.Copy(StartData.path, info + @"\AppData\Local\Temp\ProjectShareSend\" + StartData.filename, true);
            Console.WriteLine("about to make zip");
          using(ICSharpCode.SharpZipLib.Zip.ZipFile z = ICSharpCode.SharpZipLib.Zip.ZipFile.Create(info + @"\AppData\Local\Temp\ProjectShareSend.zip"))
            {
                z.NameTransform = new ICSharpCode.SharpZipLib.Zip.ZipNameTransform(info + @"\AppData\Local\Temp\ProjectShareSend\");
                z.BeginUpdate();
                z.Add(info + @"\AppData\Local\Temp\ProjectShareSend\" + System.IO.Path.GetFileName(StartData.path),ICSharpCode.SharpZipLib.Zip.CompressionMethod.Stored);
                z.CommitUpdate();
                
            }
          
           // ZipFile.CreateFromDirectory(info + @"\AppData\Local\Temp\ProjectShareSend\", info + @"\AppData\Local\Temp\ProjectShareSend.zip");
            Console.WriteLine("about to encrypt");
          //  Encrypt.EncryptFile(info + @"\AppData\Local\Temp\ProjectShareSend.zip", info + @"\AppData\Local\Temp\ProjectShareSend_Encrypt.zip", digits);
              //  System.IO.File.Delete(info + @"\AppData\Local\Temp\ProjectShareSend.zip");
          
                 Console.WriteLine("finished zip operation");
            StartData.firstpin = digits;
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // label3.Text = StartData.firstpin;
            // label3.Show();
          //  IPAddress[] array = Dns.GetHostAddresses(StartData.ip);
           // foreach (IPAddress ip in array)
           // {
            //    StartData.ip = ip.ToString();
           // }
            Console.WriteLine(StartData.ip);
          //  label2.Text = "Thank you for joining our first Beta!";
            progressBar1.Style = ProgressBarStyle.Blocks;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                string info = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                System.IO.File.Delete(info + @"\AppData\Local\Temp\ProjectShareSend.zip");
            }
            catch
            {
                Console.WriteLine("Magically dissapeared!");
            }
            progressBar1.Value = 100;
            label1.Text = "Thank You!";
            label2.Text = "Your file is in your Downloads folder.";
           // MessageBox.Show("test");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {

        }
    }
}
