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
        public StoryForm()
        {
            InitializeComponent();
        }

        public StoryForm(AXMA_Story story)
        {
            InitializeComponent();
        }

        private void StoryForm_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
        }
    }
}
