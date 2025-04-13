public class WebSocketFrameDecoder
{
    public string GetSessionFromHandshake(HttpRequest request)
    {
        // Example: Extract session ID from a cookie
        if (request.Headers.TryGetValue("Cookie", out var cookies))
        {
            var sessionCookie = cookies.Split(';')
                .FirstOrDefault(c => c.Trim().StartsWith("session_id="));
            return sessionCookie?.Split('=')[1];
        }

        // Example: Extract session token from Authorization header
        if (request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            return authHeader.Replace("Bearer ", "").Trim();
        }

        return null; // No session info found
    }



    public class DecodedFrame
    {
        public string Opcode { get; set; }
        public bool IsMasked { get; set; }
        public int PayloadLength { get; set; }
        public byte[] PayloadData { get; set; }
    }

    public static DecodedFrame DecodeFrame(byte[] frame)
    {
        if (frame == null || frame.Length < 2)
        {
            throw new ArgumentException("Invalid WebSocket frame.");
        }

        // Extract the opcode (lower 4 bits of the first byte)
        byte opcodeByte = (byte)(frame[0] & 0x0F);
        string opcode = opcodeByte switch
        {
            0x1 => "Text",
            0x2 => "Binary",
            0x8 => "Close",
            0x9 => "Ping",
            0xA => "Pong",
            _ => "Unknown"
        };

        // Check if the frame is masked (most significant bit of the second byte)
        bool isMasked = (frame[1] & 0x80) != 0;

        // Extract the payload length (lower 7 bits of the second byte)
        int payloadLength = frame[1] & 0x7F;
        int offset = 2;

        if (payloadLength == 126)
        {
            // Extended payload length (16 bits)
            payloadLength = (frame[2] << 8) | frame[3];
            offset += 2;
        }
        else if (payloadLength == 127)
        {
            // Extended payload length (64 bits)
            payloadLength = (int)((frame[2] << 56) | (frame[3] << 48) | (frame[4] << 40) | (frame[5] << 32) |
                                  (frame[6] << 24) | (frame[7] << 16) | (frame[8] << 8) | frame[9]);
            offset += 8;
        }

        // Extract the masking key if present
        byte[] maskingKey = null;
        if (isMasked)
        {
            maskingKey = frame.Skip(offset).Take(4).ToArray();
            offset += 4;
        }

        // Extract the payload data
        byte[] payloadData = frame.Skip(offset).Take(payloadLength).ToArray();

        // Unmask the payload data if masked
        if (isMasked && maskingKey != null)
        {
            for (int i = 0; i < payloadData.Length; i++)
            {
                payloadData[i] = (byte)(payloadData[i] ^ maskingKey[i % 4]);
            }
        }

        return new DecodedFrame
        {
            Opcode = opcode,
            IsMasked = isMasked,
            PayloadLength = payloadLength,
            PayloadData = payloadData
        };
    }
}
