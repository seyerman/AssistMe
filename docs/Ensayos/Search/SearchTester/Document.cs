using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class Document : ISearchable
    {

        public string Title { get; set; }
        private string _text;

        public Document(string text)
        {
            _text = text;
        }

        public string GetDocumentText()
        {
            return Title +" "+ _text;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
