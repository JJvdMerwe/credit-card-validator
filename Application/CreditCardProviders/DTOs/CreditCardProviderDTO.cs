using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CreditCardProviders.DTOs
{
    public class CreditCardProviderDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Card Number RegEx")]
        public string CardNumberRegEx { get; set; }

        [Display(Name = "Last Modified")]
        public DateTime LastModified { get; set; }
    }
}
