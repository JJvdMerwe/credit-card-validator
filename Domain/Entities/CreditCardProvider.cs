using Domain.Common;

namespace Domain.Entities
{
    public class CreditCardProvider : BaseEntity
    {
        public CreditCardProvider()
        {
            Name = string.Empty;
            CardNumberRegEx = string.Empty;
        }

        public virtual string Name { get; set; }
        public virtual string CardNumberRegEx { get; set; }
        public virtual DateTime LastModified { get; set; }
    }
}
