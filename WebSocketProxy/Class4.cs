﻿//using System.Collections.Generic;
//using System;

//internal sealed class MessageFormat
//{
//    // Message Constants Inherited Over Older Versions
//    public const byte MsgReset = 0x0F;
//    public const byte MsgRun = 0x10;

//    public const byte MsgDiscard = 0x2F;
//    public const byte MsgPull = 0x3F;

//    public const byte MsgRecord = 0x71;
//    public const byte MsgSuccess = 0x70;
//    public const byte MsgIgnored = 0x7E;
//    public const byte MsgFailure = 0x7F;

//    // Message Constants

//    public const byte MsgHello = 0x01;
//    public const byte MsgGoodbye = 0x02;
//    public const byte MsgBegin = 0x11;
//    public const byte MsgCommit = 0x12;
//    public const byte MsgRollback = 0x13;

//    //4.3+
//    public const byte MsgRoute = 0x66;

//    //5.1+
//    public const byte MsgLogon = 0x6A;
//    public const byte MsgLogoff = 0x6B;

//    // v5.4+
//    public const byte MsgTelemetry = 0x54;
//    private readonly Dictionary<byte, IPackStreamMessageDeserializer> _messageReaders = new();

//    private readonly Dictionary<byte, IPackStreamSerializer> _readerStructHandlers = new();
//    private readonly Dictionary<Type, IPackStreamSerializer> _writerStructHandlers = new();

//    internal MessageFormat(BoltProtocolVersion version, DriverContext context)
//    {
//        Version = version;
//        // Response Message Types
//        if (context.Config.MessageReaderConfig.DisablePipelinedMessageReader)
//        {
//            AddHandler(FailureMessageSerializer.Instance);
//            AddHandler(IgnoredMessageSerializer.Instance);
//            AddHandler(RecordMessageSerializer.Instance);
//            AddHandler(SuccessMessageSerializer.Instance);
//        }
//        else
//        {
//            AddMessageHandler(FailureMessageSerializer.Instance);
//            AddMessageHandler(IgnoredMessageSerializer.Instance);
//            AddMessageHandler(RecordMessageSerializer.Instance);
//            AddMessageHandler(SuccessMessageSerializer.Instance);
//        }

//        // Add V2 Spatial Types
//        AddHandler(PointSerializer.Instance);

//        // Add V2 Temporal Types
//        AddHandler(LocalDateSerializer.Instance);
//        AddHandler(LocalTimeSerializer.Instance);
//        AddHandler(LocalDateTimeSerializer.Instance);
//        AddHandler(OffsetTimeSerializer.Instance);

//        AddHandler(DurationSerializer.Instance);

//        // Add BCL Handlers
//        AddHandler(SystemDateTimeSerializer.Instance);
//        AddHandler(SystemDateTimeOffsetSerializer.Instance);
//        AddHandler(SystemTimeSpanSerializer.Instance);

//        AddHandler(PathSerializer.Instance);
//        // Struct Data Types
//        if (Version < BoltProtocolVersion.V5_0)
//        {
//            AddHandler(ZonedDateTimeSerializer.Instance);

//            AddHandler(NodeSerializer.Instance);
//            AddHandler(RelationshipSerializer.Instance);
//            AddHandler(UnboundRelationshipSerializer.Instance);
//        }
//        else
//        {
//            AddHandler(UtcZonedDateTimeSerializer.Instance);

//            AddHandler(ElementNodeSerializer.Instance);
//            AddHandler(ElementRelationshipSerializer.Instance);
//            AddHandler(ElementUnboundRelationshipSerializer.Instance);
//        }
//    }

//    // Test code.
//    internal MessageFormat(IEnumerable<IPackStreamSerializer> serializers = null)
//    {
//        foreach (var packStreamSerializer in serializers)
//        {
//            AddHandler(packStreamSerializer);
//        }
//    }

//    // Test code
//    internal MessageFormat(
//        IReadOnlyDictionary<Type, IPackStreamSerializer> writeHandlers = null,
//        IReadOnlyDictionary<byte, IPackStreamSerializer> readHandlers = null)
//    {
//        if (writeHandlers != null)
//        {
//            _writerStructHandlers = writeHandlers.ToDictionary(x => x.Key, x => x.Value);
//        }

//        if (readHandlers != null)
//        {
//            _readerStructHandlers = readHandlers.ToDictionary(x => x.Key, x => x.Value);
//        }
//    }

//    public IReadOnlyDictionary<byte, IPackStreamSerializer> ReaderStructHandlers => _readerStructHandlers;
//    public IReadOnlyDictionary<Type, IPackStreamSerializer> WriteStructHandlers => _writerStructHandlers;

//    public BoltProtocolVersion Version { get; }
//    public IReadOnlyDictionary<byte, IPackStreamMessageDeserializer> MessageReaders => _messageReaders;

//    private void AddMessageHandler<T>(T instance) where T : class, IPackStreamMessageDeserializer, IPackStreamSerializer
//    {
//        _messageReaders.Add(instance.ReadableStructs[0], instance);
//    }

//    private void AddHandler<T>(T instance) where T : class, IPackStreamSerializer
//    {
//        foreach (var readableStruct in instance.ReadableStructs)
//        {
//            _readerStructHandlers.Add(readableStruct, instance);
//        }

//        foreach (var writableType in instance.WritableTypes)
//        {
//            _writerStructHandlers.Add(writableType, instance);
//        }
//    }

//    public void UseUtcEncoder()
//    {
//        if (Version > BoltProtocolVersion.V4_4 || Version < BoltProtocolVersion.V4_3)
//        {
//            return;
//        }

//        RemoveHandler(ZonedDateTimeSerializer.Instance);
//        AddHandler(UtcZonedDateTimeSerializer.Instance);
//    }

//    private void RemoveHandler(ZonedDateTimeSerializer instance)
//    {
//        foreach (var b in instance.ReadableStructs)
//        {
//            _readerStructHandlers.Remove(b);
//        }

//        foreach (var type in instance.WritableTypes)
//        {
//            _writerStructHandlers.Remove(type);
//        }
//    }
//}

//internal interface IPackStreamMessageDeserializer
//{
//    IResponseMessage DeserializeMessage(BoltProtocolVersion formatVersion, SpanPackStreamReader packStreamReader);
//}