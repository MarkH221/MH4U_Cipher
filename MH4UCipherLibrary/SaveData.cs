using Simias.Encryption;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace MH4UCipher
{
    public class SaveData
    {
        public readonly bool RegionSupported = true;
        private static Blowfish _cipher;
        private byte[] _key = new UTF8Encoding().GetBytes("blowfish key iorajegqmrna4itjeangmb agmwgtobjteowhv9mope");

        private byte[] XOR(byte[] buffer, ushort key)
        {
            using (var m = new MemoryStream(buffer, true))
            {
                while (m.Position < buffer.Length)
                {
                    //Get short
                    var b = new byte[2];
                    m.Read(b, 0, 2);
                    ushort s = (BitConverter.ToUInt16(b, 0));
                    //Set key
                    if (key == 0) key = 1;
                    key = (ushort)((key * 0xb0) % 0xff53);
                    //XOR
                    s ^= key;
                    //Write back
                    m.Position -= 2;
                    b = BitConverter.GetBytes(s);
                    m.Write(b, 0, 2);
                }
                return buffer;
            }
        }

        private byte[] Cipher(byte[] buffer, bool decrypt)
        {
            _cipher = new Blowfish(_key);
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
                    if (decrypt) _cipher.Decipher(b, 8);
                    else _cipher.Encipher(b, 8);
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

        public byte[] Decrypt(byte[] buffer)
        {
            _cipher = new Blowfish(_key);
            var b = new byte[8];
            ushort seed = 0;
            buffer = Functions.Cipher(buffer, ref _key, true);
            using (var m = new MemoryStream(buffer, true))
            {
                //Get the XOR seed
                m.Position = 0;
                var b2 = new byte[4];
                m.Read(b2, 0, 4);
                var k = BitConverter.ToUInt32(b2, 0);
                seed = (ushort)(k >> 16);
            }
            //XOR the data using the seed
            buffer = XOR(buffer.Skip(4).ToArray(), seed);
            //Get expected checksum
            UInt32 csum = BitConverter.ToUInt32(buffer, 0);
            //Strip checksum
            byte[] end = new byte[buffer.Length - 4];
            Buffer.BlockCopy(buffer, 4, end, 0, end.Length);
            //Sum the byte[]
            UInt32 checksum = CalcChecksum(end);
            //Compare them
            if (csum != (checksum)) return end;
            return end;
        }

        /// <summary>
        /// Calculates a checksum for decrypted savedata.
        /// </summary>
        /// <param name="buffer">Decrypted savedata as a byte array</param>
        /// <returns>The checksum for the decrypted savedata</returns>
        public UInt32 CalcChecksum(byte[] buffer)
        {
            UInt32 checksum = buffer.Aggregate<byte, uint>(0, (current, b1) => current + b1);
            checksum &= 0xffffffff;
            return checksum;
        }

        public byte[] Encrypt(byte[] buffer)
        {
            _cipher = new Blowfish(_key);
            var checksum = BitConverter.GetBytes(CalcChecksum(buffer));
            var buff = new byte[buffer.Length + 4];
            checksum.CopyTo(buff, 0);
            buffer.CopyTo(buff, 4);
            //Why we actually bother to randomise this is beyond me, might as well be 0, oh well.
            var seed = (ushort)((new Random()).Next() >> 16);
            //XOR!
            buffer = XOR(buff, seed);
            //Write seed
            buff = new byte[buffer.Length + 4];
            (BitConverter.GetBytes((seed << 16) + 0x10)).CopyTo(buff, 0);
            buffer.CopyTo(buff, 4);
            //Encrypt
            buffer = Functions.Cipher(buff, ref _key, false);
            return buffer;
        }

        public SaveData(Region region)
        {
            //KOR/TWN not supported
            RegionSupported = ((int)region < 2);
        }

        public byte[] Decrypt(string filepath)
        {
            return RegionSupported ? Decrypt(File.ReadAllBytes(filepath)) : null;
        }

        public byte[] Encrypt(string filepath)
        {
            return RegionSupported ? Encrypt(File.ReadAllBytes(filepath)) : null;
        }
    }
}