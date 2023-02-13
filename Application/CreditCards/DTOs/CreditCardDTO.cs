using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CreditCards.DTOs
{
    public class CreditCardDTO
    {
        public string? Number { get; set; }

        [Display(Name = "Provider Name")]
        public string? ProviderName { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
    }
}
