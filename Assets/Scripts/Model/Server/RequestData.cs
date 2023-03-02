

using DuiChongServerCommon.ClientProtocol;
using KHCore.Utils;
using System;
using System.Collections.Generic;

public class RequestData
{
    public static Dictionary<RequestCode, int> sign;

    private RequestCode _code;
    public RequestCode code
    {
        get
        {
            return _code;
        }
    }
    private IKHSerializable _serializxable;
    private BufferWriter _bufferWrite;
    private int _encrypt = -1;
    public RequestData(RequestCode code, IKHSerializable serializable)
    {

        int encrypt = -1;
        if (sign != null)
            sign.TryGetValue(code, out encrypt);
        var len = encrypt == -1 ? 4 : 1;
        _bufferWrite = new BufferWriter(128 + len);
        _encrypt = encrypt;
        _code = code;
        if (_encrypt != -1)
        {
            RequestData.sign.TryGetValue(_code, out var sign);
            _bufferWrite.Write(sign);
            if (++sign > int.MaxValue)
            {
                sign = int.MaxValue;
            }
            RequestData.sign.Add(_code, sign);
        }
        else
        {
            _bufferWrite.Write((byte)_code);
        }
        _bufferWrite.Write(serializable);

        var buffer = EncryptTool.EncryptBinary(_bufferWrite.GetBuffer, ConfigManager.inst.encryptKeys);
        var writer = new BufferWriter(buffer.Length + 1);
        writer.Write((byte)_code);
        writer.Joint(buffer);
        _bufferWrite = writer;
    }

    public byte[] buffer
    {
        get
        {
            return _bufferWrite.GetBuffer;
        }
    }

    public string data
    {
        get
        {
            return Convert.ToBase64String(_bufferWrite.GetBuffer);
        }
    }
}