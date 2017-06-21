using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DATA.UTILITIES.RegularExpression
{
    public class NumberValidate
    {
        public string GetValidateResult(string input)
        {
            string result = string.Empty;

            Regex.Match(input, @"^[1-9]\d*$");
            return result;
        }
    }
}
