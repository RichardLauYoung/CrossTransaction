using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
//using com.okcoin.rest.future;
//using com.okcoin.rest.stock;
//namespace com.okcoin.rest

namespace CrossTransaction.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Exchange()
        {

            //StockRestApi getRequest1 = new StockRestApi(url_prex);
            //StockRestApi postRequest1 = new StockRestApi(url_prex, api_key, secret_key);
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
