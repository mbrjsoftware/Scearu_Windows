using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectShare
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            StartData.endsplash = false;
            timer1.Start();
            panel2.Hide();
        }
        startsplash splash;
        private void label2_Click(object sender, EventArgs e)
        {
       
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Create a new instance of the Form2 class
            Recieve recieveform = new Recieve();
            var dialogResult = recieveform.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
     
        }
        public void SendTCP(string M, string IPA, Int32 PortN)
        {
            byte[] SendingBuffer = null;
    TcpClient client = null;
            const int BufferSize = 8 * 1024;
            label1.Text = "";
            NetworkStream netstream = null;
            try
            {
                client = new TcpClient(IPA, PortN);
                label1.Text = "Connected to the Server...\n";
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
                    }
                    else
                        CurrentPacketLength = TotalLength;
                    SendingBuffer = new byte[CurrentPacketLength];
                    Fs.Read(SendingBuffer, 0, CurrentPacketLength);
                    netstream.Write(SendingBuffer, 0, (int)SendingBuffer.Length);
                  
                }

               
        
                     Fs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
               netstream.Close();
                client.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Send Sendform = new Send();
            var dialogResult = Sendform.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
             splash = new startsplash();
            this.Controls.Add(splash);
            
            splash.Dock = DockStyle.Fill;
            splash.Show();
            splash.BringToFront();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://scearu.mbrjsoftware.com");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            info infobox = new info();
            var dialogResult = infobox.ShowDialog();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("A help section will be made later.", "Not ready yet!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
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

        private void pictureBox4_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.BackColor = Color.Transparent;
        }

        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {
            pictureBox4.BackColor = Color.Orange;
        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(StartData.endsplash == true)
            {
                splash.Hide();
                panel2.Show();
            }
        }
    }
}
