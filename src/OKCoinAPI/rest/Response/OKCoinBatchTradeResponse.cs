using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace com.Response
{
    public class OKCoinBatchTradeResponse
    {
        /// <summary>
        ///  订单ID
        /// </summary>
        [JsonProperty(PropertyName = "order_info")]
        public IEnumerable<OKCoinBatchTradeOrderResponse> OrderInfo { get; set; }
        //true代表成功返回
        [JsonProperty(PropertyName = "result")]
        public string Result { get; set; }
    }
}
