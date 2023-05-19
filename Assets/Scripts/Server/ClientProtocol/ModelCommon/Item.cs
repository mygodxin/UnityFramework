
using System;
using System.Linq;
namespace DuiChongServerCommon.ClientProtocol
{
    public abstract class Item : IKHSerializable
    {
        public uint ID { get; set; }
        public ushort Pid { get; set; }
        public abstract Item DepCopy();
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out UInt32 _ID);
            this.ID= _ID;
            reader.Read(out UInt16 _Pid);
            this.Pid= _Pid;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(ID);
            sender.Write(Pid);
        }
        #endregion

    }
    public class UserPorpertyValue : IKHSerializable
    {
        public UserProeprtyType UserProeprtyType { get; set; }
        public double Value { get; set; }
        public override string ToString()
        {
            return $"{UserProeprtyType} = {Value}";
        }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out byte _UserProeprtyType);
            this.UserProeprtyType= (UserProeprtyType)_UserProeprtyType;
            reader.Read(out Double _Value);
            this.Value= _Value;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write((byte)UserProeprtyType);
            sender.Write(Value);
        }
        #endregion

    }

    public class Equip : Item
    {


        static EnumArray<套装类型> TZRandom = new EnumArray<套装类型>().Remove(套装类型.套装类型None);

        public sbyte MaxGrowUpIndex { get; private set; } = -1;
        //[CodeAnnotation("属性成长")]
        public float GrowUp { get; set; }
        //[CodeAnnotation("源属性占比")]
        public SDictionary<UserProeprtyType, float> OrgProperties { get; set; }
        //[CodeAnnotation("强化次数")]
        public SDictionary<UserProeprtyType, byte> IntensifyProperties { get; set; }

        public int IntensifyLevel => IntensifyProperties == null ? 0 : IntensifyProperties.Values.Sum((x) => { return x; });
        //[CodeAnnotation("套装技能")]
        public ushort ToZhuangSkill { get; set; }
        //[CodeAnnotation("为None是没有套装")]
        public 套装类型 TaoZhuang { get; set; }
        //[CodeAnnotation("技能 如果为0则为不带技能")]
        public ushort Skill { get; set; }
        //[CodeAnnotation("星级")]
        public byte Star { get; set; }
        //[CodeAnnotation("品质")]
        public 品质 PinZhi { get; set; }
        //[CodeAnnotation("锁")]
        public bool Lock { get; set; }

        //[CodeAnnotation("强化失败次数")]
        public byte IntensifyFailCount { get; set; }

        //[CodeAnnotation("生星失败次数")]
        public byte StarUpFailCount { get; set; }

        //[CodeAnnotation("成长降级次数")]
        public ushort GrowUpFailCount { get; set; }



        public override string ToString()
        {
            return "";
        }
        public override Item DepCopy()
        {
            var equip = new Equip()
            {
                // GrowUpFailCount = this.GrowUpFailCount,
                //IntensifyFailCount = this.IntensifyFailCount,
                StarUpFailCount = this.StarUpFailCount,
                GrowUp = this.GrowUp,
                IntensifyProperties = this.IntensifyProperties?.CreatCopy(),
                Lock = false,
                MaxGrowUpIndex = this.MaxGrowUpIndex,
                OrgProperties = this.OrgProperties?.CreatCopy(),
                PinZhi = this.PinZhi,
                Pid = this.Pid,
                Skill = this.Skill,
                Star = this.Star,
                TaoZhuang = this.TaoZhuang,
                ToZhuangSkill = this.ToZhuangSkill,
            };
            return equip;
        }
   
   
   
   
   
   
   
        #region AutoProtocol
        public override void Deserialize(BufferReader reader)
        {
            reader.Read(out Single _GrowUp);
            this.GrowUp= _GrowUp;
            reader.Read(out SDictionary<UserProeprtyType,Single> _OrgProperties);
            this.OrgProperties= _OrgProperties;
            reader.Read(out SDictionary<UserProeprtyType,Byte> _IntensifyProperties);
            this.IntensifyProperties= _IntensifyProperties;
            reader.Read(out UInt16 _ToZhuangSkill);
            this.ToZhuangSkill= _ToZhuangSkill;
            reader.Read(out byte _TaoZhuang);
            this.TaoZhuang= (套装类型)_TaoZhuang;
            reader.Read(out UInt16 _Skill);
            this.Skill= _Skill;
            reader.Read(out Byte _Star);
            this.Star= _Star;
            reader.Read(out byte _PinZhi);
            this.PinZhi= (品质)_PinZhi;
            reader.Read(out Boolean _Lock);
            this.Lock= _Lock;
            base.Deserialize(reader);
        }
        public override void Serialize(BufferWriter sender)
        {
            sender.Write(GrowUp);
            sender.Write(OrgProperties);
            sender.Write(IntensifyProperties);
            sender.Write(ToZhuangSkill);
            sender.Write((byte)TaoZhuang);
            sender.Write(Skill);
            sender.Write(Star);
            sender.Write((byte)PinZhi);
            sender.Write(Lock);
            base.Serialize(sender);
        }
        #endregion

    }

    public class Consumption : Item
    {
        public uint Pile { get; set; }
        public override Item DepCopy()
        {
            return new Consumption() { Pile = this.Pile };
        }
   
   
   
   
   
   
   
        #region AutoProtocol
        public override void Deserialize(BufferReader reader)
        {
            reader.Read(out UInt32 _Pile);
            this.Pile= _Pile;
            base.Deserialize(reader);
        }
        public override void Serialize(BufferWriter sender)
        {
            sender.Write(Pile);
            base.Serialize(sender);
        }
        #endregion

    }
}
