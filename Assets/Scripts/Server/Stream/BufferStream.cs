using KHCore.Utils;
using System;
using System.Drawing;
using System.Text;
using Unity.Collections.LowLevel.Unsafe;

enum MessageState : byte
{
    Empty = 0,
    UshortLen,
    IntLen,
}
public interface IKHSerializable
{
    void Serialize(BufferWriter stream);
    void Deserialize(BufferReader reader);
}

public sealed class BufferReader
{
    private int readIndex;
    public int Postion => readIndex;
    byte[] buffer;
    public byte[] Buffer => buffer;
    public int Length => buffer.Length;
    public bool ReadEnd => readIndex >= buffer.Length;
    internal void RestBuffer(byte[] buffer)
    {
        this.buffer = buffer;
    }
    internal void SetReadOffset(int offset)
    {
        this.readIndex += offset;
    }
    public BufferReader(byte[] buffer)
    {
        this.buffer = buffer;
    }
    public ReadOnlySpan<byte> UnReadSpan
    {
        get
        {
            return this.buffer.AsSpan(readIndex);
        }
    }
    public string ReadString()
    {
        this.Read(out string value);
        return value;
    }
    public char ReadChar()
    {
        return (char)this.ReadByte();
    }
    public byte ReadByte()
    {
        return this.buffer[readIndex++];
    }
    public sbyte ReadSByte()
    {
        return (sbyte)this.ReadByte();
    }
    public short ReadShort()
    {
        this.Read(out short value);
        return value;
    }
    public ushort ReadUshort()
    {
        this.Read(out ushort value);
        return value;
    }
    public int ReadInt()
    {
        this.Read(out int value);
        return value;
    }
    public uint ReadUint()
    {
        this.Read(out uint value);
        return value;
    }
    public float ReadFloat()
    {
        this.Read(out float value);
        return value;
    }
    public long ReadLong()
    {
        this.Read(out long value);
        return value;
    }
    public ulong ReadUlong()
    {
        this.Read(out ulong value);
        return value;
    }
    public double ReadDouble()
    {
        this.Read(out double value);
        return value;
    }
    public bool ReadBoolen()
    {
        this.Read(out bool value);
        return value;
    }
    public byte[] ReadBinary()
    {
        var state = (MessageState)buffer[readIndex++];
        byte[] bianry = default;
        switch (state)
        {
            case MessageState.UshortLen:
                int len = this.ReadUshort();
                bianry = new byte[len];
                Array.Copy(this.buffer, this.readIndex, bianry, 0, len);
                this.readIndex += len;
                break;
            case MessageState.IntLen:
                len = this.ReadInt();
                bianry = this.buffer.DeCompress(this.readIndex, len);
                this.readIndex += len;
                break;
            default:
                break;
        }
        return bianry;
    }
    public T ReadSerializable<T>() where T : IKHSerializable, new()
    {
        this.Read(out T value);
        return value;
    }
    public DateTime ReadDateTime()
    {
        return new DateTime(this.ReadLong());
    }
    //public T ReadEnum<T>()
    //{
    //    return this.ReadPtr<T>(Marshal.SizeOf<T>());
    //}

    public void Read(out DateTime v)
    {
        v = new DateTime(this.ReadLong());
    }
    public void Read(out byte[] binary)
    {
        binary = this.ReadBinary();
    }
    public void Read(out string v)
    {
        var state = (MessageState)buffer[readIndex++];
        switch (state)
        {
            case MessageState.Empty:
                v = string.Empty;
                break;
            case MessageState.UshortLen:
                int len = this.ReadUshort();
                var str = Encoding.UTF8.GetString(buffer, readIndex, len);
                readIndex += len;
                v = str;
                break;
            case MessageState.IntLen:
                len = this.ReadInt();
                var binary = this.buffer.DeCompress(this.readIndex, len);
                v = Encoding.UTF8.GetString(binary);
                readIndex += len;
                break;
            default:
                v = string.Empty;
                break;
        }
    }
    public void Read(out int v)
    {
        v = BitConverter.ToInt32(buffer, readIndex);
        readIndex += 4;
    }
    public void Read(out uint v)
    {
        v = BitConverter.ToUInt32(buffer, readIndex);
        readIndex += 4;
    }
    public void Read(out short v)
    {
        v = BitConverter.ToInt16(buffer, readIndex);
        readIndex += 2;
    }
    public void Read(out char v)
    {
        v = BitConverter.ToChar(buffer, readIndex);
        readIndex += 1;
    }
    public void Read(out ushort v)
    {
        v = BitConverter.ToUInt16(buffer, readIndex);
        readIndex += 2;
    }
    public void Read(out bool v)
    {
        v = BitConverter.ToBoolean(buffer, readIndex);
        v = buffer[readIndex++] == 1;
    }
    public void Read(out long v)
    {
        v = BitConverter.ToInt64(buffer, readIndex);
        readIndex += 8;
    }
    public void Read(out ulong v)
    {
        v = BitConverter.ToUInt64(buffer, readIndex);
        readIndex += 8;
    }
    public void Read(out float v)
    {
        v = BitConverter.ToSingle(buffer, readIndex);
        readIndex += 4;
    }
    public void Read(out double v)
    {
        v = BitConverter.ToDouble(buffer, readIndex);
        readIndex += 8;
    }
    public void Read(out byte v)
    {
        v = buffer[readIndex++];
    }
    public void Read(out sbyte v)
    {
        v = (sbyte)(buffer[readIndex++]);
    }
    public void Read<T>(out T value) where T : IKHSerializable, new()
    {
        this.Read(out byte bit);
        if (bit == 0)
        {
            value = default;
        }
        else if (bit == 1)
        {
            value = Activator.CreateInstance<T>();
            value.Deserialize(this);
        }
        else
        {
            var type = typeof(T);
            value = (T)Activator.CreateInstance(type);
            value.Deserialize(this);
        }
    }
    internal T ReadCustomSerializable<T>()
    {
        this.Read(out Byte bit);
        if (bit == 0)
        {
            return default;
        }
        if (bit == 1)
        {
            var instance = (IKHSerializable)Activator.CreateInstance<T>();
            instance.Deserialize(this);
            return (T)instance;
        }
        else
        {
            var type = typeof(T);
            var instance = (IKHSerializable)Activator.CreateInstance(type);
            instance.Deserialize(this);
            return (T)instance;
        }

    }
    public void RestRead()
    {
        this.readIndex = 0;
    }


}


