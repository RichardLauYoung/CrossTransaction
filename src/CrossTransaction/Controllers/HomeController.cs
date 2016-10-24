using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using com.okcoin.rest.future;
using com.okcoin.rest.stock;
using Microsoft.AspNet.Http.Internal;
using CrossTransaction.Common;
using com.Response;
using Newtonsoft.Json;

namespace CrossTransaction.Controllers
{
    public class HomeController : Controller
    {
        String cn_url = "https://www.okcoin.cn"; //国内站账号配置 为 https://www.okcoin.cn
        String inter_url = "https://www.okcoin.com";

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

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
