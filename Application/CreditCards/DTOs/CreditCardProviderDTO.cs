using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CreditCards.DTOs
{
    public class CreditCardProviderDTO
    {
        public string Name { get; set; }
        public string CardNumberRegEx { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
