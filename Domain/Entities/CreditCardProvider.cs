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

        public string Name { get; set; }
        public string CardNumberRegEx { get; set; }
        public DateTime LastModified { get; set; }
    }
}
