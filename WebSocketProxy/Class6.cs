using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebSocketProxy
{
    //https://github.com/neo4j-graph-examples/movies/blob/main/scripts/movies.cypher
    public static class MessageUnpacker
    {
        public const byte TinyString = 0x80;
        public const byte TinyList = 0x90;
        public const byte TinyMap = 0xA0;
        public const byte TinyStruct = 0xB0;
        public const byte Null = 0xC0;
        public const byte Float64 = 0xC1;
        public const byte False = 0xC2;
        public const byte True = 0xC3;
        public const byte ReservedC4 = 0xC4;
        public const byte ReservedC5 = 0xC5;
        public const byte ReservedC6 = 0xC6;
        public const byte ReservedC7 = 0xC7;
        public const byte Int8 = 0xC8;
        public const byte Int16 = 0xC9;
        public const byte Int32 = 0xCA;
        public const byte Int64 = 0xCB;
        public const byte Bytes8 = 0xCC;
        public const byte Bytes16 = 0xCD;
        public const byte Bytes32 = 0xCE;
        public const byte ReservedCf = 0xCF;
        public const byte String8 = 0xD0;
        public const byte String16 = 0xD1;
        public const byte String32 = 0xD2;
        public const byte ReservedD3 = 0xD3;
        public const byte List8 = 0xD4;
        public const byte List16 = 0xD5;
        public const byte List32 = 0xD6;
        public const byte ReservedD7 = 0xD7;
        public const byte Map8 = 0xD8;
        public const byte Map16 = 0xD9;
        public const byte Map32 = 0xDA;
        public const byte ReservedDb = 0xDB;
        public const byte Struct8 = 0xDC;
        public const byte Struct16 = 0xDD;
        public const byte ReservedDe = 0xDE;
        public const byte ReservedDf = 0xDF;
        public const byte ReservedE0 = 0xE0;
        public const byte ReservedE1 = 0xE1;
        public const byte ReservedE2 = 0xE2;
        public const byte ReservedE3 = 0xE3;
        public const byte ReservedE4 = 0xE4;
        public const byte ReservedE5 = 0xE5;
        public const byte ReservedE6 = 0xE6;
        public const byte ReservedE7 = 0xE7;
        public const byte ReservedE8 = 0xE8;
        public const byte ReservedE9 = 0xE9;
        public const byte ReservedEa = 0xEA;
        public const byte ReservedEb = 0xEB;
        public const byte ReservedEc = 0xEC;
        public const byte ReservedEd = 0xED;
        public const byte ReservedEe = 0xEE;
        public const byte ReservedEf = 0xEF;

        public const long Plus2ToThe31 = 2147483648L;
        public const long Plus2ToThe15 = 32768L;
        public const long Plus2ToThe7 = 128L;
        public const long Minus2ToThe4 = -16L;
        public const long Minus2ToThe7 = -128L;
        public const long Minus2ToThe15 = -32768L;
        public const long Minus2ToThe31 = -2147483648L;

        //https://en.wikipedia.org/wiki/Bolt_(network_protocol)

        //https://github.com/neo4j/neo4j-dotnet-driver/blob/c1f8b06efb6829b7372514f8dfc51cc22411a101/Neo4j.Driver/Neo4j.Driver/Internal/IO/SpanPackStreamReader.cs#L26
        //https://neo4j.com/docs/bolt/current/packstream/
        public static List<string> Unpack(byte[] reader)
        {
            var found = new List<string>();

            //var fieldCount = (int)ReadListHeader(reader);
            //var fields = new object[fieldCount];
            //for (var i = 0; i < fieldCount; i++)
            //{
            //    fields[i] = reader.Read();
            //}

            //return new RecordMessage(fields);

            int size;
            byte[] bytes;
            string value;

            int size1;
            int size2;
            int size3;
            int size4;

            int i = 0;

            while (i < reader.Length)
            {
                var markerByte = reader[i];

                var markerHighNibble = (byte)(markerByte & 0xF0);
                var markerLowNibble = (byte)(markerByte & 0x0F);

                if (markerHighNibble == TinyString)
                {
                    bytes = reader.Skip(i + 1).Take(markerLowNibble).ToArray();
                    value = Encoding.UTF8.GetString(bytes);

                    i += markerLowNibble;

                    found.Add(value);
                }
                else if (markerHighNibble == TinyMap)
                {
                    found.Add($"TinyMap {markerLowNibble}");
                }
                else if (markerHighNibble == TinyList)
                {
                    found.Add($"TinyList {markerLowNibble}");
                }
                else if (markerHighNibble == TinyStruct)
                {
                    i += 1;
                    found.Add($"TinyStruct {markerLowNibble}");
                }
                else
                {
                    switch (reader[i])
                    {
                        case List8:
                            i += 1;
                            size = reader[i];
                            found.Add($"List8 {size}");
                            break;
                        case List16:
                            i += 1;
                            size1 = reader[i];
                            i += 1;
                            size2 = reader[i];
                            size = size1 * 256 + size2;
                            found.Add($"List16 {size}");
                            break;
                        case List32:
                            i += 1;
                            size1 = reader[i];
                            i += 1;
                            size2 = reader[i];
                            i += 1;
                            size3 = reader[i];
                            i += 1;
                            size4 = reader[i];
                            size = size1 * 256 * 256 * 256 + size2 * 256 * 256 + size3 * 256 + size4;
                            found.Add($"List32 {size}");
                            break;

                        case Map8:
                            i += 1;
                            size = reader[i];
                            found.Add($"Map8 {size}");
                            break;
                        case Map16:
                            i += 1;
                            size1 = reader[i];
                            i += 1;
                            size2 = reader[i];
                            size = size1 * 256 + size2;
                            found.Add($"Map16 {size}");
                            break;

                        case Struct8:
                            i += 1;
                            size = reader[i];
                            i += 1;
                            found.Add($"Struct8 {size}");
                            break;
                        case Struct16:
                            i += 1;
                            size1 = reader[i];
                            i += 1;
                            size2 = reader[i];
                            size = size1 * 256 + size2;
                            found.Add($"Struct16 {size}");
                            break;

                        case String8:
                            i += 1;
                            size = reader[i];
                            i += 1;
                            bytes = reader.Skip(i).Take(size).ToArray();
                            value = Encoding.UTF8.GetString(bytes);

                            found.Add(value);

                            i += size;
                            break;

                        case String16:
                            i += 1;
                            size1 = reader[i];

                            i += 1;
                            size2 = reader[i];

                            size = size1 * 256 + size2;

                            bytes = reader.Skip(i).Take(size).ToArray();
                            value = Encoding.UTF8.GetString(bytes);

                            found.Add(value);

                            i += size;
                            break;

                        case String32:
                            i += 1;
                            size1 = reader[i];

                            i += 1;
                            size2 = reader[i];

                            i += 1;
                            size3 = reader[i];

                            i += 1;
                            size4 = reader[i];

                            size = size1 * 256 * 256 * 256 + size2 * 256 * 256 + size3 * 256 + size4;

                            bytes = reader.Skip(i).Take(size).ToArray();
                            value = Encoding.UTF8.GetString(bytes);

                            found.Add(value);
                            i += size;
                            break;

                        case Null:
                            found.Add("Null");
                            break;

                        case False:
                            found.Add("False");
                            break;

                        case True:
                            found.Add("True");
                            break;

                        case Int8:
                            i += 1;
                            value = reader[i].ToString();
                            found.Add(value);
                            break;

                        case Int16:
                            i += 1;
                            size1 = reader[i];

                            i += 1;
                            size2 = reader[i];

                            size = size1 * 256 + size2;
                            value = reader[i].ToString();
                            break;

                        case Int32:
                            i += 1;
                            size1 = reader[i];

                            i += 1;
                            size2 = reader[i];

                            i += 1;
                            size3 = reader[i];

                            i += 1;
                            size4 = reader[i];

                            size = size1 * 256 * 256 * 256 + size2 * 256 * 256 + size3 * 256 + size4;
                            value = reader[i].ToString();
                            break;

                        case Float64:
                            i += 8;
                            found.Add("Float64");
                            break;

                        case Bytes8:
                            i += 1;
                            size = reader[i];
                            found.Add("Bytes8");
                            i += size;
                            break;

                        case Bytes16:
                            i += 1;
                            size1 = reader[i];
                            i += 1;
                            size2 = reader[i];
                            size = size1 * 256 + size2;
                            found.Add("Bytes16");
                            i += size;
                            break;

                        case Bytes32:
                            i += 1;
                            size1 = reader[i];
                            i += 1;
                            size2 = reader[i];
                            i += 1;
                            size3 = reader[i];
                            i += 1;
                            size4 = reader[i];
                            size = size1 * 256 * 256 * 256 + size2 * 256 * 256 + size3 * 256 + size4;
                            found.Add("Bytes32");
                            i += size;
                            break;

                        default:
                            found.Add("Found and not interpreted " + string.Format("{0:X}", reader[i]));
                            break;
                    }
                }

                i++;
            }

            return found;
        }
    }
}
