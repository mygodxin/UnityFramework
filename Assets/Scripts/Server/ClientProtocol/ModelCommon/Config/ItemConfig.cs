
using KHCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
namespace DuiChongServerCommon.ClientProtocol
{
    public class EquipModel : AMoldeDataBase
    {
        //[CodeAnnotation("职业限制")]
        public 兵种职业 ZhiYe { get; set; }
        //[CodeAnnotation("部位")]
        public EquipPos Pos { get; set; }
        //[CodeAnnotation("源属性")]
        public SList<UserProeprtyType> OrgProperty { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public override void Deserialize(BufferReader reader)
        {
            reader.Read(out byte _ZhiYe);
            this.ZhiYe= (兵种职业)_ZhiYe;
            reader.Read(out byte _Pos);
            this.Pos= (EquipPos)_Pos;
            reader.Read(out SList<UserProeprtyType> _OrgProperty);
            this.OrgProperty= _OrgProperty;
            base.Deserialize(reader);
        }
        public override void Serialize(BufferWriter sender)
        {
            sender.Write((byte)ZhiYe);
            sender.Write((byte)Pos);
            sender.Write(OrgProperty);
            base.Serialize(sender);
        }
        #endregion

    }

    public class GropUpValue : IKHSerializable
    {
        /// <summary>
        /// 成长区间
        /// </summary>
        public Range Range { get; set; }
        /// <summary>
        /// 升级的概率
        /// </summary>
        public double UpProbability { get; set; }
        /// <summary>
        /// 降级的概率
        /// </summary>
        public double DownProbability { get; set; }
        /// <summary>
        /// 成长初始概率
        /// </summary>
        public double RandomProbability { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out Range _Range);
            this.Range= _Range;
            reader.Read(out Double _UpProbability);
            this.UpProbability= _UpProbability;
            reader.Read(out Double _DownProbability);
            this.DownProbability= _DownProbability;
            reader.Read(out Double _RandomProbability);
            this.RandomProbability= _RandomProbability;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(Range);
            sender.Write(UpProbability);
            sender.Write(DownProbability);
            sender.Write(RandomProbability);
        }
        #endregion

    }
    public class ItemConfig : ConfigBase<ItemConfig>
    {

        public Dictionary<byte, RandomCollections<DefaultRandomObject<byte>>> DMJEquipStar = new Dictionary<byte, RandomCollections<DefaultRandomObject<byte>>>();

        protected override void DeserializedInit()
        {
            InitIntensify();
            base.DeserializedInit();
        }
        #region 数据


        public RandomCollections<DefaultRandomObject<List<ushort>>> EquipSkillRandom { get; private set; }
        public SDictionary<string, SList<ushort>> EquipSkillProbability { get; set; }
        //[CodeAnnotation("装备数据")]
        public SDictionary<ushort, EquipModel> EquipModles { get; set; }
        #endregion
        #region 掉落
        /// <summary>
        /// 根据当前最高关卡的计算装备星级掉落增量
        /// </summary>
        /// <param name="highestMissionLevel"></param>
        /// <returns></returns>
        public byte GetCurrentEquipStarIncrease(int highestMissionLevel)
        {
            return (byte)Math.Floor(highestMissionLevel / this.关卡对应装备星级提升);
        }
        /// <summary>
        /// 多少关卡提升一级装备星级
        /// </summary>
        public double 关卡对应装备星级提升 { get; set; }

        public SDictionary<品质, double> 挂机装备掉落件数 { get; set; }
        public SDictionary<品质, double> 挂机装备掉落间隔_秒 { get; set; }
        #endregion
        #region 属性
        //[CodeAnnotation("装备强化属性基础值")]
        /// <summary>
        /// 装备强化属性基础值
        /// </summary>
        public SDictionary<UserProeprtyType, double> EquipIntensifyBaseValues { get; set; }
        //[CodeAnnotation("装备基础属性基础值")]
        /// <summary>
        /// 装备基础属性基础值
        /// </summary>
        public SDictionary<UserProeprtyType, double> EquipPropertyBaseValues { get; set; }
        #endregion
        #region 强化

