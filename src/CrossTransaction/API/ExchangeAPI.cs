using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.okcoin.rest.stock;
using com.Response;
using CrossTransaction.Common;
using CrossTransaction.ViewModels.Exchange;
using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CrossTransaction.API
{


    [Route("api/[controller]/[action]")]
    public class ExchangeAPI : Controller
    {
        string cn_url = "https://www.okcoin.cn"; //国内站账号配置 为 https://www.okcoin.cn
        string inter_url = "https://www.okcoin.com";
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "test";
        }

        // POST api/values
        [HttpPost]
        public void Exchange(ExchangeViewModel exchangeParamViewModel)
        {
            string cnKey = string.Empty;
            string cnSecret = string.Empty;
            string interKey = string.Empty;
            string interSecret = string.Empty;
            string strAmount = string.Empty;
            double dbHighPrice = 0;
            double dbAmountRMB = 0;
            double dbAmountBTC = 0;


            try
            {

                
                if (string.IsNullOrEmpty(exchangeParamViewModel.CNKey))
                {
                    throw new Exception();
                }
                if (string.IsNullOrEmpty(exchangeParamViewModel.CNSecret))
                {
                    throw new Exception();
                }

                if (string.IsNullOrEmpty(exchangeParamViewModel.AmountRMB))
                {
                    throw new Exception();
                }

                cnKey = exchangeParamViewModel.CNKey;
                cnSecret = exchangeParamViewModel.CNSecret;
                Double.TryParse(exchangeParamViewModel.AmountRMB, out dbAmountRMB);
                IStockRestApi cnGet = new StockRestApi(cn_url);
                IStockRestApi cnPost = new StockRestApi(inter_url, cnKey, cnSecret);

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
            }
            catch (Exception ex)
            {
                Exception ex2 = ex;
                throw ex;
            }
        }

        // POST api/values
        [HttpPost]
        public void Withdraw([FromBody]ExchangeViewModel exchangeParam)
        {
            string cnKey = string.Empty;
            string cnSecret = string.Empty;
            string interKey = string.Empty;
            string interSecret = string.Empty;
            string strBTC = string.Empty;
            double dbBTC = 0;


            if (string.IsNullOrEmpty(exchangeParam.CNKey))
            {
                throw new Exception();
            }
            if (string.IsNullOrEmpty(exchangeParam.CNSecret))
            {
                throw new Exception();
            }

            if (string.IsNullOrEmpty(exchangeParam.BTCAddress))
            {
                throw new Exception();
            }



            IStockRestApi cnGet = new StockRestApi(cn_url);
            IStockRestApi cnPost = new StockRestApi(inter_url, cnKey, cnSecret);

            //Get User Info
            OKCoinUserInfoResponse userInfo = JsonConvert.DeserializeObject<OKCoinUserInfoResponse>(cnPost.userinfo());
            strBTC = userInfo.Info.Free.Btc;
            if (!String.IsNullOrEmpty(strBTC))
            {
                double.TryParse(strBTC, out dbBTC);
                if (dbBTC > 0)
                {
                    cnPost.withdraw(Constants.SYMBOL_BTC_USD, "0", exchangeParam.Password, exchangeParam.BTCAddress, strBTC);
                }
            }
        }

        private double GetHighPrice(IStockRestApi okCoinAPI)
        {
            double dbHighPrice = 0;
            //Get Ticker information
            OKCoinTickerResponse tickerResponse = JsonConvert.DeserializeObject<OKCoinTickerResponse>
                                                             (okCoinAPI.ticker(Constants.SYMBOL_BTC_USD));
            if (!string.IsNullOrEmpty(tickerResponse.Ticker.High))
            {
                Double.TryParse(tickerResponse.Ticker.High, out dbHighPrice);
            }
            return dbHighPrice;
        }
    }
}
