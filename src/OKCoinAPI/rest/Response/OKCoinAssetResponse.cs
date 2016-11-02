using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace com.Response
{
    public class OKCoinAssetResponse
    {
        /// <summary>
        /// 网络
        /// </summary>
        [JsonProperty(PropertyName = "net")]
        public string Net { get; set; }
        /// <summary>
        /// 总额
        /// </summary>
        [JsonProperty(PropertyName = "total")]
        public string Total { get; set; }
    }
}
