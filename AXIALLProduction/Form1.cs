using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AXIALLProduction
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //
            string path1 = @"C:\ClassifiedDefects\data";
            string path2 = @"C:\ITF\XMLCEXPORT";

            if (!Directory.Exists(path1))
            {
                Directory.CreateDirectory(path1);
            }
            if (!Directory.Exists(path2))
            {
                Directory.CreateDirectory(path2);
            }


            //监控的文件夹路径
            this.fileSystemWatcher1.Path = @"C:\classifiedDefects\data";
            ContextMenuStrip listboxMenu = new ContextMenuStrip();
            ToolStripMenuItem rightMenu = new ToolStripMenuItem("截图");
            ToolStripMenuItem rightMenu2 = new ToolStripMenuItem("复制Sn");
            rightMenu.Click += new EventHandler(Copy_Click);
            rightMenu2.Click += new EventHandler(Copy_Click2);
            listboxMenu.Items.AddRange(new ToolStripItem[] { rightMenu, rightMenu2 });
            listBox2.ContextMenuStrip = listboxMenu;
            this.textBoxLine.Text = "R12";
            this.textBoxSN.Text = "1PL0123445";
            this.textBoxMachine.Text = "V810-8086S2";

          

            this.Width = 300;
            this.Height = 320;

            this.TopMost = true;

        }

        private void Copy_Click(object sender, EventArgs e)
        {
            //Clipboard.SetText(listBox2.Items[listBox2.SelectedIndex].ToString().Trim());
            string sn = listBox2.Items[listBox2.SelectedIndex].ToString().Trim().Split('_')[0];
            string lineStr = listBox2.Items[listBox2.SelectedIndex].ToString().Trim().Split('_')[1];
            string machineStr = listBox2.Items[listBox2.SelectedIndex].ToString().Trim().Split('_')[2];
            SaveImageAndDelety(sn, lineStr, machineStr);
        }
        private void Copy_Click2(object sender, EventArgs e)
        {
            string sn = listBox2.Items[listBox2.SelectedIndex].ToString().Trim().Split('_')[0];
            Clipboard.SetDataObject(sn);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.txtBoxPath.Text = @"\\172.26.120.75\aoiaxi\AXI\5DX\5DX不良";
            this.radioButton2.Checked = true;


            string dateTime = DateTime.Now.ToString("yyyyMMdd");
            comboBox1.Items.Add(@"\\DESKTOP-2D5LTMT\c\AXI\CBackup\" + dateTime);  
            comboBox1.Items.Add(@"\\8028XCCS\c\AXI\CBackup\" + dateTime);   
            comboBox1.Items.Add(@"\\3323-VVTS\c\AXI\CBackup\" + dateTime);   
            comboBox1.Items.Add(@"\\3323VVTS\c\AXI\CBackup\" + dateTime);   
            comboBox1.SelectedIndex = 0;
            this.comboBox1.Enabled = false;
            button25.Enabled = false;
            setRowNumber(this.dataGridView1);


          
            
        }
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
        private void tabPage2_Click(object sender, EventArgs e)
        {


           // MessageBox.Show("123");

        }
        private void tabPage5_Click(object sender, EventArgs e)
        {

        }




        /// <summary>
        /// 通用截图API方法的的封装 传统的截图方式
        /// </summary>
        /// <param name="snnumber">sn</param>
        /// <param name="lineStr">线别</param>
        private void SaveImageAndDelety(string snnumber, string lineStr)
        {
            string txtboxDelay = this.textBox2.Text.Trim();
            int txtboxDelayint = int.Parse(txtboxDelay);
            Screen scr = Screen.PrimaryScreen;
            Rectangle rc = scr.Bounds;
            int iWidth = rc.Width;
            int iHeight = rc.Height;
            Image myImage = new Bitmap(iWidth - 100, iHeight - 105);
            Graphics g = Graphics.FromImage(myImage);
            this.Hide();
            Thread.Sleep(txtboxDelayint);
            g.CopyFromScreen(new Point(100, 40), new Point(0, 0), new Size(iWidth - 100, iHeight - 105));
            this.Show();
            g.TranslateTransform(0, iHeight);
            g.RotateTransform(-45);
            g.RotateTransform(45);
            TestOnSeal _top = new TestOnSeal();
            _top.TextFont = new Font("黑体", 16, FontStyle.Bold);
            _top.FillColor = Color.Red;
            _top.Text = "广达(上海)制造城";
            _top.BaseString = "5DX专用";
            _top.ShowPath = true;
            _top.LetterSpace = 1;
            _top.SealSize = 180;
            _top.CharDirection = Char_Direction.Center;
            _top.SetIndent(20);
            g.DrawImage(_top.TextOnPathBitmap(), 1600, -300);
            _top.SetIndent(20);
            g.Dispose();
            string parentPath = this.textBox4.Text.Trim();
            string timefolder = DateTime.Now.ToString("mmddss");
            string timefolder2 = DateTime.Now.ToString("MMdd");
            DateTime dt = DateTime.Now;
            string dateStr1 = "20:00:00";
            string dateStr2 = dt.ToString("HH:mm:ss");
            DateTime t1 = Convert.ToDateTime(dateStr1);
            DateTime t2 = Convert.ToDateTime(dateStr2);
            int compNum = DateTime.Compare(t1, t2);
            if (compNum > 0)
            {
                string saveRealPath = parentPath + "\\" + lineStr + "\\" + timefolder2 + "\\" + snnumber + "__"  + "__" + timefolder + ".jpg";
                string filepaht = parentPath + "\\" + lineStr + "\\" + timefolder2;
                if (!Directory.Exists(filepaht))
                {
                    Directory.CreateDirectory(filepaht);
                }
                myImage.Save(saveRealPath);
                string filepath = filepaht + "\\" + "Content.txt";
                if (!File.Exists(filepath))
                {
                    TextWriter tw = new StreamWriter(filepath);
                    tw.WriteLine(snnumber.Trim().ToUpper());
                    tw.Close();
                }
                else
                {
                    TextWriter tw = new StreamWriter(filepath, true);
                    tw.WriteLine(snnumber.Trim().ToUpper());
                    tw.Close();
                }
            }
            if (compNum < 0)
            {
                string tommorm = DateTime.Now.AddDays(+1).ToString("MMdd");
                string saveRealPath = parentPath + "\\" + lineStr + "\\" + tommorm + "\\" + snnumber + "__"  + "__" + timefolder + ".jpg";
                string filepaht = parentPath + "\\" + lineStr + "\\" + tommorm;
                if (!Directory.Exists(filepaht))
                {
                    Directory.CreateDirectory(filepaht);
                }
                myImage.Save(saveRealPath);
                string filepath = filepaht + "\\" + "Content.txt";
                if (!File.Exists(filepath))
                {
                    TextWriter tw = new StreamWriter(filepath);
                    tw.WriteLine(snnumber.Trim().ToUpper());
                    tw.Close();
                }
                else
                {
                    TextWriter tw = new StreamWriter(filepath, true);
                    tw.WriteLine(snnumber.Trim().ToUpper());
                    tw.Close();
                }
            }
        }


        /// <summary>
        /// 通用截图API方法的的封装 传统的截图方式
        /// </summary>
        /// <param name="snnumber">sn</param>
        private void SaveImageAndDeletyVoid(string snnumber)
        {
            string txtboxDelay = this.textBox19.Text.Trim();
            int txtboxDelayint = int.Parse(txtboxDelay);
            Screen scr = Screen.PrimaryScreen;
            Rectangle rc = scr.Bounds;
            int iWidth = rc.Width;
            int iHeight = rc.Height;
            Image myImage = new Bitmap(iWidth - 100, iHeight - 105);
            Graphics g = Graphics.FromImage(myImage);
            this.Hide();
            Thread.Sleep(txtboxDelayint);
            g.CopyFromScreen(new Point(100, 40), new Point(0, 0), new Size(iWidth - 100, iHeight - 105));
            this.Show();
            g.TranslateTransform(0, iHeight);
            g.RotateTransform(-45);
            g.RotateTransform(45);
            TestOnSeal _top = new TestOnSeal();
            _top.TextFont = new Font("黑体", 16, FontStyle.Bold);
            _top.FillColor = Color.Red;
            _top.Text = "广达(上海)制造城";
            _top.BaseString = "5DX专用";
            _top.ShowPath = true;
            _top.LetterSpace = 1;
            _top.SealSize = 180;
            _top.CharDirection = Char_Direction.Center;
            _top.SetIndent(20);
            g.DrawImage(_top.TextOnPathBitmap(), 1600, -300);
            _top.SetIndent(20);
            g.Dispose();
            string parentPath = this.textBox3.Text.Trim();
            string timefolder = DateTime.Now.ToString("mmddss");
            string timefolder2 = DateTime.Now.ToString("yyy-MM-dd");
            DateTime dt = DateTime.Now;
            string dateStr1 = "20:00:00";
            string dateStr2 = dt.ToString("HH:mm:ss");
            DateTime t1 = Convert.ToDateTime(dateStr1);
            DateTime t2 = Convert.ToDateTime(dateStr2);
            int compNum = DateTime.Compare(t1, t2);
            if (compNum > 0)
            {
                string saveRealPath = parentPath + "\\" + timefolder2 + "\\" + snnumber + "__"  + timefolder + ".jpg";
                string filepaht = parentPath + "\\" + timefolder2;
                if (!Directory.Exists(filepaht))
                {
                    Directory.CreateDirectory(filepaht);
                }
                myImage.Save(saveRealPath);
                string filepath = filepaht + "\\" + "Content.txt";
                if (!File.Exists(filepath))
                {
                    TextWriter tw = new StreamWriter(filepath);
                    tw.WriteLine(snnumber.Trim().ToUpper());
                    tw.Close();
                }
                else
                {
                    TextWriter tw = new StreamWriter(filepath, true);
                    tw.WriteLine(snnumber.Trim().ToUpper());
                    tw.Close();
                }
            }
            if (compNum < 0)
            {
                string tommorm = DateTime.Now.AddDays(+1).ToString("yyyy-MM-dd");
                string saveRealPath = parentPath  + "\\" + tommorm + "\\" + snnumber  + "__" + timefolder + ".jpg";
                string filepaht = parentPath + "\\" + tommorm;
                if (!Directory.Exists(filepaht))
                {
                    Directory.CreateDirectory(filepaht);
                }
                myImage.Save(saveRealPath);
                string filepath = filepaht + "\\" + "Content.txt";
                if (!File.Exists(filepath))
                {
                    TextWriter tw = new StreamWriter(filepath);
                    tw.WriteLine(snnumber.Trim().ToUpper());
                    tw.Close();
                }
                else
                {
                    TextWriter tw = new StreamWriter(filepath, true);
                    tw.WriteLine(snnumber.Trim().ToUpper());
                    tw.Close();
                }
            }
        }



        /// <summary>
        /// 通用截图API方法的的封装
        /// </summary>
        /// <param name="snnumber">sn</param>
        /// <param name="lineStr">线别</param>
        private void SaveImageAndDelety(string snnumber, string lineStr, string machineStr)
        {
            string txtboxDelay = this.txtboxDelay.Text.Trim();
            int txtboxDelayint = int.Parse(txtboxDelay);
            Screen scr = Screen.PrimaryScreen;
            Rectangle rc = scr.Bounds;
            int iWidth = rc.Width;
            int iHeight = rc.Height;
            Image myImage = new Bitmap(iWidth - 100, iHeight - 105);
            Graphics g = Graphics.FromImage(myImage);
            this.Hide();
            Thread.Sleep(txtboxDelayint);
            g.CopyFromScreen(new Point(100, 40), new Point(0, 0), new Size(iWidth - 100, iHeight - 105));
            this.Show();
            g.TranslateTransform(0, iHeight);
            g.RotateTransform(-45);
            g.RotateTransform(45);
            TestOnSeal _top = new TestOnSeal();
            _top.TextFont = new Font("黑体", 16, FontStyle.Bold);
            _top.FillColor = Color.Red;
            _top.Text = "广达(上海)制造城";
            _top.BaseString = "5DX专用";
            _top.ShowPath = true;
            _top.LetterSpace = 1;
            _top.SealSize = 180;
            _top.CharDirection = Char_Direction.Center;
            _top.SetIndent(20);
            g.DrawImage(_top.TextOnPathBitmap(), 1600, -300);
            _top.SetIndent(20);
            g.Dispose();
            string parentPath = this.txtBoxPath.Text.Trim();
            string timefolder = DateTime.Now.ToString("mmddss");
            string timefolder2 = DateTime.Now.ToString("MMdd");
            DateTime dt = DateTime.Now;
            string dateStr1 = "20:00:00";
            string dateStr2 = dt.ToString("HH:mm:ss");
            DateTime t1 = Convert.ToDateTime(dateStr1);
            DateTime t2 = Convert.ToDateTime(dateStr2);
            int compNum = DateTime.Compare(t1, t2);
            if (compNum > 0)
            {
                string saveRealPath = parentPath + "\\" + lineStr + "\\" + timefolder2 + "\\" + snnumber + "__" + machineStr + "__" + timefolder + ".jpg";
                string filepaht = parentPath + "\\" + lineStr + "\\" + timefolder2;
                if (!Directory.Exists(filepaht))
                {
                    Directory.CreateDirectory(filepaht);
                }
                myImage.Save(saveRealPath);
                string filepath = filepaht + "\\" + "Content.txt";
                if (!File.Exists(filepath))
                {
                    TextWriter tw = new StreamWriter(filepath);
                    tw.WriteLine(snnumber.Trim().ToUpper());
                    tw.Close();
                }
                else
                {
                    TextWriter tw = new StreamWriter(filepath, true);
                    tw.WriteLine(snnumber.Trim().ToUpper());
                    tw.Close();
                }
            }
            if (compNum < 0)
            {
                string tommorm = DateTime.Now.AddDays(+1).ToString("MMdd");
                string saveRealPath = parentPath + "\\" + lineStr + "\\" + tommorm + "\\" + snnumber + "__" + machineStr + "__" + timefolder + ".jpg";
                string filepaht = parentPath + "\\" + lineStr + "\\" + tommorm;
                if (!Directory.Exists(filepaht))
                {
                    Directory.CreateDirectory(filepaht);
                }
                myImage.Save(saveRealPath);
                string filepath = filepaht + "\\" + "Content.txt";
                if (!File.Exists(filepath))
                {
                    TextWriter tw = new StreamWriter(filepath);
                    tw.WriteLine(snnumber.Trim().ToUpper());
                    tw.Close();
                }
                else
                {
                    TextWriter tw = new StreamWriter(filepath, true);
                    tw.WriteLine(snnumber.Trim().ToUpper());
                    tw.Close();
                }
            }
        }

        #region tab1 的所有代码公共部分
        private void button1_Click(object sender, EventArgs e)
        {
            //截图
            //非空校验
            if (this.textBoxMachine.Text.Length != 0 && textBoxSN.Text.Length != 0 && textBoxLine.Text.Length != 0)
            {
                SaveImageAndDelety(textBoxSN.Text.Trim().ToUpper(), textBoxLine.Text.Trim(), textBoxMachine.Text.Trim());

               // ALLProduction.ShowBalloonTip(1,"提示",textBoxSN.Text+" 截图成功!",ToolTipIcon.Warning);
            }
            else
            {
                MessageBox.Show("确保输入框有值");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //模糊搜索
            //模糊搜索按钮
            if (this.textBox1.Text.Length == 0)
            {
                MessageBox.Show("请填写模糊匹配的sn");
                return;
            }
            this.listBox2.Items.Clear();
            string sn = this.textBox1.Text.Trim().ToUpper();
            string dt = DateTime.Now.ToString("yyyyMMdd");
            string filepath = @"C:\sn\records\" + dt + ".txt";
            string firstStr = "";
            //读取sn异常捕获一下
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(filepath, Encoding.Default); //文件IO流的读取 创建
                List<string> listSn = new List<string>();
                listSn.Clear();
                while ((firstStr = sr.ReadLine()) != null)
                {
                    listSn.Add(firstStr.ToUpper());
                }
                foreach (string item in listSn)
                {
                    if (item.Contains(sn))
                    {
                        listBox2.Items.Add(item);
                    }
                }
                this.textBox1.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常信息是" + ex.Message);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //重置
            this.listBox1.Items.Clear();
            this.listBox2.Items.Clear();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                button4_Click(null, null);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.txtBoxPath.Text = @"\\172.26.12.16\aoi\5DX\5DX不良";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.txtBoxPath.Text = @"\\172.26.120.75\aoiaxi\AXI\5DX\5DX不良";
        }

     

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string path1 = @"\\172.26.120.75\aoiaxi\AXI\5DX\5DX不良";
            System.Diagnostics.Process.Start("explorer", path1);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string path2 = @"\\172.26.12.16\aoi\5DX\5DX不良";
            System.Diagnostics.Process.Start("explorer", path2);
        }


        //成员变量放置处
        StringBuilder sb = new StringBuilder();
        /// <summary>
        /// 文件创建的时候的执行的函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileSystemWatcher1_Created(object sender, FileSystemEventArgs e)
        {
            string lineStr = "";
            if (e.FullPath.EndsWith("BoardStatus.txt"))
            {
                sb.Clear();
                string snTextPath = e.FullPath.ToString();
                Thread.Sleep(200);
                //Console.WriteLine(snTextPath); //C:\classifiedDefects\data\V810-3328S2EX[@$@]2020-11-03-20-53-12\BoardStatus.txt
                //主线程休眠时间,防止复制的到C盘 classicDefect 的速度很慢 ，导致 BoardStatus.txt 没有复制到
                if (File.Exists(snTextPath))
                {
                    try
                    {
                        //读取sn异常捕获一下
                        StreamReader sr = new StreamReader(snTextPath, Encoding.Default); //文件IO流的读取 创建
                        string firstStr = sr.ReadLine();
                        string[] arrStr = firstStr.Split(';');
                        string sn = arrStr[1];
                        this.textBoxSN.Text = sn;
                        sb.Append(sn);
                        sr.Close();
                        string[] strArr = snTextPath.Split('\\');
                        string lastStr = strArr[strArr.Length - 2];
                        lineStr = lastStr.Split('[')[0];   // 机器编号
                        this.textBoxMachine.Text = lineStr;
                        sb.Append("_");
                        if (lineStr.Equals("V810-8057S2"))
                        {
                            this.textBoxLine.Text = "L22";
                            sb.Append("L22");
                        }
                        if (lineStr.Equals("V810-8064S2"))
                        {
                            this.textBoxLine.Text = "L12";
                            sb.Append("L12");
                        }
                        if (lineStr.Equals("V810-8070S2"))
                        {
                            this.textBoxLine.Text = "K22";
                            sb.Append("K22");
                        }
                        if (lineStr.Equals("V810-8085S2"))
                        {
                            this.textBoxLine.Text = "K12";
                            sb.Append("K12");
                        }
                        if (lineStr.Equals("V810-8096S2"))
                        {
                            this.textBoxLine.Text = "J12";
                            sb.Append("J12");
                        }
                        if (lineStr.Equals("V810-3327S2EX"))
                        {
                            this.textBoxLine.Text = "I12";
                            sb.Append("I12");
                        }
                        if (lineStr.Equals("V810-3323S2EX"))
                        {
                            this.textBoxLine.Text = "I22";
                            sb.Append("I22");
                        }
                        if (lineStr.Equals("V810-3328S2EX"))
                        {
                            this.textBoxLine.Text = "H12";
                            sb.Append("H12");
                        }
                        if (lineStr.Equals("V810-8086S2"))
                        {
                            this.textBoxLine.Text = "P12";
                            sb.Append("P12");
                        }
                        if (lineStr.Equals("V810-3325S2EX"))
                        {
                            this.textBoxLine.Text = "Q12";
                            sb.Append("Q12");
                        }
                        sb.Append("_");
                        sb.Append(lineStr);
                        this.listBox1.Items.Add(sb.ToString().Trim());
                        if (!Directory.Exists(@"C:\sn\records\"))
                        {
                            Directory.CreateDirectory(@"C:\sn\records\");
                        }
                        string dt = DateTime.Now.ToString("yyyyMMdd");
                        string filepath = @"C:\sn\records\" + dt + ".txt";
                        if (!File.Exists(filepath))
                        {
                            TextWriter tw = new StreamWriter(filepath);
                            tw.WriteLine(sb.ToString().Trim());
                            tw.Close();
                        }
                        else
                        {
                            TextWriter tw = new StreamWriter(filepath, true);
                            tw.WriteLine(sb.ToString().Trim());
                            tw.Close();
                        }
                        string[] strArray = new string[this.listBox1.Items.Count];
                        this.listBox1.Items.CopyTo(strArray, 0);
                        Array.Reverse(strArray);
                        listBox2.Items.Clear();
                        foreach (string str in strArray)
                        {
                            listBox2.Items.Add(str);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("异常信息是" + ex.Message);
                    }
                }
            }
        }
        #endregion

        #region  截图传统的所有代码公共部分抽取
        private void button7_Click(object sender, EventArgs e)
        {
            //L22
            if (txtBoxL2.Text == "" || textBoxFirst.Text=="")
            {
                MessageBox.Show("请输入Sn","提示信息");
                return;
            }
            SaveImageAndDelety(txtBoxL2.Text, textBoxFirst.Text);

            txtBoxL2.Text = "";
            txtBoxL2.Focus();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //L12

            if (txtBoxP1.Text == "" || textBoxSecond.Text == "")
            {
                MessageBox.Show("请输入Sn", "提示信息");
                return;
            }
            SaveImageAndDelety(txtBoxP1.Text, textBoxSecond.Text);
            txtBoxP1.Text = "";
            txtBoxP1.Focus();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //K22
            if (txtBoxQ1.Text == "" || textBoxThried.Text == "")
            {
                MessageBox.Show("请输入Sn", "提示信息");
                return;
            }
            SaveImageAndDelety(txtBoxQ1.Text, textBoxThried.Text);
            txtBoxQ1.Text = "";
            txtBoxQ1.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //K12
            if (txtBoxR1.Text == "" || textBoxFour.Text == "")
            {
                MessageBox.Show("请输入Sn", "提示信息");
                return;
            }
            SaveImageAndDelety(txtBoxR1.Text, textBoxFour.Text);
            txtBoxR1.Text = "";
            txtBoxR1.Focus();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            //J12
            if (textBox18.Text == "" || textBox10.Text == "")
            {
                MessageBox.Show("请输入Sn", "提示信息");
                return;
            }
            SaveImageAndDelety(textBox18.Text, textBox10.Text);
            textBox18.Text = "";
            textBox18.Focus();

        }

        private void button18_Click(object sender, EventArgs e)
        {
            //I22
            if (textBox16.Text == "" || textBox9.Text == "")
            {
                MessageBox.Show("请输入Sn", "提示信息");
                return;
            }
            SaveImageAndDelety(textBox16.Text, textBox9.Text);
            textBox16.Text = "";
            textBox16.Focus();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            //I12
            if (textBox14.Text == "" || textBox8.Text == "")
            {
                MessageBox.Show("请输入Sn", "提示信息");
                return;
            }
            SaveImageAndDelety(textBox14.Text, textBox8.Text);
            textBox14.Text = "";
            textBox14.Focus();

        }

        private void button15_Click(object sender, EventArgs e)
        {
            //H12
            if (textBox12.Text == "" || textBox7.Text == "")
            {
                MessageBox.Show("请输入Sn", "提示信息");
                return;
            }
            SaveImageAndDelety(textBox12.Text, textBox7.Text);
            textBox12.Text = "";
            textBox12.Focus();

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            // 16 Server 选中
            this.textBox4.Text = @"\\172.26.12.16\aoi\5DX\5DX不良";
           // radioButton4.Checked = true;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            // 75 Server 选中
            this.textBox4.Text = @"\\172.26.120.75\aoiaxi\AXI\5DX\5DX不良";
           // radioButton3.Checked = true;
        }

        private void txtBoxL2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                button7_Click(null,null);
            }
        }

        private void txtBoxP1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button8_Click(null, null);
            }
        }

        private void txtBoxQ1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button6_Click(null, null);
            }
        }

        private void txtBoxR1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2_Click(null, null);
            }
        }

        private void textBox18_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button17_Click(null, null);
            }
        }

        private void textBox16_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button18_Click(null, null);
            }
        }

        private void textBox14_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button16_Click(null, null);
            }

        }

        private void textBox12_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button15_Click(null, null);
            }
        }
       #endregion

        #region 气泡收集所有代码
        private void button9_Click(object sender, EventArgs e)
        {
            if (this.textBoxBubble.Text=="")
            {
                MessageBox.Show("请填写Sn","提示信息");
                return;
            }
            SaveImageAndDeletyVoid(this.textBoxBubble.Text.Trim().ToUpper());
            textBoxBubble.Text = "";
            textBoxBubble.Focus();
        }

        private void textBoxBubble_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                button9_Click(null,null);
            }
        }
        #endregion

      

        

        private void button20_Click(object sender, EventArgs e)
        {
            this.textBox6.Text = "";
        }

        private void button21_Click(object sender, EventArgs e)
        {
            //Copy
            if (textBox6.Text != "")
            {
                Clipboard.SetDataObject(textBox6.Text);
                this.label3.Text =textBox6.Text+ " 复制成功!";
            }
            else 
            {
                MessageBox.Show("确保输入框有值!");
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            //NTF
            Clipboard.SetDataObject("NTF");
        }

        private void button14_Click(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            if (this.textBox5.Text == "")
            {
                MessageBox.Show("请输入SN");
                return;
            }
            string lastname = this.textBox5.Text.Trim().ToUpper();
            string sn4 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
            "<ns1:BoardTestXMLExport xmlns:ns1=\"http://tempuri.org/BoardTestXMLExport.xsd\" testTime=\"2020-04-20T00:02:01.000+08:00\" testerTestStartTime=\"2020-04-20T00:02:01.000+08:00\" testerTestEndTime=\"2020-04-20T00:02:14.000+08:00\" numberOfComponentsTested=\"1\" numberOfJointsTested=\"1667\" numberOfIndictedComponents=\"0\" numberOfIndictedPins=\"0\" numberOfDefects=\"0\" testStatus=\"Passed\" repairStatus=\"Repair None\" repairStationId=\"3328-VVTS\">\r\n" +
            "    <ns1:BoardXML serialNumber=\"" + lastname + "\" imageId=\"1\" boardType=\"F20-MB-00Y0-E3N-DD-02\" boardRevision=\"1587312134000\" assemblyRevision=\"F20-MB-00Y0-E3N-DD-02\"/>\r\n" +
            "    <ns1:StationXML testerName=\"V810-8086S2\" stage=\"v810\"/>\r\n" +
            "    <ns1:RepairEventXML repairStartTime=\"2020-04-20T00:02:16.000+08:00\" repairEndTime=\"2020-04-20T00:03:59.000+08:00\" repairOperator=\"c_admin\" numberOfActiveDefects=\"0\" numberOfActiveComponents=\"0\" numberOfActivePins=\"0\" numberOfFalseCalledDefects=\"0\" numberOfFalseCalledComponents=\"0\" numberOfFalseCalledPins=\"0\" numberOfRepairedDefects=\"0\" numberOfRepairedComponents=\"0\" numberOfRepairedPins=\"0\" numberOfRepairLaterDefects=\"0\" numberOfRepairLaterComponents=\"0\" numberOfRepairLaterPins=\"0\" numberOfVariationOkDefects=\"0\" numberOfVariationOkComponents=\"0\" numberOfVariationOkPins=\"0\"/>\r\n" +
            "</ns1:BoardTestXMLExport>\r\n" +
            "";
            string fileoutname = "C:\\ITF\\XMLCEXPORT\\" + lastname + "_#F08-MB-00B0-F3J-DD-01#AXI#system-764#1_012524.xml";
            if (!File.Exists(fileoutname))
            {
                TextWriter tw2 = new StreamWriter(fileoutname);
                tw2.WriteLine(sn4);
                tw2.Close();
            }
            else
            {
            }
            this.label3.Text = textBox5.Text + " 补资料成功!";
            listBox3.Items.Add(textBox5.Text.Trim().ToUpper());
            this.textBox5.Text = "";
            this.textBox5.Focus();
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            if (this.textBox6.Text == "")
            {
                MessageBox.Show("请输入SN");
                return;
            }
            string lastname = this.textBox6.Text.Trim().ToUpper();
            string sn4 = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
            "<ns1:BoardTestXMLExport xmlns:ns1=\"http://tempuri.org/BoardTestXMLExport.xsd\" testTime=\"2020-04-20T00:02:01.000+08:00\" testerTestStartTime=\"2020-04-20T00:02:01.000+08:00\" testerTestEndTime=\"2020-04-20T00:02:14.000+08:00\" numberOfComponentsTested=\"1\" numberOfJointsTested=\"1667\" numberOfIndictedComponents=\"0\" numberOfIndictedPins=\"0\" numberOfDefects=\"0\" testStatus=\"Passed\" repairStatus=\"Repair None\" repairStationId=\"3328-VVTS\">\r\n" +
            "    <ns1:BoardXML serialNumber=\"" + lastname + "\" imageId=\"1\" boardType=\"F20-MB-00Y0-E3N-DD-02\" boardRevision=\"1587312134000\" assemblyRevision=\"F20-MB-00Y0-E3N-DD-02\"/>\r\n" +
            "    <ns1:StationXML testerName=\"V810-8086S2\" stage=\"v810\"/>\r\n" +
            "    <ns1:RepairEventXML repairStartTime=\"2020-04-20T00:02:16.000+08:00\" repairEndTime=\"2020-04-20T00:03:59.000+08:00\" repairOperator=\"c_admin\" numberOfActiveDefects=\"0\" numberOfActiveComponents=\"0\" numberOfActivePins=\"0\" numberOfFalseCalledDefects=\"0\" numberOfFalseCalledComponents=\"0\" numberOfFalseCalledPins=\"0\" numberOfRepairedDefects=\"0\" numberOfRepairedComponents=\"0\" numberOfRepairedPins=\"0\" numberOfRepairLaterDefects=\"0\" numberOfRepairLaterComponents=\"0\" numberOfRepairLaterPins=\"0\" numberOfVariationOkDefects=\"0\" numberOfVariationOkComponents=\"0\" numberOfVariationOkPins=\"0\"/>\r\n" +
            "</ns1:BoardTestXMLExport>\r\n" +
            "";
            string fileoutname = "C:\\ITF\\XMLCEXPORT\\" + lastname + "_#F08-MB-00B0-F3J-DD-01#AXI#system-764#1_012524.xml";
            if (!File.Exists(fileoutname))
            {
                TextWriter tw2 = new StreamWriter(fileoutname);
                tw2.WriteLine(sn4);
                tw2.Close();
            }
            else
            {
            }
            listBox3.Items.Add(textBox6.Text.Trim().ToUpper());
           // this.textBox5.Text = "";
            this.textBox6.Focus();
            this.label3.Text = textBox6.Text + " 补资料成功!";
        }

        private void linkLabel23_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //XML 
            string path1 = @"C:\ITF\XMLCEXPORT";
            System.Diagnostics.Process.Start("explorer", path1);
        }

        private void listBox3_MouseClick(object sender, MouseEventArgs e)
        {
            if(listBox3.SelectedItem!=null)
            {
                Clipboard.SetDataObject(listBox3.SelectedItem.ToString());
                this.label3.Text = listBox3.SelectedItem.ToString() + " 复制成功!";
             
            }
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button11_Click_1(null, null);
            }
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
              button10_Click_1(null,null);
            
            }
        }

        private void linkLabel13_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string datas = DateTime.Now.ToString("MMdd");
            string path = @"\\172.26.12.16\aoi\5DX\5DX不良\K22\" + datas;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("不存在的路径,请检查", "提示信息");
                return;
            }

            System.Diagnostics.Process.Start("explorer", path);
        }

        private void linkLabel16_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string datas = DateTime.Now.ToString("MMdd");
            string path = @"\\172.26.12.16\aoi\5DX\5DX不良\L12\" + datas;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("不存在的路径,请检查", "提示信息");
                return;
            }

            System.Diagnostics.Process.Start("explorer", path);
        }

        private void linkLabel15_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string datas = DateTime.Now.ToString("MMdd");
            string path = @"\\172.26.12.16\aoi\5DX\5DX不良\L22\" + datas;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("不存在的路径,请检查", "提示信息");
                return;
            }

            System.Diagnostics.Process.Start("explorer", path);
           
        }

        private void linkLabel17_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string datas = DateTime.Now.ToString("MMdd");
            string path = @"\\172.26.12.16\aoi\5DX\5DX不良\P12\" + datas;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("不存在的路径,请检查", "提示信息");
                return;
            }

            System.Diagnostics.Process.Start("explorer", path);
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string datas = DateTime.Now.ToString("MMdd");
            string path = @"\\172.26.120.75\aoiaxi\AXI\5DX\5DX不良\K22\" + datas;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("不存在的路径,请检查", "提示信息");
                return;
            }
            System.Diagnostics.Process.Start("explorer", path);
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string datas = DateTime.Now.ToString("MMdd");
            string path = @"\\172.26.120.75\aoiaxi\AXI\5DX\5DX不良\L12\" + datas;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("不存在的路径,请检查", "提示信息");
                return;
            }
            System.Diagnostics.Process.Start("explorer", path);
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string datas = DateTime.Now.ToString("MMdd");
            string path = @"\\172.26.120.75\aoiaxi\AXI\5DX\5DX不良\L22\" + datas;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("不存在的路径,请检查", "提示信息");
                return;
            }
            System.Diagnostics.Process.Start("explorer", path);
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string datas = DateTime.Now.ToString("MMdd");
            string path = @"\\172.26.120.75\aoiaxi\AXI\5DX\5DX不良\P12\" + datas;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("不存在的路径,请检查", "提示信息");
                return;
            }
            System.Diagnostics.Process.Start("explorer", path);
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string datas = DateTime.Now.ToString("MMdd");
            string path = @"\\172.26.120.75\aoiaxi\AXI\5DX\5DX不良\Q12\" + datas;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("不存在的路径,请检查", "提示信息");
                return;
            }
            System.Diagnostics.Process.Start("explorer", path);
        }

        private void linkLabel12_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string datas = DateTime.Now.AddDays(+1).ToString("MMdd");
            string path = @"\\172.26.120.75\aoiaxi\AXI\5DX\5DX不良\K22\" + datas;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("不存在的路径,请检查", "提示信息");
                return;
            }
            System.Diagnostics.Process.Start("explorer", path);
        }

        private void linkLabel11_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string datas = DateTime.Now.AddDays(+1).ToString("MMdd");
            string path = @"\\172.26.120.75\aoiaxi\AXI\5DX\5DX不良\L12\" + datas;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("不存在的路径,请检查", "提示信息");
                return;
            }
            System.Diagnostics.Process.Start("explorer", path);
        }

        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            string datas = DateTime.Now.AddDays(+1).ToString("MMdd");
            string path = @"\\172.26.120.75\aoiaxi\AXI\5DX\5DX不良\L22\" + datas;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("不存在的路径,请检查", "提示信息");
                return;
            }
            System.Diagnostics.Process.Start("explorer", path);
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            string datas = DateTime.Now.AddDays(+1).ToString("MMdd");
            string path = @"\\172.26.120.75\aoiaxi\AXI\5DX\5DX不良\P12\" + datas;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("不存在的路径,请检查", "提示信息");
                return;
            }
            System.Diagnostics.Process.Start("explorer", path);
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            string datas = DateTime.Now.AddDays(+1).ToString("MMdd");
            string path = @"\\172.26.120.75\aoiaxi\AXI\5DX\5DX不良\Q12\" + datas;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("不存在的路径,请检查", "提示信息");
                return;
            }

            System.Diagnostics.Process.Start("explorer", path);
        }

        private void linkLabel14_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string datas = DateTime.Now.ToString("MMdd");
            string path = @"\\172.26.12.16\aoi\5DX\5DX不良\Q12\" + datas;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("不存在的路径,请检查", "提示信息");
                return;
            }

            System.Diagnostics.Process.Start("explorer", path);
        }

        private void linkLabel19_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string datas = DateTime.Now.AddDays(+1).ToString("MMdd");
            string path = @"\\172.26.12.16\aoi\5DX\5DX不良\K22\" + datas;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("不存在的路径,请检查", "提示信息");
                return;
            }

            System.Diagnostics.Process.Start("explorer", path);
        }

        private void linkLabel21_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string datas = DateTime.Now.AddDays(+1).ToString("MMdd");
            string path = @"\\172.26.12.16\aoi\5DX\5DX不良\L12\" + datas;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("不存在的路径,请检查", "提示信息");
                return;
            }

            System.Diagnostics.Process.Start("explorer", path);
        }

        private void linkLabel22_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string datas = DateTime.Now.AddDays(+1).ToString("MMdd");
            string path = @"\\172.26.12.16\aoi\5DX\5DX不良\L22\" + datas;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("不存在的路径,请检查", "提示信息");
                return;
            }

            System.Diagnostics.Process.Start("explorer", path);
        }

        private void linkLabel20_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string datas = DateTime.Now.AddDays(+1).ToString("MMdd");
            string path = @"\\172.26.12.16\aoi\5DX\5DX不良\P12\" + datas;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("不存在的路径,请检查", "提示信息");
                return;
            }

            System.Diagnostics.Process.Start("explorer", path);
        }

        private void linkLabel18_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string datas = DateTime.Now.AddDays(+1).ToString("MMdd");
            string path = @"\\172.26.12.16\aoi\5DX\5DX不良\Q12\" + datas;
            if (!Directory.Exists(path))
            {
                MessageBox.Show("不存在的路径,请检查", "提示信息");
                return;
            }

            System.Diagnostics.Process.Start("explorer", path);
        }


        #region
        private void button12_Click(object sender, EventArgs e)
        {
            KillProcess("RepairTool");
        }

        private void KillProcess(string processName)
        {
            Process[] myproc = Process.GetProcesses();
            bool flag = false;
            foreach (Process item in myproc)
            {
                Console.WriteLine(item.ProcessName);
                if (item.ProcessName == processName)
                {
                    item.Kill();
                    flag = true;
                }
            }

            if (flag == false)
            {
                MessageBox.Show("没有要关闭的VVTS,请重试!");
            }
            if (flag)
            {
                if (checkBox1.Checked == true)
                {
                    Process process3 = new Process();
                    process3.StartInfo.FileName = @"C:\Program Files\RepairToolAxi\RepairTool.exe";
                    process3.StartInfo.Arguments = string.Format("10");
                    process3.Start();
                }
            }
        }



        private void button13_Click(object sender, EventArgs e)
        {

            Process process = null;
            Process process2 = null;

            if (numDown.Value == 1) 
            {

                System.Diagnostics.ProcessStartInfo Info = new System.Diagnostics.ProcessStartInfo();
                try
                {
                    process = new Process();
                    process.StartInfo.FileName = @"C:\Program Files\RepairToolAxi\RepairTool.exe";
                    process.StartInfo.Arguments = string.Format("10");
                    Info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized; //最小化启动
                    process.Start();
                }
                catch (Exception ex)
                {
                }


            }
            else if (numDown.Value == 2)
            {
                System.Diagnostics.ProcessStartInfo Info = new System.Diagnostics.ProcessStartInfo();
                try
                {
                    process = new Process();
                    process.StartInfo.FileName = @"C:\Program Files\RepairToolAxi\RepairTool.exe";
                    process.StartInfo.Arguments = string.Format("10");
                    Info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized; //最小化启动
                    process.Start();
                    Thread.Sleep(1000);
                    process2 = new Process();
                    process2.StartInfo.FileName = @"C:\Program Files\RepairToolAxi\RepairTool.exe";
                    process2.StartInfo.Arguments = string.Format("10");
                    process2.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal; //最小化启动
                    process2.Start();

                }
                catch (Exception ex)
                {
                }

            } 
            
            
            
            
           


          
        }

        #endregion
        #region 托资料系统全部代码模块

        //成员变量 放置处
        List<string> listSn = new List<string>();
        //  List<string> listMachines = new List<string>();
        List<string> listrecipse = new List<string>();
        List<string> listCreateTime = new List<string>();
        List<string> listPath = new List<string>();
        bool first = true;

        private void button29_Click(object sender, EventArgs e)
        {
            this.checkedListBox1.Items.Clear();
            DirectoryInfo TheFolder = new DirectoryInfo(this.textBox21.Text.Trim());
            string textMohuSn = this.textBox20.Text.Trim().ToUpper();
            if (textMohuSn.Length != 0)
            {
                foreach (FileInfo NextFolder in TheFolder.GetFiles())
                {
                    if (NextFolder.Name.Split('#')[0].ToUpper().Contains(textMohuSn))
                    {
                        this.checkedListBox1.Items.Add(NextFolder.Name.Split('#')[0].ToUpper());
                    }
                }
            }
            else {

                MessageBox.Show("请输入Sn","提示信息");
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string path = this.comboBox1.SelectedItem.ToString();

            this.textBox21.Text = path;
            comboBox1.Enabled=false;

        }


        private void button24_Click(object sender, EventArgs e)
        {
            comboBox1.Enabled = true;
        }


        private void button27_Click(object sender, EventArgs e)
        {
            textBox22.Text = "";
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            string path = comboBox1.SelectedItem.ToString().Substring(0, comboBox1.SelectedItem.ToString().Length - 8);


            string dataStr = this.dateTimePicker1.Text;
            dataStr = dataStr.Replace("-", "");
            this.textBox21.Text = path + dataStr;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listSn.Clear();
            // listMachines.Clear();
            listCreateTime.Clear();
            listrecipse.Clear();
            listPath.Clear();
            DirectoryInfo TheFolder = new DirectoryInfo(this.textBox21.Text.Trim());
            this.dataGridView1.Rows.Clear();
            string textsn = this.textBox22.Text.Trim().ToUpper();
            if (textsn.Length != 0)
            {

                foreach (FileInfo NextFolder in TheFolder.GetFiles())
                {
                    if (textsn.Equals(NextFolder.Name.Split('#')[0].ToUpper()))
                    {
                        listCreateTime.Add(NextFolder.CreationTime.ToString());
                        listPath.Add(NextFolder.Name);
                        string[] strArr = NextFolder.Name.Split('#');
                        listSn.Add(strArr[0]);
                        //  listMachines.Add(strArr[3].Substring(0, 9));
                        listrecipse.Add(strArr[1]);
                    }
                }
                for (int i = 0; i < listSn.Count(); i++)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dataGridView1);
                    row.Cells[1].Value = listSn[i];
                    row.Cells[2].Value = listCreateTime[i];  //时间
                    // row.Cells[3].Value = listMachines[i];
                    row.Cells[4].Value = listrecipse[i];
                    row.Cells[5].Value = listPath[i];
                    this.dataGridView1.Rows.Add(row);
                }
                MessageBox.Show(textsn + "查询结果");
                return;
            }
            foreach (FileInfo NextFolder in TheFolder.GetFiles())
            {
                listCreateTime.Add(NextFolder.CreationTime.ToString());
                listPath.Add(NextFolder.Name);
                string[] strArr = NextFolder.Name.Split('#');
                listSn.Add(strArr[0]);
                //  listMachines.Add(strArr[3].Substring(0, 9));
                listrecipse.Add(strArr[1]);
            }
            for (int i = 0; i < listSn.Count(); i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView1);
                row.Cells[1].Value = listSn[i];
                row.Cells[2].Value = listCreateTime[i];  //时间
                //   row.Cells[3].Value = listMachines[i];
                row.Cells[4].Value = listrecipse[i];
                row.Cells[5].Value = listPath[i];
                this.dataGridView1.Rows.Add(row);
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            string textsn = this.textBox22.Text.Trim().ToUpper();
            int count = this.dataGridView1.Rows.Count;
            string asdas = "";
            List<string> arr = new List<string>();
            string[] strArr = { };
            for (int i = 0; i < count; i++)
            {
                DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)this.dataGridView1.Rows[i].Cells[0];
                Boolean flag = Convert.ToBoolean(checkCell.Value);
                if (true == flag)
                {
                    asdas = this.dataGridView1.Rows[i].Cells[1].Value.ToString();
                    arr.Add(this.dataGridView1.Rows[i].Cells[5].Value.ToString());
                }
                else
                {
                    continue;
                }
            }
            DirectoryInfo direct = new DirectoryInfo(this.textBox21.Text.Trim());
            FileInfo[] files = direct.GetFiles();
            string mainpath = textBox21.Text.Replace('\\', '/').Split('/')[2];
            foreach (FileInfo file in files)
            {
                foreach (string sn in arr)
                {
                    if (sn.Equals(file.Name))
                    {
                        file.CopyTo(Path.Combine(@"\\" + mainpath + @"\c\itf\xmlcexport", file.Name), true);
                    }
                }
            }

            MessageBox.Show(asdas + "拖资料成功!");
            this.button25.Enabled = false;


            foreach (string sn2 in arr)
            {
                this.listBox4.Items.Add(sn2.ToUpper().ToString().Split('#')[0]);

            }
            this.textBox22.Text = "";
            this.textBox22.Focus();
            for (int i = 0; i < count; i++)
            {
                DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)this.dataGridView1.Rows[i].Cells[0];
                Boolean flag = Convert.ToBoolean(checkCell.Value);
                if (true == flag)
                {
                    checkCell.Value = false;
                }
                else
                {
                    continue;
                }
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dataGridView1.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView1.RowHeadersDefaultCellStyle.Font, rectangle,
                dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string stds1 = this.dataGridView1.CurrentRow.Cells[1].Value.ToString();
            string action = dataGridView1.Columns[e.ColumnIndex].Name;
            Console.WriteLine(action);
            switch (action)
            {
                case "Column2":
                    Clipboard.SetText(stds1);
                    label15.Text = "提示信息";
                    this.label16.Text = stds1;
                    this.label17.Text = "复制成功!";
                    break;
                case "Column1":
                    this.button25.Enabled = true;
                    break;
                default:
                    break;
            }
        }


        private void setRowNumber(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
            }
        }



        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedItem.ToString().Length == 0)
            {
                return;
            }
            string stds1 = checkedListBox1.SelectedItem.ToString();
            //  Clipboard.SetText(stds1);
            Clipboard.SetDataObject(stds1);
            MessageBox.Show(stds1 + " 复制成功","提示信息");
            
        }

        private void listBox4_MouseClick(object sender, MouseEventArgs e)
        {
            if (listBox4.SelectedItem != null)
            {
                Clipboard.SetDataObject(listBox4.SelectedItem.ToString().Trim());
                MessageBox.Show(listBox4.SelectedItem.ToString() + " 复制成功", "提示信息");
            }
        }
        #endregion
         #region 强制关闭SNInfor
        const int WM_CLOSE = 0x0010;
        private void button22_Click(object sender, EventArgs e)
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new System.Timers.ElapsedEventHandler(theout);
            aTimer.Interval = 2000;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
            MessageBox.Show("开启成功!每2S检查一次窗口,如果发现窗口,则强制关闭","提示信息");
        }
        public void theout(object source,System.Timers.ElapsedEventArgs e) 
        {
            IntPtr awin = MouseHookHelper.FindWindow("ThunderRT6FormDC", "Error Message 20011220 VER.2.5");
            if (awin == IntPtr.Zero)
            {
              //  MessageBox.Show("没有找到窗体");
               // return;
            }
            else
            { 
              MouseHookHelper.SendMessage(awin, WM_CLOSE, 0, 0);
            }


        }

        private void button23_Click(object sender, EventArgs e)
        {
            MessageBox.Show("未实现该功能,请自行关闭本软件,再打开", "提示信息");
        }
         #endregion

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl1.SelectedIndex==0)
            {
                Size s = new Size(300, 320);
                this.Size = s;

                this.TopMost = true;
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                Size s = new Size(250,424);
                this.Size = s;
                this.TopMost = true;
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                Size s = new Size(273, 183);
                this.Size = s;
                this.TopMost = true;
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                Size s = new Size(406, 336);
                this.Size = s;
                this.TopMost = false;
            }
            else if (tabControl1.SelectedIndex == 4)
            {
                Size s = new Size(352, 317);
                this.Size = s;
                this.TopMost = false;
            }
            else if (tabControl1.SelectedIndex == 5)
            {
                Size s = new Size(279, 282);
                this.Size = s;
                this.TopMost = false;
            }
            else if (tabControl1.SelectedIndex == 6)
            {
                Size s = new Size(919, 387);
                this.Size = s;
                this.TopMost = false;
            }
            else if (tabControl1.SelectedIndex == 7)
            {
                Size s = new Size(919, 387);
                this.Size = s;
                this.TopMost = false;
            }
            else if (tabControl1.SelectedIndex == 8)
            {
                Size s = new Size(919, 387);
                this.Size = s;
                this.TopMost = false;
            }
        }
    }
}
