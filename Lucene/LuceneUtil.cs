using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using ProjectLab.Models.Entities;
using System.Text;

namespace ProjectLab.Utils
{
    public class LuceneUtil
    {
        // Define a method to index the text content of all PDF files
        public static void IndexPDFs(List<Product> products)
        {
            string indexDir = @"C:\LuceneIndex";
            var dir = FSDirectory.Open(new DirectoryInfo(indexDir));
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            var writer = new IndexWriter(dir, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED);

            foreach (var product in products)
            {
                // Open the PDF file and extract its text content
                string text = ExtractTextFromPDF(product.FilePath);

                // Create a Lucene document and add the text content and other fields
                var doc = new Document();
                doc.Add(new Field("id", product.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("name", product.Name, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("text", text, Field.Store.YES, Field.Index.ANALYZED));
                writer.AddDocument(doc);
            }

            writer.Dispose();
        }

        // Define a method to extract the text content from a PDF file
        public static string ExtractTextFromPDF(string filePath)
        {
            using (var reader = new PdfReader(filePath))
            {
                var text = new StringBuilder();
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }
                return text.ToString();
            }
        }
    }


}