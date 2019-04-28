using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace note
{
    

    public partial class Note : Form
    {
        public Note()
        {
            InitializeComponent();
            this.Size = new Size(480, 400);
            int nWidth = SystemInformation.WorkingArea.Width - this.Width;
            int nHeight = SystemInformation.WorkingArea.Height - this.Height;
            this.Location = new Point(nWidth, nHeight);

           

        }

        private void ToolStripMenuItem_Clear_Click(object sender, EventArgs e)
        {
            clear();
        }


        private void ToolStripMenuItem_save_Click(object sender, EventArgs e)
        {
            save();  
        }

      

        /// <summary>
        /// 保存界面的值到csv文件和变量,相同不能保存。
        /// </summary>
        private void save()
        {
            string strTitle = this.textBox1.Text.Replace(',', '&');
            if (!string.IsNullOrEmpty(strTitle))
            {
                string strTemp = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ",";

                string strContext = this.richTextBox1.Text.Replace(',', '&');
                
                strContext = strContext.TrimEnd().Replace('\n', ',');

                strTemp += strTitle + ",";
                strTemp += strContext + '\n';
                try
                {
                    if (m_strTitle == strTitle && m_strContent == strContext)
                    {
                        return;
                    }
                    
                    if (File.Exists(m_strPath))
                    {
                        string[] strList = File.ReadAllLines(m_strPath);
                        for (int n = 0; n < strList.Length; n++)
                        {
                            if (strList[n].Contains(strTitle) && strList[n].Contains(strContext))
                            {
                                return;
                            }

                        }
                    
                    }

                    m_strTitle = strTitle ;
                    m_strContent = strContext;

                    File.AppendAllText(m_strPath, strTemp);
                }
                catch
                {
                    return;
                }


            }
        }

        private void clear() 
        {
            m_strContent = string.Empty;
            m_strTitle = string.Empty;

            this.textBox2.Clear();
            this.richTextBox1.Clear();
            this.textBox1.Clear();
        }

        /// <summary>
        /// richTextBox1键盘响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.Q)
            {
                e.Handled = true;
                this.Close();
            }
            else if(e.Control && e.KeyCode == Keys.S)
            {
                e.Handled = true;
                save();
            }
            else if (e.Control && e.KeyCode == Keys.B)
            {
                e.Handled = true;
                clear();
            }
            else if (e.Control && e.KeyCode == Keys.M)
            {
                e.Handled = true;
                controlForm();
            }

            else if (e.Control && e.KeyValue == 38)
            {
                e.Handled = true;
                readUp();
            }

            else if (e.Control && e.KeyValue == 40)
            {
                e.Handled = true;
                readDown();
            }
            
        }

        /// <summary>
        /// 向上读取，标题空，读取最后一行。标题不为空，查找并向上读取。
        /// </summary>
        private void readUp()
        {
            string[] strList = File.ReadAllLines(m_strPath);
            if (string.IsNullOrEmpty(m_strTitle) && strList.Length > 0)
            {
                set(strList[strList.Length - 1]);
                return;
            }

            for (int n = 0; n < strList.Length; n++)
            {
                if (strList[n].Contains(m_strTitle) && strList[n].Contains(m_strContent) && n - 1 >= 0)
                {
                    set(strList[n - 1]);
                    return;
                }

            }

        }

        /// <summary>
        /// 向下读取，标题空，读取最前一行。标题不为空，查找并向下读取。
        /// </summary>
        private void readDown()
        {
            string[] strList = File.ReadAllLines(m_strPath);
            if (string.IsNullOrEmpty(m_strTitle) && strList.Length > 0)
            {
                set(strList[0]);
                return;
            }

            for (int n = 0; n < strList.Length; n++)
            {
                if (strList[n].Contains(m_strTitle) && strList[n].Contains(m_strContent) && n + 1 < strList.Length)
                {
                    set(strList[n + 1]);
                    return;
                }

            }

        }
        /// <summary>
        /// 从csv里读出值，设置到界面和变量。
        /// </summary>
        /// <param name="strData"></param>
        private void set(string strData) 
        {
            clear();

            string[] strList = strData.Split(',');
            if (strList.Length > 1)
            {
                this.m_strTitle = strList[1] ;
                this.textBox1.Text = strList[1];
                this.textBox2.Text = strList[0];

            }

           
            for (int n = 2; n < strList.Length; n++ )
            {
                m_strContent += strList[n];
                if (strList.Length != n+1) 
                {
                    m_strContent += ",";
                }

                this.richTextBox1.Text += strList[n].Replace('&', ',') + '\n';  
            }
            
        }

        
        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            controlForm();
        }

        /// <summary>
        /// 缩小放大窗口
        /// </summary>
        private void controlForm() 
        {
            if (this.Size.Height == 400)
            {
                this.Size = new Size(50, 50);
            }
            else
            {
                this.Size = new Size(480, 400);
            }

            int nWidth = SystemInformation.WorkingArea.Width - this.Width;
            int nHeight = SystemInformation.WorkingArea.Height - this.Height;
            this.Location = new Point(nWidth, nHeight);
        }

        string m_strPath = "E://Note.csv";
        string m_strContent;
        string m_strTitle;

       

        
    }
}
