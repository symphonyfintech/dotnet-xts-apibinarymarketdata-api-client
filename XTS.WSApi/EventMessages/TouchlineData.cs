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
    public sealed class TouchlineData
    {
        private static TouchlineData _empty = new TouchlineData(
            MarketDepthRowInfo.Empty,
            MarketDepthRowInfo.Empty,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

        private MarketDepthRowInfo _bidInfo;
        private MarketDepthRowInfo _askInfo;
        private double _lastTradedPrice;
        private int _lastTradedQunatity;
        private uint _totalBuyQuantity;
        private uint _totalSellQuantity;
        private uint _totalTradedQuantity;
        private double _averageTradedPrice;
        private long _lastTradedTime;
        private double _percentChange;
        private double _open;
        private double _high;
        private double _low;
        private double _close;
        private bool _isDayHigh, _isDayLow;

        [Obsolete]
        private double _totalValueTraded = double.NaN;

        private long _lastUpdateTime;
        private BuyBackMarketMaker _bbTotalBuy;
        private BuyBackMarketMaker _bbTotalSell;

        public MarketDepthRowInfo BidInfo { get { return _bidInfo; } }
        public MarketDepthRowInfo AskInfo { get { return _askInfo; } }
        public double LastTradedPrice
        {
            get
            {
                if (double.IsNaN(_lastTradedPrice) || double.IsInfinity(_lastTradedPrice))
                {
                    return 0;
                }
                return _lastTradedPrice;
            }
        }
        public int LastTradedQunatity { get { return _lastTradedQunatity; } }
        public uint TotalBuyQuantity { get { return _totalBuyQuantity; } }
        public uint TotalSellQuantity { get { return _totalSellQuantity; } }
        public uint TotalTradedQuantity { get { return _totalTradedQuantity; } }
        public double AverageTradedPrice
        {
            get
            {
                if (double.IsNaN(_averageTradedPrice) || double.IsInfinity(_averageTradedPrice))
                {
                    return 0;
                }
                else
                {
                    return _averageTradedPrice;
                }
            }
        }
        public long LastTradedTime { get { return _lastTradedTime; } }
        public long LastUpdateTime { get { return _lastUpdateTime; } }
        public double PercentChange
        {
            get
            {
                if (double.IsNaN(_percentChange) || double.IsInfinity(_percentChange))
                {
                    return 0;
                }
                else
                {
                    return _percentChange;
                }
            }
        }
        public double Open
        {
            get
            {
                if (double.IsNaN(_open) || double.IsInfinity(_open))
                {
                    return 0;
                }
                else
                {
                    return _open;
                }
            }
        }
        public double High
        {
            get
            {
                if (double.IsNaN(_high) || double.IsInfinity(_high))
                {
                    return 0;
                }
                else
                {
                    return _high;
                }
            }
        }

        public bool IsDayHigh
        {
            get
            {
                return _isDayHigh;
            }
        }

        private void CalculateDayHigh(double LTP, double High)
        {
            _isDayHigh = false;
            if (LTP >= High && High > 0 && LTP > 0)
                _isDayHigh = true;

        }
        private void CalculateDayLow(double LTP, double Low)
        {
            _isDayLow = false;
            if (LTP <= Low && Low > 0 && LTP > 0)
                _isDayLow = true;

        }

        private void CalculateDayHigh(double High)
        {
            _isDayHigh = false;
            if (High >= _high && High > 0 && _high > 0)
                _isDayHigh = true;

        }

        private void CalculateDayLow(double Low)
        {
            _isDayLow = false;
            if (Low <= _low && Low > 0 && _low > 0)
                _isDayLow = true;

        }

        public bool IsDayLow
        {
            get
            {
                return _isDayLow;
            }
        }

        public double Low
        {
            get
            {
                if (double.IsNaN(_low) || double.IsInfinity(_low))
                {
                    return 0;
                }
                else
                {
                    return _low;
                }
            }
        }
        public double Close
        {
            get
            {
                if (double.IsNaN(_close) || double.IsInfinity(_close))
                {
                    return 0;
                }
                else
                {
                    return _close;
                }
            }
        }

        public BuyBackMarketMaker BbTotalBuy { get { return _bbTotalBuy; } }
        public BuyBackMarketMaker BbTotalSell { get { return _bbTotalSell; } }

        internal TouchlineData(MarketDepthRowInfo bidInfo, MarketDepthRowInfo askInfo, long lastUpdateTime,
            double lastTradedPrice, int lastTradedQty, long lastTradedTime, uint totalBuyQuantity, uint totalSellQuantity, uint totalTradedQuantity, double averageTradedPrice,
            double percentChange, double open, double high, double low, double close)
        {
            _bidInfo = bidInfo;
            _askInfo = askInfo;
            _lastUpdateTime = lastUpdateTime;
            _lastTradedPrice = lastTradedPrice;
            _lastTradedQunatity = lastTradedQty;
            _totalBuyQuantity = totalBuyQuantity;
            _totalSellQuantity = totalSellQuantity;
            _totalTradedQuantity = totalTradedQuantity;
            _averageTradedPrice = averageTradedPrice;
            _lastTradedTime = lastTradedTime;
            _percentChange = percentChange;
            _open = open;
            CalculateDayHigh(LastTradedPrice, high);
            CalculateDayLow(LastTradedPrice, low);
            _high = high;
            _low = low;
            _close = close;
            _bbTotalBuy = BuyBackMarketMaker.None;
            _bbTotalSell = BuyBackMarketMaker.None;
        }

        internal bool Deserialize(BinaryStreamReader binaryStreamReader, out string errorString)
        {
            try
            {
                _bidInfo = (MarketDepthRowInfo)FormatterServices.GetUninitializedObject(typeof(MarketDepthRowInfo));
                _askInfo = (MarketDepthRowInfo)FormatterServices.GetUninitializedObject(typeof(MarketDepthRowInfo));

                bool success = _bidInfo.Deserialize(binaryStreamReader, out errorString);
                if (!success)
                    return success;
                success = _askInfo.Deserialize(binaryStreamReader, out errorString);
                if (!success)
                    return success;

                _lastUpdateTime = binaryStreamReader.ReadInt64();
                _lastTradedPrice = binaryStreamReader.ReadDouble();
                _lastTradedQunatity = binaryStreamReader.ReadInt32();
                _totalBuyQuantity = binaryStreamReader.ReadUInt32();
                _totalSellQuantity = binaryStreamReader.ReadUInt32();
                _totalTradedQuantity = binaryStreamReader.ReadUInt32();
                _averageTradedPrice = binaryStreamReader.ReadDouble();
                _lastTradedTime = binaryStreamReader.ReadInt64();
                _percentChange = binaryStreamReader.ReadDouble();
                _open = binaryStreamReader.ReadDouble();

                Double high = binaryStreamReader.ReadDouble();
                Double low = binaryStreamReader.ReadDouble();
                CalculateDayHigh(LastTradedPrice, high);
                CalculateDayLow(LastTradedPrice, low);
                _high = high;
                _low = low;

                _close = binaryStreamReader.ReadDouble();
                _totalValueTraded = binaryStreamReader.ReadDouble();
                _bbTotalBuy = (BuyBackMarketMaker)binaryStreamReader.ReadInt16();
                _bbTotalSell = (BuyBackMarketMaker)binaryStreamReader.ReadInt16();
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
            StringBuilder touchLineLogString = new StringBuilder();
            touchLineLogString.Append("Bid:[").Append(_bidInfo.Size);//0
            touchLineLogString.Append("|").Append(_bidInfo.Price);//1
            touchLineLogString.Append("|").Append(_bidInfo.TotalOrders);//2

            touchLineLogString.Append("], Ask:[").Append(_askInfo.Size);//3
            touchLineLogString.Append("|").Append(_askInfo.Price);//4
            touchLineLogString.Append("|").Append(_askInfo.TotalOrders);//5

            touchLineLogString.Append("], LTP:").Append(_lastTradedTime);//6
            touchLineLogString.Append("|").Append(_lastTradedQunatity);//7
            touchLineLogString.Append("|").Append(_lastTradedPrice);//8

            touchLineLogString.Append(", TBQ:").Append(_totalBuyQuantity);//9

            touchLineLogString.Append(", TSQ:").Append(_totalSellQuantity);//10

            touchLineLogString.Append(", TotalVol:").Append(_totalTradedQuantity);//11

            touchLineLogString.Append(", O:").Append(_open);//12
            touchLineLogString.Append(", H:").Append(_high);//13
            touchLineLogString.Append(", L:").Append(_low);//14
            touchLineLogString.Append(", C:").Append(_close);//15

            return touchLineLogString.ToString();
        }
    }
}
