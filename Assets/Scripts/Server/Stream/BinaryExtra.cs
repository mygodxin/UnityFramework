using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Reflection;
using System.Text;
//using static KHCore.Map.MapGrid2D;
using System.Security.Cryptography;

namespace KHCore.Utils
{
    public static class BinaryExtra
    {
        public static string ToUTF8String(this byte[] binary,int startIndx,int len)
        {
            return Encoding.UTF8.GetString(binary, startIndx, len);
        }
        public static string ToUTF8String(this byte[] binary)
        {
            return binary.ToUTF8String(0, binary.Length);
        }
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="binary"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] Compress(this byte[] binary, int startIndex,int length)
        {
            using MemoryStream outMS = new MemoryStream();
            using GZipStream gzs = new GZipStream(outMS, CompressionMode.Compress, true);
            gzs.Write(binary, startIndex, length);
            gzs.Close();
            return outMS.ToArray();
        }
        public static byte[] Compress(this byte[] binary)
        {
            return binary.Compress(0, binary.Length);
        }
        public static byte[] DeCompress(this byte[] binary)
        {
            return binary.DeCompress(0, binary.Length);
        }
        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="binary"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] DeCompress(this byte[] binary, int startIndex, int length)
        {
            using MemoryStream inputMS = new MemoryStream(binary, startIndex, length);
            using MemoryStream outMs = new MemoryStream();
            using GZipStream gzs = new GZipStream(inputMS, CompressionMode.Decompress);
            byte[] bytes = new byte[1024];
            int len = 0;
            while ((len = gzs.Read(bytes, 0, bytes.Length)) > 0)
            {
                outMs.Write(bytes, 0, len);
            }
            gzs.Close();
            return outMs.ToArray();
        }
        public static string Md5(this byte[] binary, int startIndex, int length)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(binary, startIndex, length);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        public static string Md5(this byte[] binary)
        {
            return binary.Md5(0, binary.Length);
        }





    }
}
