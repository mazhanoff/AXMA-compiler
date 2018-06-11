using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace AXMA_compiler
{
    public partial class StoryForm : Form
    {
        AXMA_Story story;
        Dictionary<int, Dictionary<string,int>> links;
        int curParagraph;
        int prevParagraph;
        WMPLib.WindowsMediaPlayer player;
        public StoryForm()
        {
            InitializeComponent();
        }

        public StoryForm(AXMA_Story str)
        {
            InitializeComponent();
            story = str;
            createLinks();
            this.KeyUp += goNextParagraph;
            this.FormClosing += closing;
            foreach (var par in story.Paragraphs)
            {
                if (par.Name == "Start")
                {
                    curParagraph = story.Paragraphs.IndexOf(par);
                    prevParagraph = curParagraph;
                }
            }
        }

        private void closing(object sender, FormClosingEventArgs e)
        {
            player.close();
        }

        private void createLinks()
        {
            links = new Dictionary<int, Dictionary<string, int>>();
            for (int i = 0; i < story.Paragraphs.Count; i++) 
            {
                var a = story.Paragraphs[i];
                Dictionary<string, int> buffer = new Dictionary<string, int>();
                if (a.Link.Count!=0 && a.Link.First() != "SysParagraph")                
                {
                    foreach(var b in a.Link)
                    {
                        string _link = b.Substring(b.IndexOf('|') + 1).Trim();
                        for (int j = 0; j < story.Paragraphs.Count; j++)
                        {
                            if (_link == story.Paragraphs[j].Name)
                                buffer.Add(_link, j);
                        }
                    }                    
                }
                links.Add(i, buffer);
            }
        }

        private void StoryForm_Load(object sender, EventArgs e)
        {
            //this.TopMost = true;
            //this.FormBorderStyle = FormBorderStyle.None;
            //this.WindowState = FormWindowState.Maximized;
            showTitles();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            player.close();
            this.Close();
        }

        private void showTitles()
        {
            Label title = new Label();
            title.Text = story.Title;
            title.ForeColor = Color.White;
            title.AutoSize = true;
            title.Font = new Font("Arial", 48, FontStyle.Bold);
            this.Controls.Add(title);
            title.BringToFront();
            title.Location = new Point(this.Size.Width / 2 - title.Size.Width / 2, this.Size.Height / 2 - title.Size.Height);
            Label authors = new Label();
            authors.Text = story.Author;
            authors.ForeColor = Color.White;
            authors.AutoSize = true;
            authors.Font = new Font("Arial", 20, FontStyle.Bold);
            this.Controls.Add(authors);
            authors.BringToFront();
            authors.Location = new Point(this.Size.Width / 2 - authors.Size.Width / 2, this.Size.Height / 2 + 2 * authors.Size.Height);
            Timer myTimer = new Timer();
            myTimer.Tick += delegate
            {
                myTimer.Stop();
                this.Controls.Remove(authors);
                this.Controls.Remove(title);
                Render(curParagraph);
            };
            myTimer.Interval = 1000;
            myTimer.Start();

        }

        private void Render(int curPar)
        {
            prevParagraph = curParagraph;
            tableLayout.Visible = true;
            var par = story.Paragraphs[curPar];
            linkLayoutPanel.Controls.Clear();
            linkLayoutPanel.ColumnCount = 1;
            backgroundImage.ImageLocation = par.Image;
            showText(par.Text);
            showLinks(par.Link);
            curParagraph = story.Paragraphs.IndexOf(par);
            player = new WindowsMediaPlayer();
            player.URL = par.Music;
        }

        private void showLinks(List<string> link)
        {
            foreach(var l in link)
            {
                string _l = "|   " + l.Substring(0, l.IndexOf('|')) + "   |";
                Label text = new Label();
                text.Name = "link";
                text.Text = _l;
                text.AutoEllipsis = true;
                text.ForeColor = Color.Brown;
                text.AutoSize = true;
                text.Cursor = Cursors.Hand;
                text.Font = new Font("Book Antiqua", 16, FontStyle.Bold);
                text.Click += goNextParagraphByLink;
                text.Anchor = (AnchorStyles.Left | AnchorStyles.Right);
                linkLayoutPanel.Controls.Add(text,linkLayoutPanel.ColumnCount-1,0);
                linkLayoutPanel.ColumnCount++;
            }
        }

        

        private void showText(string _text)
        {
            textBox.Clear();
            textBox.AppendText(_text);
            //var a = tableLayout.Controls.Find("text", true);
            //if (a.Length>0)
            //    tableLayout.Controls.Remove(a[0]);            
            //Label text = new Label();
            //text.Name = "text";
            //text.Text = _text;
            //text.AutoEllipsis = true;
            //text.ForeColor = Color.Black;
            //text.AutoSize = true;
            //text.Font = new Font("Book Antiqua",16,FontStyle.Regular);
            //tableLayout.Controls.Add(text,0,0);
        }

        private void goNextParagraphByLink(object sender, EventArgs e)
        {
            backBtn.Enabled = true;
            Label s = sender as Label;
            var n = linkLayoutPanel.Controls.IndexOf(s);
            Render(links[curParagraph].ElementAt(n).Value);
        }
        private void goNextParagraph(object sender, KeyEventArgs e)
        {
            backBtn.Enabled = true;
            if (e.KeyCode == Keys.Space)
            {
                if (story.Paragraphs[curParagraph].Link.Count == 1)
                {
                    string link = story.Paragraphs[curParagraph].Link.First()?.Substring(story.Paragraphs[curParagraph].Link.First().IndexOf('|') + 1);
                    if (link != null)
                    {
                        int nextPar = links[curParagraph][link];
                        Render(nextPar);
                    }
                    else
                        this.Close();
                }                
            }
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            Render(prevParagraph);
            backBtn.Enabled = false;
        }
    }
}
