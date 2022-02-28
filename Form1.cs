using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
namespace BBBig
{
    public partial class GroupForm : Form
    {
        public GroupForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Icon = new Icon($@"C:\Users\{Environment.UserName}\Documents\BBBig\lock.ico");
            const string pathRegistryKeyStartup = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
            using (RegistryKey registryKeyStartup = Registry.CurrentUser.OpenSubKey(pathRegistryKeyStartup, true))
            {
                registryKeyStartup.SetValue("BBBig", string.Format("\"{0}\"", System.Reflection.Assembly.GetExecutingAssembly().Location));
            }
            if (File.Exists($@"C:\Users\{Environment.UserName}\Documents\BBBig\group.txt"))
            {
                using (StreamReader reader = new StreamReader($@"C:\Users\{Environment.UserName}\Documents\BBBig\group.txt"))
                {
                    string group = reader.ReadToEnd();
                    if (!string.IsNullOrEmpty(group))
                    {
                        GroupNumber.Text = group;
                        reader.Close();
                        button1_Click(Next, new EventArgs());
                    }
                }
            }
            else
            {
                File.Create($@"C:\Users\{Environment.UserName}\Documents\BBBig\group.txt");
                File.Create($@"C:\Users\{Environment.UserName}\Documents\BBBig\login.txt");
                File.Create($@"C:\Users\{Environment.UserName}\Documents\BBBig\neighbors.txt");
                File.Create($@"C:\Users\{Environment.UserName}\Documents\BBBig\compliances.txt");
                MessageBox.Show("Перезапусти меня!", "Внимание");
                Close();
                Application.Exit();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(GroupNumber.Text))
            {
                using (StreamWriter writer = new StreamWriter($@"C:\Users\{Environment.UserName}\Documents\BBBig\group.txt", false))
                {
                    writer.Write(GroupNumber.Text);
                }
                Close();
            }
            else
                MessageBox.Show("Введите номер группы!", "Внимание");
        }
    }
}
