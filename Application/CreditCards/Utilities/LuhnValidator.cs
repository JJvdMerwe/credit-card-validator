using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CreditCards.Utilities
{
    public class LuhnValidator
    {
        public bool Validate(string value)
        {
            long sum = 0;

            for (int i = 0; i < value.Length; i++)
            {
                var digit = value[value.Length - 1 - i] - '0';
                sum += (i % 2 != 0) ? GetDouble(digit) : digit;
            }

            return sum % 10 == 0;

            
        }
        private int GetDouble(long i)
        {
            switch (i)
            {
                case 0: return 0;
                case 1: return 2;
                case 2: return 4;
                case 3: return 6;
                case 4: return 8;
                case 5: return 1;
                case 6: return 3;
                case 7: return 5;
                case 8: return 7;
                case 9: return 9;
                default: return 0;
            }
        }
    }
}
