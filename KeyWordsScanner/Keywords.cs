using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KeyWordsScanner
{
    public class Keywords
    {
        //default constructor
        public Keywords()
        {

        }

        //constructor
        public Keywords(string keyword, int count)
        {
            KeyWord = keyword;
            KeyWordCount = count;
        }

        //properities
        public int KeyWordCount {get;set;}
        public string KeyWord { get; set; }

        
    }

    public static class KeywordsStats
    {
        //returns server response as string
        public static string GetServerResponse(string url)
        {
            // Create a request for the URL.
            WebRequest request = WebRequest.Create(url);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            Task<WebResponse> responseTask = request.GetResponseAsync();
            HttpWebResponse response = (HttpWebResponse)responseTask.Result;


            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            
            // Cleanup
            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }

        //returns whole html document
        public static HtmlAgilityPack.HtmlDocument GetHtmlDoc(string url)
        {
            //create and return html document
            string responseFromServer = GetServerResponse(url);
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(responseFromServer);
            return doc;
        }

        //returns keywords as string list
        public static List<string> GetKeywords(HtmlAgilityPack.HtmlDocument doc)
        {
            //create kewyords list
            //var list = doc.DocumentNode.SelectNodes("//meta");
            var list = doc.DocumentNode.SelectNodes("//meta[@name='keywords']");
            List<string> keywordsList = new List<string>();
            foreach (var node in list)
            {
                /*
                string name = node.GetAttributeValue("name", "");
                */
               
                keywordsList.AddRange(node.GetAttributeValue("content", "").Split(','));
            }
            keywordsList = keywordsList.Select(s => s.Trim()).ToList();
            return keywordsList;
        }

        //returns number of specific keyword in text
        public static int CoutKeywordInText(string keyword, string plainText)
        {
            //create source array
            string[] source = plainText.Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' }, StringSplitOptions.RemoveEmptyEntries);

            // create the query 
            var matchQuery = from word in source
                             where word.ToLowerInvariant() == keyword.ToLowerInvariant()
                             select word;

            // count matching elements
            return matchQuery.Count();
            
        }

        //returnss body html tag text
        public static string GetBodyTextFromHtml(HtmlAgilityPack.HtmlDocument doc)
        {
            return doc.DocumentNode.SelectSingleNode("//body").InnerText;
            /*
            var root = doc.DocumentNode;
            var sb = new StringBuilder();
            foreach (var node in root.DescendantNodesAndSelf())
            {
                if (!node.HasChildNodes)
                {
                    string text = node.InnerText;
                    if (!string.IsNullOrEmpty(text))
                        sb.AppendLine(text.Trim());
                }
            }
            */
            //return sb.ToString();
        }
    }
}
