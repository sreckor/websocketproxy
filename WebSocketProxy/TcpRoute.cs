using System;
using System.Collections.Generic;
using System.Linq;

namespace WebSocketProxy
{
    internal class TcpRoute : IDisposable
    {
        public string ClinetId { get; set; }

        private readonly TcpHost _clientMachine;
        private readonly TcpHost _serverMachine;

        #region Events

        public delegate void DisconnectedDelegate(TcpRoute route);

        public event DisconnectedDelegate Disconnected;

        protected void OnDisconnected()
        {
            if (Disconnected != null)
            {
                Disconnected(this);
            }
        }

        #endregion

        public bool Connected
        {
            get { return _clientMachine.Connected && _serverMachine.Connected; }
        }

        public TcpRoute(TcpHost clientMachine, TcpHost serverMachine)
        {
            _clientMachine = clientMachine;
            _serverMachine = serverMachine;
        }

        public void Start()
        {
            RegisterHostEvents();
            _clientMachine.StartReading();
            _serverMachine.StartReading();
        }

        public string DecodeBoltProtocol(byte[] data)
        {
            if (data == null || data.Length < 4)
            {
                return "Invalid data";
            }

            var messageType = data[3];
            string message = messageType switch
            {
                0x0f => "Reset",
                0x10 => "Run",
                0x2f => "Discard",
                0x3f => "Pull",
                0x71 => "Record",
                0x70 => "Success",
                0x7e => "Ignore",
                0x7f => "Failure",
                0x01 => "Hello",
                0x11 => "Begin",
                0x02 => "Goodbye",
                0x12 => "Commit",
                0x13 => "Rollback",
                0x54 => "Telemetry",
                _ => $"Unknown message type: {messageType}"
            };

            return message;
        }

        public static byte[] DecodeMessage(Byte[] bytes)
        {
            //string incomingData = string.Empty;
            byte secondByte = bytes[1];
            int dataLength = secondByte & 127;
            int indexFirstMask = 2;

            var mask = (secondByte & 0x80) == 0x80 ? true : false;

            if (dataLength == 126)
                indexFirstMask = 4;
            else if (dataLength == 127)
                indexFirstMask = 10;

            IEnumerable<byte> keys = bytes.Skip(indexFirstMask).Take(4);
            int indexFirstDataByte = indexFirstMask + 4;

            byte[] decoded = new byte[bytes.Length - indexFirstDataByte];

            for (int i = indexFirstDataByte, j = 0; i < bytes.Length; i++, j++)
            {
                if (mask)
                {
                    decoded[j] = (byte)(bytes[i] ^ keys.ElementAt(j % 4));
                }
                else
                {
                    decoded[j] = bytes[i];
                }
            }

            return decoded;
        }


        void _serverMachine_DataAvailable(TcpHost host, byte[] data, int length)
        {
            //_clientMachine.

            _clientMachine.Send(data, length);

            //var payload = DecodeMessage(data.Take(length).ToArray());
            //Console.WriteLine(DecodeBoltProtocol(payload));

            // Decoder.Deserialize(payload);

            // Console.WriteLine(string.Join(",", Decoder.Deserialize(payload)));
        }

        void _clientMachine_DataAvailable(TcpHost host, byte[] data, int length)
        {
            _serverMachine.Send(data, length);

            var payload = DecodeMessage(data.Take(length).ToArray());

            Console.WriteLine(DecodeBoltProtocol(payload));

            Console.WriteLine($"socket id: {ClinetId}");

            Console.WriteLine(string.Join(" | ", Decoder.Deserialize(payload)));
        }

        void _serverMachine_Disconnected(TcpHost host)
        {
            Stop();
        }

        void _clientMachine_Disconnected(TcpHost host)
        {
            Stop();
        }

        private void RegisterHostEvents()
        {
            _clientMachine.Disconnected += _clientMachine_Disconnected;
            _serverMachine.Disconnected += _serverMachine_Disconnected;

            _clientMachine.DataAvailable += _clientMachine_DataAvailable;
            _serverMachine.DataAvailable += _serverMachine_DataAvailable;
        }


        public void Stop()
        {
            _clientMachine.Close();
            _serverMachine.Close();

            OnDisconnected();
        }


        public void Dispose()
        {
            Stop();
        }
    }
}