public sealed class BufferWriter
{
    private byte[] buffer;
    private int writeIndex = 0;
    private int capcity;
    public int Postion
    {
        get
        {
            return writeIndex;
        }
    }
    public byte[] GetBuffer
    {
        get
        {
            var bytes = new byte[writeIndex];
            Buffer.BlockCopy(this.buffer, 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
    public void JoinHeader(byte[] header)
    {
        this.MoveHeaderSize(header.Length);
        Buffer.BlockCopy(header, 0, this.buffer, 0, header.Length);
    }
    public void JoinHeader<T>(in T header, int size) where T : struct
    {
        this.MoveHeaderSize(size);
        //this.buffer.WritePtr<T>(header, size, 0);
    }

    public void JoinHeader<T1, T2>(in T1 header1, int size1, in T2 header2, int size2) where T1 : struct where T2 : struct
    {
        this.MoveHeaderSize(size1 + size2);
        //this.buffer.WritePtr(header1, size1, 0);
        //this.buffer.WritePtr(header2, size2, size1);
    }






    /// <summary>
    /// 已经写入的范围
    /// </summary>
    /// <returns></returns>
    public ReadOnlySpan<byte> GetWriteSpan()
    {
        return this.buffer.AsSpan(0, this.writeIndex);
    }
    public BufferReader Reader { get; private set; }
    public string GetBase64
    {
        get
        {
            return Convert.ToBase64String(this.GetBuffer);
        }
    }
    public void RestWrite()
    {
        this.writeIndex = 0;
    }
    public BufferWriter(int capcity)
    {
        this.capcity = capcity;
        this.buffer = new byte[capcity];
        this.Reader = new BufferReader(this.buffer);
    }
    /// <summary>
    /// 指针写入(慎用)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="len"></param>
    /// <returns></returns>
    public BufferWriter WritePtr<T>(T value, int len)
    {
        this.MoreBuffer(len);
        //this.buffer.WritePtr(value, len, writeIndex);
        this.writeIndex += len;
        return this;
    }
    public BufferWriter WriteEnum<T>(T @enum) where T : struct, Enum
    {
        return this.WritePtr(@enum, UnsafeUtility.SizeOf<T>());
    }
    public BufferWriter Write(in DateTime data)
    {
        this.Write(data.Ticks);
        return this;
    }
    public BufferWriter Write(byte[] bianry)
    {
        if (bianry == null)
        {
            this.Write((byte)MessageState.Empty);
        }
        else if (bianry.Length <= ushort.MaxValue)
        {
            this.Write((byte)MessageState.UshortLen);
            this.Write((ushort)bianry.Length);
            this.Joint(bianry);
        }
        else if (bianry.Length <= int.MaxValue)
        {
            bianry = bianry.Compress();
            this.Write((byte)MessageState.IntLen);
            this.Write(bianry.Length);
            this.Joint(bianry);
        }
        else
        {
            throw new Exception("字节数组长度过长");
        }
        return this;
    }
    public BufferWriter Write(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            this.Write((byte)(MessageState.Empty));
        }
        else
        {
            int len = Encoding.UTF8.GetByteCount(data);
            if (len <= ushort.MaxValue)
            {
                this.Write((byte)(MessageState.UshortLen));
                this.Write((ushort)len);
                this.MoreBuffer(len);
                writeIndex += Encoding.UTF8.GetBytes(data, 0, data.Length, this.buffer, writeIndex);
            }
            else if (len <= int.MaxValue)
            {
                var stringBinary = Encoding.UTF8.GetBytes(data);
                stringBinary = stringBinary.Compress();
                this.Write((byte)(MessageState.IntLen));
                this.Write(stringBinary.Length);
                this.Joint(stringBinary);
            }
            else
            {
                throw new Exception($"字符串长度过大");
            }
        }
        return this;
    }
    public BufferWriter Write(char data)
    {
        BitConverter.TryWriteBytes(buffer.AsSpan(writeIndex), data);
        return this;
    }
    public BufferWriter Write(int data)
    {
        MoreBuffer(4);
        BitConverter.TryWriteBytes(buffer.AsSpan(writeIndex), data);
        writeIndex += 4;
        return this;
    }
    public BufferWriter Write(uint data)
    {
        MoreBuffer(4);
        BitConverter.TryWriteBytes(buffer.AsSpan(writeIndex), data);
        writeIndex += 4;
        return this;
    }
    public BufferWriter Write(short data)
    {
        MoreBuffer(2);
        BitConverter.TryWriteBytes(buffer.AsSpan(writeIndex), data);
        writeIndex += 2;
        return this;
    }
    public BufferWriter Write(ushort data)
    {
        MoreBuffer(2);
        BitConverter.TryWriteBytes(buffer.AsSpan(writeIndex), data);
        writeIndex += 2;
        return this;
    }
    public BufferWriter Write(bool data)
    {
        MoreBuffer(1);
        this.buffer[writeIndex++] = data ? (byte)1 : (byte)0;
        return this;
    }
    public BufferWriter Write(in long data)
    {
        MoreBuffer(8);
        BitConverter.TryWriteBytes(buffer.AsSpan(writeIndex), data);
        writeIndex += 8;
        return this;
    }
    public BufferWriter Write(in ulong data)
    {
        MoreBuffer(8);
        BitConverter.TryWriteBytes(buffer.AsSpan(writeIndex), data);
        writeIndex += 8;
        return this;
    }
    public BufferWriter Write(float data)
    {
        MoreBuffer(4);
        BitConverter.TryWriteBytes(buffer.AsSpan(writeIndex), data);
        writeIndex += 4;
        return this;
    }
    public BufferWriter Write(in double data)
    {
        MoreBuffer(8);
        BitConverter.TryWriteBytes(buffer.AsSpan(writeIndex), data);
        writeIndex += 8;
        return this;
    }
    public BufferWriter Write(byte data)
    {
        MoreBuffer(1);
        buffer[writeIndex++] = data;
        return this;
    }
    public BufferWriter Write(sbyte data)
    {
        MoreBuffer(1);
        buffer[writeIndex++] = (byte)data;
        return this;
    }
    void MoveHeaderSize(int moveSize)
    {
        var wanaSize = this.writeIndex + moveSize;
        if (wanaSize > this.buffer.Length)
        {
            wanaSize += (capcity - wanaSize % this.capcity);
            var newbuffer = new byte[wanaSize];
            Buffer.BlockCopy(this.buffer, 0, newbuffer, moveSize, this.writeIndex);
            this.buffer = newbuffer;
            this.Reader.RestBuffer(this.buffer);
        }
        else
        {
            Buffer.BlockCopy(this.buffer, 0, this.buffer, moveSize, this.writeIndex);
        }
        this.writeIndex += moveSize;
        this.Reader.SetReadOffset(moveSize);
    }
    public BufferWriter Write<T>(in T data) where T : IKHSerializable
    {
        byte state = 0;
        if (data == null)
        {
            this.Write(state);
            return this;
        }
        if (SerializeFamilly.IsSerializeFamillyType(data.GetType(), out state))
        {
            this.Write(state);
        }
        else
        {
            state = 1;
            this.Write(state);
        }
        data.Serialize(this);
        return this;
    }
    public BufferWriter Joint(byte[] data, int len)
    {
        this.MoreBuffer(len);
        Buffer.BlockCopy(data, 0, this.buffer, this.writeIndex, len);
        writeIndex += len;
        return this;
    }
    public BufferWriter Joint(byte[] data, int indexOffset, int len)
    {
        this.MoreBuffer(len);
        Buffer.BlockCopy(data, indexOffset, this.buffer, this.writeIndex, len);
        writeIndex += len;
        return this;
    }
    public BufferWriter Joint(byte[] data)
    {
        this.MoreBuffer(data.Length);
        Buffer.BlockCopy(data, 0, this.buffer, this.writeIndex, data.Length);
        writeIndex += data.Length;
        return this;
    }
    void MoreBuffer(int count)
    {
        var wanaSize = this.writeIndex + count;
        if (wanaSize > this.buffer.Length)
        {
            wanaSize += (capcity - wanaSize % this.capcity);
            Array.Resize(ref this.buffer, wanaSize);
            this.Reader.RestBuffer(this.buffer);
        }
    }
}

