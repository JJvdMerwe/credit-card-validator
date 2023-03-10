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
            Provider = new CreditCardProvider();
            DateCreated = DateTime.Now;
        }

        public virtual string Number { get; set; }
        public virtual CreditCardProvider Provider { get; set; }
        public virtual DateTime DateCreated { get; set; }
    }
}
