using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectShare
{
    public partial class landing : UserControl
    {
        public landing()
        {
            InitializeComponent();
            
        }

        public void button2_Click(object sender, EventArgs e)
        {
            
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                StartData.path = textBox2.Text;
                int i = 0;
                string ip = "";
                string[] alphabet = new string[] {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K"};
                //convert letters to ip---------------------------------------------------
                foreach(char c in textBox1.Text)
                {
                    if(c.ToString() == "-")
                    {
                        ip = ip + ".";
                    }
                    else
                    {
                        while(alphabet[i] != c.ToString())
                        {
                            i++;
                        }
                         ip = ip + i.ToString();
                    }
                    i = 0;
                }
                Console.WriteLine(ip);
                StartData.ip = ip;
                StartData.close = true;
                Console.WriteLine("1");
            }
            else
            {
                MessageBox.Show("ERROR");
            }
            
            
        }

        private void landing_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "All files (*.*)|*.*";
            DialogResult result = openFileDialog1.ShowDialog();
            StartData.filename = openFileDialog1.SafeFileName;
            
            }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

            textBox2.Text = openFileDialog1.FileName;
        
        
    }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
