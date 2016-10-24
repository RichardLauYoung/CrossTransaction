using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CrossTransaction.ViewModels.Exchange
{
    public class ExchangeViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Amount RMB")]
        public string AmountRMB { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string CNKey { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string CNSecret { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string InterKey { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string InterSecret { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string BTCAddress { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Password { get; set; }
    }
}
