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
        Dictionary<string, int> links;
        int curParagraph;
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
            foreach (var par in story.Paragraphs)
            {
                if (par.Name == "Start")
                {
                    curParagraph = story.Paragraphs.IndexOf(par);
                }
            }
        }



        private void createLinks()
        {
            links = new Dictionary<string, int>();
            foreach (var a in story.Paragraphs)
            {
                if (a.Link != null && a.Link != "SysParagraph")
                {
                    string link = a.Link.Substring(a.Link.IndexOf('|') + 1);
                    for (int i = 0; i < story.Paragraphs.Count; i++)
                    {
                        if (link == story.Paragraphs[i].Name)
                            links.Add(link, i);
                    }
                }
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
            myTimer.Interval = 3000;
            myTimer.Start();

        }

        private void Render(int curPar)
        {
            textPanel.Visible = true;
            var par = story.Paragraphs[curPar];

            backgroundImage.ImageLocation = par.Image;
            showText(par.Text);
            curParagraph = story.Paragraphs.IndexOf(par);
            WMPLib.WindowsMediaPlayer player = new WindowsMediaPlayer();
            player.URL = par.Music;
        }

        private void showText(string _text)
        {
            var a = textPanel.Controls.Find("text", true);
            if (a.Length>0)
                textPanel.Controls.Remove(a[0]);            
            Label text = new Label();
            text.Name = "text";
            text.Text = _text;
            text.AutoEllipsis = true;
            text.ForeColor = Color.Black;
            text.AutoSize = true;
            text.Font = new Font("Arial", 20, FontStyle.Regular);
            textPanel.Controls.Add(text);
            //text.BringToFront();
            //text.Location = new Point(textPanel.Location.X + 25, textPanel.Location.Y + 25);

        }

        private void goNextParagraph(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                string link = story.Paragraphs[curParagraph].Link?.Substring(story.Paragraphs[curParagraph].Link.IndexOf('|') + 1);
                if (link != null)
                {
                    int nextPar = links[link];
                    Render(nextPar);
                }
                else
                    this.Close();
            }
        }        
    }
}
