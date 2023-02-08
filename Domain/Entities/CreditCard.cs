using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CreditCard : BaseEntity
    {
        public CreditCard()
        {
            Number = string.Empty;
            CreditCardProvider = new CreditCardProvider();
        }

        public string Number { get; set; }
        public CreditCardProvider CreditCardProvider { get; set; }
    }
}
