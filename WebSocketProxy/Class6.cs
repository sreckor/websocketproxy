using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace WebSocketProxy
{

    //internal sealed class BoltProtocolVersion : IEquatable<BoltProtocolVersion>, IComparable<BoltProtocolVersion>
    //{
    //    // The int 1213486160 is 0x‭48 54 54 50 - or HTTP in ascii codes... determines the reserved version. It should never be sent by the server
    //    private const int MaxMajorVersion = 255;
    //    private const int MaxMinorVersion = 255;
    //    public const int ManifestSchema = 255;
    //    public const int ManifestVersion = 1;

    //    private const int PackingIntValue = 0x00FF;

    //    public static readonly BoltProtocolVersion Unknown = new(1, 0);

    //    // ReSharper disable InconsistentNaming
    //    public static readonly BoltProtocolVersion V3_0 = new(3, 0);
    //    public static readonly BoltProtocolVersion V4_0 = new(4, 0);
    //    public static readonly BoltProtocolVersion V4_1 = new(4, 1);
    //    public static readonly BoltProtocolVersion V4_2 = new(4, 2);
    //    public static readonly BoltProtocolVersion V4_3 = new(4, 3);
    //    public static readonly BoltProtocolVersion V4_4 = new(4, 4);
    //    public static readonly BoltProtocolVersion V5_0 = new(5, 0);
    //    public static readonly BoltProtocolVersion V5_1 = new(5, 1);
    //    public static readonly BoltProtocolVersion V5_2 = new(5, 2);
    //    public static readonly BoltProtocolVersion V5_3 = new(5, 3);
    //    public static readonly BoltProtocolVersion V5_4 = new(5, 4);
    //    public static readonly BoltProtocolVersion V5_5 = new(5, 5);
    //    public static readonly BoltProtocolVersion V5_6 = new(5, 6);
    //    public static readonly BoltProtocolVersion V5_7 = new(5, 7);
    //    public static readonly BoltProtocolVersion V5_8 = new(5, 8);

    //    public static readonly BoltProtocolVersion LatestVersion = V5_8;
    //    public static readonly BoltProtocolVersion HandshakeManifestV1 = new(ManifestSchema, ManifestVersion);
    //    // ReSharper restore InconsistentNaming

    //    private readonly int _compValue;

    //    private bool IsVersionValid(int majorVersion, int minorVersion)
    //    {
    //        return (majorVersion is <= MaxMajorVersion and >= 0 && minorVersion is <= MaxMinorVersion and >= 0);
    //    }

    //    public BoltProtocolVersion(int majorVersion, int minorVersion)
    //    {
    //        if (!IsVersionValid(majorVersion, minorVersion))
    //        {
    //            throw new NotSupportedException($"Attempting to create a BoltProtocolVersion with out of bounds major: {majorVersion} or minor: {minorVersion}");
    //        }

    //        MajorVersion = majorVersion;
    //        MinorVersion = minorVersion;
    //        _compValue = MajorVersion * 1000000 + MinorVersion;
    //    }

    //    public BoltProtocolVersion(int largeVersion)
    //    {
    //        //This version of the constructor is only to be used to handle error codes that come in that are not strictly containing packed values. 
    //        MajorVersion = UnpackMajor(largeVersion);
    //        MinorVersion = UnpackMinor(largeVersion);
    //        _compValue = MajorVersion * 1000000 + MinorVersion;

    //        if (!IsVersionValid(MajorVersion, MinorVersion))
    //        {
    //            throw new NotSupportedException(
    //                "Attempting to create a BoltProtocolVersion with a large (error code) version number.  " +
    //                "Resulting Major and Minor are in range of valid versions, which is not allowed: " +
    //                MajorVersion +
    //                " or minor: " +
    //                MinorVersion);
    //        }
    //    }

    //    public int MajorVersion { get; }
    //    public int MinorVersion { get; }

    //    public bool Equals(BoltProtocolVersion rhs)
    //    {
    //        if (ReferenceEquals(null, rhs))
    //        {
    //            return false;
    //        }

    //        if (ReferenceEquals(this, rhs))
    //        {
    //            return true;
    //        }

    //        return _compValue == rhs._compValue;
    //    }

    //    public int CompareTo(BoltProtocolVersion other)
    //    {
    //        // If other is not a valid object reference, this instance is greater so return 1.
    //        // If it is a valid reference then proceed to do the comparison. Implementation needed for IComparable
    //        return other == null ? 1 : _compValue.CompareTo(other._compValue);
    //    }

    //    private static int UnpackMajor(int rawVersion)
    //    {
    //        return rawVersion & PackingIntValue;
    //    }

    //    private static int UnpackMinor(int rawVersion)
    //    {
    //        return (rawVersion >> 8) & PackingIntValue;
    //    }

    //    public static BoltProtocolVersion FromPackedInt(int rawVersion)
    //    {
    //        return new BoltProtocolVersion(UnpackMajor(rawVersion), UnpackMinor(rawVersion));
    //    }

    //    public static int RangeFromPackedInt(int rawVersion)
    //    {
    //        var shiftedRawVersion = rawVersion >> 16;
    //        return UnpackMajor(shiftedRawVersion);
    //    }

    //    public void CheckVersionRange(BoltProtocolVersion minVersion)
    //    {
    //        if (MajorVersion != minVersion.MajorVersion)
    //        {
    //            throw new NotSupportedException("Versions should be from same major version");
    //        }

    //        if (MinorVersion < minVersion.MinorVersion)
    //        {
    //            throw new NotSupportedException("Max version should be newer than minimum version");
    //        }
    //    }

    //    public int PackToIntRange(BoltProtocolVersion minVersion)
    //    {
    //        CheckVersionRange(minVersion);

    //        var range = MinorVersion - minVersion.MinorVersion;
    //        return (range << 16) | PackToInt();
    //    }

    //    public int PackToInt()
    //    {
    //        return (MinorVersion << 8) | MajorVersion;
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        return ReferenceEquals(this, obj) || (obj is BoltProtocolVersion other && Equals(other));
    //    }

    //    public bool Equals(int majorVersion, int minorVersion)
    //    {
    //        var tempVersion = new BoltProtocolVersion(majorVersion, minorVersion);
    //        return Equals(tempVersion);
    //    }

    //    public static bool operator ==(BoltProtocolVersion lhs, BoltProtocolVersion rhs)
    //    {
    //        return lhs?._compValue == rhs?._compValue;
    //    }

    //    public static bool operator !=(BoltProtocolVersion lhs, BoltProtocolVersion rhs)
    //    {
    //        return lhs._compValue != rhs._compValue;
    //    }

    //    public static bool operator >=(BoltProtocolVersion lhs, BoltProtocolVersion rhs)
    //    {
    //        return lhs._compValue >= rhs._compValue;
    //    }

    //    public static bool operator <=(BoltProtocolVersion lhs, BoltProtocolVersion rhs)
    //    {
    //        return lhs._compValue <= rhs._compValue;
    //    }

    //    public static bool operator >(BoltProtocolVersion lhs, BoltProtocolVersion rhs)
    //    {
    //        return lhs._compValue > rhs._compValue;
    //    }

    //    public static bool operator <(BoltProtocolVersion lhs, BoltProtocolVersion rhs)
    //    {
    //        return lhs._compValue < rhs._compValue;
    //    }

    //    public override int GetHashCode()
    //    {
    //        return _compValue;
    //    }

    //    public override string ToString()
    //    {
    //        return $"{MajorVersion}.{MinorVersion}";
    //    }
    //}
    //internal sealed class FailureMessage : IResponseMessage
    //{
    //    public FailureMessage()
    //    {
    //    }

    //    public FailureMessage(string code, string message)
    //    {
    //        Code = code;
    //        Message = message;
    //    }

    //    /// <summary>Code is the Neo4j-specific error code, to be deprecated in favor of GqlStatus.</summary>
    //    public string Code { get; set; }

    //    /// <summary>The specific error message describing the failure.</summary>
    //    public string Message { get; set; }

    //    /// <summary>Returns the GQLSTATUS.</summary>
    //    public string GqlStatus { get; set; }

    //    /// <summary>Provides a standard description for the associated GQLStatus code.</summary>
    //    public string GqlStatusDescription { get; set; }

    //    /// <summary>A high-level categorization of the error, specific to GQL error handling.</summary>
    //    public string GqlClassification { get; set; }

    //    /// <summary>The raw classification as received from the server.</summary>
    //    public string GqlRawClassification { get; set; }

    //    /// <summary>
    //    /// GqlDiagnosticRecord returns further information about the status for diagnostic purposes. GqlDiagnosticRecord
    //    /// is part of the GQL compliant errors preview feature.
    //    /// </summary>
    //    public Dictionary<string, object> GqlDiagnosticRecord { get; set; }

    //    /// <summary>
    //    /// GqlCause represents the underlying error, if any, which caused the current error. GqlCause is part of the GQL
    //    /// compliant errors preview feature (see README on what it means in terms of support and compatibility guarantees)
    //    /// </summary>
    //    public FailureMessage GqlCause { get; set; }

    //    public void Dispatch(IResponsePipeline pipeline)
    //    {
    //        pipeline.OnFailure(this);
    //    }

    //    public IPackStreamSerializer Serializer => FailureMessageSerializer.Instance;

    //    public override string ToString()
    //    {
    //        return $"FAILURE code={Code}, message={Message}";
    //    }
    //}

    //[DataContract]
    //public class Neo4jException : Exception, IGqlErrorPreview
    //{
    //    private readonly string _gqlClassification;
    //    private readonly Dictionary<string, object> _gqlDiagnosticRecord;
    //    private readonly string _gqlRawClassification;
    //    private readonly string _gqlStatus;
    //    private readonly string _gqlStatusDescription;

    //    /// <summary>Create a new <see cref="Neo4jException"/></summary>
    //    public Neo4jException()
    //    {
    //    }

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="Neo4jException"/> class using the specified
    //    /// <see cref="FailureMessage"/>.
    //    /// </summary>
    //    /// <param name="failureMessage">The failure message containing error details.</param>
    //    /// <param name="innerException">The inner exception.</param>
    //    internal Neo4jException(FailureMessage failureMessage, Exception innerException = null)
    //        : base(failureMessage.Message, innerException)
    //    {
    //        Code = failureMessage.Code;
    //        _gqlStatus = failureMessage.GqlStatus;
    //        _gqlStatusDescription = failureMessage.GqlStatusDescription;
    //        _gqlClassification = failureMessage.GqlClassification;
    //        _gqlRawClassification = failureMessage.GqlRawClassification;
    //        _gqlDiagnosticRecord = failureMessage.GqlDiagnosticRecord;
    //    }

    //    /// <summary>Create a new <see cref="Neo4jException"/> with an error message</summary>
    //    /// <param name="message">The error message.</param>
    //    public Neo4jException(string message) : this(null, message)
    //    {
    //    }

    //    /// <summary>Create a new <see cref="Neo4jException"/> with an error code and an error message</summary>
    //    /// <param name="code">The error code.</param>
    //    /// <param name="message">The error message</param>
    //    public Neo4jException(string code, string message)
    //        : base(message)
    //    {
    //        Code = code;
    //    }

    //    /// <summary>Create a new <see cref="Neo4jException"/> with an error message and an exception.</summary>
    //    /// <param name="message">The error message.</param>
    //    /// <param name="innerException">The inner exception</param>
    //    public Neo4jException(string message, Exception innerException)
    //        : this(null, message, innerException)
    //    {
    //    }

    //    /// <summary>Create a new <see cref="Neo4jException"/> with an error code, an error message and an exception.</summary>
    //    /// <param name="code">The error code.</param>
    //    /// <param name="message">The error message.</param>
    //    /// <param name="innerException">The inner exception.</param>
    //    public Neo4jException(string code, string message, Exception innerException)
    //        : base(message, innerException)
    //    {
    //        Code = code;
    //    }

    //    /// <summary>Gets whether the exception retriable or not.</summary>
    //    public virtual bool IsRetriable => false;

    //    /// <summary>Gets or sets the code of a Neo4j exception.</summary>
    //    public string Code { get; set; }

    //    /// <inheritdoc/>
    //    string IGqlErrorPreview.GqlStatus => _gqlStatus;

    //    /// <inheritdoc/>
    //    string IGqlErrorPreview.GqlStatusDescription => _gqlStatusDescription;

    //    /// <inheritdoc/>
    //    string IGqlErrorPreview.GqlClassification => _gqlClassification;

    //    /// <inheritdoc/>
    //    string IGqlErrorPreview.GqlRawClassification => _gqlRawClassification;

    //    /// <inheritdoc/>
    //    Dictionary<string, object> IGqlErrorPreview.GqlDiagnosticRecord => _gqlDiagnosticRecord;

    //    internal static Neo4jException Create(FailureMessage failureMessage)
    //    {
    //        Exception innerException = null;
    //        if (failureMessage.GqlCause is not null)
    //        {
    //            innerException = Create(failureMessage.GqlCause);
    //        }

    //        return new Neo4jException(failureMessage, innerException);
    //    }
    //}

    //internal interface IResponsePipeline
    //{
    //    bool HasNoPendingMessages { get; }
    //    bool IsHealthy(out Exception error);
    //    void Enqueue(IResponseHandler handler);
    //    void OnSuccess(IDictionary<string, object> metadata);
    //    void OnRecord(object[] fieldValues);
    //    void OnFailure(FailureMessage failureMessage);
    //    void OnIgnored();
    //    void AssertNoFailure();
    //    void AssertNoProtocolViolation();
    //}

    //internal sealed class RecordMessage : IResponseMessage
    //{
    //    public RecordMessage(object[] fields)
    //    {
    //        Fields = fields;
    //    }

    //    public object[] Fields { get; }

    //    public void Dispatch(IResponsePipeline pipeline)
    //    {
    //        pipeline.OnRecord(Fields);
    //    }

    //    public IPackStreamSerializer Serializer => RecordMessageSerializer.Instance;

    //    public override string ToString()
    //    {
    //        return $"RECORD {Fields.ToContentString()}";
    //    }
    //}

    //public class ProtocolException : Neo4jException
    //{
    //    /// <summary>Create a new <see cref="ProtocolException"/> with an error message.</summary>
    //    /// <param name="message">The error message.</param>
    //    public ProtocolException(string message) : base(message)
    //    {
    //    }

    //    internal ProtocolException(FailureMessage failureMessage, Exception innerException)
    //        : base(failureMessage, innerException)
    //    {
    //    }
    //}

    public class Decoder
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


        //public string GetRecord(int position, byte[] reader, out int positionIncrement)
        //{
        //    var headerByte = reader[position];

        //    positionIncrement = 1;

        //    switch (reader[i])
        //    {
        //        case String8:

        //            i += 1;
        //            var size = reader[i];

        //            i += 1;
        //            var bytes = reader.Skip(i).Take(size).ToArray();
        //            var value = Encoding.UTF8.GetString(bytes);

        //            found.Add(value);

        //            i += size;
        //            break;

        //        case String16:

        //            i += 1;
        //            var size1 = reader[i];

        //            i += 1;
        //            var size2 = reader[i];

        //            var sizex = size1 * 256 + size2;


        //            var bytes1 = reader.Skip(i).Take(sizex).ToArray();
        //            var value1 = Encoding.UTF8.GetString(bytes1);

        //            found.Add(value1);

        //            i += sizex;
        //            break;

        //        case String32:
        //            found.Add("String32");
        //            i += 4;
        //            break;

        //        case TinyString:
        //            found.Add("TinyString");
        //            break;

        //        case Null:
        //            found.Add("Null");
        //            break;

        //        case False:
        //            found.Add("False");
        //            break;

        //        case True:
        //            found.Add("True");
        //            break;

        //        case Int8:
        //            found.Add("Int8");
        //            i += 1;
        //            break;

        //        case Int16:
        //            found.Add("Int16");
        //            i += 2;
        //            break;

        //        case Int32:
        //            found.Add("Int32");
        //            i += 4;
        //            break;
        //    }
        //}

        public static List<string> Deserialize(byte[] reader)
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

            for (int i = 0; i < reader.Length; i++)
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
                else
                {
                    switch (reader[i])
                    {
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
                    }
                }
            }

            return found;
        }

        //public long ReadListHeader(byte[] reader)
        //{
        //    var markerByte = reader[0];//Stream.ReadByte();
        //    var markerHighNibble = (byte)(markerByte & 0xF0);
        //    var markerLowNibble = (byte)(markerByte & 0x0F);

        //    if (markerHighNibble == PackStream.TinyList)
        //    {
        //        return markerLowNibble;
        //    }

        //    switch (markerByte)
        //    {
        //        case List8:
        //            return ReadUint8();

        //        case List16:
        //            return ReadUint16();

        //        case List32:
        //            return ReadUint32();

        //        default:
        //            throw new ProtocolException($"Expected a list, but got: 0x{markerByte & 0xFF:X2}");
        //    }
        //}

        //private int ReadUint8()
        //{
        //    return NextByte() & 0xFF;
        //}

        //private int ReadUint16()
        //{
        //    return NextShort() & 0xFFFF;
        //}

        //private long ReadUint32()
        //{
        //    return NextInt() & 0xFFFFFFFFL;
        //}
    }
}
