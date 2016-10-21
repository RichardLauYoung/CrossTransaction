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

        public IActionResult Exchange(FormCollection form)
        {
            string cnKey = string.Empty;
            string cnSecret = string.Empty;
            string interKey = string.Empty;
            string interSecret = string.Empty;
            double dbHighPrice = 0;
            double dbAmountRMB = 0;
            double dbAmountBTC = 0;

            if (!string.IsNullOrEmpty(form["CNKey"]))
            {
                cnKey = form["CNKey"].ToString();
            }
            if (!string.IsNullOrEmpty(form["CNSecret"]))
            {
                cnSecret = form["CNSecret"].ToString();
            }
            if (!string.IsNullOrEmpty(form["InterKey"]))
            {
                interKey = form["InterKey"].ToString();
            }
            if (!string.IsNullOrEmpty(form["InterSecret"]))
            {
                interSecret = form["InterSecret"].ToString();
            }
            if (!string.IsNullOrEmpty(form["AmountRMB"]))
            {
                Double.TryParse(form["AmountRMB"], out dbAmountRMB);
            }
            
            IStockRestApi cnGet = new StockRestApi(cn_url);
            IStockRestApi cnPost = new StockRestApi(inter_url, cnKey, cnSecret);
            IStockRestApi interGet = new StockRestApi(cn_url);
            IStockRestApi interPost = new StockRestApi(inter_url, interKey, interSecret);

            //Get current BTC high price
            dbHighPrice = GetHighPrice(cnGet);
            dbAmountBTC = dbAmountRMB / dbHighPrice;
            //More than 1000BTC OTC otherwise trade
            if (dbAmountBTC > 1000)
            {
                cnPost.submit_otc_order(Constants.SYMBOL_BTC_USD, 
                                        Constants.OTC_ORDER_TYPE_BUY, 
                                        dbAmountBTC.ToString(),
                                        string.Empty, 
                                        Constants.OTC_ORDER_OPEN_TYPE_PUBLIC);
            }
            else
            {
                cnPost.trade(Constants.SYMBOL_BTC_USD, Constants.OTC_ORDER_TYPE_BUY, string.Empty, dbAmountBTC.ToString());
            }

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

        private double GetHighPrice(IStockRestApi okCoinAPI)
        {
            double dbHighPrice = 0;
            //Get Ticker information
            OKCoinTickerResponse tickerResponse = JsonConvert.DeserializeObject<OKCoinTickerResponse>
                                                             (okCoinAPI.ticker(Constants.SYMBOL_BTC_USD));
            if (!string.IsNullOrEmpty(tickerResponse.Ticker.High))
            {
                Double.TryParse( tickerResponse.Ticker.High, out dbHighPrice);
            }
            return dbHighPrice;
        }
    }
}
