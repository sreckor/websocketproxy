//using Neo4j.Driver;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace WebSocketProxy
//{
//    public static class Buffers
//    {
//        public static byte[] ByteArray(this byte[] buffer)
//        {
//            return buffer.Take(1).ToArray();
//        }

//        public static byte[] LongBuffer(this byte[] buffer)
//        {
//            return buffer.Take(8).ToArray();
//        }

//        public static byte[] ShortBuffer(this byte[] buffer)
//        {
//            return buffer.Take(2).ToArray();
//        }

//        public static byte[] IntBuffer(this byte[] buffer)
//        {
//            return buffer.Take(4).ToArray();
//        }

//        //public static readonly byte[] ByteArray = new byte[1];
//        //public static readonly byte[] ShortBuffer = new byte[2];
//        //public static readonly byte[] IntBuffer = new byte[4];
//        //public static readonly byte[] LongBuffer = new byte[8];
//    }

//    internal static class PackStreamBitConverter
//    {
//        /// <summary>Converts a byte to bytes.</summary>
//        /// <param name="value">The byte value to convert.</param>
//        /// <returns>The specified byte value as an array of bytes.</returns>
//        public static byte[] GetBytes(byte value)
//        {
//            var bytes = new[] { value };
//            return bytes;
//        }

//        /// <summary>Converts a short (Int16) to bytes.</summary>
//        /// <param name="value">The short (Int16) value to convert.</param>
//        /// <returns>The specified short (Int16) value as an array of bytes.</returns>
//        public static byte[] GetBytes(short value)
//        {
//            var bytes = BitConverter.GetBytes(value);
//            return ToTargetEndian(bytes);
//        }

//        /// <summary>Converts a short (UInt16) to bytes.</summary>
//        /// <param name="value">The short (UInt16) value to convert.</param>
//        /// <returns>The specified short (UInt16) value as an array of bytes.</returns>
//        public static byte[] GetBytes(ushort value)
//        {
//            var bytes = BitConverter.GetBytes(value);
//            return ToTargetEndian(bytes);
//        }

//        /// <summary>Converts an int (Int32) to bytes.</summary>
//        /// <param name="value">The int (Int32) value to convert.</param>
//        /// <returns>The specified int (Int32) value as an array of bytes.</returns>
//        public static byte[] GetBytes(int value)
//        {
//            var bytes = BitConverter.GetBytes(value);
//            return ToTargetEndian(bytes);
//        }

//        /// <summary>Converts an uint (UInt32) to bytes.</summary>
//        /// <param name="value">The uint (UInt32) value to convert.</param>
//        /// <returns>The specified uint (UInt32) value as an array of bytes.</returns>
//        public static byte[] GetBytes(uint value)
//        {
//            var bytes = BitConverter.GetBytes(value);
//            return ToTargetEndian(bytes);
//        }

//        /// <summary>Converts an int (Int64) to bytes.</summary>
//        /// <param name="value">The int (Int64) value to convert.</param>
//        /// <returns>The specified int (Int64) value as an array of bytes.</returns>
//        public static byte[] GetBytes(long value)
//        {
//            var bytes = BitConverter.GetBytes(value);
//            return ToTargetEndian(bytes);
//        }

//        /// <summary>Converts an int (double) to bytes.</summary>
//        /// <param name="value">The int (double) value to convert.</param>
//        /// <returns>The specified int (double) value as an array of bytes.</returns>
//        public static byte[] GetBytes(double value)
//        {
//            var bytes = BitConverter.GetBytes(value);
//            return ToTargetEndian(bytes);
//        }

//        /// <summary>Converts an string to bytes.</summary>
//        /// <param name="value">The string value to convert.</param>
//        /// <returns>The specified string value as an array of bytes.</returns>
//        public static byte[] GetBytes(string value)
//        {
//            return Encoding.UTF8.GetBytes(value);
//        }

