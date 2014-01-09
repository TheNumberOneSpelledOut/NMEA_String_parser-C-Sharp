
using System;

namespace NMEAParser.Utils
{
    public class DoubleUtil
    {
        
        public static bool IsDouble(string input)
        {
            Double dec;
            return Double.TryParse(input, out dec);
        }
    }
}
