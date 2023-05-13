//using System;
//using System.Collections.Generic;



//public class SList<T> : List<T>, IKHSerializable
//{
//    private static readonly SerializeTool.SerializeValue<T> SerFunc = SerializeTool.GetSerializeFunc<T>();
//    private static readonly SerializeTool.DeserializeValue<T> DesFunc = SerializeTool.GetDeserializeFunc<T>();
//    public SList()
//    {
//    }
//    public SList(int capacity) : base(capacity)
//    {
//    }
//    public SList(IEnumerable<T> collection) : base(collection)
//    {
//    }
//    public virtual void Deserialize(BufferReader reader)
//    {
//        reader.Read(out ushort count);
//        this.Capacity = count;
//        for (ushort i = 0; i < count; i++)
//        {
//            base.Add(DesFunc(reader));
//        }
//    }
//    public virtual void Serialize(BufferWriter sender)
//    {
//        if (this.Count > ushort.MaxValue)
//        {
//            throw new Exception("集合长度超过65535");
//        }
//        sender.Write((ushort)this.Count);
//        for (int i = 0; i < this.Count; i++)
//        {
//            var element = this[i];
//            SerFunc(element, sender);
//        }
//    }
//}
//public class SHashSet<T> : HashSet<T>, IKHSerializable
//{
//    private static readonly SerializeTool.SerializeValue<T> serFunc = SerializeTool.GetSerializeFunc<T>();
//    private static readonly SerializeTool.DeserializeValue<T> desFunc = SerializeTool.GetDeserializeFunc<T>();
//    public SHashSet()
//    {

//    }
//    public SHashSet(int capacity) : base(capacity)
//    {

//    }
//    public SHashSet(IEnumerable<T> collection) : base(collection)
//    {
//    }
//    public virtual void Deserialize(BufferReader reader)
//    {
//        reader.Read(out ushort count);
//        base.EnsureCapacity(count);
//        for (ushort i = 0; i < count; i++)
//        {
//            base.Add(desFunc(reader));
//        }
//    }
//    public virtual void Serialize(BufferWriter sender)
//    {
//        if (this.Count > ushort.MaxValue)
//        {
//            throw new Exception("集合长度超过65535");
//        }
//        sender.Write((ushort)this.Count);
//        foreach (var item in this)
//        {
//            serFunc(item, sender);
//        }
//    }

//}
//public class SDictionary<Key, Value> : Dictionary<Key, Value>, IKHSerializable
//{
//    private static readonly SerializeTool.SerializeValue<Key> keySerFunc = SerializeTool.GetSerializeFunc<Key>();
//    private static readonly SerializeTool.DeserializeValue<Key> keyDesFunc = SerializeTool.GetDeserializeFunc<Key>();
//    private static readonly SerializeTool.SerializeValue<Value> valueSerFunc = SerializeTool.GetSerializeFunc<Value>();
//    private static readonly SerializeTool.DeserializeValue<Value> valueDesFunc = SerializeTool.GetDeserializeFunc<Value>();
//    public SDictionary<Key, Value> CreatCopy()
//    {
//        var dic = new SDictionary<Key, Value>();
//        foreach (var item in this)
//        {
//            dic.Add(item.Key, item.Value);
//        }
//        return dic;
//    }
//    public virtual void Deserialize(BufferReader reader)
//    {
//        reader.Read(out ushort count);
//        base.EnsureCapacity(count);
//        for (ushort i = 0; i < count; i++)
//        {
//            base.Add(keyDesFunc(reader), valueDesFunc(reader));
//        }
//    }
//    public virtual void Serialize(BufferWriter writer)
//    {
//        if (this.Count > ushort.MaxValue)
//        {
//            throw new Exception("集合长度超过65535");
//        }
//        writer.Write((ushort)this.Count);
//        foreach (var item in this)
//        {
//            keySerFunc(item.Key, writer);
//            valueSerFunc(item.Value, writer);
//        }
//    }
















//}
