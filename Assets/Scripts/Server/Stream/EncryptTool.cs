using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace KHCore.Utils
{
    public static class EncryptTool
    {
        public static int[] CreatKey(byte[] keys)
        {
            var Int32Kes = ToIntArray(keys, false);
            if (Int32Kes.Length < 4)
            {
                int[] key2 = new int[4];
                System.Array.Copy(Int32Kes, 0, key2, 0, Int32Kes.Length);
                Int32Kes = key2;
            }
            return Int32Kes;
        }


        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] EncryptBinary(ReadOnlySpan<byte> data, int[] keys)
        {
            if (data.Length == 0)
            {
                return data.ToArray();
            }
            return ToByteArray(Encrypt(ToIntArray(data, true), keys), false);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] DecryptBinary(ReadOnlySpan<byte> data, IList<int> keys)
        {
            if (data.Length == 0)
            {
                return data.ToArray();
            }
            return ToByteArray(Decrypt(ToIntArray(data, false), keys), true);
        }
        private static int[] Encrypt(int[] data, IList<int> keys)
        {
            int n = data.Length - 1;
            if (n < 1)
            {
                return data;
            }
            int z = data[n], y;
            int delta = -1640531527;
            int sum = 0, e;
            int p, q = 6 + 52 / (n + 1);
            while (q-- > 0)
            {
                sum = (int)(sum + delta);
                e = sum.RightMove(2) & 3;
                for (p = 0; p < n; p++)
                {
                    y = data[p + 1];
                    z = data[p] += (z.RightMove(5) ^ y << 2) + (y.RightMove(3) ^ z << 4) ^ (sum ^ y) + (keys[p & 3 ^ e] ^ z);
                }
                y = data[0];
                z = data[n] += (z.RightMove(5) ^ y << 2) + (y.RightMove(3) ^ z << 4) ^ (sum ^ y) + (keys[p & 3 ^ e] ^ z);
            }
            return data;
        }
        private static int[] Decrypt(int[] data, IList<int> keys)
        {
            int n = data.Length - 1;
            if (n < 1)
            {
                return data;
            }

            //if (Int32Kes==null)
            //{
            //    SetEncryptKeys(Keys);
            //}

            int z, y = data[0], delta = -1640531527, sum, e;
            int p, q = 6 + 52 / (n + 1);

            sum = q * delta;
            while (sum != 0)
            {
                e = sum.RightMove(2) & 3;
                for (p = n; p > 0; p--)
                {
                    z = data[p - 1];
                    y = data[p] -= (z.RightMove(5) ^ y << 2) + (y.RightMove(3) ^ z << 4) ^ (sum ^ y) + (keys[p & 3 ^ e] ^ z);
                }
                z = data[n];
                y = data[0] -= (z.RightMove(5) ^ y << 2) + (y.RightMove(3) ^ z << 4) ^ (sum ^ y) + (keys[p & 3 ^ e] ^ z);
                sum -= delta;
            }
            return data;
        }
        private static int[] ToIntArray(ReadOnlySpan<byte> data, bool includeLength)
        {
            int n = (data.Length & 3) == 0 ? data.Length.RightMove(2) : (data.Length.RightMove(2)) + 1;
            int[] result;
            if (includeLength)
            {
                result = new int[n + 1];
                result[n] = data.Length;
            }
            else
            {
                result = new int[n];
            }
            n = data.Length;
            for (int i = 0; i < n; i++)
            {
                result[i.RightMove(2)] |= (0x000000ff & data[i]) << ((i & 3) << 3);
            }
            return result;
        }
        private static byte[] ToByteArray(int[] data, bool includeLength)
        {
            int n = data.Length << 2;
            if (includeLength)
            {
                int m = data[data.Length - 1];
                if (m > n)
                {
                    return null;
                }
                else
                {
                    n = m;
                }
            }
            byte[] result = new byte[n];
            for (int i = 0; i < n; i++)
            {
                result[i] = (byte)((data[i.RightMove(2)].RightMove((i & 3) << 3)) & 0xff);
            }
            return result;
        }
        private static int RightMove(this int value, int pos)
        {
            if (pos != 0)
            {
                int mask = int.MaxValue;
                value = value >> 1;
                value = value & mask;
                value = value >> pos - 1;
            }
            return value;
        }







        #region DES加密解密

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDES(this string encryptString, string encryptKey, byte[] rgbIV)
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            rgbIV ??= new byte[] { 102, 120, 22, 24, 88, 6, 119, 88 };
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptDES(this string decryptString, string decryptKey, byte[] rgbIV)
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey.Substring(0, 8));
            rgbIV ??= new byte[] { 102, 120, 22, 24, 88, 6, 119, 88 };
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }
        #endregion
    }
}
