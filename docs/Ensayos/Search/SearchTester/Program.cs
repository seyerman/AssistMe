using AssistMeProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SearchTester
{
    class Program
    {

        public List<Document> Docs;
        private BM25Searcher _searcher;

        Program()
        {
            Docs = new List<Document>();
            _searcher = new BM25Searcher();
            ReadArticles();
            int userInput = 0;
            do
            {
                userInput = DisplayMenu();
                switch (userInput)
                {
                    case 1:
                        AddDocument();
                        break;
                    case 2:
                        Query();
                        break;
                    case 3:
                        SeeDocuments();
                        break;
                }
            } while (userInput != 4);
        }

        private void ReadArticles()
        {
            foreach (string file in Directory.EnumerateFiles("../../data/articles"))
            {
                string title = Path.GetFileNameWithoutExtension(file.ToString());
                string contents = File.ReadAllText(file);
                AddDocument(title + "\n" + contents);
            }
        }

        private void SeeDocuments()
        {
            int counter = 1;
            foreach (Document doc in Docs)
            {
                Console.WriteLine("Document #" + counter);
                Console.WriteLine(doc);
                Console.WriteLine();
                counter++;
            }
        }

        static void Main(string[] args)
        {
            new Program();
        }

        private void Query()
        {

            Console.Write("Search: ");
            string line = Console.ReadLine();
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            _searcher.Search(line);
            watch.Stop();
            int counter = 1;
            foreach (SearchDocument doc in _searcher.Documents)
            {
                if (doc.Score == 0 || counter == 15) break;
                Console.WriteLine("Document #" + counter);
                Console.WriteLine(doc);
                Console.WriteLine();
                counter++;
            }
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
        }

        private void AddDocument()
        {
            Console.WriteLine("Write \"0 0\" to stop writing");
            string line = Console.ReadLine();
            StringBuilder sb = new StringBuilder();
            while (line != "0 0")
            {
                sb.Append(line + "\n");
                line = Console.ReadLine();
            }
            AddDocument(sb.ToString());
        }

        public int DisplayMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Search Tester");
            Console.WriteLine();
            Console.WriteLine("1. Add a document");
            Console.WriteLine("2. Query");
            Console.WriteLine("3. See Documents");
            Console.WriteLine("4. Exit");
            var result = Console.ReadLine();
            return Convert.ToInt32(result);
        }

        public void AddDocument(string text)
        {
            Document doc = new Document(text);
            Docs.Add(doc);
            _searcher.AddDocument(doc);
        }


    }
}
