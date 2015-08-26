using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KeyWordsWebScanner.Models;
using System.Text;

namespace KeyWordsWebScanner.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            //create model for view with default info
            HomeModel model = new HomeModel();
            model.UrlInfo = "Wprowadź adres url";
            //pass model to view
            return View(model);
        }


        //
        // POST: /Home/GetKeywordsStats
        [HttpPost]
        public ActionResult GetKeywordsStats(HomeModel homeModel)
        {
            string url = homeModel.RequestedUrl;

            //create stringbuilder for urlinfo
            StringBuilder sb = new StringBuilder();
            sb.Append("Wprowadź adres url");
            //basic validate
            if (String.IsNullOrWhiteSpace(url))
            {
                homeModel.UrlInfo = sb.ToString();

                //return to index with updated model
                return View("Index",homeModel);
            }
            if (Uri.IsWellFormedUriString(url, UriKind.Relative))
            {
                //append stringbuilder if url is not well formated
                sb.Append("<br />Wprowadź prawidłowy adres np. http://www.przyklad.pl");
                homeModel.UrlInfo = sb.ToString();
                //return to index with updated model
                return View("Index",homeModel);
            }

            //main process start
            HtmlAgilityPack.HtmlDocument doc = KeywordsStats.GetHtmlDoc("http://www.borbis.pl/");
            List<string> keywordsList = KeywordsStats.GetKeywords(doc);
            if (keywordsList.Count() < 1)
            {
                sb.Append("<br />Nie znaleziono słów kluczowych w nagłówku strony");
                homeModel.UrlInfo = sb.ToString();
                //return to index with updated model
                return View("Index", homeModel);
            }
            
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

            //create mdoel for stats view
            KeywordsStatsViewModel model = new KeywordsStatsViewModel();
            model.KeyWordsList = KeywordsClassList;
            //pass the model to view
            return View(model);
        }
    }
}
