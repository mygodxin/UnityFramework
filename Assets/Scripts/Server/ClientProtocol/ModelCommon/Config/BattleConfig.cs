using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using KHCore.Utils;

namespace DuiChongServerCommon.ClientProtocol
{
    //[CodeAnnotation("战斗上阵限制")]
    public class BattleLimit : IKHSerializable
    {
        public 兵种职业 ZhiYe { get; set; }
        public 种族 ZhongZu { get; set; }
        public int Count { get; set; }







        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out byte _ZhiYe);
            this.ZhiYe= (兵种职业)_ZhiYe;
            reader.Read(out byte _ZhongZu);
            this.ZhongZu= (种族)_ZhongZu;
            reader.Read(out Int32 _Count);
            this.Count= _Count;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write((byte)ZhiYe);
            sender.Write((byte)ZhongZu);
            sender.Write(Count);
        }
        #endregion

    }
    //[CodeAnnotation("关卡数据")]
    public class MissionModel : IKHSerializable
    {
        //[CodeAnnotation("奖励")]
        public Award Award { get; set; }







        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out Award _Award);
            this.Award= _Award;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(Award);
        }
        #endregion

    }
    public class FubenModel : AMoldeDataBase
    {
        //[CodeAnnotation("副本类型")]
        public 副本类型 Type { get; set; }
        //[CodeAnnotation("时间限制")]
        public ushort BattleTime { get; set; }
        //[CodeAnnotation("颜色")]
        public string Color { get; set; }
        //[CodeAnnotation("自定义数据")]
        public SDictionary<string, int> CustomData { get; set; }
        //[CodeAnnotation("怪物属性系数")]
        public SDictionary<UserProeprtyType, Range> PropertyCoefficient { get; set; }
        //[CodeAnnotation("副本奖励")]
        public SList<Award> Awards { get; set; }
        //[CodeAnnotation("最大等级")]
        public ushort MaxLevel { get; set; }
        //[CodeAnnotation("进入次数")]
        public uint TimeLimit { get; set; }








        #region AutoProtocol
        public override void Deserialize(BufferReader reader)
        {
            reader.Read(out byte _Type);
            this.Type= (副本类型)_Type;
            reader.Read(out UInt16 _BattleTime);
            this.BattleTime= _BattleTime;
            reader.Read(out String _Color);
            this.Color= _Color;
            reader.Read(out SDictionary<String, Int32> _CustomData);
            this.CustomData= _CustomData;
            reader.Read(out SDictionary<UserProeprtyType, Range> _PropertyCoefficient);
            this.PropertyCoefficient= _PropertyCoefficient;
            reader.Read(out SList<Award> _Awards);
            this.Awards= _Awards;
            reader.Read(out UInt16 _MaxLevel);
            this.MaxLevel= _MaxLevel;
            reader.Read(out UInt32 _TimeLimit);
            this.TimeLimit= _TimeLimit;
            base.Deserialize(reader);
        }
        public override void Serialize(BufferWriter sender)
        {
            sender.Write((byte)Type);
            sender.Write(BattleTime);
            sender.Write(Color);
            sender.Write(CustomData);
            sender.Write(PropertyCoefficient);
            sender.Write(Awards);
            sender.Write(MaxLevel);
            sender.Write(TimeLimit);
            base.Serialize(sender);
        }
        #endregion

    }
    //[CodeAnnotation("活动数据")]
    public class ActivityModel : IKHSerializable
    {
        //[CodeAnnotation("活动类型")]
        public 活动类型 Type { get; set; }
        //[CodeAnnotation("进入次数")]
        public ushort EnterCount { get; set; }
        //[CodeAnnotation("活动排行的奖励(最后一个为参与奖)")]
        public SList<Award> RankingAward { get; set; }
        //[CodeAnnotation("战斗时间限制(秒)")]
        public uint TimeLimit { get; set; }
        //[CodeAnnotation("总时间限制(分钟)")]
        public ushort TimeLimitTotal { get; set; }
        //[CodeAnnotation("怪物属性系数")]
        public SDictionary<UserProeprtyType, Range> PropertyCoefficient { get; set; }
        //[CodeAnnotation("开放时间")]
        public DateTime OpenDate { get; set; }
        //[CodeAnnotation("结束时间")]
        public DateTime CloseDate { get; set; }
        public bool ActivityOnTime()
        {
            var now = DateTime.Now;
            var enterDate = new DateTime(now.Year, now.Month, now.Day, OpenDate.Hour, OpenDate.Minute, OpenDate.Second);
            if (now < enterDate)
            {
                return false;
            }
            var closeDate = new DateTime(now.Year, now.Month, now.Day, CloseDate.Hour, CloseDate.Minute, CloseDate.Second);
            if (now > closeDate)
            {
                return false;
            }
            return true;
        }







        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out byte _Type);
            this.Type= (活动类型)_Type;
            reader.Read(out UInt16 _EnterCount);
            this.EnterCount= _EnterCount;
            reader.Read(out SList<Award> _RankingAward);
            this.RankingAward= _RankingAward;
            reader.Read(out UInt32 _TimeLimit);
            this.TimeLimit= _TimeLimit;
            reader.Read(out UInt16 _TimeLimitTotal);
            this.TimeLimitTotal= _TimeLimitTotal;
            reader.Read(out SDictionary<UserProeprtyType, Range> _PropertyCoefficient);
            this.PropertyCoefficient= _PropertyCoefficient;
            reader.Read(out DateTime _OpenDate);
            this.OpenDate= _OpenDate;
            reader.Read(out DateTime _CloseDate);
            this.CloseDate= _CloseDate;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write((byte)Type);
            sender.Write(EnterCount);
            sender.Write(RankingAward);
            sender.Write(TimeLimit);
            sender.Write(TimeLimitTotal);
            sender.Write(PropertyCoefficient);
            sender.Write(OpenDate);
            sender.Write(CloseDate);
        }
        #endregion

    }
    //[CodeAnnotation("怪物强度修正")]
    public class MonsterFactor : IKHSerializable
    {
        //[CodeAnnotation("属性修正系数")]
        public SDictionary<UserProeprtyType, Range> PropertyFactor { get; set; }
        //[CodeAnnotation("值")]
        public Range Value { get; set; }







        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out SDictionary<UserProeprtyType, Range> _PropertyFactor);
            this.PropertyFactor= _PropertyFactor;
            reader.Read(out Range _Value);
            this.Value= _Value;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(PropertyFactor);
            sender.Write(Value);
        }
        #endregion

    }

    //[CodeAnnotation("战斗配置数据")]
    public class BattleConfig : ConfigBase<BattleConfig>
    {
        protected override void DeserializedInit()
        {
            this.大秘境最大进入次数 = ((byte)Math.Round(24 * 3600 / 大秘境刷新时间_秒));
        }
        public int Boss关卡 { get; set; }
        public int 闯关阶梯等级 { get; set; }
        public double Boss加强系数 { get; set; }
        public int 关卡生命周期 { get; set; }
        public uint 最高关卡等级 { get; set; }
        public double 进阶影响系数 { get; set; }
        public double 品质影响系数 { get; set; }
        public double 等级影响系数 { get; set; }
        public int 基础等级 { get; set; }
        public int 挂机奖励间隔_秒 { get; set; }
        public double 挂机奖励初始系数 { get; set; }
        public double 挂机奖励最大系数 { get; set; }
        public double 大秘境刷新时间_秒 { get; set; }

        public byte 大秘境最大进入次数 { get; set; }
        public double 大秘境难度增加系数 { get; set; }
        public int 大秘境闯关数量 { get; set; }
        public double 大秘境奖励等级增长系数 { get; set; }
        public double 大秘境奖励难度增长系数 { get; set; }
        //[CodeAnnotation("战斗属性基础值")]
        public SDictionary<UserProeprtyType, double> BattleOrgProperty { get; set; }
        //[CodeAnnotation("战斗最大增加系数")]
        public SDictionary<UserProeprtyType, double> BattleMaxCoefficient { get; set; }
        //[CodeAnnotation("羁绊数据")]
        public SDictionary<string, SList<JiBanProperty>> JiBanDatas { get; set; }
        //[CodeAnnotation("精通影响系数")]
        public SDictionary<string, SDictionary<UserProeprtyType, double>> JTCoefficients { get; set; }
        //[CodeAnnotation("关卡数据")]
        public SDictionary<int, MissionModel> MissionModels { get; set; }
        ////[CodeAnnotation("无限关卡重生奖励")]
        //public SList<Award> InfiniteMissionBackAward { get; set; }
        //[CodeAnnotation("关卡重生奖励")]
        public Award GuaJiAward { get; set; }
        /// <summary>
        /// 世界BOSS参与奖
        /// </summary>
        //[CodeAnnotation("世界BOSS参与奖")]
        public Award BossAward { get; set; }
        //[CodeAnnotation("关卡剧情")]
        public SList<string> JuQing { get; set; }
        //[CodeAnnotation("副本数据")]
        public SDictionary<ushort, FubenModel> FubenModels { get; set; }
        //[CodeAnnotation("活动数据")]
        public SDictionary<活动类型, ActivityModel> ActivityModels { get; set; }
        //[CodeAnnotation("关卡怪物强度修正")]
        public SList<MonsterFactor> MonsterFactors { get; set; }
        //[CodeAnnotation("关卡怪物")]
        public SList<SList<ushort>> MissionMonsters { get; set; }
        //[CodeAnnotation("大秘境奖励")]
        public Award DMJAward { get; set; }
        public double GetGoldPlus(int seconds, int starTotal, uint hightMissionLevel)
        {
            var config = BattleConfig.Instance;
            hightMissionLevel -= (hightMissionLevel % (uint)config.闯关阶梯等级);
            hightMissionLevel = ((uint)Math.Clamp(hightMissionLevel, config.闯关阶梯等级, int.MaxValue));
            if (!CardConfig.Instance.总进阶影响金币产出系数.TryGetValue(starTotal, out var coffence1))
            {
                coffence1 = CardConfig.Instance.总进阶影响金币产出系数.Values.Max();
            }
            var coffence2 = CardConfig.Instance.总关卡影响金币产出系数[((int)hightMissionLevel)];
            var gold = CardConfig.Instance.JinBiAwardSec * (1 + coffence1 + coffence2) * seconds;
            return gold;
        }







        #region AutoProtocol
        public override void Deserialize(BufferReader reader)
        {
            reader.Read(out Int32 _Boss关卡);
            this.Boss关卡= _Boss关卡;
            reader.Read(out Int32 _闯关阶梯等级);
            this.闯关阶梯等级= _闯关阶梯等级;
            reader.Read(out Double _Boss加强系数);
            this.Boss加强系数= _Boss加强系数;
            reader.Read(out Int32 _关卡生命周期);
            this.关卡生命周期= _关卡生命周期;
            reader.Read(out UInt32 _最高关卡等级);
            this.最高关卡等级= _最高关卡等级;
            reader.Read(out Double _进阶影响系数);
            this.进阶影响系数= _进阶影响系数;
            reader.Read(out Double _品质影响系数);
            this.品质影响系数= _品质影响系数;
            reader.Read(out Double _等级影响系数);
            this.等级影响系数= _等级影响系数;
            reader.Read(out Int32 _基础等级);
            this.基础等级= _基础等级;
            reader.Read(out Int32 _挂机奖励间隔_秒);
            this.挂机奖励间隔_秒= _挂机奖励间隔_秒;
            reader.Read(out Double _挂机奖励初始系数);
            this.挂机奖励初始系数= _挂机奖励初始系数;
            reader.Read(out Double _挂机奖励最大系数);
            this.挂机奖励最大系数= _挂机奖励最大系数;
            reader.Read(out Double _大秘境刷新时间_秒);
            this.大秘境刷新时间_秒= _大秘境刷新时间_秒;
            reader.Read(out Double _大秘境难度增加系数);
            this.大秘境难度增加系数= _大秘境难度增加系数;
            reader.Read(out Int32 _大秘境闯关数量);
            this.大秘境闯关数量= _大秘境闯关数量;
            reader.Read(out Double _大秘境奖励等级增长系数);
            this.大秘境奖励等级增长系数= _大秘境奖励等级增长系数;
            reader.Read(out Double _大秘境奖励难度增长系数);
            this.大秘境奖励难度增长系数= _大秘境奖励难度增长系数;
            reader.Read(out SDictionary<UserProeprtyType, Double> _BattleOrgProperty);
            this.BattleOrgProperty= _BattleOrgProperty;
            reader.Read(out SDictionary<UserProeprtyType, Double> _BattleMaxCoefficient);
            this.BattleMaxCoefficient= _BattleMaxCoefficient;
            reader.Read(out SDictionary<String, SList<JiBanProperty>> _JiBanDatas);
            this.JiBanDatas= _JiBanDatas;
            reader.Read(out SDictionary<String, SDictionary<UserProeprtyType, Double>> _JTCoefficients);
            this.JTCoefficients= _JTCoefficients;
            reader.Read(out SDictionary<Int32, MissionModel> _MissionModels);
            this.MissionModels= _MissionModels;
            reader.Read(out Award _GuaJiAward);
            this.GuaJiAward= _GuaJiAward;
            reader.Read(out Award _BossAward);
            this.BossAward= _BossAward;
            reader.Read(out SList<String> _JuQing);
            this.JuQing= _JuQing;
            reader.Read(out SDictionary<UInt16, FubenModel> _FubenModels);
            this.FubenModels= _FubenModels;
            reader.Read(out SDictionary<活动类型, ActivityModel> _ActivityModels);
            this.ActivityModels= _ActivityModels;
            reader.Read(out SList<MonsterFactor> _MonsterFactors);
            this.MonsterFactors= _MonsterFactors;
            reader.Read(out SList<SList<UInt16>> _MissionMonsters);
            this.MissionMonsters= _MissionMonsters;
            reader.Read(out Award _DMJAward);
            this.DMJAward= _DMJAward;
            base.Deserialize(reader);
        }
        public override void Serialize(BufferWriter sender)
        {
            sender.Write(Boss关卡);
            sender.Write(闯关阶梯等级);
            sender.Write(Boss加强系数);
            sender.Write(关卡生命周期);
            sender.Write(最高关卡等级);
            sender.Write(进阶影响系数);
            sender.Write(品质影响系数);
            sender.Write(等级影响系数);
            sender.Write(基础等级);
            sender.Write(挂机奖励间隔_秒);
            sender.Write(挂机奖励初始系数);
            sender.Write(挂机奖励最大系数);
            sender.Write(大秘境刷新时间_秒);
            sender.Write(大秘境难度增加系数);
            sender.Write(大秘境闯关数量);
            sender.Write(大秘境奖励等级增长系数);
            sender.Write(大秘境奖励难度增长系数);
            sender.Write(BattleOrgProperty);
            sender.Write(BattleMaxCoefficient);
            sender.Write(JiBanDatas);
            sender.Write(JTCoefficients);
            sender.Write(MissionModels);
            sender.Write(GuaJiAward);
            sender.Write(BossAward);
            sender.Write(JuQing);
            sender.Write(FubenModels);
            sender.Write(ActivityModels);
            sender.Write(MonsterFactors);
            sender.Write(MissionMonsters);
            sender.Write(DMJAward);
            base.Serialize(sender);
        }
        #endregion

    }
    ////[CodeAnnotation("用户的副本数据")]
    //public class FubenData : IKHSerializable
    //{
    //    public ushort Pid { get; set; }
    //    //[CodeAnnotation("进入的次数")]
    //    public byte EnterCount { get; set; }
    //    
    //    //[CodeAnnotation("最后进入的时间")]
    //    public DateTime EnterTime { get; set; }
    //    //[CodeAnnotation("副本等级")]
    //    public ushort Level { get; set; }
    //    public void RefeshEnterTime(FubenModle modle, float userPropertyValue)
    //    {
    //        switch (modle.Type)
    //        {
    //            case 副本类型.日常副本:
    //                RefeshRichangTime(modle, userPropertyValue);
    //                break;
    //            case 副本类型.种族副本:
    //                RefeshZhongzuTime(modle);
    //                break;
    //            default:
    //                break;
    //        }
    //    }
    //    void RefeshZhongzuTime(FubenModle modle)
    //    {
    //        var now = DateTime.Now;
    //        if (!MathTool.IsSameDay(this.EnterTime, now))
    //        {
    //            this.EnterTime = now;
    //            this.EnterCount = modle.EnterCountMax;
    //        }
    //    }
    //    void RefeshRichangTime(FubenModle modle, float userPropertyValue)
    //    {
    //        var now = DateTime.Now;
    //        //当前允许的次数 大于等于最大的次数 (不需要计时)
    //        //当前允许的次数 大于等于最大的次数 (不需要计时)
    //        if (this.EnterCount >= modle.EnterCountMax)
    //        {
    //            this.EnterTime = now;
    //            this.EnterCount = modle.EnterCountMax;
    //            return;
    //        }
    //        var passSec = (now - EnterTime).TotalSeconds;
    //        var maxCount = modle.EnterCountMax;
    //        var plusIntervalSec = (float)modle.RefeshEnterTimeMin * 60 * (1 - userPropertyValue);
    //        var temp = Math.Floor(passSec / plusIntervalSec);
    //        byte plusCount = (temp > maxCount ? maxCount : (byte)temp);
    //        if (plusCount <= 0)
    //        {
    //            return;
    //        }
    //        var oldCount = this.EnterCount;
    //        this.EnterCount += plusCount;
    //        this.EnterCount = this.EnterCount > maxCount ? maxCount : this.EnterCount;
    //        var countOffset = this.EnterCount - oldCount;
    //        if (EnterCount >= maxCount)
    //        {
    //            this.EnterTime = now;
    //        }
    //        else if (countOffset > 0)
    //        {
    //            this.EnterTime = this.EnterTime.AddSeconds(countOffset * plusIntervalSec);
    //        }
    //    }
    //    #region AutoProtocol
    //    public virtual void Deserialize(BufferReader reader)
    //    {
    //        reader.Read(out UInt16 _Pid);
    //        this.Pid = _Pid;
    //        reader.Read(out Byte _EnterCount);
    //        this.EnterCount = _EnterCount;
    //        reader.Read(out DateTime _EnterTime);
    //        this.EnterTime = _EnterTime;
    //        reader.Read(out UInt16 _Level);
    //        this.Level = _Level;
    //    }
    //    public virtual void Serialize(BufferWriter sender)
    //    {
    //        sender.Write(Pid);
    //        sender.Write(EnterCount);
    //        sender.Write(EnterTime);
    //        sender.Write(Level);
    //    }
    //    #endregion
    //}
}
