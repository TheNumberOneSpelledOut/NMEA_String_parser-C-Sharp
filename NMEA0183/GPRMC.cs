

using System;
using NMEAParser;
using NMEAParser.Utils;

namespace NMEAParser.NMEA0183
{
    public class GPRMC : BaseSentence
    {
        #region "Fields"
        protected LatLon _latLon;
        protected double _bearing;
        protected double _speedKnots;
        protected double _magVar;
        protected DateTime _timeStamp;
        #endregion

        #region "Properties"
        public LatLon LatLon
        {
            get
            {
                return _latLon;
            }
        }

        public double Bearing
        {
            get
            {
                return _bearing;
            }
        }
        public double SpeedKnots
        {
            get
            {
                return _speedKnots;
            }
        }
        public double SpeedKM
        {
            get
            {
                return SpeedKnots * 1.852;
            }
        }

        public DateTime TimeStamp
        {
            get
            {
                return _timeStamp;
            }
        }
        public double MagVar
        {
            get
            {
                return _magVar;
            }
        }
  
        #endregion

        internal GPRMC(LatLon latLon, double bearing, double speedKnots, DateTime timeStamp, double magVar) 
        {
            this._sentenceType = "GPRMC";
            this._latLon = latLon;
            this._bearing = bearing;
            this._speedKnots = speedKnots;
            this._timeStamp = timeStamp;
            this._magVar = magVar;
          
           Console.WriteLine("Speed KNote:"+ speedKnots);
           Console.WriteLine("Bearinge:" + bearing);
           Console.WriteLine("magVar:" + magVar);
           Console.WriteLine("Speed Km/h:" + SpeedKM);
        }
    }

    public class GPRMCParser : IParser
    {
        
        public bool IsSentence(string sentence)
        {
            //Check if the sentence is null
            if (String.IsNullOrWhiteSpace(sentence))
            {
                return false;
            }

            //Check if the sentence contains comma seperated values
            if (!sentence.Contains(","))
            {
                return false;
            }

            string[] fields = sentence.Split(',');

            //Check if the field length is valid for GPRMC
            if (fields.Length != 12)
            {
                return false;
            }

            //Check if the first field is a valid GPRMC sentence type
            if (fields[0].ToUpper() != "$GPRMC")
            {
                return false;
            }

            //Check if the second field is a valid time
            if (!DateTimeUtil.IsTime(fields[1]))
            {
                return false;
            }

            //Check if the fourth field is a valid latitude
            if (!ParserUtil.IsLatitude(fields[3]))
            {
                return false;
            }

            //Check if the fifth field is a valid latitude hemisphere
            if (!ParserUtil.IsLatitudeHemisphere(fields[4]))
            {
                return false;
            }

            //Check if the sixth field is a valid longtitude
            if (!ParserUtil.IsLongtitude(fields[5]))
            {
                return false;
            }

            //Check if the seventh field is a valid longtitude hemisphere
            if (!ParserUtil.IsLongtitudeHemisphere(fields[6]))
            {
                return false;
            }

            //Check if the eight field is a valid speed
            if (!DoubleUtil.IsDouble(fields[7]))
            {
                return false;
            }

            //Check if the ninth field is a valid bearing
            if (!DoubleUtil.IsDouble(fields[8]))
            {
                return false;
            }

            //Check if the tenth field is a valid date
            if (!DateTimeUtil.IsShortDate(fields[9]))
            {
                return false;
            }

            //Check if the eleventh field is a valid magnetic variation
            //Optional field, therefore only validate if it is supplied
            if (!String.IsNullOrWhiteSpace(fields[10]) && !DoubleUtil.IsDouble(fields[10]))
            {
                return false;
            }

            //Check if the twelfth field is a valid checksum
            string checksum = ParserUtil.GenerateChecksum(sentence);
            if (fields[11].Substring(1) != checksum)
            {
                return false;
            }
            return true;
        }

        
        public BaseSentence ParseSentence(string sentence)
        {
            //Check if the sentence is null
            if (String.IsNullOrWhiteSpace(sentence))
            {
                throw new ArgumentNullException("No sentence provided to parse");
            }

            //Check if the sentence contains comma seperated values
            if (!sentence.Contains(","))
            {
                throw new ArgumentException("Sentence cannot be parsed");
            }

            string[] fields = sentence.Split(',');

            //Check if the field length is valid for GPRMC
            if (fields.Length != 12)
            {
                throw new ArgumentException("Invalid field length found in sentence");
            }

            //Check if the first field is a valid GPRMC sentence type
            if (fields[0].ToUpper() != "$GPRMC")
            {
                throw new ArgumentException("Invalid sentence type provided in sentence");
            }

            //Check if the second field is a valid time
            if (!DateTimeUtil.IsTime(fields[1]))
            {
                throw new ArgumentException("Invalid time provided in sentence");
            }

            //Check if the fourth field is a valid latitude
            if (!ParserUtil.IsLatitude(fields[3]))
            {
                throw new ArgumentException("Invalid latitude provided in sentence");
            }

            //Check if the fifth field is a valid latitude hemisphere
            if (!ParserUtil.IsLatitudeHemisphere(fields[4]))
            {
                throw new ArgumentException("Invalid latitude hemisphere provided in sentence");
            }

            //Check if the sixth field is a valid longtitude
            if (!ParserUtil.IsLongtitude(fields[5]))
            {
                throw new ArgumentException("Invalid longtitude provided in sentence");
            }

            //Check if the seventh field is a valid longtitude hemisphere
            if (!ParserUtil.IsLongtitudeHemisphere(fields[6]))
            {
                throw new ArgumentException("Invalid longtitude hemisphere provided in sentence");
            }

            //Check if the tenth field is a valid date
            if (!DateTimeUtil.IsShortDate(fields[9]))
            {
                throw new ArgumentException("Invalid date provided in sentence");
            }

            //Check if the eleventh field is a valid magnetic variation
            //Optional field, therefore only use it if it is supplied
            if (!String.IsNullOrWhiteSpace(fields[10]) && !DoubleUtil.IsDouble(fields[10]))
            {
                throw new ArgumentException("Invalid magnetic variation provided in sentence");
            }

            //Check if the twelfth field is a valid checksum
            string checksum = ParserUtil.GenerateChecksum(sentence);
            if (fields[11].Substring(1) != checksum)
            {
                throw new ArgumentException("Invalid checksum provided for sentence");
            }

            DateTime timestamp = DateTimeUtil.GetDate(fields[9], fields[1]);

            double latitude = ParserUtil.GetLatitude(fields[3], fields[4]);
            double longtitude = ParserUtil.GetLongtitude(fields[5], fields[6]);
            double speed = 0;
            double bearing = 0;
            double magVar = 0;

            

            //Only convert the speed field if it's actually used
            if (DoubleUtil.IsDouble(fields[7]))
            {
                speed = Double.Parse(fields[7]);
                
            }

            //Only convert the bearing field if it's actually used
            if (DoubleUtil.IsDouble(fields[8]))
            {
                bearing = Double.Parse(fields[8]);
            }

            //Only convert the magnetic variation field if it's actually used
            if (DoubleUtil.IsDouble(fields[10]))
            {
                magVar = Double.Parse(fields[10]);
            }
           

            return new GPRMC(new LatLon(latitude, longtitude), bearing, speed, timestamp, magVar);

            

        }
    }
}
