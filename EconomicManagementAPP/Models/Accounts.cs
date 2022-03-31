﻿using EconomicManagementAPP.Validations;
using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class Accounts
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public int AccountTypeId { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        public decimal Balance { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public string AccountType { get; set; }
    }
}
