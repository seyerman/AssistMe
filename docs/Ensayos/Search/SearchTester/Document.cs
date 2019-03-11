using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class Document : ISearchable
    {

        private string _text;

        public Document(string text)
        {
            _text = text;
        }

        public string GetDocumentText()
        {
            return _text;
        }

        public override string ToString()
        {
            return _text;
        }
    }
}
