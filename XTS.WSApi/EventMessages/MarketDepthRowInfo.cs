using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTS.WSApi.Enums;
using XTS.WSApi.Lib;

namespace XTS.WSApi.EventMessages
{
    public struct MarketDepthRowInfo
    {
        private static MarketDepthRowInfo _empty = new MarketDepthRowInfo(0, 0, 0);

        private int _size;
        private double _price;
        private int _totalOrders;
        private BuyBackMarketMaker _buyBackMarketMakerFlag;

        public int Size { get { return _size; } }
        public double Price { get { return _price; } }
        public int TotalOrders { get { return _totalOrders; } }
        public BuyBackMarketMaker BuyBackMarketMaker { get { return _buyBackMarketMakerFlag; } }
        internal MarketDepthRowInfo(int size, double price, int totalOrders)
        {
            _size = size;
            _price = price;
            _totalOrders = totalOrders;
            _buyBackMarketMakerFlag = BuyBackMarketMaker.None;
        }

        internal void Set(int size, double price, int totalOrders, BuyBackMarketMaker buyBackMarketMakerFlag = BuyBackMarketMaker.None)
        {
            _size = size;
            _price = price;
            _totalOrders = totalOrders;
            _buyBackMarketMakerFlag = buyBackMarketMakerFlag;
        }

        internal bool Deserialize(BinaryStreamReader binaryStreamReader, out string errorString)
        {
            errorString = string.Empty;
            try
            {
                _size = binaryStreamReader.ReadInt32();
                _price = binaryStreamReader.ReadDouble();
                _totalOrders = binaryStreamReader.ReadInt32();
                _buyBackMarketMakerFlag = (BuyBackMarketMaker)binaryStreamReader.ReadInt16();
                return true;
            }
            catch (Exception oEx)
            {
                errorString = oEx.Message;
                return false;
            }
        }

        internal static MarketDepthRowInfo Empty { get { return _empty; } }
    }
}
