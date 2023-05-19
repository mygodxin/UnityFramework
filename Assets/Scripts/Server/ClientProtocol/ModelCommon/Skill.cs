
using System;
namespace DuiChongServerCommon.ClientProtocol
{
    public class SkillModel : AMoldeDataBase
    {
        //[CodeAnnotation("最大等级")]
        public ushort MaxLevel { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public override void Deserialize(BufferReader reader)
        {
            reader.Read(out UInt16 _MaxLevel);
            this.MaxLevel= _MaxLevel;
            base.Deserialize(reader);
        }
        public override void Serialize(BufferWriter sender)
        {
            sender.Write(MaxLevel);
            base.Serialize(sender);
        }
        #endregion

    }
    public class BuffModel : AMoldeDataBase
    {
        //[CodeAnnotation("Buff类型")]
        public BUFFType BUFFType { get; set; }
        //[CodeAnnotation("时间计算")]
        public string TimeMath { get; set; }
        //[CodeAnnotation("重复获取间隔时间")]
        public ushort RepeatInterval { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public override void Deserialize(BufferReader reader)
        {
            reader.Read(out byte _BUFFType);
            this.BUFFType= (BUFFType)_BUFFType;
            reader.Read(out String _TimeMath);
            this.TimeMath= _TimeMath;
            reader.Read(out UInt16 _RepeatInterval);
            this.RepeatInterval= _RepeatInterval;
            base.Deserialize(reader);
        }
        public override void Serialize(BufferWriter sender)
        {
            sender.Write((byte)BUFFType);
            sender.Write(TimeMath);
            sender.Write(RepeatInterval);
            base.Serialize(sender);
        }
        #endregion

    }
    public class TaoZhuangSkillModel : SkillModel
    {
        //[CodeAnnotation("触发数量")]
        public byte LimitCount { get; set; }
        //[CodeAnnotation("类型")]
        public 套装类型 Type { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public override void Deserialize(BufferReader reader)
        {
            reader.Read(out Byte _LimitCount);
            this.LimitCount= _LimitCount;
            reader.Read(out byte _Type);
            this.Type= (套装类型)_Type;
            base.Deserialize(reader);
        }
        public override void Serialize(BufferWriter sender)
        {
            sender.Write(LimitCount);
            sender.Write((byte)Type);
            base.Serialize(sender);
        }
        #endregion

    }
}
