using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace BBBig
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text))
            {
                using (StreamWriter writer = new StreamWriter($@"C:\Users\{Environment.UserName}\Documents\BBBig\neighbors.txt", false))
                {
                    writer.Write(textBox1.Text + " " + textBox2.Text);
                    Close();
                }
            }
            else
                MessageBox.Show("Введите \"соседей\"!", "Внимание");
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            Icon = new Icon($@"C:\Users\{Environment.UserName}\Documents\BBBig\lock.ico");
        }
    }
}
