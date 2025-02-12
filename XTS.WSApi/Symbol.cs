using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTS.WSApi.Enums;

namespace XTS.WSApi
{
    public class Symbol
    {
        public Segment exchangeSegment { get; private set; }
        public long exchangeInstrumentID { get; private set; }
        public Symbol(Segment segment, long token)
        {
            exchangeSegment = segment;
            exchangeInstrumentID = token;
        }
    }
}
