using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
namespace 透明浏览器
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();







        }
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);



        bool beginMove = false;//初始化鼠标位置
        int currentXPosition;
        int currentYPosition;
        private long page = 0;
        private long index = 0;
        string[] lines;
        string path;
        string configpath = Application.StartupPath + "\\config.txt";

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] config1 = getcongfig(configpath);
            page = long.Parse(config1[0])-6;
            path = config1[1];
            lines = File.ReadAllLines(path);
        }
        private string[] getcongfig(string path)
        {
            string[] a = File.ReadAllLines(path);
           
               
           
           
            return a;
        }
        public void DeleteFile(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            if (attr == FileAttributes.Directory)
            {
                Directory.Delete(path, true);
            }
            else
            {
                File.Delete(path);
            }
        }
       /// <summary>
       /// 关闭窗口时保存当前进度
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
           
            string line1 = page.ToString() ;
            String line2 = path;
            try
            {
                if (path!=null)
                {
                    DeleteFile (configpath);
                    StreamWriter sw = new StreamWriter(configpath,true  );
                    sw.WriteLine(line1);
                    sw.WriteLine(line2);
                    sw.Close();
                }
            }catch(Exception)
            {
                
            }

            
        }
        //获取鼠标按下时的位置
        private void loginForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                beginMove = true;
                currentXPosition = MousePosition.X;//鼠标的x坐标为当前窗体左上角x坐标
                currentYPosition = MousePosition.Y;//鼠标的y坐标为当前窗体左上角y坐标
            }
        }
        //获取鼠标移动到的位置
        private void loginForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (beginMove)
            {
                this.Left += MousePosition.X - currentXPosition;//根据鼠标x坐标确定窗体的左边坐标x
                this.Top += MousePosition.Y - currentYPosition;//根据鼠标的y坐标窗体的顶部，即Y坐标
                currentXPosition = MousePosition.X;
                currentYPosition = MousePosition.Y;
            }
        }
        //释放鼠标时的位置
        private void loginForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                currentXPosition = 0; //设置初始状态
                currentYPosition = 0;
                beginMove = false;
            }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            path = openFileDialog1.FileName;
            //FileStream fs = new FileStream(path ,FileMode.Open);
            //StreamReader sr = new StreamReader(fs);
            //string lines = sr.ReadToEnd();
            //textBox1 .Text = lines;
            if (path != "openFileDialog1")
            {
                lines = File.ReadAllLines(path);
                page = 0;
                index = 0;
            }
            else
            {
                MessageBox.Show("文件打开失败，请重新选择文件！");
            }
            //for (long i=page; i <=page+20; i++)
            //{
            //    textBox1.AppendText(lines[i]+ Environment.NewLine);
            //}

            //foreach (string line in lines)
            //{
            //    textBox1.AppendText(line + Environment .NewLine );
            //}
            page = 0;
            index = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            index = index + 1;
            for (long i = page; i <= page + 5; i++)
            {
                if(i<=lines.Length) {
                    textBox1.AppendText(lines[i] + Environment.NewLine);
                    
                }
                else
                {
                    MessageBox.Show("已经到最后了！");
                    break;
                }
                
            }
            textBox1.AppendText("*****" + "第" + index + "页" + "******" + Environment.NewLine);
            page = page + 6;

        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:
                    //this.WindowState = FormWindowState.Maximized;
                    break;
                case Keys.Left:
                    this.WindowState = FormWindowState.Minimized;
                    break;
                case Keys.Up:
                    Application.Exit();
                    break;
                case Keys.Down:

                    index = index + 1;
                    for (long i = page; i <= page + 5; i++)
                    {
                        if (i <= lines.Length-1)
                        {
                            textBox1.AppendText(lines[i] + Environment.NewLine);

                        }
                        else
                        {
                            MessageBox.Show("已经到最后了！");
                            break;
                        }
                    }
                    textBox1.AppendText("*****" + "第" + index + "页" + "******" + Environment.NewLine);
                    page = page + 6;
                    break;
                case Keys.Space:
                    openFileDialog1.ShowDialog();
                    path = openFileDialog1.FileName;
                    //FileStream fs = new FileStream(path ,FileMode.Open);
                    //StreamReader sr = new StreamReader(fs);
                    //string lines = sr.ReadToEnd();
                    //textBox1 .Text = lines;
                    if (path!="openFileDialog1"  )
                    {
                        lines = File.ReadAllLines(path);
                        page = 0;
                        index = 0;
                    }else
                    {
                        MessageBox.Show("文件打开失败，请重新选择文件！");
                    }
                    
                    //for (long i=page; i <=page+20; i++)
                    break;
                case Keys.Enter:
                    MessageBox.Show("enter");
                    break;
            }
            return true;
        }
    }
}
