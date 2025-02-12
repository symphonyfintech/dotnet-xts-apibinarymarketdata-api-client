using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTS.WSApi.Lib;

namespace XTS.WSApi.EventMessages
{
    public abstract class DataMessage
    {
        public long TokenID { get; set; }
        public ushort ApplicationType { get; internal set; }
        public long SequenceNumber { get; protected set; }
        private ConcurrentDictionary<string, string> _extendedPropertyMap = new ConcurrentDictionary<string, string>();

        protected DataMessage(ushort messageCode, ushort messageVersion)
        {
            MessageCode = messageCode;
            MessageVersion = messageVersion;
            SequenceNumber = -1;
        }

        public ushort MessageCode { get; private set; }

        public ushort MessageVersion { get; private set; }

        internal virtual bool Deserialize(BinaryStreamReader binaryStreamReader, out string errorString)
        {
            errorString = string.Empty;
            try
            {
                MessageCode = binaryStreamReader.ReadUInt16();
                MessageVersion = binaryStreamReader.ReadUInt16();
                ApplicationType = binaryStreamReader.ReadUInt16();
                TokenID = binaryStreamReader.ReadInt64();
                SequenceNumber = binaryStreamReader.ReadInt64();

                _extendedPropertyMap = new ConcurrentDictionary<string, string>();
                {
                    _extendedPropertyMap = new ConcurrentDictionary<string, string>();
                    int extendedPropCount = binaryStreamReader.ReadInt32();
                    for (int iCnt = 0; iCnt < extendedPropCount; iCnt++)
                    {
                        string key = binaryStreamReader.ReadString();
                        string value = binaryStreamReader.ReadString();
                        _extendedPropertyMap.TryAdd(key, value);
                    }
                }

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
