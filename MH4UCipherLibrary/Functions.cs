using Simias.Encryption;
using System.IO;

namespace MH4UCipher
{
    internal static class Functions
    {
        internal static byte[] Cipher(byte[] buffer, ref byte[] key, bool decrypt)
        {
            var cipher = new Blowfish(key);
            var b = new byte[8];
            using (var m = new MemoryStream(buffer, true))
            {
                while (m.Position < m.Length)
                {
                    //Endian swap 2 sets of 4 bytes
                    for (int i = 3; i >= 0; i--)
                    {
                        b[i] = (byte)m.ReadByte();
                    }
                    for (int i = 7; i >= 4; i--)
                    {
                        b[i] = (byte)m.ReadByte();
                    }
                    //cipher the 8 bytes
                    if (decrypt) cipher.Decipher(b, 8);
                    else cipher.Encipher(b, 8);
                    //Reset stream position to prepare for writing.
                    m.Position -= 8;
                    //Endian swap 4 bytes twice
                    for (int i = 3; i >= 0; i--)
                    {
                        m.WriteByte(b[i]);
                    }
                    for (int i = 7; i >= 4; i--)
                    {
                        m.WriteByte(b[i]);
                    }
                }
            }
            return buffer;
        }
    }
}