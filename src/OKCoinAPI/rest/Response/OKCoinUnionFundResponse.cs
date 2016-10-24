﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace com.Response
{
    public class OKCoinUnionFundResponse
    {
        /// <summary>
        ///  bitcoin
        /// </summary>
        [JsonProperty(PropertyName = "btc")]
        public string Btc { get; set; }
        /// <summary>
        /// Litcoin
        /// </summary>
        [JsonProperty(PropertyName = "ltc")]
        public string Ltc { get; set; }
    }
}
