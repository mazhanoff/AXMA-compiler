using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AXMA_compiler
{
    enum States
    {
        parHeader, parBody, parLink, parTag, parEnd
    };
    
    public partial class Compiler : Form
    {
        private Dictionary<string, string> Gram = new Dictionary<string, string>();
        AXMA_Story story;
        string path;
        public Compiler()
        {
            story = new AXMA_Story();
            InitializeComponent();
            BuildGram();
        }

        private void BuildGram()
        {
            Gram.Add("headerStart", "::");
            Gram.Add("headerEnd", "[::]");
            Gram.Add("tagStart", "<<");
            Gram.Add("tagEnd", ">>");
            Gram.Add("brackStart", "[[");
            Gram.Add("brackEnd", "]]");
            Gram.Add("tagPic", "pic");
            Gram.Add("tagMusic", "");
        }

        private void openFileBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "AXMA project Files|*.sm";
            openFileDialog1.Title = "Select a AXMA project";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);
                try
                {
                    logsRTB.AppendText(Parse(sr));
                    printParagraphs();
                }
                catch (Exception ex)
                {
                    logsRTB.AppendText(ex.Source + "\n");
                    logsRTB.AppendText(ex.StackTrace + "\n");
                    logsRTB.AppendText(ex.Message + "\n");
                }
                sr.Close();
            }
        }

        private string Parse(StreamReader sr)
        {
            States state = States.parHeader;
            string res = "";
            AXMA_Story.Paragraph paragraph = new AXMA_Story.Paragraph();
            paragraph.Link = new List<string>();
            while (!sr.EndOfStream)
            {
                var index = sr.GetPosition();
                string cur = sr.ReadLine();
                switch (state)
                {
                    case States.parHeader:
                        if (cur.IndexOf(Gram["headerStart"]) == 0)
                        {
                            string header = "";
                            header = cur.Substring(3, cur.IndexOf(Gram["headerEnd"]) - 3);
                            paragraph.Name = header;
                            state = States.parBody;
                        }
                        else
                            res = "Compiling successful\n";
                        break;
                    case States.parBody:
                        if (cur.Length == 0)
                            break;
                        else if (cur.Substring(0, 2) == Gram["headerStart"])
                        {
                            sr.SetPosition(index);
                            state = States.parEnd;
                            switch (paragraph.Name)
                            {
                                case "StoryAuthor":
                                    paragraph.Link.Add("SysParagraph");
                                    story.Author = paragraph.Text;
                                    break;
                                case "StoryTitle":
                                    paragraph.Link.Add("SysParagraph");
                                    story.Title = paragraph.Text;
                                    break;
                                case "End":
                                    paragraph.Link.Add("End");
                                    break;
                            }
                        }
                        else if (cur[0] == '#')
                            break;
                        else if (cur.Substring(0, 2) == Gram["tagStart"])
                        {
                            sr.SetPosition(index);
                            state = States.parTag;
                        }
                        else if (cur.Substring(0, 2) == Gram["brackStart"])
                        {
                            if (cur.Substring(2, 4) == "File" || cur.Substring(2, 4) == "Файл")
                            {
                                string path = cur.Substring(cur.IndexOf(':') + 2, cur.Length - (cur.IndexOf(':') + 1) - 3);
                                paragraph.Music = path;
                            }
                            else
                            {
                                sr.SetPosition(index);
                                state = States.parLink;
                            }
                        }
                        else
                            paragraph.Text += cur + '\n';
                        break;
                    case States.parTag:
                        if (cur.Substring(2, cur.IndexOf(' ') - 2) == "pic")
                        {
                            paragraph.Image = cur.Substring(cur.IndexOf("'") + 1, cur.Length - 4 - cur.IndexOf("'"));
                            state = States.parBody;
                        }
                        else
                        {
                            state = States.parBody;                            
                        }
                        break;
                    case States.parLink:                        
                        if (cur!="" && cur[0] == '[')
                        {
                            string link = cur.Substring(2, cur.Length - 4);
                            paragraph.Link.Add(link);
                            state = States.parLink;
                        }
                        else if (cur.Length == 0)
                            break;
                        else
                        {
                            sr.SetPosition(index);
                            state = States.parEnd;
                        }
                        
                        break;
                    case States.parEnd:
                        sr.SetPosition(index);
                        story.Paragraphs.Add(paragraph);
                        paragraph.Name = null;
                        paragraph.Text = null;
                        paragraph.Link = new List<string>();
                        paragraph.Image = null;
                        paragraph.Music = null;
                        state = States.parHeader;
                        break;
                }
            }
            story.Paragraphs.Add(paragraph);
            return res;
        }

        private void printParagraphs()
        {
            int count = 0;
            foreach(var par in story.Paragraphs)
            {
                logsRTB.AppendText("\nParagraph #" + ++count+"\n");
                logsRTB.AppendText("Name: " + par.Name + "\n");
                logsRTB.AppendText("Text: " + par.Text);
                if (par.Link.Count!=0)
                    foreach (var a in par.Link)
                        logsRTB.AppendText("Link: " + a + "\n");
                else
                    logsRTB.AppendText("Link: NULL\n");
                if (par.Music != null)
                    logsRTB.AppendText("Music: " + par.Music + "\n" );
                else
                    logsRTB.AppendText("Music: NULL\n");
                if (par.Image != null)
                    logsRTB.AppendText("Image: " + par.Image + "\n");
                else
                    logsRTB.AppendText("Image: NULL\n");
            }
        }

        private void playBtn_Click(object sender, EventArgs e)
        {            
            try
            {
                StoryForm storyForm = new StoryForm(story);
                storyForm.Show();
                storyForm.FormClosed += delegate { this.Show(); };
                this.Hide();
            }
            catch (Exception ex)
            {
                this.Show();
                logsRTB.AppendText(ex.Source + "\n");
                logsRTB.AppendText(ex.StackTrace + "\n");
                logsRTB.AppendText(ex.Message+"\n");
            }   
        }
    }

    /// <summary>
    /// This class extends StreamReader behavior and allows to save the position of StreamReader 
    /// And backups when something goes wrong
    /// </summary>
    public static class StreamReaderExtensions
    {
        readonly static FieldInfo charPosField = typeof(StreamReader).GetField("charPos", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | BindingFlags.DeclaredOnly);
        readonly static FieldInfo byteLenField = typeof(StreamReader).GetField("byteLen", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | BindingFlags.DeclaredOnly);
        readonly static FieldInfo charBufferField = typeof(StreamReader).GetField("charBuffer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | BindingFlags.DeclaredOnly);

        public static long GetPosition(this StreamReader reader)
        {
            //shift position back from BaseStream.Position by the number of bytes read
            //into internal buffer.
            int byteLen = (int)byteLenField.GetValue(reader);
            var position = reader.BaseStream.Position - byteLen;

            //if we have consumed chars from the buffer we need to calculate how many
            //bytes they represent in the current encoding and add that to the position.
            int charPos = (int)charPosField.GetValue(reader);
            if (charPos > 0)
            {
                var charBuffer = (char[])charBufferField.GetValue(reader);
                var encoding = reader.CurrentEncoding;
                var bytesConsumed = encoding.GetBytes(charBuffer, 0, charPos).Length;
                position += bytesConsumed;
            }

            return position;
        }

        public static void SetPosition(this StreamReader reader, long position)
        {
            reader.DiscardBufferedData();
            reader.BaseStream.Seek(position, SeekOrigin.Begin);
        }
    }

}