//        /// <summary>Converts an byte array to a short.</summary>
//        /// <param name="bytes">The byte array to convert.</param>
//        /// <returns>A short converted from the byte array.</returns>
//        public static short ToInt16(byte[] bytes)
//        {
//            bytes = ToPlatformEndian(bytes);
//            return BitConverter.ToInt16(bytes, 0);
//        }

//        /// <summary>Converts an byte array to a unsigned short.</summary>
//        /// <param name="bytes">The byte array to convert.</param>
//        /// <returns>A unsigned short converted from the byte array.</returns>
//        public static ushort ToUInt16(byte[] bytes)
//        {
//            bytes = ToPlatformEndian(bytes);
//            return BitConverter.ToUInt16(bytes, 0);
//        }

//        /// <summary>Converts an byte array to a int (Int32).</summary>
//        /// <param name="bytes">The byte array to convert.</param>
//        /// <returns>A int (Int32) converted from the byte array.</returns>
//        public static int ToInt32(byte[] bytes)
//        {
//            bytes = ToPlatformEndian(bytes);
//            return BitConverter.ToInt32(bytes, 0);
//        }

//        /// <summary>Converts an byte array to a int (Int64).</summary>
//        /// <param name="bytes">The byte array to convert.</param>
//        /// <returns>A int (Int64) converted from the byte array.</returns>
//        public static long ToInt64(byte[] bytes)
//        {
//            bytes = ToPlatformEndian(bytes);
//            return BitConverter.ToInt64(bytes, 0);
//        }

//        /// <summary>Converts an byte array to a int (double).</summary>
//        /// <param name="bytes">The byte array to convert.</param>
//        /// <returns>A int (double) converted from the byte array.</returns>
//        public static double ToDouble(byte[] bytes)
//        {
//            bytes = ToPlatformEndian(bytes);
//            return BitConverter.ToDouble(bytes, 0);
//        }

//        /// <summary>Converts an byte array of a UTF8 encoded string to a string</summary>
//        /// <param name="bytes">The byte array to convert.</param>
//        /// <returns>A string converted from the byte array</returns>
//        public static string ToString(byte[] bytes)
//        {
//            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
//        }

//        /// <summary>Converts the bytes to big endian.</summary>
//        /// <param name="bytes">The bytes to convert.</param>
//        /// <returns>The bytes converted to big endian.</returns>
//        private static byte[] ToTargetEndian(byte[] bytes)
//        {
//            if (BitConverter.IsLittleEndian)
//            {
//                Array.Reverse(bytes);
//            }

//            return bytes;
//        }

//        /// <summary>Converts the bytes to the platform endian type.</summary>
//        /// <param name="bytes">The bytes to convert.</param>
//        /// <returns>The bytes converted to the platform endian type.</returns>
//        private static byte[] ToPlatformEndian(byte[] bytes)
//        {
//            if (BitConverter.IsLittleEndian)
//            {
//                Array.Reverse(bytes);
//            }

//            return bytes;
//        }
//    }

//    internal static class PackStream
//    {
//        #region PackStream Constants

