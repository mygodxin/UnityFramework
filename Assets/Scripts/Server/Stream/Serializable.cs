using KHCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Unity.Collections.LowLevel.Unsafe;

/// <summary>
/// 多肽序列化特性(可使用基类序列化反序列化)
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class SerializeFamillyAttribute : Attribute
{
}



/// <summary>
/// 同一序列化家族应该有不同的序列化标识ID(纯虚类不需要此特性)
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class SerializeFamillyIDAttribute : Attribute
{




    public const byte IDMax = 253;
    internal byte BuildID => ((byte)(ID + 2));
    public byte ID { get; private set; }
    /// <summary>
    /// 限制范围(0-253)
    /// </summary>
    /// <param name="id"></param>
    public SerializeFamillyIDAttribute(byte id)
    {

        if (id > IDMax)
        {
            throw new Exception($"序列化id 必须<={IDMax}");
        }
        this.ID = id;
    }
}

public class DontAutoProtocol : Attribute
{

}

class SerializeFamilly
{
    private static Dictionary<Type, SerializeFamilly> AllBaseFamilly = new Dictionary<Type, SerializeFamilly>();
    private static Dictionary<Type, SerializeFamilly> AllTypeFamilly = new Dictionary<Type, SerializeFamilly>();
    class TypeFamilly<T>
    {
        private static readonly Expression<Func<byte, Type>> NewExpression = (famillyID) => SerializeFamilly.AllTypeFamilly[typeof(T)].FammillyIDType[famillyID];
        public static readonly Func<byte, Type> CreatInstance = NewExpression.Compile();
    }

    public static bool IsSerializeFamillyType(Type type, out byte famillyID)
    {
        famillyID = 0;
        return AllTypeFamilly.TryGetValue(type, out var familly) && familly.FamillyTypeID.TryGetValue(type, out famillyID);
    }
    public static Type GetFamillyType<T>(byte famillyID)
    {
        return AllTypeFamilly[typeof(T)].FammillyIDType[famillyID];
    }
    public static void FindFamilly(Type type)
    {
        var att = type.GetCustomAttribute<SerializeFamillyAttribute>();
        if (att != null)
        {
            var baseType = FindBaseType(type);
            if (!AllBaseFamilly.TryGetValue(baseType, out var familly))
            {
                familly = new SerializeFamilly(baseType);
                AllBaseFamilly.Add(baseType, familly);
            }
            familly.Add(type);
            AllTypeFamilly.Add(type, familly);
        }
    }
    static Type FindBaseType(Type type)
    {
        while (type.BaseType.GetCustomAttribute<SerializeFamillyAttribute>() != null)
        {
            type = type.BaseType;
        }
        return type;
    }
    private Type BaseType { get; set; }
    private Dictionary<Type, byte> FamillyTypeID = new Dictionary<Type, byte>();
    private Dictionary<byte, Type> FammillyIDType = new Dictionary<byte, Type>();
    List<Type> AllTypes { get; set; } = new List<Type>();
    public SerializeFamilly(Type baseType)
    {
        this.BaseType = baseType;
        AllTypes.Add(baseType);
    }
    private void Add(Type type)
    {
        if (!type.IsAbstract && !this.FamillyTypeID.ContainsKey(type))
        {
            var idAtt = type.GetCustomAttribute<SerializeFamillyIDAttribute>();
            if (idAtt == null)
            {
                throw new Exception($"可序列化家族非纯虚类型 必须实现特性 {typeof(SerializeFamillyIDAttribute).Name}");
            }
            var buildID = idAtt.BuildID;
            if (this.FammillyIDType.TryGetValue(buildID, out var orgType) && orgType != type)
            {
                throw new Exception($"可序列化家族类型 {orgType.Name}与{type.Name} 序列化ID[{idAtt.ID}]重复 同一基类下所有子类不能拥有相同ID");
            }
            FammillyIDType.AddOrUpdate(buildID, type);// = type;
            FamillyTypeID.AddOrUpdate(type, buildID);
        }
    }
}



public static class SerializeTool
{

