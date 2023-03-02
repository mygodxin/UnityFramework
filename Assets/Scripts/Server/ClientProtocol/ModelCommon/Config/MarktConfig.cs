
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DuiChongServerCommon.ClientProtocol
{
    //[CodeAnnotation("商城商品")]
    public class CommodityModel : AMoldeDataBase
    {
        //[CodeAnnotation("区域")]
        public CommodityPos Postion { get; set; }
        //[CodeAnnotation("序号")]
        public int Index { get; set; }
        //[CodeAnnotation("每日购买限制")]
        public int BuyLimitOneDay { get; set; }
        //[CodeAnnotation("总购买限制")]
        public int BuyLimitTotal { get; set; }
        //[CodeAnnotation("货物")]
        public Award Award { get; set; }
        //[CodeAnnotation("价格")]
        public Currency Price { get; set; }
        //[CodeAnnotation("其他价格")]
        public SList<Currency> OtherPrice { get; set; }
        //[CodeAnnotation("动态价格")]
        public bool DynamicPrice { get; set; }
        //[CodeAnnotation("动态货物")]
        public bool DynamicAward { get; set; }
        //[CodeAnnotation("随机货物")]
        public SList<Award> RandomAwards { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public override void Deserialize(BufferReader reader)
        {
            reader.Read(out byte _Postion);
            this.Postion= (CommodityPos)_Postion;
            reader.Read(out Int32 _Index);
            this.Index= _Index;
            reader.Read(out Int32 _BuyLimitOneDay);
            this.BuyLimitOneDay= _BuyLimitOneDay;
            reader.Read(out Int32 _BuyLimitTotal);
            this.BuyLimitTotal= _BuyLimitTotal;
            reader.Read(out Award _Award);
            this.Award= _Award;
            reader.Read(out Currency _Price);
            this.Price= _Price;
            reader.Read(out SList<Currency> _OtherPrice);
            this.OtherPrice= _OtherPrice;
            reader.Read(out Boolean _DynamicPrice);
            this.DynamicPrice= _DynamicPrice;
            reader.Read(out Boolean _DynamicAward);
            this.DynamicAward= _DynamicAward;
            reader.Read(out SList<Award> _RandomAwards);
            this.RandomAwards= _RandomAwards;
            base.Deserialize(reader);
        }
        public override void Serialize(BufferWriter sender)
        {
            sender.Write((byte)Postion);
            sender.Write(Index);
            sender.Write(BuyLimitOneDay);
            sender.Write(BuyLimitTotal);
            sender.Write(Award);
            sender.Write(Price);
            sender.Write(OtherPrice);
            sender.Write(DynamicPrice);
            sender.Write(DynamicAward);
            sender.Write(RandomAwards);
            base.Serialize(sender);
        }
        #endregion

    }
    public class BuyExtraAwardSelect : AwardSelect
    {
        //[CodeAnnotation("选择的技能")]
        public ushort SkillID { get; set; }
        //[CodeAnnotation("选择的职业")]
        public 兵种职业 ZhiYe { get; set; }
        //[CodeAnnotation("选择的装备位置")]
        public EquipPos Pos { get; set; }
        //[CodeAnnotation("可选技能装备的 选择Index")]
        public byte EquipIndex { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public override void Deserialize(BufferReader reader)
        {
            reader.Read(out UInt16 _SkillID);
            this.SkillID= _SkillID;
            reader.Read(out byte _ZhiYe);
            this.ZhiYe= (兵种职业)_ZhiYe;
            reader.Read(out byte _Pos);
            this.Pos= (EquipPos)_Pos;
            reader.Read(out Byte _EquipIndex);
            this.EquipIndex= _EquipIndex;
            base.Deserialize(reader);
        }
        public override void Serialize(BufferWriter sender)
        {
            sender.Write(SkillID);
            sender.Write((byte)ZhiYe);
            sender.Write((byte)Pos);
            sender.Write(EquipIndex);
            base.Serialize(sender);
        }
        #endregion

    }
    public class BuyExtraAward : OptionalAwards
    {
        //[CodeAnnotation("可选的技能的装备")]
        public SList<SHashSet<ushort>> Skills { get; set; }
        #region AutoProtocol
        public override void Deserialize(BufferReader reader)
        {
            reader.Read(out SList<SHashSet<UInt16>> _Skills);
            this.Skills= _Skills;
            base.Deserialize(reader);
        }
        public override void Serialize(BufferWriter sender)
        {
            sender.Write(Skills);
            base.Serialize(sender);
        }
        #endregion

    }
    //[CodeAnnotation("商城配置表")]
    public class MarktConfig : ConfigBase<MarktConfig>
    {
        RandomCollections<DefaultRandomObject<品质>> MangHeRandom;
        public CommodityModel 普通招募商品 { get; set; }
        //[CodeAnnotation("抽奖宝石价格")]
        public Currency 抽奖宝石价格 { get; set; }
        //[CodeAnnotation("抽奖好友积分价格")]
        public Currency 抽奖好友积分价格 { get; set; }
        //[CodeAnnotation("广告区域日观看次数")]
        public int 广告区域日观看次数 { get; set; }
        //[CodeAnnotation("抽奖日限制次数")]
        public int 抽奖日限制次数 { get; set; }
        public int 抽奖宝箱获得次数 { get; set; }
        /// <summary>
        /// 盲盒币最小消费
        /// </summary>
        //[CodeAnnotation("盲盒币最小消费")]
        public double MinMangheCost { get; set; }
        /// <summary>
        /// 盲盒币随机概率
        /// </summary>
        //[CodeAnnotation("盲盒币随机概率")]
        public SDictionary<品质, double> MangheProbability { get; set; }
        /// <summary>
        /// 盲盒币兑换碎片比率
        /// </summary>
        //[CodeAnnotation("盲盒币兑换碎片比率1盲盒币换几个品质碎片")]
        public SDictionary<品质, double> MangheExchange { get; set; }
        //[CodeAnnotation("抽奖物品")]
        public SList<ChouJiangAward> ChouJiangAwards { get; set; }
        //[CodeAnnotation("商品数据")]
        public SDictionary<ushort, CommodityModel> CommodityData { get; set; }
        //[CodeAnnotation("首充可选奖励 index=0 是不可选的 后边的是可选的货物 ")]
        public OptionalAwards ShouChongAward { get; set; }
        /// <summary>
        /// 广告宝箱收益
        /// </summary>
        //[CodeAnnotation("广告宝箱收益")]
        public SList<ScoreAward> ADBoxAward { get; set; }
   
   
   
   
   
   
   
   
        #region AutoProtocol
        public override void Deserialize(BufferReader reader)
        {
            reader.Read(out Currency _抽奖宝石价格);
            this.抽奖宝石价格= _抽奖宝石价格;
            reader.Read(out Currency _抽奖好友积分价格);
            this.抽奖好友积分价格= _抽奖好友积分价格;
            reader.Read(out Int32 _广告区域日观看次数);
            this.广告区域日观看次数= _广告区域日观看次数;
            reader.Read(out Int32 _抽奖日限制次数);
            this.抽奖日限制次数= _抽奖日限制次数;
            reader.Read(out Int32 _抽奖宝箱获得次数);
            this.抽奖宝箱获得次数= _抽奖宝箱获得次数;
            reader.Read(out Double _MinMangheCost);
            this.MinMangheCost= _MinMangheCost;
            reader.Read(out SDictionary<品质,Double> _MangheProbability);
            this.MangheProbability= _MangheProbability;
            reader.Read(out SDictionary<品质,Double> _MangheExchange);
            this.MangheExchange= _MangheExchange;
            reader.Read(out SList<ChouJiangAward> _ChouJiangAwards);
            this.ChouJiangAwards= _ChouJiangAwards;
            reader.Read(out SDictionary<UInt16,CommodityModel> _CommodityData);
            this.CommodityData= _CommodityData;
            reader.Read(out OptionalAwards _ShouChongAward);
            this.ShouChongAward= _ShouChongAward;
            reader.Read(out SList<ScoreAward> _ADBoxAward);
            this.ADBoxAward= _ADBoxAward;
            base.Deserialize(reader);
        }
        public override void Serialize(BufferWriter sender)
        {
            sender.Write(抽奖宝石价格);
            sender.Write(抽奖好友积分价格);
            sender.Write(广告区域日观看次数);
            sender.Write(抽奖日限制次数);
            sender.Write(抽奖宝箱获得次数);
            sender.Write(MinMangheCost);
            sender.Write(MangheProbability);
            sender.Write(MangheExchange);
            sender.Write(ChouJiangAwards);
            sender.Write(CommodityData);
            sender.Write(ShouChongAward);
            sender.Write(ADBoxAward);
            base.Serialize(sender);
        }
        #endregion

    }
}
