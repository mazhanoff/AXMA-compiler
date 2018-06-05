using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXMA_compiler
{
    public class AXMA_Story
    {
        public struct Paragraph
        {
            public string Name;
            public string Text;
            public string Link;
            public string Image;
            public string Music;
        }
        public string Author;
        public string Title;
        public List<Paragraph> Paragraphs = new List<Paragraph>();
    }
}