//        public const byte TinyString = 0x80;
//        public const byte TinyList = 0x90;
//        public const byte TinyMap = 0xA0;
//        public const byte TinyStruct = 0xB0;
//        public const byte Null = 0xC0;
//        public const byte Float64 = 0xC1;
//        public const byte False = 0xC2;
//        public const byte True = 0xC3;
//        public const byte ReservedC4 = 0xC4;
//        public const byte ReservedC5 = 0xC5;
//        public const byte ReservedC6 = 0xC6;
//        public const byte ReservedC7 = 0xC7;
//        public const byte Int8 = 0xC8;
//        public const byte Int16 = 0xC9;
//        public const byte Int32 = 0xCA;
//        public const byte Int64 = 0xCB;
//        public const byte Bytes8 = 0xCC;
//        public const byte Bytes16 = 0xCD;
//        public const byte Bytes32 = 0xCE;
//        public const byte ReservedCf = 0xCF;
//        public const byte String8 = 0xD0;
//        public const byte String16 = 0xD1;
//        public const byte String32 = 0xD2;
//        public const byte ReservedD3 = 0xD3;
//        public const byte List8 = 0xD4;
//        public const byte List16 = 0xD5;
//        public const byte List32 = 0xD6;
//        public const byte ReservedD7 = 0xD7;
//        public const byte Map8 = 0xD8;
//        public const byte Map16 = 0xD9;
//        public const byte Map32 = 0xDA;
//        public const byte ReservedDb = 0xDB;
//        public const byte Struct8 = 0xDC;
//        public const byte Struct16 = 0xDD;
//        public const byte ReservedDe = 0xDE;
//        public const byte ReservedDf = 0xDF;
//        public const byte ReservedE0 = 0xE0;
//        public const byte ReservedE1 = 0xE1;
//        public const byte ReservedE2 = 0xE2;
//        public const byte ReservedE3 = 0xE3;
//        public const byte ReservedE4 = 0xE4;
//        public const byte ReservedE5 = 0xE5;
//        public const byte ReservedE6 = 0xE6;
//        public const byte ReservedE7 = 0xE7;
//        public const byte ReservedE8 = 0xE8;
//        public const byte ReservedE9 = 0xE9;
//        public const byte ReservedEa = 0xEA;
//        public const byte ReservedEb = 0xEB;
//        public const byte ReservedEc = 0xEC;
//        public const byte ReservedEd = 0xED;
//        public const byte ReservedEe = 0xEE;
//        public const byte ReservedEf = 0xEF;

//        public const long Plus2ToThe31 = 2147483648L;
//        public const long Plus2ToThe15 = 32768L;
//        public const long Plus2ToThe7 = 128L;
//        public const long Minus2ToThe4 = -16L;
//        public const long Minus2ToThe7 = -128L;
//        public const long Minus2ToThe15 = -32768L;
//        public const long Minus2ToThe31 = -2147483648L;

//        #endregion

//        #region Helper Methods

//        public static readonly Dictionary<string, object> EmptyDictionary = new();

//        public static void EnsureStructSize(string structName, int expected, long actual)
//        {
//            if (expected != actual)
//            {
//                //throw new ClientException(
//                //    $"{structName} structures should have {expected} fields, however received {actual} fields.");
//            }
//        }

//        #endregion
//    }

//    internal enum PackStreamType
//    {
//        Null,
//        Boolean,
//        Integer,
//        Float,
//        Bytes,
//        String,
//        List,
//        Map,
//        Struct
//    }

//    internal interface IPackStreamSerializer
//    {
//        byte[] ReadableStructs { get; }

//        IEnumerable<Type> WritableTypes { get; }

//        object Deserialize(BoltProtocolVersion version, PackStreamReader reader, byte signature, long size);
//        //void Serialize(BoltProtocolVersion version, PackStreamWriter writer, object value);
//        //(object, int) DeserializeSpan(BoltProtocolVersion version, SpanPackStreamReader reader, byte signature, int size);
//    }

//    internal interface IMessage
//    {
//        IPackStreamSerializer Serializer { get; }
//    }

//    internal interface IResponseMessage : IMessage
//    {
//        void Dispatch(IResponsePipeline pipeline);
//    }

//    internal sealed class PackStreamReader
//    {
//        //public MemoryStream Stream;

//        internal PackStreamReader(MessageFormat format,  byte[] buffers)
//        {
//            Format = format;
//            //Stream = stream;
//            Buffers = buffers;
//        }

//        public byte[] Buffers { get; }
//        public MessageFormat Format { get; }

//        public object Read()
//        {
//            var type = PeekNextType();
//            var result = ReadValue(type);
//            return result;
//        }

//        public IResponseMessage ReadMessage()
//        {
//            var size = ReadStructHeader();
//            var signature = ReadStructSignature();

//            if (Format.ReaderStructHandlers.TryGetValue(signature, out var handler))
//            {
//                return (IResponseMessage)handler.Deserialize(Format.Version, this, signature, size);
//            }

//            throw new ProtocolException("Unknown structure type: " + signature);
//        }

