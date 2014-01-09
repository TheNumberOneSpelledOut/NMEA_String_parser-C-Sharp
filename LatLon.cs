

namespace NMEAParser
{
    public struct LatLon
    {
        public double Latitude;
        public double Longtitude;

        public LatLon(double latitude, double longtitude)
        {
            Latitude = latitude;
            Longtitude = longtitude;

            System.Console.WriteLine("Latitude, Longtitude" + Latitude +"," + Longtitude);
            
        }
    }
}