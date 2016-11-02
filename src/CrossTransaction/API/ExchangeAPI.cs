using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.okcoin.rest.stock;
using com.Response;
using CrossTransaction.Common;
using CrossTransaction.ViewModels.Exchange;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CrossTransaction.API
{


    [Route("api/[controller]/[action]")]
    public class ExchangeAPI : Controller
    {
        string cn_url = "https://www.okcoin.cn"; //国内站账号配置 为 https://www.okcoin.cn
        string inter_url = "https://www.okcoin.com";
        OKCoinTickerResponse tickerResponse = new OKCoinTickerResponse();
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
        public void Exchange(ExchangeViewModel paramViewModel)
        {
            string cnKey = string.Empty;
            string cnSecret = string.Empty;
            string interKey = string.Empty;
            string interSecret = string.Empty;
            string strAmount = string.Empty;
            double dbLastPrice = 0;
            double dbAmountRMB = 0;
            double dbAmountBTC = 0;
            string returnResult = string.Empty;
            OKCoinOTCOrderResponse otcResp = new OKCoinOTCOrderResponse();
            OKCoinTradeResponse tradeResp = new OKCoinTradeResponse();

            try
            {

                
                if (string.IsNullOrEmpty(paramViewModel.CNKey))
                {
                    throw new Exception();
                }
                if (string.IsNullOrEmpty(paramViewModel.CNSecret))
                {
                    throw new Exception();
                }

                if (string.IsNullOrEmpty(paramViewModel.AmountRMB))
                {
                    throw new Exception();
                }

                cnKey = paramViewModel.CNKey;
                cnSecret = paramViewModel.CNSecret;
                Double.TryParse(paramViewModel.AmountRMB, out dbAmountRMB);
                IStockRestApi cnGet = new StockRestApi(cn_url);
                IStockRestApi cnPost = new StockRestApi(cn_url, cnKey, cnSecret);

                //Get current BTC high price
                dbLastPrice = GetPrice(cnGet);
                dbAmountBTC = dbAmountRMB / dbLastPrice;
                //More than 1000BTC OTC otherwise trade
                if (dbAmountBTC > 1000)
                {
                    returnResult = cnPost.submit_otc_order(Common.Constants.SYMBOL_BTC_CNY,
                                                           Common.Constants.OTC_ORDER_TYPE_BUY,
                                                           dbAmountBTC.ToString(),
                                                           tickerResponse.Ticker.Last,
                                                           Common.Constants.OTC_ORDER_OPEN_TYPE_PUBLIC);
                }
                else
                {
                    returnResult = cnPost.trade(Common.Constants.SYMBOL_BTC_CNY,
                                                Common.Constants.OTC_ORDER_TYPE_BUY_MARKET, 
                                                paramViewModel.AmountRMB);
                }
                //Save order ID
                if(string.IsNullOrEmpty(returnResult)){
                    System.IO.File.WriteAllText(@"Orders.json", returnResult);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // POST api/values
        [HttpPost]
        public void Withdraw(ExchangeViewModel paramViewModel)
        {
            try
            {
                string cnKey = string.Empty;
                string cnSecret = string.Empty;
                string strBTC = string.Empty;
                //double dbBTC = 0;
                OKCoinWithdrawResponse withdrawResp = new OKCoinWithdrawResponse();
                string returnResult = string.Empty;
                string orderID = string.Empty;

                if (string.IsNullOrEmpty(paramViewModel.CNKey))
                {
                    throw new Exception();
                }
                if (string.IsNullOrEmpty(paramViewModel.CNSecret))
                {
                    throw new Exception();
                }

                if (string.IsNullOrEmpty(paramViewModel.BTCAddress))
                {
                    throw new Exception();
                }

                cnKey = paramViewModel.CNKey;
                cnSecret = paramViewModel.CNSecret;
                IStockRestApi cnPost = new StockRestApi(cn_url, cnKey, cnSecret);

                //Get Order Info
                //OKCoinUserInfoResponse userInfo = JsonConvert.DeserializeObject<OKCoinUserInfoResponse>(cnPost.userinfo());
                //var builder = new ConfigurationBuilder().AddJsonFile("Orders.json");
                //IConfigurationRoot orderJson = builder.Build();
                //orderID = orderJson["order_id"];

                //strBTC = userInfo.Info.Funds.Free.Btc;
                strBTC = "0.01";
                if (!String.IsNullOrEmpty(strBTC))
                {
                    //double.TryParse(strBTC, out dbBTC);
                    //if (dbBTC > 0)
                    //{
                    returnResult = cnPost.withdraw(Common.Constants.SYMBOL_BTC_CNY,
                                                   Common.Constants.CHARGE_FEE, 
                                                   paramViewModel.Password, 
                                                   paramViewModel.BTCAddress, 
                                                   strBTC);
                        //WithdrawUSD
                        //WithdrawUSD(paramViewModel);
                    //}
                }
            }catch(Exception ex)
            {   
                throw ex;
            }


        }

        // POST api/values
        [HttpPost]
        private void WithdrawUSD(ExchangeViewModel paramViewModel)
        {
            try
            {
                string interKey = string.Empty;
                string interSecret = string.Empty;
                string strBTC = string.Empty;
                //double dbBTC = 0;


                if (string.IsNullOrEmpty(paramViewModel.InterKey))
                {
                    throw new Exception();
                }
                if (string.IsNullOrEmpty(paramViewModel.InterSecret))
                {
                    throw new Exception();
                }



                interKey = paramViewModel.InterKey;
                interSecret = paramViewModel.InterSecret;
                IStockRestApi interPost = new StockRestApi(inter_url, interKey, interSecret);
                //Get User Info
                OKCoinUserInfoResponse userInfo = JsonConvert.DeserializeObject<OKCoinUserInfoResponse>(interPost.userinfo());
                strBTC = userInfo.Info.Funds.Free.Btc;
                //strBTC = "0.01";
                if (String.IsNullOrEmpty(strBTC))
                {
                    throw new Exception();
                }
                interPost.trade(Common.Constants.SYMBOL_BTC_USD, Common.Constants.OTC_ORDER_TYPE_SELL_MARKET, string.Empty, strBTC);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private double GetPrice(IStockRestApi okCoinAPI)
        {
            double dbLastPrice = 0;
            //Get Ticker information
            tickerResponse = JsonConvert.DeserializeObject<OKCoinTickerResponse>
                                                             (okCoinAPI.ticker(Common.Constants.SYMBOL_BTC_USD));
            if (!string.IsNullOrEmpty(tickerResponse.Ticker.Last))
            {
                Double.TryParse(tickerResponse.Ticker.Last, out dbLastPrice);
            }
            return dbLastPrice;
        }
    }
}
