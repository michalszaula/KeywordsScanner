using mshtml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace KeyWordsScanner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //clear old data
            textBox1.Clear();
            chart1.Series.Clear();

            //basic validate
            if (String.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Wprowadź adres url");
                return;
            }
            if (Uri.IsWellFormedUriString(textBox2.Text, UriKind.Relative))
            {
                MessageBox.Show("Wprawadź prawidłowy adres url");
                return;
            }

            //user interface information
            label1.Text = "Przetwarzanie";
            
            //main process start
            HtmlAgilityPack.HtmlDocument doc = KeywordsStats.GetHtmlDoc("http://www.borbis.pl/");
            List<string> keywordsList = KeywordsStats.GetKeywords(doc);
            if (keywordsList.Count() < 1)
            {
                MessageBox.Show("Nie znaleziono słów kluczowych w nagłówku strony");
                return;
            }
            //print keywords-----------------------
            textBox1.AppendText("Słowa kluczowe" + Environment.NewLine);
            foreach (var key in keywordsList)
            {
                textBox1.AppendText(key + Environment.NewLine);
            }
            //-----------------------

            //getting body html
            string bodyText = KeywordsStats.GetBodyTextFromHtml(doc);

            //create keywords class list
            List<Keywords> KeywordsClassList = new List<Keywords>();
            int keywordCount = 0;
            foreach (var key in keywordsList)
            {
                keywordCount = KeywordsStats.CoutKeywordInText(key, bodyText);
                Keywords keyword = new Keywords(key, keywordCount);
                KeywordsClassList.Add(keyword);
            }
            //add test keyword object test
            KeywordsClassList.Add(new Keywords("test", 10));

            //create chart
            Series KeywordsSeries = new Series("KeyWords");
            chart1.Series.Add(KeywordsSeries);
            foreach (var keyword in KeywordsClassList)
            {
                KeywordsSeries.Points.AddXY(keyword.KeyWord, keyword.KeyWordCount);
            }
            
            
            label1.Text = "Zakończono";
        }
    }
}
