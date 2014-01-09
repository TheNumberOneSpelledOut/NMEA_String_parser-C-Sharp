
using System;

namespace NMEAParser.Utils
{
    public class DateTimeUtil
    {
       
        public static bool IsDate(string input)
        {
            DateTime date;
            return DateTime.TryParse(input, out date);
        }

        
        public static bool IsShortDate(string input)
        {
            if (!DoubleUtil.IsDouble(input))
            {
                return false;
            }

            if (input.Length != 6)
            {
                return false;
            }

            if (Int32.Parse(input) < 010101 || Int32.Parse(input) > 311299)
            {
                return false;
            }

            int year = 2000 + Int32.Parse(input.Substring(4, 2));

            bool isDate = IsDate(String.Format("{0}-{1}-{2}", year, input.Substring(2, 2), input.Substring(0, 2)));

            return isDate;
        }

        
        public static bool IsTime(string input)
        {
            if (!DoubleUtil.IsDouble(input))
            {
                return false;
            }

            if (input.Length != 10)
            {
                return false;
            }

            double time = Double.Parse(input);

            if (time < 0 || time > 235959.999)
            {
                return false;
            }

            return true;
        }

       
        public static DateTime GetDate(string date, string time)
        {
            if (!IsShortDate(date))
            {
                throw new ArgumentException("Invalid date argument");
            }

            if (!IsTime(time))
            {
                throw new ArgumentException("Invalid time argument");
            }

            int day = Int32.Parse(date.Substring(0, 2));
            int month = Int32.Parse(date.Substring(2, 2));
            int year = 2000 + Int32.Parse(date.Substring(4, 2));
            int hour = Int32.Parse(time.Substring(0, 2));
            int minute = Int32.Parse(time.Substring(2, 2));
            int second = Int32.Parse(time.Substring(4, 2));
            int millisecond = Int32.Parse(time.Substring(7, 3));

            DateTime dateTime = new DateTime(year, month, day, hour, minute, second, millisecond);
            Console.WriteLine("Date Time:"+ dateTime);
            return dateTime;
        }
    }
}
