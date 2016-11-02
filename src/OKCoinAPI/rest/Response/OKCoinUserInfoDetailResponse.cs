using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace com.Response
{
    public class OKCoinUserInfoDetailResponse
    {
        /// <summary>
        ///  账户资产，包含净资产及总资产
        /// </summary>
        [JsonProperty(PropertyName = "funds")]
        public OKCoinFundsResponse Funds { get; set; }
    }
}
