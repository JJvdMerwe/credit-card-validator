using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CreditCards.DTOs
{
    public class CreditCardProviderDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CardNumberRegEx { get; set; }
        public DateTime LastModified { get; set; }
    }
}
