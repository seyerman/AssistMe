using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistMeProject.Models
{
    public class BM25Searcher
    {
        public List<SearchDocument> Documents { get; set; }

        public List<ISearchable> Search(string query)
        {
            var punctuation = query.Where(Char.IsPunctuation).Distinct().ToArray();
            var qTerms = query.Split().Select(x => x.Trim(punctuation));

            return null;
        }

    }
}
