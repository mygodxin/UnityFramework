

//using DuiChongServerCommon.ClientProtocol;
//using KHCore.Utils;
//using System;
//using System.Collections.Generic;

//public class RequestData
//{
//    public static Dictionary<RequestCode, int> sign;
//    public static int[] keys;

//    private RequestCode _code;
//    public RequestCode code
//    {
//        get
//        {
//            return _code;
//        }
//    }
//    private IKHSerializable _serializxable;
//    private BufferWriter _bufferWrite;
//    private int _encrypt = -1;
//    public RequestData(RequestCode code, IKHSerializable serializable = null)
//    {

//        int encrypt = -1;
//        if (sign != null)
//            sign.TryGetValue(code, out encrypt);
//        var len = encrypt == -1 ? 1 : 4;
//        _bufferWrite = new BufferWriter(128 + len);
//        _encrypt = encrypt;
//        _code = code;
//        if (_encrypt != -1)
//        {
//            RequestData.sign.TryGetValue(_code, out var sign);
//            _bufferWrite.Write(sign);
//            if (++sign > int.MaxValue)
//            {
//                sign = int.MaxValue;
//            }
//            RequestData.sign[_code] = sign;
//        }
//        else
//        {
//            _bufferWrite.Write((byte)_code);
//        }
//        if (serializable != null)
//        {
//            _bufferWrite.Write(serializable);
//        }
//    }

//    public byte[] buffer
//    {
//        get
//        {
//           return this._bufferWrite.GetBuffer;
//        }
//    }

//    public BufferWriter bufferWriter
//    {
//        get
//        {
//            return _bufferWrite;
//        }
//    }
//}