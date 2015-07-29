using Simias.Encryption;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace MH4UCipher
{
    public enum Region
    {
        EUR_USA,
        JPN,
        KOR,
        TWN
    }

    public class DLC
    {
        /// <summary>
        /// The string found at 0x4 in decrypted DLC, used to check encryption state.
        /// </summary>
        private static readonly byte[] Magic = { 0x76, 0x30, 0x30, 0x35 };

        private static Blowfish _cipher;

        /// <summary>
        /// The region of the DLC, for blowfish purposes.
        /// </summary>

        private readonly string[] _keys =
            {
                "AgK2DYheaCjyHGPB", "AgK2DYheaCjyHGP8",
                "AgK2DYheaOjyHGP8", "Capcom123 "
            };

        /// <summary>
        /// Get the encryption key used for the blowfish cipher.
        /// </summary>
        public byte[] Key;

        /// <summary>
        ///DLC class used to cipher DLC files.
        /// </summary>
        /// <param name="region">The DLC's region</param>
        public DLC(Region region)
        {
            Key = new UTF8Encoding().GetBytes(_keys[(int)region]);
        }

        /// <summary>
        /// Checks if DLC is already decrypted by examining offset 0x4 for the magic string.
        /// </summary>
        /// <param name="filePath">A DLC file</param>
        /// <returns></returns>
        public bool IsDecrypted(string filePath)
        {
            using (var f = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read) { Position = 4 })
            {
                var magic = new byte[4];
                f.Read(magic, 0, 4);
                return (magic.SequenceEqual(Magic));
            }
        }

        /// <summary>
        /// Checks if DLC is already decrypted by examining offset 0x4 for the magic string.
        /// </summary>
        /// <param name="buffer">DLC read into a byte array</param>
        /// <returns></returns>
        public bool IsDecrypted(byte[] buffer)
        {
            using (var m = new MemoryStream(buffer, false) { Position = 4 })
            {
                var magic = new byte[4];
                m.Read(magic, 0, 4);
                return (magic.SequenceEqual(Magic));
            }
        }

        /// <summary>
        /// Decrypt a MH4U DLC file
        /// </summary>
        /// <param name="FilePath">The DLC filepath.</param>
        /// <returns>Decrypted DLC as byte array</returns>
        public byte[] Decrypt(string FilePath)
        {
            return Functions.Cipher(File.ReadAllBytes(FilePath), ref Key, true);// Decrypt();
        }

        /// <summary>
        /// Encrypt a MH4U DLC file
        /// </summary>
        /// <param name="FilePath">The DLC filepath.</param>
        /// <returns>Encrypted DLC as byte array</returns>

        public byte[] Encrypt(string FilePath)
        {
            return Encrypt(File.ReadAllBytes(FilePath));
        }

        public byte[] Encrypt(byte[] buffer)
        {
            //hash sh1
            var sh1 = System.Security.Cryptography.SHA1.Create().ComputeHash(buffer);
            //Add sh1 to buffer
            var b = new byte[buffer.Length + sh1.Length];
            buffer.CopyTo(b, 0);
            sh1.CopyTo(b, b.Length - sh1.Length);
            //Get size
            var size = b.Length;
            if (size % 8 != 0)
            {
                var b2 = new byte[b.Length + (8 - size % 8)];
                b.CopyTo(b2, 0);
                b = b2;
                b2 = null;
            }

            //Encrypt
            buffer = Functions.Cipher(b, ref Key, false);
            //Append endianswapped size
            var sizeb = BitConverter.GetBytes(size).Reverse().ToArray();
            var end = new byte[buffer.Length + 4];
            buffer.CopyTo(end, 0);
            sizeb.CopyTo(end, end.Length - 4);
            return end;
        }

        /// <summary>
        /// Decrypt a buffer containing MH4U DLC
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns>Decrypted DLC</returns>
        public byte[] Decrypt(byte[] buffer)
        {
            _cipher = new Blowfish(Key);
            var b = new byte[8];
            //var hash = new byte[20];
            using (var m = new MemoryStream(buffer, true))
            {
                while (m.Position < m.Length - 4) //0x1c //4 //28=24+4
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
                    //Decrypt the 8 bytes
                    _cipher.Decipher(b, 8);
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
                //m.Position -= 24;
                ////capture sh1 hash
                //m.Read(hash, 0, 20);
                //var sh1 = System.Security.Cryptography.SHA1.Create(); sh1.ComputeHash(buffer, 0, buffer.Length - 28);
                //if (!sh1.Hash.SequenceEqual(hash))
                //{
                //    MessageBox.Show("Invalid SHA1 hash in footer.");
                //    return;
                //}
            }

            //Trim the hash (24) and other useless data (4) by cloning into a new array.
            return StripJunk(buffer);
        }

        private byte[] StripJunk(byte[] b)
        {
            var end = new byte[b.Length - 28];
            Buffer.BlockCopy(b, 0, end, 0, end.Length);
            return end;
        }
    }
}