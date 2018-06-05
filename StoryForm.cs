using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AXMA_compiler
{
    public partial class StoryForm : Form
    {
        AXMA_Story story;

        public StoryForm()
        {
            InitializeComponent();
        }

        public StoryForm(AXMA_Story str)
        {
            InitializeComponent();
            story = str;            
        }

        private void StoryForm_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
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
            myTimer.Tick += delegate {
                myTimer.Stop();
                this.Controls.Remove(authors);
                this.Controls.Remove(title);
                Render();
            };
            myTimer.Interval = 3000;
            myTimer.Start();

        }

        private void Render()
        {
            textPanel.Visible = true;
            foreach(var par in story.Paragraphs)
            {
                if (par.Name == "Start")
                {
                    backgroundImage.ImageLocation = par.Image;
                    showText(par.Text);
                }                
            }
        }

        private void showText(string _text)
        {
            Label text = new Label();
            text.Text = _text;
            text.AutoEllipsis = true;
            text.ForeColor = Color.Black;
            text.AutoSize = true;
            text.Font = new Font("Arial", 20, FontStyle.Regular);
            textPanel.Controls.Add(text);
            //text.BringToFront();
            //text.Location = new Point(textPanel.Location.X + 25, textPanel.Location.Y + 25);
            
        }
    }
}
