using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
namespace BBBig
{
    public partial class Form2 : Form
    {
        readonly IWebDriver driver = new ChromeDriver();
        string endTime;
        bool see = true;
        bool stopTimer = true;
        bool addLink = false;
        public Form2()
        {
            InitializeComponent();
        }
        private int TimeToInt(string time)
        {
            time = time.Remove(time.IndexOf(":"), 1);
            return int.Parse(time);
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            notifyIcon1.Icon = new Icon(@"C:\Program Files (x86)\BBBig\lock.ico");
            Icon = new Icon(@"C:\Program Files (x86)\BBBig\lock.ico");
            if (new GroupForm().ShowDialog() == DialogResult.OK) {}
            string group;
            using (StreamReader reader = new StreamReader(@"C:\Program Files (x86)\BBBig\group.txt"))
            {
                group = reader.ReadToEnd();
            }
            driver.Navigate().GoToUrl($"https://www.bsu.edu.ru/bsu/resource/schedule/groups/index.php?group={group}");
            Thread.Sleep(2000);
            var elements = driver.FindElements(By.XPath("//*[@id=\"shedule\"]/tbody/tr"));
            bool check = false;
            foreach (var ee in elements)
            {
                if (check)
                {
                    if (ee.Text.Contains(DateTime.Now.AddDays(1).ToShortDateString()))
                        break;
                    var cells = ee.FindElements(By.CssSelector("td"));
                    int index = 0;
                    try
                    {
                        index = cells[1].Text.IndexOf("-");
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        driver.Dispose();
                        Application.Exit();
                    }
                    string[] times =
                    {
                        cells[1].Text.Substring(0, index - 1),
                        cells[1].Text.Substring(index + 2, cells[1].Text.Length - index - 2)
                    };
                    if(TimeToInt(DateTime.Now.ToShortTimeString()) < TimeToInt(times[0]))
                    {
                        int first = TimeToInt(times[0]) - TimeToInt(DateTime.Now.ToShortTimeString());
                        int hours = first / 100;
                        int min = first - hours * 100;
                        Thread.Sleep(new TimeSpan(hours, min, 0));
                    }
                    endTime = times[1];
                    timer1.Start();
                    if(TimeToInt(DateTime.Now.AddMinutes(31).ToShortTimeString()) > TimeToInt(times[0])
                        && TimeToInt(DateTime.Now.ToShortTimeString()) < TimeToInt(times[1]))
                    {
                        cells[3].FindElement(By.CssSelector("a")).Click();
                        Thread.Sleep(500);
                        driver.SwitchTo().Window(driver.WindowHandles[1]);
                        if (driver.Url == "https://pegas.bsu.edu.ru/login/index.php")
                        {
                            using (StreamReader reader = new StreamReader(@"C:\Program Files (x86)\BBBig\login.txt"))
                            {
                                string[] data = reader.ReadToEnd().Split('\n');
                                if (!string.IsNullOrEmpty(data[0]))
                                {
                                    textBox1.Text = data[0];
                                    textBox2.Text = data[1];
                                    reader.Close();
                                    button1_Click(button1, new EventArgs());
                                }
                                else
                                    Visible = true;
                            }
                        }
                        break;
                    }
                }
                if (ee.Text.Contains(DateTime.Now.ToShortDateString()))
                    check = true;
            }
            if (!check)
            {
                MessageBox.Show("Пар нет!", "Внимание");
                driver.Dispose();
                Application.Exit();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Visible = false;
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text))
            {
                using (StreamWriter writer = new StreamWriter(@"C:\Program Files (x86)\BBBig\login.txt", false))
                {
                    writer.Write(textBox1.Text);
                    writer.Write("\n");
                    writer.Write(textBox2.Text);
                }
                var element = driver.FindElement(By.CssSelector("#username"));
                element.SendKeys(textBox1.Text);
                element = driver.FindElement(By.CssSelector("#password"));
                element.SendKeys(textBox2.Text);
                driver.FindElement(By.CssSelector("#loginbtn")).Click();
                Thread.Sleep(500);
                driver.SwitchTo().Window(driver.WindowHandles[1]);
                element = driver.FindElement(By.CssSelector("#page-header > div > div > div > div:nth-child(2)"));
                string courseName = element.Text.Substring(0, element.Text.IndexOf('(') == -1 ? element.Text.Length : element.Text.IndexOf('(') - 1);
                using (StreamReader reader = new StreamReader(@"C:\Program Files (x86)\BBBig\compliances.txt"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if(line.Split('_')[0] == courseName)
                        {
                            see = false;
                            textBox1.Text = courseName;
                            textBox2.Text = line.Split('_')[1];
                            reader.Close();
                            button2_Click(button2, new EventArgs());
                            break;
                        }
                    }
                    if (see)
                    {
                        textBox1.Clear();
                        textBox2.Clear();
                        label1.Text = "Название курса:";
                        label2.Text = "Ссылка на онлайн-занятие:";
                        button1.Visible = false;
                        button2.Visible = true;
                        Visible = true;
                    }
                }
            }
            else
                MessageBox.Show("Введите логин и пароль!", "Внимание");
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (TimeToInt(DateTime.Now.ToShortTimeString()) > TimeToInt(endTime))
            {
                driver.Dispose();
                Application.Restart();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Visible = false;
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text))
            {
                if (see)
                {
                    using (StreamWriter writer = new StreamWriter(@"C:\Program Files (x86)\BBBig\compliances.txt", true))
                    {
                        writer.Write(textBox1.Text);
                        writer.Write("_");
                        writer.WriteLine(textBox2.Text);
                    }
                }
                if (!addLink)
                {
                    driver.Navigate().GoToUrl(textBox2.Text);
                    driver.FindElement(By.CssSelector("#join_button_input")).Click();
                    Thread.Sleep(500);
                    driver.SwitchTo().Window(driver.WindowHandles[2]);
                    Thread.Sleep(3500);
                    driver.FindElement(By.CssSelector("body > div.portal--27FHYi > div > div > div.content--IVOUy > div > div > span > button:nth-child(2)")).Click();
                    timer2.Start();
                }
                else
                    addLink = false;
            }
            else
                MessageBox.Show("Вставьте ссылку на онлайн-занятие!", "Внимание");
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                driver.FindElement(By.CssSelector("#chatPanel > div > button")).Click();
            }
            catch (Exception) {}
            using (StreamReader reader = new StreamReader(@"C:\Program Files (x86)\BBBig\neighbors.txt"))
            {
                string[] neighbors = reader.ReadToEnd().Split();
                if (!string.IsNullOrEmpty(neighbors[0]))
                {
                    var element = driver.FindElement(By.CssSelector("#chatPanel > div > div > div:nth-child(1) > div > div"));
                    var arr = element.FindElements(By.CssSelector(".content--BYIui"));
                    foreach (var ee in arr)
                    {
                        string surname = ee.Text.Split()[0];
                        if (surname == neighbors[0] || surname == neighbors[1])
                        {
                            if (ee.Text.Substring(ee.Text.Length - 1, 1) == "+")
                            {
                                if (stopTimer)
                                {
                                    timer2.Interval = 60000;
                                    stopTimer = false;
                                }
                                else
                                {
                                    timer2.Interval = 8000;
                                    stopTimer = true;
                                }
                                driver.FindElement(By.CssSelector("#message-input")).SendKeys("+");
                                driver.FindElement(By.CssSelector(".sendButton--Z93EzE")).Click();
                                break;
                            }
                        }
                    }
                }
                else
                {
                    timer2.Stop();
                    reader.Dispose();
                    if (new Form3().ShowDialog() == DialogResult.OK) { }
                    timer2.Start();
                }
            }
        }
        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            driver.Dispose();
            Application.Exit();
        }
        private async void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Task.Run(()=>MessageBox.Show("Автор программы: Kamishiro Iyamoto\nVK: https://vk.com/kamishiro_iyamoto\n\n\nПоддержать автора:\nСбер: 2202201724442484", "Справка"));
        }
        private void изменитьГруппуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            driver.Dispose();
            using (StreamWriter writer = new StreamWriter(@"C:\Program Files (x86)\BBBig\group.txt", false))
            {
                writer.Write("");
            }
            Application.Restart();
        }
        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            driver.Dispose();
            using (StreamWriter writer = new StreamWriter(@"C:\Program Files (x86)\BBBig\login.txt", false))
            {
                writer.Write("");
            }
            Application.Restart();
        }
        private void изменитьсоседейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form3().Show();
        }
        private void Form2_Shown(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
                Visible = false;
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            see = true;
            addLink = true;
            textBox1.Clear();
            textBox2.Clear();
            label1.Text = "Название курса:";
            label2.Text = "Ссылка на онлайн-занятие:";
            button1.Visible = false;
            button2.Visible = true;
            Visible = true;
        }
        private void удалитьИзАвтозагрузкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string pathRegistryKeyStartup = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
            using (RegistryKey registryKeyStartup = Registry.CurrentUser.OpenSubKey(pathRegistryKeyStartup, true))
            {
                registryKeyStartup.DeleteValue("BBBig", false);
            }
        }
    }
}
