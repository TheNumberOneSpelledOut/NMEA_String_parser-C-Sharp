
using System;

namespace NMEAParser.Utils
{
    public class ParserUtil
    {
     
        public static bool IsGPSLock(string input)
        {
            if (String.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            input = input.ToUpper().Trim();

            if (input != "A")
            {
                return false;
            }

            return true;
        }

       
        public static bool IsLatitudeHemisphere(string input)
        {
            if (String.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            input = input.ToUpper().Trim();

            if (input != "N" && input != "S")
            {
                return false;
            }

            return true;
        }

      
        public static bool IsLongtitudeHemisphere(string input)
        {
            if (String.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            input = input.ToUpper().Trim();

            if (input != "E" && input != "W")
            {
                return false;
            }

            return true;
        }

      
        public static bool IsLatitude(string input)
        {
            if (!DoubleUtil.IsDouble(input))
            {
                return false;
            }

            double latitude = Double.Parse(input);

            if (latitude < 0)
            {
                latitude = latitude * -1;
            }

            if (latitude > 9000)
            {
                return false;
            }

            return true;
        }

     
        public static bool IsLongtitude(string input)
        {
            if (!DoubleUtil.IsDouble(input))
            {
                return false;
            }

            double longtitude = Double.Parse(input);

            if (longtitude < 0)
            {
                longtitude = longtitude * -1;
            }

            if (longtitude > 18000)
            {
                return false;
            }

            return true;
        }

       
      
        public static string GenerateChecksum(string sentence)
        {
            if (String.IsNullOrWhiteSpace(sentence))
            {
                throw new ArgumentNullException("No sentence provided");
            }

            if (!sentence.Contains("$"))
            {
                throw new ArgumentException("Sentence is missing start character");
            }

            if (!sentence.Contains("*"))
            {
                throw new ArgumentException("Sentence is missing end character");
            }

            int startIndex = sentence.IndexOf('$');
            int endIndex = sentence.IndexOf('*');
            int checksum = Convert.ToByte(sentence[startIndex + 1]);

            for (int i = startIndex + 2; i < endIndex; i++)
            {
                checksum ^= Convert.ToByte(sentence[i]);
            }

            return checksum.ToString("X2");
        }

       
        public static double GetLatitude(string latitude, string hemisphere)
        {
            double hours;
            double minutes;
            double lat = 0;

            hemisphere = hemisphere.ToUpper().Trim();

            if (!IsLatitude(latitude))
            {
                throw new ArgumentException("Invalid latitude provided");
            }

            if (!IsLatitudeHemisphere(hemisphere))
            {
                throw new ArgumentException("Invalid latitude hemisphere provided");
            }

            hours = double.Parse(latitude.Substring(0, 2));
            minutes = double.Parse(latitude.Substring(2)) / 60;
            lat = hours + minutes;

            //If the latitude is south of the equator convert it to a minus number
            if (hemisphere == "S")
            {
                lat = lat * -1;
            }

            return lat;

        }

       
        public static double GetLongtitude(string longtitude, string hemisphere)
        {
            double hours;
            double minutes;
            double lon = 0;

            hemisphere = hemisphere.ToUpper().Trim();

            if (!IsLongtitude(longtitude))
            {
                throw new ArgumentException("Invalid longtitude provided");
            }

            if (!IsLongtitudeHemisphere(hemisphere))
            {
                throw new ArgumentException("Invalid longtitude hemisphere provided");
            }

            hours = double.Parse(longtitude.Substring(0, 3));
            minutes = double.Parse(longtitude.Substring(3)) / 60;
            lon = hours + minutes;

            //If the longtitude is west of the Prime Meridian convert it to a minus number
            if (hemisphere == "W")
            {
                lon = lon * -1;
            }

            return lon;

        }
    }
}