//        public Dictionary<string, object> ReadMap()
//        {
//            var size = (int)ReadMapHeader();
//            if (size == 0)
//            {
//                return new Dictionary<string, object>(0);
//            }

//            var map = new Dictionary<string, object>(size);
//            for (var i = 0; i < size; i++)
//            {
//                var key = ReadString();
//                map.Add(key, Read());
//            }

//            return map;
//        }

//        public IList<object> ReadList()
//        {
//            var size = (int)ReadListHeader();
//            var vals = new object[size];
//            for (var j = 0; j < size; j++)
//            {
//                vals[j] = Read();
//            }

//            return new List<object>(vals);
//        }

//        private object ReadValue(PackStreamType streamType)
//        {
//            switch (streamType)
//            {
//                case PackStreamType.Bytes:
//                    return ReadBytes();

//                case PackStreamType.Null:
//                    return ReadNull();

//                case PackStreamType.Boolean:
//                    return ReadBoolean();

//                case PackStreamType.Integer:
//                    return ReadLong();

//                case PackStreamType.Float:
//                    return ReadDouble();

//                case PackStreamType.String:
//                    return ReadString();

//                case PackStreamType.Map:
//                    return ReadMap();

//                case PackStreamType.List:
//                    return ReadList();

//                case PackStreamType.Struct:
//                    return ReadStruct();

//                default:
//                    throw new ArgumentOutOfRangeException(
//                        nameof(streamType),
//                        streamType,
//                        $"Unknown value type: {streamType}");
//            }
//        }

//        public object ReadStruct()
//        {
//            var size = ReadStructHeader();
//            var signature = ReadStructSignature();

//            if (Format.ReaderStructHandlers.TryGetValue(signature, out var handler))
//            {
//                return handler.Deserialize(Format.Version, this, signature, size);
//            }

//            throw new ProtocolException("Unknown structure type: " + signature);
//        }

//        public object ReadNull()
//        {
//            var markerByte = NextByte();
//            if (markerByte != PackStream.Null)
//            {
//                throw new ProtocolException($"Expected a null, but got: 0x{markerByte & 0xFF:X2}");
//            }

//            return null;
//        }

//        public bool ReadBoolean()
//        {
//            var markerByte = NextByte();
//            switch (markerByte)
//            {
//                case PackStream.True:
//                    return true;

//                case PackStream.False:
//                    return false;

//                default:
//                    throw new ProtocolException($"Expected a boolean, but got: 0x{markerByte & 0xFF:X2}");
//            }
//        }

//        public int ReadInteger()
//        {
//            var markerByte = NextByte();
//            if ((sbyte)markerByte >= PackStream.Minus2ToThe4)
//            {
//                return (sbyte)markerByte;
//            }

//            switch (markerByte)
//            {
//                case PackStream.Int8:
//                    return NextSByte();

//                case PackStream.Int16:
//                    return NextShort();

//                case PackStream.Int32:
//                    return NextInt();

//                case PackStream.Int64:
//                    throw new OverflowException($"Unexpectedly large Integer value unpacked {NextLong()}");

//                default:
//                    throw new ProtocolException($"Expected an integer, but got: 0x{markerByte:X2}");
//            }
//        }

//        public long ReadLong()
//        {
//            var markerByte = NextByte();
//            if ((sbyte)markerByte >= PackStream.Minus2ToThe4)
//            {
//                return (sbyte)markerByte;
//            }

//            switch (markerByte)
//            {
//                case PackStream.Int8:
//                    return NextSByte();

//                case PackStream.Int16:
//                    return NextShort();

//                case PackStream.Int32:
//                    return NextInt();

//                case PackStream.Int64:
//                    return NextLong();

//                default:
//                    throw new ProtocolException($"Expected an integer, but got: 0x{markerByte:X2}");
//            }
//        }

//        public double ReadDouble()
//        {
//            var markerByte = NextByte();
//            if (markerByte == PackStream.Float64)
//            {
//                return NextDouble();
//            }

//            throw new ProtocolException($"Expected a double, but got: 0x{markerByte:X2}");
//        }

