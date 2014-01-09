using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMEAParser.NMEA0183;


namespace NMEA
{
    class Program
    {

       
        static void Main(string[] args)
             

        {

            // Sample Nmea String
            string path = "$GPRMC,132336.000,A,5152.4256,N,00832.4759,W,2.02,45.86,170111,,*2F";
   

       
       
                BaseSentence sentence = null;
                GPRMCParser parser = new GPRMCParser();

                if (parser.IsSentence(path))
                {
                    sentence = parser.ParseSentence(path);

                
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine("");
                }

         
           Console.ReadLine();
            
        }

    }
}

