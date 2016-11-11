using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CrossTransaction.Models
{
    [Table("AD_USERS")]
    public class UserAccount: IdentityUser
    {
        /// <summary>
        /// Primary Key automatic increase
        /// </summary>
        [Key]
        public int UserID { get; set; }
        /// <summary>
        /// User password
        /// </summary>
        [StringLength(50, MinimumLength =6)]
        public string Password { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [StringLength(100)]
        public string Name { get; set; }
        /// <summary>
        /// User cell phone number
        /// </summary>
        [StringLength(11)]
        public string CellPhone { get; set; }
        /// <summary>
        /// User tele phone number
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// User type normal user or company 
        /// </summary>
        [StringLength(3)]
        public string UserType { get; set; }
        /// <summary>
        /// User birthday
        /// </summary>
        [StringLength(10)]
        public string Birthday { get; set; }
        /// <summary>
        /// Identity number
        /// </summary>
        [StringLength(30)]
        public string IdentityNo { get; set; }
        /// <summary>
        /// Country code
        /// </summary>
        public string CountryCode { get; set; }

    }
}
