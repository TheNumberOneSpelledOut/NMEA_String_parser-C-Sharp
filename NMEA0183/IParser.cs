namespace NMEAParser.NMEA0183
{
    public interface IParser
    {
        bool IsSentence(string sentence);
        BaseSentence ParseSentence(string sentence);
    }
}
