using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class SearchDocument : IComparable<SearchDocument>
    {
        public double Score { get; set; }

        public ISearchable Value { get; private set; }

        public SearchDocument(ISearchable value)
        {
            Score = 0;
            this.Value = value;
        }

        public Dictionary<string, int> GetTermFrequencies()
        {
            var text = Value.GetDocumentText();
            var punctuation = text.Where(Char.IsPunctuation).Distinct().ToArray();
            var words = text.Split().Select(x => x.Trim(punctuation));
            Dictionary<string, int> frequencies = new Dictionary<string, int>();
            foreach (String word in words)
            {
                string w = word.ToUpper();
                if(frequencies.ContainsKey(w))
                {
                    frequencies[w]++;
                }
                else
                {
                    frequencies[w] = 1;
                }
            }
            return frequencies;
        }

        public int GetWordsCount()
        {
            int count = 0;
            var text = Value.GetDocumentText();
            var punctuation = text.Where(Char.IsPunctuation).Distinct().ToArray();
            var words = text.Split().Select(x => x.Trim(punctuation));
            foreach (String word in words)
            {
                count++;
            }
            return count;
        }

        public int CompareTo(SearchDocument other)
        {
            int val = -Score.CompareTo(other.Score);
            if (val == 0 && Value is IComparable)
                return ((IComparable)Value).CompareTo(other.Value);
            else
                return val;
        }

        public override string ToString()
        {
            return Value.ToString()+" - Score: "+Score;
        }

    }
}
