using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrossTransaction.Models
{
    public class BankAccount
    {
        /// <summary>
        /// Bank card number 
        /// </summary>
        public string BankCard { set; get; }
        /// <summary>
        /// Bank contact cell phone
        /// </summary>
        public string BankContact { set; get; }
        /// <summary>
        /// Bank name 
        /// </summary>
        public string BankName { set; get; }
        /// <summary>
        /// Account user 
        /// </summary>
        public string AccountUser { set; get; }

    }
}
