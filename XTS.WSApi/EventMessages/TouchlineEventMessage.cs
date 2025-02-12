using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using XTS.WSApi.Enums;
using XTS.WSApi.Lib;

namespace XTS.WSApi.EventMessages
{
    public sealed class TouchlineEventMessage : MarketDataEventMessage
    {
        public XTSMarketType MarketType { get; private set; }
        public XTSBookType BookType { get; private set; }
        public TouchlineData Touchline { get; private set; }

        private TouchlineEventMessage()
            : base((ushort)SubscribtionCode.TouchlineEvent, 1)
        {
        }

        internal override bool Deserialize(BinaryStreamReader binaryStreamReader, out string errorString)
        {
            if (!base.Deserialize(binaryStreamReader, out errorString))
                return false;

            try
            {
                Touchline = (TouchlineData)FormatterServices.GetUninitializedObject(typeof(TouchlineData));
                Touchline.Deserialize(binaryStreamReader, out errorString);
                BookType = (XTSBookType)binaryStreamReader.ReadInt16();
                MarketType = (XTSMarketType)binaryStreamReader.ReadInt16();
                return true;
            }
            catch (Exception oEx)
            {
                errorString = oEx.Message;
                return false;
            }
        }

        public override string ToString()
        {
            StringBuilder touchLineEventReqLogString = new StringBuilder();
            touchLineEventReqLogString.Clear();
            touchLineEventReqLogString.Append(base.ToString());//0
            touchLineEventReqLogString.Append(", MarketType:");
            touchLineEventReqLogString.Append(MarketType.ToString());//1
            touchLineEventReqLogString.Append(", BookType:");
            touchLineEventReqLogString.Append(BookType.ToString());//2
            touchLineEventReqLogString.Append(", TouchlineData:[");
            touchLineEventReqLogString.Append(Touchline == null ? string.Empty : Touchline.ToString()).Append("]");//3

            return touchLineEventReqLogString.ToString();
        }
    }
}
