using System.Text.RegularExpressions;

namespace Application.CreditCards.Utilities
{
    public class RegExValidator
    {
        public IEnumerable<int> Validate(Dictionary<int, string> RegularExpressions, string expression)
        {
            List<int> keys = new List<int>();

            foreach (var ex in RegularExpressions)
            {
                var regEx = new Regex(ex.Value);

                if (regEx.IsMatch(expression))
                {
                    keys.Add(ex.Key);
                }
            }

            return keys;
        }
    }
}
