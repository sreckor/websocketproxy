using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace org.neo4j.field.boltproxy
{
    /**
     * TODO - replace byte/hex utils with System.Hex
     * @author garymann
     */
    public class Utils
    {
        public static string ByteToHex(byte singleByte)
        {
            return ByteArrayToHex(new byte[] { singleByte });
        }

        public static string ByteArrayToHex(byte[] byteArray)
        {
            var stringBuilder = new System.Text.StringBuilder(byteArray.Length * 2);
            foreach (byte singleByte in byteArray)
            {
                stringBuilder.AppendFormat("{0:x2}", singleByte);
            }
            return stringBuilder.ToString();
        }

        public static byte[] HexToByteArray(string hexString)
        {
            int length = hexString.Length;
            if (length % 2 == 1)
            {
                throw new ArgumentException("Hex string must have even number of characters");
            }
            byte[] byteData = new byte[length / 2]; // Allocate 1 byte per 2 hex characters
            for (int i = 0; i < length; i += 2)
            {
                byteData[i / 2] = (byte)((Convert.ToInt32(hexString[i].ToString(), 16) << 4) +
                                          Convert.ToInt32(hexString[i + 1].ToString(), 16));
            }
            return byteData;
        }

        //public static List<byte[]> Tokens(byte[] array, byte[] delimiter)
        //{
        //    var byteArrays = new List<byte[]>();
        //    if (delimiter.Length == 0)
        //    {
        //        return byteArrays;
        //    }
        //    int startIndex = 0;

        //outer:
        //    for (int i = 0; i < array.Length - delimiter.Length + 1; i++)
        //    {
        //        for (int j = 0; j < delimiter.Length; j++)
        //        {
        //            if (array[i + j] != delimiter[j])
        //            {
        //                continue outer;
        //            }
        //        }
        //        byteArrays.Add(array[startIndex..i]);
        //        startIndex = i + delimiter.Length;
        //    }
        //    byteArrays.Add(array[startIndex..array.Length]);
        //    return byteArrays;
        //}

        public static List<byte[]> Messages(byte[] array)
        {
            var messageArrays = new List<byte[]>();
            int index = 0;

            while (index < array.Length)
            {
                short length = (short)(((array[index] & 0xFF) << 8) | (array[index + 1] & 0xFF));
                index += 2;
                messageArrays.Add(array.Skip(index).Take(length).ToArray());// [index..(index + length)]);
                index += length;
                index += 2; //marker
            }
            return messageArrays;
        }

        public static byte[] Prune(byte[] bytes)
        {
            if (bytes.Length == 0) return bytes;
            int lastIndex = bytes.Length - 1;
            while (bytes[lastIndex] == 0)
            {
                lastIndex--;
            }
            byte[] copy = new byte[lastIndex + 1];
            Array.Copy(bytes, copy, lastIndex + 1);
            return copy;
        }

        public static byte[] Len(short length)
        {
            return new byte[] { (byte)((length >> 8) & 0xFF), (byte)(length & 0xFF) };
        }

        public static short GetLength(byte[] array)
        {
            return (short)(((array[0] & 0xFF) << 8) | (array[1] & 0xFF));
        }

        // combine with len and eom marker
        public static byte[] Combine(List<byte[]> messages)
        {
            byte[] result = Array.Empty<byte>();
            foreach (byte[] entry in messages)
            {
                short length = (short)entry.Length;
                byte[] combined = new byte[0];
                combined = CombineArrays(combined, Len(length));
                combined = CombineArrays(combined, entry);
                combined = CombineArrays(combined, new byte[] { 0, 0 });
                result = CombineArrays(result, combined);
            }
            return result;
        }

        private static byte[] CombineArrays(byte[] array1, byte[] array2)
        {
            byte[] combinedArray = new byte[array1.Length + array2.Length];
            Array.Copy(array1, combinedArray, array1.Length);
            Array.Copy(array2, 0, combinedArray, array1.Length, array2.Length);
            return combinedArray;
        }
    }
}