//        public string ReadString()
//        {
//            var markerByte = NextByte();
//            if (markerByte == PackStream.TinyString) // Note no mask, so we compare to 0x80.
//            {
//                return string.Empty;
//            }

//            return PackStreamBitConverter.ToString(ReadUtf8(markerByte));
//        }

//        public byte[] ReadBytes()
//        {
//            var markerByte = NextByte();

//            switch (markerByte)
//            {
//                case PackStream.Bytes8:
//                    return ReadBytes(ReadUint8());

//                case PackStream.Bytes16:
//                    return ReadBytes(ReadUint16());

//                case PackStream.Bytes32:
//                    {
//                        var size = ReadUint32();
//                        if (size <= int.MaxValue)
//                        {
//                            return ReadBytes((int)size);
//                        }

//                        throw new ProtocolException($"BYTES_32 {size} too long for PackStream");
//                    }

//                default:
//                    throw new ProtocolException($"Expected binary data, but got: 0x{markerByte & 0xFF:X2}");
//            }
//        }

//        internal byte[] ReadBytes(int size)
//        {
//            if (size == 0)
//            {
//                return Array.Empty<byte>();
//            }

//            var heapBuffer = Buffers.Take(size).ToArray();// new byte[size];
//            //Stream.Read(heapBuffer);
//            return heapBuffer;
//        }

//        private byte[] ReadUtf8(byte markerByte)
//        {
//            var markerHighNibble = (byte)(markerByte & 0xF0);
//            var markerLowNibble = (byte)(markerByte & 0x0F);

//            if (markerHighNibble == PackStream.TinyString)
//            {
//                return ReadBytes(markerLowNibble);
//            }

//            switch (markerByte)
//            {
//                case PackStream.String8:
//                    return ReadBytes(ReadUint8());

//                case PackStream.String16:
//                    return ReadBytes(ReadUint16());

//                case PackStream.String32:
//                    {
//                        var size = ReadUint32();
//                        if (size <= int.MaxValue)
//                        {
//                            return ReadBytes((int)size);
//                        }

//                        throw new ProtocolException($"STRING_32 {size} too long for PackStream");
//                    }

//                default:
//                    throw new ProtocolException($"Expected a string, but got: 0x{markerByte & 0xFF:X2}");
//            }
//        }

//        public long ReadMapHeader()
//        {
//            var markerByte = Buffers[0];//  Stream.ReadByte();
//            var markerHighNibble = (byte)(markerByte & 0xF0);
//            var markerLowNibble = (byte)(markerByte & 0x0F);

//            if (markerHighNibble == PackStream.TinyMap)
//            {
//                return markerLowNibble;
//            }

//            switch (markerByte)
//            {
//                case PackStream.Map8:
//                    return ReadUint8();

//                case PackStream.Map16:
//                    return ReadUint16();

//                case PackStream.Map32:
//                    return ReadUint32();

//                default:
//                    throw new ProtocolException($"Expected a map, but got: 0x{markerByte:X2}");
//            }
//        }

//        public long ReadListHeader()
//        {
//            var markerByte = Buffers[0];//Stream.ReadByte();
//            var markerHighNibble = (byte)(markerByte & 0xF0);
//            var markerLowNibble = (byte)(markerByte & 0x0F);

//            if (markerHighNibble == PackStream.TinyList)
//            {
//                return markerLowNibble;
//            }

//            switch (markerByte)
//            {
//                case PackStream.List8:
//                    return ReadUint8();

//                case PackStream.List16:
//                    return ReadUint16();

//                case PackStream.List32:
//                    return ReadUint32();

//                default:
//                    throw new ProtocolException($"Expected a list, but got: 0x{markerByte & 0xFF:X2}");
//            }
//        }

//        public byte ReadStructSignature()
//        {
//            return NextByte();
//        }

//        public long ReadStructHeader()
//        {
//            var markerByte = Buffers[0];//Stream.ReadByte();
//            var markerHighNibble = (byte)(markerByte & 0xF0);
//            var markerLowNibble = (byte)(markerByte & 0x0F);

