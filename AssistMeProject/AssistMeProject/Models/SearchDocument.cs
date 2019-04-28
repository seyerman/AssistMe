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

        private Dictionary<string, int> _termFrequencies;
        private int _wordsCount;

        public ISearchable Value { get; private set; }

        public SearchDocument(ISearchable value)
        {
            Score = 0;
            this.Value = value;
            _termFrequencies = CalculateTermFrequencies();
        }

        public Dictionary<string, int> GetTermFrequencies()
        {
            return _termFrequencies;
        }

        private Dictionary<string, int> CalculateTermFrequencies()
        {
            var text = Value.GetDocumentText();
            var words = BM25Searcher.Tokenize(text);
            int count = 0;
            Dictionary<string, int> frequencies = new Dictionary<string, int>();
            foreach (String word in words)
            {
                string w = word.ToUpper();
                if (frequencies.ContainsKey(w))
                {
                    frequencies[w]++;
                }
                else
                {
                    frequencies[w] = 1;
                }
                count++;
            }
            _wordsCount = count;
            return frequencies;
        }

        public int GetWordsCount()
        {
            return _wordsCount;
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
