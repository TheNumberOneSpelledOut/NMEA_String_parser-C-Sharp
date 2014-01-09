
using System;
using System.Collections.Generic;
using System.Linq;

namespace NMEAParser.Utils
{
    public class GeoUtil
    {
        
        private const double EARTH_RADIUS_KM = 6371;

       
        private static double ToRad(double input)
        {
            return input * (Math.PI / 180);
        }

      
        public static double GetDistanceKM(LatLon point1, LatLon point2)
        {
            double dLat = ToRad(point2.Latitude - point1.Latitude);
            double dLon = ToRad(point2.Longtitude - point1.Longtitude);

            double h = Math.Pow(Math.Sin(dLat / 2), 2) +
                       Math.Cos(ToRad(point1.Latitude)) * Math.Cos(ToRad(point2.Latitude)) *
                       Math.Pow(Math.Sin(dLon / 2), 2);

            h = 2 * Math.Asin(Math.Sqrt(h));

            double d = EARTH_RADIUS_KM * h;
            return d;
        }

       
        public static double GetDistanceKM(List<LatLon> route)
        {
            double distance = 0;
            LatLon? previous = null;

            foreach (LatLon p in route)
            {
                distance += previous != null ? GetDistanceKM(previous.Value, p) : 0;
                previous = p;
            }

            return distance;
        }

        
        public static double GetDistanceKM(List<LatLon> route, double distanceKM)
        {
            double distance = 0;
            LatLon? previous = null;

            foreach (LatLon p in route)
            {
                double pointDistance = previous != null ? GetDistanceKM(previous.Value, p) : 0;
                distance += pointDistance >= distanceKM ? pointDistance : 0;
                previous = p;
            }

            return distance;
        }
  
      
        public static bool LatLonExists(List<LatLon> route, LatLon point, double distanceKM)
        {
            foreach (LatLon p in route)
            {
                if (GetDistanceKM(p, point) <= distanceKM)
                {
                    return true;
                }
            }

            return false;
        }

       
        public static double RouteExists(List<LatLon> routeA, List<LatLon> routeB, double distanceKM)
        {
            //If no valid route data is provided, return 0
            if ((routeA == null || routeA.Count < 1) || (routeB == null || routeB.Count < 1)) { return 0; }

            //for each point in the shortest route, check if it
            //exists in the longest route, nearest to the distance provided
            int points = routeB.Sum(x => LatLonExists(routeA, x, distanceKM) ? 1 : 0);

            //return the number of points in routeB found in routeA
            return points / (double)routeB.Count * 100;
        }
    }
}
