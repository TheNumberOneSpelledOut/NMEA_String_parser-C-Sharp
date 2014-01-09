
using System;

namespace NMEAParser.NMEA0183
{
    public abstract class BaseSentence
    {
        #region "Fields"

        protected string _sentenceType;
        #endregion

        #region "Properties"
        public string SentenceType
        {
            get
            {
                return _sentenceType;
            }
        }


        #endregion

        protected BaseSentence()
        {
        }
    }
}
