using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using KHCore.Utils;

namespace DuiChongServerCommon.ClientProtocol
{
    public class VIPAward : IKHSerializable
    {
        //[CodeAnnotation("每日领取奖励")]
        public OptionalAwards Award { get; set; }
        //[CodeAnnotation("获得Vip时奖励的属性")]
        public SDictionary<UserProeprtyType, double> Properties { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out OptionalAwards _Award);
            this.Award= _Award;
            reader.Read(out SDictionary<UserProeprtyType,Double> _Properties);
            this.Properties= _Properties;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(Award);
            sender.Write(Properties);
        }
        #endregion

    }
    public class UserConfig : ConfigBase<UserConfig>
    {

        public Dictionary<UserInfoType, bool> DefaultUserInfoState { get; private set; }
        public int 最大好友数量 { get; set; }
        public int 最大邮件数量 { get; set; }
        public int 最大好友点赞量 { get; set; }
        public int 最大好友获赞量 { get; set; }
        public byte 最大接受援助次数 { get; set; }
        public int 每日最大援助别人次数 { get; set; }
        public double 援助次数刷新时间_秒 { get; set; }
        public SList<Currency> 初始货币 { get; set; }
        public SList<ushort> 初始兵种 { get; set; }
        public SDictionary<ushort, uint> 初始碎片 { get; set; }
        public SList<RandomEquip> 初始道具 { get; set; }
        //[CodeAnnotation("月卡可选货物 0是不可以选 后边的是可选的")]
        public VIPAward YueKaAward { get; set; }
        //[CodeAnnotation("终身卡奖励")]
        public VIPAward VipAward { get; set; }
        //[CodeAnnotation("改名价格")]
        public Currency ChangeNamePrice { get; set; }
        //[CodeAnnotation("游戏玩法功能开启条件")]
        public SDictionary<Functional, int> FunctionalLock { get; set; }
        //[CodeAnnotation("助阵价格(index为排名-1)")]
        public SList<Currency> HelpPrice { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public override void Deserialize(BufferReader reader)
        {
            reader.Read(out Int32 _最大好友数量);
            this.最大好友数量= _最大好友数量;
            reader.Read(out Int32 _最大邮件数量);
            this.最大邮件数量= _最大邮件数量;
            reader.Read(out Int32 _最大好友点赞量);
            this.最大好友点赞量= _最大好友点赞量;
            reader.Read(out Int32 _最大好友获赞量);
            this.最大好友获赞量= _最大好友获赞量;
            reader.Read(out Byte _最大接受援助次数);
            this.最大接受援助次数= _最大接受援助次数;
            reader.Read(out Int32 _每日最大援助别人次数);
            this.每日最大援助别人次数= _每日最大援助别人次数;
            reader.Read(out Double _援助次数刷新时间_秒);
            this.援助次数刷新时间_秒= _援助次数刷新时间_秒;
            reader.Read(out SList<Currency> _初始货币);
            this.初始货币= _初始货币;
            reader.Read(out SList<UInt16> _初始兵种);
            this.初始兵种= _初始兵种;
            reader.Read(out SDictionary<UInt16,UInt32> _初始碎片);
            this.初始碎片= _初始碎片;
            reader.Read(out SList<RandomEquip> _初始道具);
            this.初始道具= _初始道具;
            reader.Read(out VIPAward _YueKaAward);
            this.YueKaAward= _YueKaAward;
            reader.Read(out VIPAward _VipAward);
            this.VipAward= _VipAward;
            reader.Read(out Currency _ChangeNamePrice);
            this.ChangeNamePrice= _ChangeNamePrice;
            reader.Read(out SDictionary<Functional,Int32> _FunctionalLock);
            this.FunctionalLock= _FunctionalLock;
            reader.Read(out SList<Currency> _HelpPrice);
            this.HelpPrice= _HelpPrice;
            base.Deserialize(reader);
        }
        public override void Serialize(BufferWriter sender)
        {
            sender.Write(最大好友数量);
            sender.Write(最大邮件数量);
            sender.Write(最大好友点赞量);
            sender.Write(最大好友获赞量);
            sender.Write(最大接受援助次数);
            sender.Write(每日最大援助别人次数);
            sender.Write(援助次数刷新时间_秒);
            sender.Write(初始货币);
            sender.Write(初始兵种);
            sender.Write(初始碎片);
            sender.Write(初始道具);
            sender.Write(YueKaAward);
            sender.Write(VipAward);
            sender.Write(ChangeNamePrice);
            sender.Write(FunctionalLock);
            sender.Write(HelpPrice);
            base.Serialize(sender);
        }
        #endregion

    }
}