        public UserProeprtyType[] IntensifyProperties { get; private set; }
        void InitIntensify()
        {
            IntensifyProperties = EquipIntensifyBaseValues.Keys.ToArray();
        }
        public byte 装备最大强化等级 { get; set; }
        public int 装备强化最大词条数量 { get; set; }
        //[CodeAnnotation("装备强化消耗")]
        /// <summary>
        /// 装备强化消耗
        /// </summary>
        public SDictionary<品质, SDictionary<int, Currency>> EquipIntensifyCost { get; set; }
        //[CodeAnnotation("装备强化成功概率")]
        /// <summary>
        /// 装备强化成功概率
        /// </summary>
        public SDictionary<品质, SDictionary<int, double>> EquipIntensifyProbability { get; set; }
        /// <summary>
        /// 装备强化分解价值
        /// </summary>
        //[CodeAnnotation("装备强化分解价值 key1 品质 key2 强化等级")]
        public SDictionary<品质, SDictionary<int, Currency>> EquipIntensifyBack { get; set; }
        #endregion
        #region 星级
        public byte 装备最大星级 { get; set; }
        //[CodeAnnotation("装备生星消耗")]
        /// <summary>
        /// 装备生星消耗
        /// </summary>
        public SDictionary<品质, SDictionary<int, Currency>> EquipStarUpCost { get; set; }
        //[CodeAnnotation("装备生星概率")]
        /// <summary>
        /// 装备生星概率
        /// </summary>
        public SDictionary<品质, SDictionary<int, double>> EquipStarUpProbability { get; set; }
        /// <summary>
        /// 星级分解
        /// </summary>
        //[CodeAnnotation("装备星级分解价值 key1 品质 key2 Equip.Star")]
        public SDictionary<品质, SDictionary<int, Currency>> EquipStarBack { get; set; }
        #endregion
        #region 成长

        public Dictionary<品质, RandomCollections<DefaultRandomObject<Range>>> GrowUpRandoms { get; private set; }

        public SDictionary<品质, SList<GropUpValue>> GrowUpDatas { get; set; }
        #endregion
        #region 洗练
        /// <summary>
        /// 传奇以上装备洗练石分解
        /// </summary>
        //[CodeAnnotation("传奇以上装备洗练石分解")]
        public Currency EquipXLBack { get; set; }
        //[CodeAnnotation("装备强化属性洗练价格 key 当前强化等级 ")]
        public Currency EquipXLQiangHuaPrice { get; set; }
        //[CodeAnnotation("装备技能洗练价格")]
        public Currency EquipXLSkillPrice { get; set; }
        //[CodeAnnotation("装备成长洗练价格")]
        public Currency EquipXLGrowUPPrice { get; set; }
        //[CodeAnnotation("装备资质洗练价格")]
        public Currency EquipZZXLPrice { get; set; }
        #endregion
        #region 装备大作战

        Award EquipBigFightBaodi;

        List<RandomCollections<ChouJiangAward>> EquipBigFightRandom;
        //[CodeAnnotation("装备大作战价格")]
        public SList<Currency> EquipBigFightPrice { get; set; }
        //[CodeAnnotation("装备大作战奖励")]
        public SList<SList<ChouJiangAward>> EquipBigFightAward { get; set; }
        /// <summary>
        /// 装备宝箱大作战开启时间间隔(分钟)
        /// </summary>
        //[CodeAnnotation("装备宝箱大作战开启时间间隔(分钟)")]
        public SList<int> EquipBigFightInterval { get; set; }
        #endregion
   
   
   
   
   
   
   
