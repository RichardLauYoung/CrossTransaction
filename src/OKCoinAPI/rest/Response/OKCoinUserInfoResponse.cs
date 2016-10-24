using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace com.Response
{
    public class OKCoinUserInfoResponse
    {
        /// <summary>
        ///  详细信息
        /// </summary>
        [JsonProperty(PropertyName = "info")]
        public OKCoinUserInfoDetailResponse Info { get; set; }
        //true代表成功返回
        [JsonProperty(PropertyName = "result")]
        public string Result { get; set; }
    }
}
