using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class BM25Searcher
    {
        private const double K1 = 2;
        private const double B = 0.75;

        public List<SearchDocument> Documents { get; set; }

        private Dictionary<string, double> _idfList;

        public BM25Searcher()
        {
            Documents = new List<SearchDocument>();
        }

        public void AddDocument(ISearchable doc)
        {
            Documents.Add(new SearchDocument(doc));
        }

        public List<ISearchable> Search(string query)
        {
            _idfList = new Dictionary<string, double>();
            var punctuation = query.Where(Char.IsPunctuation).Distinct().ToArray();
            var queryTerms = query.Split().Select(x => x.Trim(punctuation));

            foreach (SearchDocument d in Documents)
            {
                d.Score = Score(d, queryTerms);
            }

            Documents.Sort();

            return GetSearchableList();
        }

        private List<ISearchable> GetSearchableList()
        {
            List<ISearchable> docs = new List<ISearchable>();
            foreach (SearchDocument d in Documents)
            {
                docs.Add(d.Value);
            }
            return docs;
        }

        private double Score(SearchDocument d, IEnumerable<string> queryTerms)
        {
            double score = 0;
            foreach (string qTerm in queryTerms)
            {
                string q = qTerm.ToUpper();
                double idf = Idf(q);
                var frequencies = d.GetTermFrequencies();
                int tf = frequencies.ContainsKey(q) ? frequencies[q] : 0;
                int D = d.GetWordsCount();
                double avg = GetAverageLength();
                double prop = avg == 0 ? 0 : D / avg;
                double qTf = (tf * (K1 + 1)) / (tf + K1 * (1 - B + B * prop));

                score += idf * qTf;
            }
            return score;
        }

        private double GetAverageLength()
        {
            if (Documents.Count == 0) return 0;
            int sum = 0;
            foreach (SearchDocument d in Documents)
            {
                sum += d.GetWordsCount();
            }
            return (double)sum / Documents.Count;
        }

        private double Idf(string q)
        {
            if (_idfList.ContainsKey(q))
            {
                return _idfList[q];
            }
            else
            {
                int N = Documents.Count;
                int nq = DocumentsThatContain(q);
                double val = (N - nq + 0.5) / (nq + 0.5);
                val = Math.Log(val);
                if (val < 0) val = 0;
                return _idfList[q] = val;
            }
        }

        private int DocumentsThatContain(string q)
        {
            int count = 0;
            foreach (SearchDocument d in Documents)
            {
                if (d.GetTermFrequencies().ContainsKey(q))
                {
                    count++;
                }
            }
            return count;
        }
    }
}
