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
        /// <summary>
        /// 账户借款信息(只有在账户有借款信息时才会返回)
        /// </summary>
        [JsonProperty(PropertyName = "borrow")]
        public OKCoinBorrowResponse Borrow { get; set; }
        /// <summary>
        /// 账户余额
        /// </summary>
        [JsonProperty(PropertyName = "free")]
        public OKCoinFreeResponse Free { get; set; }
        /// <summary>
        /// 账户冻结余额
        /// </summary>
        [JsonProperty(PropertyName = "freezed")]
        public OKCoinFreezedResponse Freezed { get; set; }
        /// <summary>
        /// 账户理财信息(只有在账户有理财信息时才返回)
        /// </summary>
        [JsonProperty(PropertyName = "union_fund")]
        public OKCoinUnionFundResponse Union_fund { get; set; }

    }
}
