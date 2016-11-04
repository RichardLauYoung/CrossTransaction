﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CrossTransaction.Models
{
    public class AD_Users
    {
        /// <summary>
        /// Primary Key automatic increase
        /// </summary>
        [Key]
        public int UserID { get; set; }
        /// <summary>
        /// User nickname
        /// </summary>
        [StringLength(50)]
        public string UserName { get; set; }
        /// <summary>
        /// User password
        /// </summary>
        [StringLength(50, MinimumLength =6)]
        public string Password { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [StringLength(100)]
        public string FirstName { get; set; }
        /// <summary>
        /// Mid name
        /// </summary>
        [StringLength(100)]
        public string MidName { get; set; }
        /// <summary>
        /// Family name
        /// </summary>
        [StringLength(100)]
        public string LastName { get; set; }
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
        /// Email
        /// </summary>
        [StringLength(50)]
        public string Email { get; set; }
        /// <summary>
        /// User type normal user or company 
        /// </summary>
        [StringLength(3)]
        public string UserType { get; set; }

    }
}