using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTS.WSApi.Lib
{
    sealed class BinaryStreamReader : IDisposable
    {
        private BinaryReader _binaryReader;

        // --------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a new instance using the specified
        ///     <see cref="System.IO.BinaryReader"/>.
        /// </summary>
        /// <param name="binaryReader">The <see cref="BinaryReader"/> instance to use</param>
        // --------------------------------------------------------------------------------
        public BinaryStreamReader(BinaryReader binaryReader)
        {
            _binaryReader = binaryReader;
        }

        public Boolean ReadBoolean()
        {
            return _binaryReader.ReadBoolean();
        }

        public Byte ReadByte()
        {
            return _binaryReader.ReadByte();
        }

        public SByte ReadSByte()
        {
            return _binaryReader.ReadSByte();
        }

        public Char ReadChar()
        {
            return _binaryReader.ReadChar();
        }

        public Int16 ReadInt16()
        {
            return _binaryReader.ReadInt16();
        }

        public UInt16 ReadUInt16()
        {
            return _binaryReader.ReadUInt16();
        }

        public Int32 ReadInt32()
        {
            return _binaryReader.ReadInt32();
        }

        public UInt32 ReadUInt32()
        {
            return _binaryReader.ReadUInt32();
        }

        public Int64 ReadInt64()
        {
            return _binaryReader.ReadInt64();
        }

        public UInt64 ReadUInt64()
        {
            return _binaryReader.ReadUInt64();
        }

        public Decimal ReadDecimal()
        {
            return _binaryReader.ReadDecimal();
        }

        public DateTime ReadDateTime()
        {
            long tickValue = _binaryReader.ReadInt64();
            if (tickValue <= DateTime.MinValue.Ticks)
                return DateTime.MinValue;
            else if (tickValue >= DateTime.MaxValue.Ticks)
                return DateTime.MaxValue;
            else
                return new DateTime(tickValue);
        }

        public Single ReadSingle()
        {
            return _binaryReader.ReadSingle();
        }

        public Double ReadDouble()
        {
            return _binaryReader.ReadDouble();
        }

        public Guid ReadGuid()
        {
            return new Guid(_binaryReader.ReadBytes(16));
        }

        public String ReadString()
        {
            if (_binaryReader.ReadByte() == 0) return string.Empty;
            // Updated By Dharmesh as on 12/01/2021 - For Memory Performance
            //return _binaryReader.ReadString();
            return string.Intern(_binaryReader.ReadString());
        }

        public byte[] ReadByteArray()
        {
            if (_binaryReader.ReadByte() == 0) return null;
            int length = _binaryReader.ReadInt32();
            return _binaryReader.ReadBytes(length);
        }

        public Boolean? ReadNullableBoolean()
        {
            if (_binaryReader.ReadByte() == 0) return null;
            return _binaryReader.ReadBoolean();
        }

        public Byte? ReadNullableByte()
        {
            if (_binaryReader.ReadByte() == 0) return null;
            return _binaryReader.ReadByte();
        }

        public SByte? ReadNullableSByte()
        {
            if (_binaryReader.ReadByte() == 0) return null;
            return _binaryReader.ReadSByte();
        }

        public Char? ReadNullableChar()
        {
            if (_binaryReader.ReadByte() == 0) return null;
            return _binaryReader.ReadChar();
        }

        public Int16? ReadNullableInt16()
        {
            if (_binaryReader.ReadByte() == 0) return null;
            return _binaryReader.ReadInt16();
        }

        public UInt16? ReadNullableUInt16()
        {
            if (_binaryReader.ReadByte() == 0) return null;
            return _binaryReader.ReadUInt16();
        }

        public Int32? ReadNullableInt32()
        {
            if (_binaryReader.ReadByte() == 0) return null;
            return _binaryReader.ReadInt32();
        }

        public UInt32? ReadNullableUInt32()
        {
            if (_binaryReader.ReadByte() == 0) return null;
            return _binaryReader.ReadUInt32();
        }

        public Int64? ReadNullableInt64()
        {
            if (_binaryReader.ReadByte() == 0) return null;
            return _binaryReader.ReadInt64();
        }

        public UInt64? ReadNullableUInt64()
        {
            if (_binaryReader.ReadByte() == 0) return null;
            return _binaryReader.ReadUInt64();
        }

        public Decimal? ReadNullableDecimal()
        {
            if (_binaryReader.ReadByte() == 0) return null;
            return _binaryReader.ReadDecimal();
        }

        public DateTime? ReadNullableDateTime()
        {
            if (_binaryReader.ReadByte() == 0) return null;
            return new DateTime(_binaryReader.ReadInt64());
        }

        public Single? ReadNullableSingle()
        {
            if (_binaryReader.ReadByte() == 0) return null;
            return _binaryReader.ReadSingle();
        }

        public Double? ReadNullableDouble()
        {
            if (_binaryReader.ReadByte() == 0) return null;
            return _binaryReader.ReadDouble();
        }

        public Guid? ReadNullableGuid()
        {
            if (_binaryReader.ReadByte() == 0) return null;
            return new Guid(_binaryReader.ReadBytes(16));
        }

        /// <summary>
        /// Returns an Int32 value from the stream that was stored optimized.
        /// </summary>
        /// <returns>An Int32 value.</returns>
        public int ReadOptimizedInt32()
        {
            int result = 0;
            int bitShift = 0;
            while (true)
            {
                byte nextByte = ReadByte();
                result |= ((int)nextByte & 0x7f) << bitShift;
                bitShift += 7;
                if ((nextByte & 0x80) == 0) return result;
            }
        }

        /// <summary>
        /// Returns a UInt32 value from the stream that was stored optimized.
        /// </summary>
        /// <returns>A UInt32 value.</returns>
        public uint ReadOptimizedUInt32()
        {
            uint result = 0;
            int bitShift = 0;
            while (true)
            {
                byte nextByte = ReadByte();
                result |= ((uint)nextByte & 0x7f) << bitShift;
                bitShift += 7;
                if ((nextByte & 0x80) == 0) return result;
            }
        }

        /// <summary>
        /// Returns an Int64 value from the stream that was stored optimized.
        /// </summary>
        /// <returns>An Int64 value.</returns>
        public long ReadOptimizedInt64()
        {
            long result = 0;
            int bitShift = 0;
            while (true)
            {
                byte nextByte = ReadByte();
                result |= ((long)nextByte & 0x7f) << bitShift;
                bitShift += 7;
                if ((nextByte & 0x80) == 0) return result;
            }
        }

        /// <summary>
        /// Returns a UInt64 value from the stream that was stored optimized.
        /// </summary>
        /// <returns>A UInt64 value.</returns>
        public ulong ReadOptimizedUInt64()
        {
            ulong result = 0;
            int bitShift = 0;
            while (true)
            {
                byte nextByte = ReadByte();
                result |= ((ulong)nextByte & 0x7f) << bitShift;
                bitShift += 7;
                if ((nextByte & 0x80) == 0) return result;
            }
        }

        public object ReadObject(Type type)
        {
            if (type == typeof(Boolean)) return ReadBoolean();
            else if (type == typeof(Boolean?)) return ReadNullableBoolean();
            else if (type == typeof(Byte)) return ReadByte();
            else if (type == typeof(Byte?)) return ReadNullableByte();
            else if (type == typeof(Byte[])) return ReadByteArray();
            else if (type == typeof(Char)) return ReadChar();
            else if (type == typeof(Char?)) return ReadNullableChar();
            else if (type == typeof(DateTime)) return ReadDateTime();
            else if (type == typeof(DateTime?)) return ReadNullableDateTime();
            else if (type == typeof(Decimal)) return ReadDecimal();
            else if (type == typeof(Decimal?)) return ReadNullableDecimal();
            else if (type == typeof(Double)) return ReadDouble();
            else if (type == typeof(Double?)) return ReadNullableDouble();
            else if (type == typeof(Guid)) return ReadGuid();
            else if (type == typeof(Guid?)) return ReadNullableGuid();
            else if (type == typeof(Int16)) return ReadInt16();
            else if (type == typeof(Int16?)) return ReadNullableInt16();
            else if (type == typeof(Int32)) return ReadInt32();
            else if (type == typeof(Int32?)) return ReadNullableInt32();
            else if (type == typeof(Int64)) return ReadInt64();
            else if (type == typeof(Int64?)) return ReadNullableInt64();
            else if (type == typeof(SByte)) return ReadSByte();
            else if (type == typeof(SByte?)) return ReadNullableSByte();
            else if (type == typeof(Single)) return ReadSingle();
            else if (type == typeof(Single?)) return ReadNullableSingle();
            else if (type == typeof(String)) return ReadString();
            else if (type == typeof(UInt16)) return ReadUInt16();
            else if (type == typeof(UInt16?)) return ReadNullableUInt16();
            else if (type == typeof(UInt32)) return ReadUInt32();
            else if (type == typeof(UInt32?)) return ReadNullableUInt32();
            else if (type == typeof(UInt64)) return ReadUInt64();
            else if (type == typeof(UInt64?)) return ReadNullableUInt64();
            else
            {
                throw new InvalidOperationException(type.ToString());
            }
        }

        public bool IsEOF()
        {
            return _binaryReader.BaseStream.Position == _binaryReader.BaseStream.Length;
        }

        // --------------------------------------------------------------------------------
        /// <summary>
        /// Cleans up resources held by this instance.
        /// </summary>
        // --------------------------------------------------------------------------------
        void IDisposable.Dispose()
        {
            (_binaryReader as IDisposable).Dispose();
        }
    }
}
