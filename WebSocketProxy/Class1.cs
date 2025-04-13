//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Net;
//using System.Net.Http;
//using System.Threading.Tasks;
//using org.neo4j.field.boltproxy.backend;
//using io.javalin.websocket;
//using io.netty.buffer;
//using org.apache.commons.configuration;
//using org.slf4j;
//using org.neo4j.bolt.message;
//using org.neo4j.bolt.unpacker;
//using org.neo4j.bolt.value;
//using org.neo4j.bolt.json;
//using org.neo4j.bolt.logger;
//using org.neo4j.driver;
//using static org.neo4j.driver.Values;
//using org.neo4j.driver.internal.messaging.common;
//using org.neo4j.driver.internal.messaging.encode;
//using org.neo4j.driver.internal.messaging.request;
//using org.neo4j.driver.internal.packstream;
//using org.neo4j.driver.internal.util.io;

//using Neo4j.Driver;
using System;
using System.Collections.Generic;

namespace org.neo4j.field.boltproxy
{
    public class BoltWsMessageHandler
    {
        //private static Logger logger = LoggerFactory.GetLogger(typeof(BoltWsMessageHandler));
        //private WsBinaryMessageContext binaryMessageContext = null;
        //private WsConnectContext connectContext = null;
        //private BoltWsBackend backendProxy = null;
        //private Configuration configurations = null;

        //public BoltWsMessageHandler(WsBinaryMessageContext binaryContext)
        //{
        //    binaryMessageContext = binaryContext;
        //}

        //public BoltWsMessageHandler(WsConnectContext connectionContext, Configuration config)
        //{
        //    connectContext = connectionContext;
        //    try
        //    {
        //        configurations = config;
        //        logger.Debug("Creating new backend ws connection");
        //        backendProxy = new BoltWsBackend(new Uri(configurations.GetString("boltproxy.backend.bolt", "ws://localhost:7687")));
        //        backendProxy.SetHandler(this);
        //        bool connectionSuccessful = backendProxy.ConnectBlocking();
        //        logger.Info("Connected to backend: " + connectionSuccessful);
        //    }
        //    catch (UriFormatException e)
        //    {
        //        logger.Error("Exception on URI! ", e);
        //    }
        //    catch (InterruptedException ie)
        //    {
        //        logger.Error("Interrupted while connecting! ", ie);
        //    }
        //}

        //public void SetConfigurations(Configuration config)
        //{
        //    configurations = config;
        //}

        //public void SetBinaryContext(WsBinaryMessageContext binaryContext)
        //{
        //    binaryMessageContext = binaryContext;
        //}

        //public void SendMessage(byte[] messageBytes)
        //{
        //    logger.Info("Preparing message to backend: " + Utils.ByteArrayToHex(messageBytes));
        //    byte[] inspectedMessage = InspectMessage(messageBytes);
        //    if (inspectedMessage != null)
        //    {
        //        logger.Debug("Sending updated message to backend: " + Utils.ByteArrayToHex(inspectedMessage));
        //        backendProxy.Send(inspectedMessage);
        //    }
        //    else
        //    {
        //        backendProxy.Send(messageBytes);
        //    }
        //}

        //public void ProcessResponse(ByteBuffer responseBuffer)
        //{
        //    logger.Info("Preparing message to client: " + Utils.ByteArrayToHex(responseBuffer.Array));
        //    InspectMessage(responseBuffer.Array);
        //    connectContext.Send(responseBuffer);
        //}

        ///**
        // * Add some params to a Login message
        // */
        //private byte[] UpdateAuth(Message message)
        //{
        //    logger.Info("Updating auth with metadata: " + message.GetName());
        //    // Injecting these parameters into the Auth token.
        //    var parameters = new Dictionary<string, string> { { "param1", "value1" }, { "param2", "value2" } };
        //    var metadata = ((org.neo4j.bolt.message.LogonMessage)message).GetMetadata();

        //    metadata["parameters"] = Value(parameters);
        //    var logonMessage = new LogonMessage(metadata);
        //    var buffer = Unpooled.Buffer();
        //    PackOutput output = new ByteBufOutput(buffer);
        //    CommonValuePacker valuePacker = new CommonValuePacker(output, true);
        //    LogonMessageEncoder messageEncoder = new LogonMessageEncoder();
        //    try
        //    {
        //        messageEncoder.Encode(logonMessage, valuePacker);
        //        byte[] packedMessage = Utils.Prune(buffer.Array);
        //        logger.Debug("New login::" + Utils.ByteArrayToHex(packedMessage));
        //        return packedMessage;
        //    }
        //    catch (IOException ie)
        //    {
        //        logger.Error("Error encoding new logon message");
        //    }
        //    return new byte[1];
        //}

        public byte[] InspectMessage(byte[] messageBytes)
        {
            bool hasUpdatedMessage = false;
            if ((messageBytes[0] == 0x60 && messageBytes[1] == 0x60 && messageBytes[2] == (byte)0xb0 && messageBytes[3] == 0x17) || (messageBytes[0] == 0x00 && messageBytes[1] == 0x00))
            {
                //logger.Info("Skip inspection of protocol negotiation frames");
            }
            else
            {
                //logger.Debug("New Inspection::" + Utils.ByteArrayToHex(messageBytes));
                List<byte[]> byteMessages = Utils.Messages(messageBytes);
                for (int i = 0; i < byteMessages.Count; i++)
                {
                    try
                    {
                        //logger.Debug("Message to process::" + Utils.ByteArrayToHex(byteMessages[i]));
                        
                        
                        //ByteBuffer buffer = ByteBuffer.Wrap(byteMessages[i]);
                        //ValueUnpacker valueUnpacker = new DriverValueUnpacker(buffer);
                        //Message unpackedMessage = MessageUnpacker.Unpack(valueUnpacker);
                        
                        
                        
                        //LogRecord logRecord = new LogRecord(unpackedMessage, connectContext.Session.GetRemoteAddress(), connectContext.Session.GetLocalAddress());
                        //string serializedRecord = Serializer.ToJson(logRecord);
                        //logger.Info("Message Processed::" + serializedRecord);
                        //if (unpackedMessage.GetName().Equals("LOGON"))
                        //{
                        //    //logger.Info("Augmenting login with metadata");
                        //    byte[] updatedMessage = UpdateAuth(unpackedMessage);
                        //    byteMessages[i] = updatedMessage;
                        //    hasUpdatedMessage = true;
                        //}
                    }
                    catch (Exception e)
                    {
                        //logger.Error("Error parsing bolt message: ", e);
                    }
                }
                if (hasUpdatedMessage)
                {
                    return Utils.Combine(byteMessages);
                }
            }
            return null;
        }
    }
}