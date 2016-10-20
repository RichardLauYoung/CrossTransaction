using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace com.Response
{
    public class OKCoinBatchTradeOrderResponse
    {
        /// <summary>
        ///  订单ID
        /// </summary>
        [JsonProperty(PropertyName = "order_id")]
        public string OrderId { get; set; }
        //true代表成功返回
        [JsonProperty(PropertyName = "error_code")]
        public string ErrorCode { get; set; }
    }
}
