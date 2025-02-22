using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

public class CopyUtil
{
    //几种方法都只适合在特定情况下使用,c#截至目前没有泛用性较高的类
    //利用二进制序列化和反序列化
    //public static T DeepCopy<T>(T Item)
    //{
    //    object retval;
    //    using (MemoryStream ms = new MemoryStream())
    //    {
    //        BinaryFormatter bf = new BinaryFormatter();
    //        //序列化成流
    //        bf.Serialize(ms, Item);
    //        ms.Seek(0, SeekOrigin.Begin);
    //        //反序列化成对象
    //        retval = bf.Deserialize(ms);
    //        ms.Close();
    //    }
    //    return (T)retval;
    //}

    //利用反射
    //public static T DeepCopy<T>(T Item)
    //{
    //    //如果是字符串或值类型则直接返回
    //    if (Item is string || Item.GetType().IsValueType) return Item;


    //    object retval = Activator.CreateInstance(Item.GetType());
    //    FieldInfo[] fields = Item.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
    //    foreach (FieldInfo field in fields)
    //    {
    //        try { field.SetValue(retval, DeepCopy(field.GetValue(Item))); }
    //        catch { }
    //    }
    //    return (T)retval;
    //}

    //利用xml序列化和反序列化实现
    //public T DeepCopy<T>(T Item)
    //{
    //    object retval;
    //    using (MemoryStream ms = new MemoryStream())
    //    {
    //        XmlSerializer xml = new XmlSerializer(typeof(T));
    //        xml.Serialize(ms, Item);
    //        ms.Seek(0, SeekOrigin.Begin);
    //        retval = xml.Deserialize(ms);
    //        ms.Close();
    //    }
    //    return (T)retval;
    //}
}