        #region AutoProtocol
        public override void Deserialize(BufferReader reader)
        {
            reader.Read(out SDictionary<String,SList<UInt16>> _EquipSkillProbability);
            this.EquipSkillProbability= _EquipSkillProbability;
            reader.Read(out SDictionary<UInt16,EquipModel> _EquipModles);
            this.EquipModles= _EquipModles;
            reader.Read(out Double _关卡对应装备星级提升);
            this.关卡对应装备星级提升= _关卡对应装备星级提升;
            reader.Read(out SDictionary<品质,Double> _挂机装备掉落间隔_秒);
            this.挂机装备掉落间隔_秒= _挂机装备掉落间隔_秒;
            reader.Read(out SDictionary<UserProeprtyType,Double> _EquipIntensifyBaseValues);
            this.EquipIntensifyBaseValues= _EquipIntensifyBaseValues;
            reader.Read(out SDictionary<UserProeprtyType,Double> _EquipPropertyBaseValues);
            this.EquipPropertyBaseValues= _EquipPropertyBaseValues;
            reader.Read(out Byte _装备最大强化等级);
            this.装备最大强化等级= _装备最大强化等级;
            reader.Read(out Int32 _装备强化最大词条数量);
            this.装备强化最大词条数量= _装备强化最大词条数量;
            reader.Read(out SDictionary<品质,SDictionary<Int32,Currency>> _EquipIntensifyCost);
            this.EquipIntensifyCost= _EquipIntensifyCost;
            reader.Read(out SDictionary<品质,SDictionary<Int32,Double>> _EquipIntensifyProbability);
            this.EquipIntensifyProbability= _EquipIntensifyProbability;
            reader.Read(out SDictionary<品质,SDictionary<Int32,Currency>> _EquipIntensifyBack);
            this.EquipIntensifyBack= _EquipIntensifyBack;
            reader.Read(out Byte _装备最大星级);
            this.装备最大星级= _装备最大星级;
            reader.Read(out SDictionary<品质,SDictionary<Int32,Currency>> _EquipStarUpCost);
            this.EquipStarUpCost= _EquipStarUpCost;
            reader.Read(out SDictionary<品质,SDictionary<Int32,Double>> _EquipStarUpProbability);
            this.EquipStarUpProbability= _EquipStarUpProbability;
            reader.Read(out SDictionary<品质,SDictionary<Int32,Currency>> _EquipStarBack);
            this.EquipStarBack= _EquipStarBack;
            reader.Read(out SDictionary<品质,SList<GropUpValue>> _GrowUpDatas);
            this.GrowUpDatas= _GrowUpDatas;
            reader.Read(out Currency _EquipXLBack);
            this.EquipXLBack= _EquipXLBack;
            reader.Read(out Currency _EquipXLQiangHuaPrice);
            this.EquipXLQiangHuaPrice= _EquipXLQiangHuaPrice;
            reader.Read(out Currency _EquipXLSkillPrice);
            this.EquipXLSkillPrice= _EquipXLSkillPrice;
            reader.Read(out Currency _EquipXLGrowUPPrice);
            this.EquipXLGrowUPPrice= _EquipXLGrowUPPrice;
            reader.Read(out Currency _EquipZZXLPrice);
            this.EquipZZXLPrice= _EquipZZXLPrice;
            reader.Read(out SList<Currency> _EquipBigFightPrice);
            this.EquipBigFightPrice= _EquipBigFightPrice;
            reader.Read(out SList<SList<ChouJiangAward>> _EquipBigFightAward);
            this.EquipBigFightAward= _EquipBigFightAward;
            reader.Read(out SList<Int32> _EquipBigFightInterval);
            this.EquipBigFightInterval= _EquipBigFightInterval;
            base.Deserialize(reader);
        }
        public override void Serialize(BufferWriter sender)
        {
            sender.Write(EquipSkillProbability);
            sender.Write(EquipModles);
            sender.Write(关卡对应装备星级提升);
            sender.Write(挂机装备掉落间隔_秒);
            sender.Write(EquipIntensifyBaseValues);
            sender.Write(EquipPropertyBaseValues);
            sender.Write(装备最大强化等级);
            sender.Write(装备强化最大词条数量);
            sender.Write(EquipIntensifyCost);
            sender.Write(EquipIntensifyProbability);
            sender.Write(EquipIntensifyBack);
            sender.Write(装备最大星级);
            sender.Write(EquipStarUpCost);
            sender.Write(EquipStarUpProbability);
            sender.Write(EquipStarBack);
            sender.Write(GrowUpDatas);
            sender.Write(EquipXLBack);
            sender.Write(EquipXLQiangHuaPrice);
            sender.Write(EquipXLSkillPrice);
            sender.Write(EquipXLGrowUPPrice);
            sender.Write(EquipZZXLPrice);
            sender.Write(EquipBigFightPrice);
            sender.Write(EquipBigFightAward);
            sender.Write(EquipBigFightInterval);
            base.Serialize(sender);
        }
        #endregion

    }
}
