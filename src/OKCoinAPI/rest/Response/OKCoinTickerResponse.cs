using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace com.Response
{
    public class OKCoinTickerResponse
    {
        /// <summary>
        ///  日期
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }
        //true代表成功返回
        [JsonProperty(PropertyName = "ticker")]
        public string Ticker { get; set; }
    }
}