//            if (markerHighNibble == PackStream.TinyStruct)
//            {
//                return markerLowNibble;
//            }

//            switch (markerByte)
//            {
//                case PackStream.Struct8:
//                    return ReadUint8();

//                case PackStream.Struct16:
//                    return ReadUint16();

//                default:
//                    throw new ProtocolException($"Expected a struct, but got: 0x{markerByte:X2}");
//            }
//        }

//        internal PackStreamType PeekNextType()
//        {
//            var markerByte = PeekByte();
//            var markerHighNibble = (byte)(markerByte & 0xF0);

//            switch (markerHighNibble)
//            {
//                case PackStream.TinyString:
//                    return PackStreamType.String;

//                case PackStream.TinyList:
//                    return PackStreamType.List;

//                case PackStream.TinyMap:
//                    return PackStreamType.Map;

//                case PackStream.TinyStruct:
//                    return PackStreamType.Struct;
//            }

//            if ((sbyte)markerByte >= PackStream.Minus2ToThe4)
//            {
//                return PackStreamType.Integer;
//            }

//            switch (markerByte)
//            {
//                case PackStream.Null:
//                    return PackStreamType.Null;

//                case PackStream.True:
//                case PackStream.False:
//                    return PackStreamType.Boolean;

//                case PackStream.Float64:
//                    return PackStreamType.Float;

//                case PackStream.Bytes8:
//                case PackStream.Bytes16:
//                case PackStream.Bytes32:
//                    return PackStreamType.Bytes;

//                case PackStream.String8:
//                case PackStream.String16:
//                case PackStream.String32:
//                    return PackStreamType.String;

//                case PackStream.List8:
//                case PackStream.List16:
//                case PackStream.List32:
//                    return PackStreamType.List;

//                case PackStream.Map8:
//                case PackStream.Map16:
//                case PackStream.Map32:
//                    return PackStreamType.Map;

//                case PackStream.Struct8:
//                case PackStream.Struct16:
//                    return PackStreamType.Struct;

//                case PackStream.Int8:
//                case PackStream.Int16:
//                case PackStream.Int32:
//                case PackStream.Int64:
//                    return PackStreamType.Integer;

//                default:
//                    throw new ProtocolException($"Unknown type 0x{markerByte:X2}");
//            }
//        }

//        private int ReadUint8()
//        {
//            return NextByte() & 0xFF;
//        }

//        private int ReadUint16()
//        {
//            return NextShort() & 0xFFFF;
//        }

//        private long ReadUint32()
//        {
//            return NextInt() & 0xFFFFFFFFL;
//        }

//        internal sbyte NextSByte()
//        {
//            //Stream.Read(Buffers.ByteArray);
//            return (sbyte)Buffers.ByteArray()[0];
//        }

//        public byte NextByte()
//        {
//            //Stream.Read(Buffers.ByteArray);

//            return Buffers.ByteArray()[0];
//        }

//        public short NextShort()
//        {
//            //Stream.Read(Buffers.ShortBuffer());

//            return PackStreamBitConverter.ToInt16(Buffers.ShortBuffer());
//        }

//        public int NextInt()
//        {
//            //Stream.Read(Buffers.IntBuffer);

//            return PackStreamBitConverter.ToInt32(Buffers.IntBuffer());
//        }

//        public long NextLong()
//        {
//            //Stream.Read(Buffers.LongBuffer);

//            return PackStreamBitConverter.ToInt64(Buffers.LongBuffer());
//        }

//        public double NextDouble()
//        {
//            //Stream.Read(Buffers.LongBuffer);

//            return PackStreamBitConverter.ToDouble(Buffers.LongBuffer());
//        }

//        public byte PeekByte()
//        {
//            if (Stream.Length - Stream.Position < 1)
//            {
//                throw new ProtocolException("Unable to peek 1 byte from buffer.");
//            }

//            try
//            {
//                return (byte)Stream.ReadByte();
//            }
//            finally
//            {
//                Stream.Seek(-1, SeekOrigin.Current);
//            }
//        }
//    }
//}