    internal static bool IsSerializeFamillyType(Type type, out byte famillyID)
    {
        return SerializeFamilly.IsSerializeFamillyType(type, out famillyID);
    }
    internal static Type GetFamillyType<T>(byte famillyID)
    {
        return SerializeFamilly.GetFamillyType<T>(famillyID);
    }
    public static void InitSerializeFamilly(params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            var allTypes = assembly.GetTypes().ToList();
            allTypes.RemoveAll(t => !typeof(IKHSerializable).IsAssignableFrom(t));
            foreach (var item in allTypes)
            {
                SerializeFamilly.FindFamilly(item);
            }
        }
    }
    internal delegate T DeserializeValue<T>(in BufferReader reader);
    internal delegate void SerializeValue<T>(in T value, BufferWriter writer);
    static readonly Dictionary<Type, Delegate> SerFunc = new Dictionary<Type, Delegate>()
        {
            {typeof(int),new SerializeValue<int>((in int v, BufferWriter w)=>{ w.Write(v); }) },
            {typeof(uint),new SerializeValue<uint>((in uint v,BufferWriter w)=>{ w.Write(v); }) },
            {typeof(byte),new SerializeValue<byte>((in byte v,BufferWriter w)=>{ w.Write(v); }) },
            {typeof(short),new SerializeValue<short>((in short v, BufferWriter w)=>{ w.Write(v); }) },
            {typeof(ushort),new SerializeValue<ushort>((in ushort v,BufferWriter w)=>{ w.Write(v); }) },
            {typeof(bool),new SerializeValue<bool>((in bool v,BufferWriter w)=>{ w.Write(v); }) },
            {typeof(long),new SerializeValue<long>((in long v,BufferWriter w)=>{ w.Write(v); }) },
            {typeof(ulong),new SerializeValue<ulong>((in ulong v,BufferWriter w)=>{ w.Write(v); }) },
            {typeof(float),new SerializeValue<float>((in float v,BufferWriter w)=>{ w.Write(v); }) },
            {typeof(double),new SerializeValue<double>((in double v,BufferWriter w)=>{ w.Write(v); }) },
            {typeof(string),new SerializeValue<string>((in string v,BufferWriter w)=>{ w.Write(v); }) },
            {typeof(sbyte),new SerializeValue<sbyte>((in sbyte v,BufferWriter w)=>{ w.Write(v); }) },
            {typeof(DateTime),new SerializeValue<DateTime>((in DateTime v,BufferWriter w)=>{ w.Write(v); }) },
        };
    static readonly Dictionary<Type, Delegate> DecFunc = new Dictionary<Type, Delegate>()
        {
            {typeof(int),new DeserializeValue<int>(( in BufferReader r)=>{ r.Read(out int v); return v; }) },
            {typeof(uint),new DeserializeValue<uint>((in BufferReader r)=>{ r.Read(out uint v); return v; }) },
            {typeof(byte),new DeserializeValue<byte>((in BufferReader r)=>{ r.Read(out byte v); return v; }) },
            {typeof(short),new DeserializeValue<short>((in BufferReader r)=>{ r.Read(out short v); return v; }) },
            {typeof(ushort),new DeserializeValue<ushort>((in BufferReader r)=>{ r.Read(out ushort v); return v; }) },
            {typeof(bool),new DeserializeValue<bool>((in BufferReader r)=>{ r.Read(out bool v); return v; }) },
            {typeof(long),new DeserializeValue<long>((in BufferReader r)=>{ r.Read(out long v); return v; }) },
            {typeof(ulong),new DeserializeValue<ulong>((in BufferReader r)=>{ r.Read(out ulong v); return v; }) },
            {typeof(float),new DeserializeValue<float>((in BufferReader r)=>{ r.Read(out float v); return v; }) },
            {typeof(double),new DeserializeValue<double>((in BufferReader r)=>{ r.Read(out double v); return v; }) },
            {typeof(string),new DeserializeValue<string>((in BufferReader r)=>{ r.Read(out string v); return v; }) },
            {typeof(sbyte),new DeserializeValue<sbyte>((in BufferReader r)=>{ r.Read(out sbyte v); return v; }) },
            {typeof(DateTime),new DeserializeValue<DateTime>((in BufferReader r)=>{ r.Read(out DateTime v); return v; }) },
        };
    internal static SerializeValue<T> GetSerializeFunc<T>()
    {
        var type = typeof(T);
        if (SerFunc.TryGetValue(type, out Delegate @delegate))
        {
            return @delegate as SerializeValue<T>;
        }
        lock (SerFuncLocker)
        {
            if (!SerFunc.TryGetValue(type, out @delegate))
            {
                if (typeof(IKHSerializable).IsAssignableFrom(type))
                {
                    @delegate = new SerializeValue<T>((in T value, BufferWriter w) =>
                    {
                        w.Write(value as IKHSerializable);
                    });
                }
                else if (type.IsEnum)
                {
                    var size = UnsafeUtility.SizeOf(typeof(T));
                    @delegate = new SerializeValue<T>((in T v, BufferWriter w) =>
                    {
                        w.WritePtr(v, size);
                    });
                }
                else
                {
                    throw new NotSupportedException($"{typeof(T)} 未知序列化类型");
                }
                SerFunc.Add(type, @delegate);
            }
            return @delegate as SerializeValue<T>;
        }
    }
    static readonly object DesFuncLocker = new object();
    static readonly object SerFuncLocker = new object();
    internal static DeserializeValue<T> GetDeserializeFunc<T>()
    {
        var type = typeof(T);
        if (DecFunc.TryGetValue(type, out Delegate @delegate))
        {
            return @delegate as DeserializeValue<T>;
        }
        lock (DesFuncLocker)
        {
            if (!DecFunc.TryGetValue(type, out @delegate))
            {
                if (typeof(IKHSerializable).IsAssignableFrom(type))
                {
                    @delegate = new DeserializeValue<T>((in BufferReader r) =>
                    {
                        return r.ReadCustomSerializable<T>();
                    });
                }
                else if (type.IsEnum)
                {
                    //var size = UnsafeUtility.SizeOf(typeof(T));
                    //@delegate = new DeserializeValue<T>((in BufferReader r) =>
                    //{
                    //    return r.ReadPtr<T>(size);
                    //});
                    return null;
                }
                else
                {
                    throw new NotSupportedException($"{typeof(T)} 不可反序列化");
                }
                DecFunc.Add(type, @delegate);
            }
            return @delegate as DeserializeValue<T>;
        }
    }
}








