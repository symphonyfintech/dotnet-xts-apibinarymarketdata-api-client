using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTS.WSApi.Enums;
using XTS.WSApi.Lib;

namespace XTS.WSApi.EventMessages
{
    public abstract class MarketDataEventMessage : DataMessage
    {
        public Segment ExchangeSegment { get; private set; }
        public int ExchangeInstrumentID { get; private set; }
        public ulong ExchangeTimeStamp { get; private set; }

        protected MarketDataEventMessage(ushort messageCode, ushort messageVersion) :
            base(messageCode, messageVersion)
        {
        }

        internal override bool Deserialize(BinaryStreamReader binaryStreamReader, out string errorString)
        {
            if (!base.Deserialize(binaryStreamReader, out errorString))
                return false;

            try
            {
                ExchangeSegment = (Segment)binaryStreamReader.ReadInt16();
                ExchangeInstrumentID = binaryStreamReader.ReadInt32();
                ExchangeTimeStamp = binaryStreamReader.ReadUInt64();
                return true;
            }
            catch (Exception oEx)
            {
                errorString = oEx.Message;
                return false;
            }
        }
    }
}
