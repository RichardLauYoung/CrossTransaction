using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace com.Response
{
    public class OKCoinTickerDetailResponse
    {

        /// <summary>
        ///  买一价
        /// </summary>
        [JsonProperty(PropertyName = "buy")]
        public string Buy { get; set; }
        /// <summary>
        /// 最高价
        /// </summary>
        [JsonProperty(PropertyName = "high")]
        public string High { get; set; }
        /// <summary>
        ///  最新成交价
        /// </summary>
        [JsonProperty(PropertyName = "last")]
        public string Last { get; set; }
        /// <summary>
        /// 最低价
        /// </summary>
        [JsonProperty(PropertyName = "low")]
        public string Low { get; set; }
        /// <summary>
        ///  卖一价
        /// </summary>
        [JsonProperty(PropertyName = "sell")]
        public string Sell { get; set; }
        /// <summary>
        /// 成交量
        /// </summary>
        [JsonProperty(PropertyName = "vol")]
        public string Vol { get; set; }
    }
}
