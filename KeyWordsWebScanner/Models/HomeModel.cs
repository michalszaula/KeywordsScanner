using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KeyWordsWebScanner.Models
{
    public class HomeModel
    {
        public string RequestedUrl { get; set; }
        public string UrlInfo { get; set; }
    }

    public class KeywordsStatsViewModel
    {
        public List<Keywords> KeyWordsList { get; set; }
    }
}