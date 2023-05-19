

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuiChongServerCommon.ClientProtocol
{

    public class VipData : IKHSerializable
    {
        public static VipData CreatNew()
        {
            return new VipData();
        }
        //[CodeAnnotation("用户的终身VIP等级")]
        public byte VIPLevel { get; set; }
        //[CodeAnnotation("终身VIP奖励领取时间")]
        
        public DateTime VIPAwardTime { get; set; }
        //[CodeAnnotation("月卡到期时间")]
        
        public DateTime VipYueKa { get; set; }
        //[CodeAnnotation("月卡奖励领取时间")]
        
        public DateTime YueKaAwardTime { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out Byte _VIPLevel);
            this.VIPLevel= _VIPLevel;
            reader.Read(out DateTime _VIPAwardTime);
            this.VIPAwardTime= _VIPAwardTime;
            reader.Read(out DateTime _VipYueKa);
            this.VipYueKa= _VipYueKa;
            reader.Read(out DateTime _YueKaAwardTime);
            this.YueKaAwardTime= _YueKaAwardTime;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(VIPLevel);
            sender.Write(VIPAwardTime);
            sender.Write(VipYueKa);
            sender.Write(YueKaAwardTime);
        }
        #endregion

    }

    public class FubenData : IKHSerializable
    {
        public ushort Pid { get; set; }
        
        //[CodeAnnotation("上一次进入时间")]
        public DateTime EnterDate { get; set; }
        //[CodeAnnotation("当前等级")]
        public ushort Level { get; set; }


        public 副本类型 Type => BattleConfig.Instance.FubenModels[Pid].Type;
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out UInt16 _Pid);
            this.Pid= _Pid;
            reader.Read(out DateTime _EnterDate);
            this.EnterDate= _EnterDate;
            reader.Read(out UInt16 _Level);
            this.Level= _Level;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(Pid);
            sender.Write(EnterDate);
            sender.Write(Level);
        }
        #endregion

    }

    public class ActivityData : IKHSerializable
    {

        public double RankingValue { get; set; }
        //[CodeAnnotation("进入次数,每日清0")]
        public ushort EnterTime { get; set; }
        public void Rest()
        {
            RankingValue = 0;
            EnterTime = 0;
        }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out UInt16 _EnterTime);
            this.EnterTime= _EnterTime;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(EnterTime);
        }
        #endregion

    }

    public class BattleData : IKHSerializable
    {
        public static BattleData CreatNew()
        {
            var data = new BattleData();
            data.CurrentMissionLevel = 1;
            data.HightestMissionLevel = 1;
            data.BattleCards = new SDictionary<BattleType, SDictionary<ushort, Pos>>();
            data.FubenDatas = new SDictionary<ushort, FubenData>();
            var modles = BattleConfig.Instance.FubenModels.Values;
            foreach (var item in modles)
            {
                data.FubenDatas.Add(item.Pid, new FubenData() { Level = 1, Pid = item.Pid });
            }
            data.ArenaMatchCounts = new SDictionary<BattleType, uint>();
            return data;
        }

        public SDictionary<BattleType, uint> ArenaMatchCounts { get; set; }
        //[CodeAnnotation("竞技场段位分数")]
        public int ArenaScore { get; set; }
        //[CodeAnnotation("历史最高分数")]
        public uint ArenaHightestScore { get; set; }
        //[CodeAnnotation("竞技场总场次")]
        public uint ArenaMatchCount { get; set; }
        //[CodeAnnotation("总胜场")]
        public uint ArenaWinTotal { get; set; }
        //[CodeAnnotation("领取的奖励次数")]
        public ushort ArenaAwardIndex { get; set; }

        //[CodeAnnotation("今日竞技场胜利场次")]
        public uint ArenaWinToday { get; set; }
        //[CodeAnnotation("关卡当前等级")]
        public uint CurrentMissionLevel { get; set; } = 1;
        //[CodeAnnotation("历史最高等级")]
        public uint HightestMissionLevel { get; set; } = 1;
        //[CodeAnnotation("闯关奖励领取次数")]
        public ushort MissionAwardInex { get; set; }
        //[CodeAnnotation("战斗的上阵卡牌")]
        public SDictionary<BattleType, SDictionary<ushort, Pos>> BattleCards { get; set; }
        //[CodeAnnotation("副本数据")]
        public SDictionary<ushort, FubenData> FubenDatas { get; set; }
        //[CodeAnnotation("活动数据")]
        public SDictionary<活动类型, ActivityData> ActivitiesDatas { get; set; }
        //[CodeAnnotation("战斗怪物记录")]
        public SDictionary<BattleType, SList<ushort>> Monsters { get; set; }
        //[CodeAnnotation("大秘境的允许进入次数")]
        public byte DMJEnterCount { get; set; }
        //[CodeAnnotation("大秘境的难度等级   (每次战斗胜利后自增,每日赛季结算设置为1)")]
        public byte DMJHardLevel { get; set; }
        //[CodeAnnotation("大秘境次数刷新时间")]
        
        public DateTime DMJRefeshDate { get; set; }
        //[CodeAnnotation("接受战斗援助次数")]
        public byte GetHelpCount { get; set; }
        //[CodeAnnotation("援助次数刷新时间")]
        
        public DateTime GetHelpRefeshDate { get; set; }
   
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out Int32 _ArenaScore);
            this.ArenaScore= _ArenaScore;
            reader.Read(out UInt32 _ArenaHightestScore);
            this.ArenaHightestScore= _ArenaHightestScore;
            reader.Read(out UInt32 _ArenaMatchCount);
            this.ArenaMatchCount= _ArenaMatchCount;
            reader.Read(out UInt32 _ArenaWinTotal);
            this.ArenaWinTotal= _ArenaWinTotal;
            reader.Read(out UInt16 _ArenaAwardIndex);
            this.ArenaAwardIndex= _ArenaAwardIndex;
            reader.Read(out UInt32 _CurrentMissionLevel);
            this.CurrentMissionLevel= _CurrentMissionLevel;
            reader.Read(out UInt32 _HightestMissionLevel);
            this.HightestMissionLevel= _HightestMissionLevel;
            reader.Read(out UInt16 _MissionAwardInex);
            this.MissionAwardInex= _MissionAwardInex;
            reader.Read(out SDictionary<BattleType,SDictionary<UInt16,Pos>> _BattleCards);
            this.BattleCards= _BattleCards;
            reader.Read(out SDictionary<UInt16,FubenData> _FubenDatas);
            this.FubenDatas= _FubenDatas;
            reader.Read(out SDictionary<活动类型,ActivityData> _ActivitiesDatas);
            this.ActivitiesDatas= _ActivitiesDatas;
            reader.Read(out SDictionary<BattleType,SList<UInt16>> _Monsters);
            this.Monsters= _Monsters;
            reader.Read(out Byte _DMJEnterCount);
            this.DMJEnterCount= _DMJEnterCount;
            reader.Read(out Byte _DMJHardLevel);
            this.DMJHardLevel= _DMJHardLevel;
            reader.Read(out DateTime _DMJRefeshDate);
            this.DMJRefeshDate= _DMJRefeshDate;
            reader.Read(out Byte _GetHelpCount);
            this.GetHelpCount= _GetHelpCount;
            reader.Read(out DateTime _GetHelpRefeshDate);
            this.GetHelpRefeshDate= _GetHelpRefeshDate;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(ArenaScore);
            sender.Write(ArenaHightestScore);
            sender.Write(ArenaMatchCount);
            sender.Write(ArenaWinTotal);
            sender.Write(ArenaAwardIndex);
            sender.Write(CurrentMissionLevel);
            sender.Write(HightestMissionLevel);
            sender.Write(MissionAwardInex);
            sender.Write(BattleCards);
            sender.Write(FubenDatas);
            sender.Write(ActivitiesDatas);
            sender.Write(Monsters);
            sender.Write(DMJEnterCount);
            sender.Write(DMJHardLevel);
            sender.Write(DMJRefeshDate);
            sender.Write(GetHelpCount);
            sender.Write(GetHelpRefeshDate);
        }
        #endregion

    }

    //[CodeAnnotation("离线奖励")]
    public class OutlineData : IKHSerializable
    {
        //[CodeAnnotation("离线闯关数量")]
        public int MissionLevel { get; set; }
        //[CodeAnnotation("离线奖励")]
        public Award OutlineAward { get; set; }
        //[CodeAnnotation("离线时间(秒)")]
        public uint OutlineTimeSec { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out Int32 _MissionLevel);
            this.MissionLevel= _MissionLevel;
            reader.Read(out Award _OutlineAward);
            this.OutlineAward= _OutlineAward;
            reader.Read(out UInt32 _OutlineTimeSec);
            this.OutlineTimeSec= _OutlineTimeSec;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(MissionLevel);
            sender.Write(OutlineAward);
            sender.Write(OutlineTimeSec);
        }
        #endregion

    }

    public class TinyUserList : IKHSerializable
    {
        public TinyUserList()
        {
        }
        public TinyUserList(bool creat)
        {
            UserInfoList = new();
            UserId = new();
        }

        public int Count => UserId == null ? 0 : UserId.Count;

        public SList<UserInfoTiny> UserInfoList { get; set; }

        public SHashSet<uint> UserId { get; set; }
        public bool Contains(uint uerID)
        {
            return this.UserId.Contains(uerID);
        }
        public void Add(UserInfoTiny user)
        {
            if (!this.UserId.Contains(user.UserID))
            {
                this.UserId.Add(user.UserID);
                this.UserInfoList?.Add(user);
            }
        }
        public bool Remove(uint userID, out UserInfoTiny userInfoTiny)
        {
            userInfoTiny = null;
            if (this.UserId.Remove(userID))
            {
                if (UserInfoList != null)
                {
                    for (int i = 0; i < UserInfoList.Count; i++)
                    {
                        if (UserInfoList[i].UserID == userID)
                        {
                            userInfoTiny = UserInfoList[i];
                            UserInfoList.RemoveAt(i);
                            break;
                        }
                    }
                }
                return true;
            }
            return false;
        }
        //public void Refesh(IDictionary<uint, UserInfoTiny> pairs)
        //{
        //    UserInfoList ??= new SList<UserInfoTiny>();
        //    this.UserInfoList.Clear();
        //    List<uint>? needRemove = null;
        //    foreach (var item in this.UserId)
        //    {
        //        if (pairs.TryGetValue(item, out var userInfoTiny))
        //        {
        //            this.UserInfoList.Add(userInfoTiny);
        //        }
        //        else
        //        {
        //            needRemove ??= new List<uint>();
        //            needRemove.Add(item);
        //        }
        //    }
        //    if (needRemove != null)
        //    {
        //        foreach (var item in needRemove)
        //        {
        //            this.UserId.Remove(item);
        //        }
        //    }
        //}
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out SList<UserInfoTiny> _UserInfoList);
            this.UserInfoList= _UserInfoList;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(UserInfoList);
        }
        #endregion

    }
    //[CodeAnnotation("社交数据")]

    public class SocialData : IKHSerializable
    {
        public static SocialData CreatNew()
        {
            return new SocialData()
            {
                FriendApplyList = new(true),
                FriendList = new(true),
                FromFriendZan = new SDictionary<uint, uint>(),
                Mails = new SDictionary<uint, Mail>(),
            };
        }
        //[CodeAnnotation("好友列表")]
        public TinyUserList FriendList { get; set; }
        //[CodeAnnotation("申请列表")]
        public TinyUserList FriendApplyList { get; set; }
        //[CodeAnnotation("邮件")]
        public SDictionary<uint, Mail> Mails { get; set; }
        //[CodeAnnotation("今日给别人赞的次数")]
        public ushort ZanCountToday { get; set; }
        //[CodeAnnotation("好友给自己点赞的次数")]
        public SDictionary<uint, uint> FromFriendZan { get; set; }

        //[CodeAnnotation("今日已获得的赞量,最大不可超过 UserConfig.Instance.最大好友获赞量  (日结算清0)")]
        public ushort GetedZanCountToday { get; set; }
        

        public DateTime GetedZanCountRefeshDate { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out TinyUserList _FriendList);
            this.FriendList= _FriendList;
            reader.Read(out TinyUserList _FriendApplyList);
            this.FriendApplyList= _FriendApplyList;
            reader.Read(out SDictionary<UInt32,Mail> _Mails);
            this.Mails= _Mails;
            reader.Read(out UInt16 _ZanCountToday);
            this.ZanCountToday= _ZanCountToday;
            reader.Read(out SDictionary<UInt32,UInt32> _FromFriendZan);
            this.FromFriendZan= _FromFriendZan;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(FriendList);
            sender.Write(FriendApplyList);
            sender.Write(Mails);
            sender.Write(ZanCountToday);
            sender.Write(FromFriendZan);
        }
        #endregion

    }

    public class CheatData
    {
        /// <summary>
        /// 最后检测时间
        /// </summary>
       // 
        // public DateTime LastCheck { get; set; }
        /// <summary>
        /// 作弊次数
        /// </summary>
        public Dictionary<CheatType, int> CheatCount { get; set; }
        public Dictionary<CheatType, DateTime> CheatTime { get; set; }
    }

    public class GuaJiData : IKHSerializable
    {
        //[CodeAnnotation("挂机的奖励货币")]
        public SDictionary<CurrencyType, double> CurrencyValues { get; set; }
        //[CodeAnnotation("挂机奖励的装备 全是1星的 key是品质 value是数量")]
        public SDictionary<品质, uint> Equips { get; set; }
        /// <summary>
        /// 货币最后刷新的时间
        /// </summary>

        
        public DateTime CurrencyRefeshDate { get; set; }
        /// <summary>
        /// 装备最后刷新的时间
        /// </summary>

        
        public Dictionary<品质, DateTime> EquipRefeshDate { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out SDictionary<CurrencyType,Double> _CurrencyValues);
            this.CurrencyValues= _CurrencyValues;
            reader.Read(out SDictionary<品质,UInt32> _Equips);
            this.Equips= _Equips;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(CurrencyValues);
            sender.Write(Equips);
        }
        #endregion

    }

    public class User :  IKHSerializable
    {

        public bool Alive { get; set; } = true;

        public string Account { get; set; }
        /// <summary>
        /// 表示此人是否可见
        /// </summary>

        public bool Visible { get; set; }
        public Platform Platform { get; set; }

        //[CodeAnnotation("此游戏的用户数据库唯一ID")]
        public uint UserID;
        //[CodeAnnotation("昵称")]
        public string Name { get; set; }
        //[CodeAnnotation("头像URL 如果为空 As the default avatar")]
        public string AvatarUrl { get; set; }
        //[CodeAnnotation("总在线时长(分钟)")]
        public double OnlineTimesTotal { get; set; }
        
        //[CodeAnnotation("下线时间")]
        public DateTime OutlineTime { get; set; }
        //[CodeAnnotation("信息展示开关")]
        public SDictionary<UserInfoType, bool> InfoShow { get; set; }
        //[CodeAnnotation("市场数据")]
        public MarktData MarktData { get; set; }
        //[CodeAnnotation("玩家的所有货币 key 是货币类型 value 是数量")]
        public SDictionary<CurrencyType, double> UserCurrency { get; set; }
        //[CodeAnnotation("玩家拥有的所有卡片 key pid ")]
        public SDictionary<ushort, Card> Cards { get; set; }
        //[CodeAnnotation("所有卡片的皮肤")]
        public SDictionary<ushort, SHashSet<byte>> CardSkins { get; set; }
        //[CodeAnnotation("当前星座总下标")]
        public byte XingZuo { get; set; }
        //[CodeAnnotation("当前星座的属性下标")]
        public byte XingZuoLevel { get; set; }
        //[CodeAnnotation("当前星座战斗等级  战斗回复成功后自增 日结算重置为1 默认1")]
        public ushort XingZuoBattleLevel { get; set; }
        //[CodeAnnotation("卡牌碎片 key pid value 数量")]
        public SDictionary<ushort, int> CardDebris { get; set; }
        /// <summary>
        /// 任务数据
        /// </summary>
        public TaskData TaskData { get; set; }
        //[CodeAnnotation("战斗数据")]
        public BattleData BattleDatas { get; set; }
        //[CodeAnnotation("已经领取过的兑换码")]
        public SHashSet<string> Duihuanma { get; set; }
        
        //[CodeAnnotation("礼包码兑换时间")]
        public DateTime DuiHuanMaTime { get; set; }
        //[CodeAnnotation("社交数据")]
        public SocialData SocialData { get; set; }
        //[CodeAnnotation("拥有的神器")]
        public SDictionary<ushort, ShenQi> ShengQi { get; set; }
        //[CodeAnnotation("天赋")]
        public SDictionary<种族, SList<TianfuRect>> Tianfu { get; set; }
        //[CodeAnnotation("道具")]
        public SDictionary<uint, Item> Bag { get; set; }

        public uint ItemCurrentID { get; set; }
        //[CodeAnnotation("VIP数据")]
        public VipData VipData { get; set; }

        public uint MailCurrentID { get; set; }
        //[CodeAnnotation("服务器开放的总自然日")]
        public ushort OpenDays { get; set; }
        //[CodeAnnotation("服务器开放的总自然周")]
        public ushort OpenWeeks { get; set; }
        //[CodeAnnotation("服务器开放的总自然月")]
        public ushort OpenMonthes { get; set; }
        

        public DateTime MonthSeasonTime { get; set; }
        

        public DateTime DaySeasonTime { get; set; }
        

        public DateTime WeekSeasonTime { get; set; }


        public DateTime OnlineTime { get; set; }

        public UserProperty UserProperties { get; set; }

        public SDictionary<ClientEvent, bool> EventListener { get; set; }
        /// <summary>
        /// 已经领取过的礼包码
        /// </summary>

        public HashSet<string> GettedLibao { get; set; }
        //[CodeAnnotation("用户行为记录,用于记录一写乱七八糟的行为 用于判断行为次数")]
        public SDictionary<UserBehavior, uint> UserBehaviors { get; set; }
        //[CodeAnnotation("挂机数据")]
        public GuaJiData GuaJiData { get; set; }
        //[CodeAnnotation("挂机时间(秒)")]
        public int GuajiSec { get; set; }
        /// <summary>
        /// 作弊数据
        /// </summary>

        public CheatData CheatDatas { get; set; }
        /// <summary>
        /// 新号七天乐
        /// </summary>

        public byte SevenDayAward { get; set; }
        //[CodeAnnotation("自定义活动数据")]
        public SDictionary<CustomActivity, CustomActivityData> CustomActivityDatas { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out byte _Platform);
            this.Platform= (Platform)_Platform;
            reader.Read(out UInt32 _UserID);
            reader.Read(out String _Name);
            this.Name= _Name;
            reader.Read(out String _AvatarUrl);
            this.AvatarUrl= _AvatarUrl;
            reader.Read(out Double _OnlineTimesTotal);
            this.OnlineTimesTotal= _OnlineTimesTotal;
            reader.Read(out DateTime _OutlineTime);
            this.OutlineTime= _OutlineTime;
            reader.Read(out SDictionary<UserInfoType,Boolean> _InfoShow);
            this.InfoShow= _InfoShow;
            reader.Read(out MarktData _MarktData);
            this.MarktData= _MarktData;
            reader.Read(out SDictionary<CurrencyType,Double> _UserCurrency);
            this.UserCurrency= _UserCurrency;
            reader.Read(out SDictionary<UInt16,Card> _Cards);
            this.Cards= _Cards;
            reader.Read(out SDictionary<UInt16,SHashSet<Byte>> _CardSkins);
            this.CardSkins= _CardSkins;
            reader.Read(out Byte _XingZuo);
            this.XingZuo= _XingZuo;
            reader.Read(out Byte _XingZuoLevel);
            this.XingZuoLevel= _XingZuoLevel;
            reader.Read(out UInt16 _XingZuoBattleLevel);
            this.XingZuoBattleLevel= _XingZuoBattleLevel;
            reader.Read(out SDictionary<UInt16,Int32> _CardDebris);
            this.CardDebris= _CardDebris;
            reader.Read(out TaskData _TaskData);
            this.TaskData= _TaskData;
            reader.Read(out BattleData _BattleDatas);
            this.BattleDatas= _BattleDatas;
            reader.Read(out SHashSet<String> _Duihuanma);
            this.Duihuanma= _Duihuanma;
            reader.Read(out DateTime _DuiHuanMaTime);
            this.DuiHuanMaTime= _DuiHuanMaTime;
            reader.Read(out SocialData _SocialData);
            this.SocialData= _SocialData;
            reader.Read(out SDictionary<UInt16,ShenQi> _ShengQi);
            this.ShengQi= _ShengQi;
            reader.Read(out SDictionary<种族,SList<TianfuRect>> _Tianfu);
            this.Tianfu= _Tianfu;
            reader.Read(out SDictionary<UInt32,Item> _Bag);
            this.Bag= _Bag;
            reader.Read(out VipData _VipData);
            this.VipData= _VipData;
            reader.Read(out UInt16 _OpenDays);
            this.OpenDays= _OpenDays;
            reader.Read(out UInt16 _OpenWeeks);
            this.OpenWeeks= _OpenWeeks;
            reader.Read(out UInt16 _OpenMonthes);
            this.OpenMonthes= _OpenMonthes;
            reader.Read(out UserProperty _UserProperties);
            this.UserProperties= _UserProperties;
            reader.Read(out SDictionary<ClientEvent,Boolean> _EventListener);
            this.EventListener= _EventListener;
            reader.Read(out SDictionary<UserBehavior,UInt32> _UserBehaviors);
            this.UserBehaviors= _UserBehaviors;
            reader.Read(out GuaJiData _GuaJiData);
            this.GuaJiData= _GuaJiData;
            reader.Read(out Int32 _GuajiSec);
            this.GuajiSec= _GuajiSec;
            reader.Read(out SDictionary<CustomActivity,CustomActivityData> _CustomActivityDatas);
            this.CustomActivityDatas= _CustomActivityDatas;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write((byte)Platform);
            sender.Write(UserID);
            sender.Write(Name);
            sender.Write(AvatarUrl);
            sender.Write(OnlineTimesTotal);
            sender.Write(OutlineTime);
            sender.Write(InfoShow);
            sender.Write(MarktData);
            sender.Write(UserCurrency);
            sender.Write(Cards);
            sender.Write(CardSkins);
            sender.Write(XingZuo);
            sender.Write(XingZuoLevel);
            sender.Write(XingZuoBattleLevel);
            sender.Write(CardDebris);
            sender.Write(TaskData);
            sender.Write(BattleDatas);
            sender.Write(Duihuanma);
            sender.Write(DuiHuanMaTime);
            sender.Write(SocialData);
            sender.Write(ShengQi);
            sender.Write(Tianfu);
            sender.Write(Bag);
            sender.Write(VipData);
            sender.Write(OpenDays);
            sender.Write(OpenWeeks);
            sender.Write(OpenMonthes);
            sender.Write(UserProperties);
            sender.Write(EventListener);
            sender.Write(UserBehaviors);
            sender.Write(GuaJiData);
            sender.Write(GuajiSec);
            sender.Write(CustomActivityDatas);
        }
        #endregion

    }
    //[CodeAnnotation("用户信息")]
    public class UserInfo : IKHSerializable
    {
        public UserInfo()
        {
        }
        /// <summary>
        /// 真实玩家构造函数
        /// </summary>
        /// <param name="user"></param>
        public UserInfo(User user, bool isOnline)
        {
            this.UserID = user.UserID;
            this.Name = user.Name;
            this.AvatarUrl = user.AvatarUrl;
            this.Cards = user.Cards;
            this.UserProperty = new UserProperty(user);
            this.IsOnline = isOnline;
            this.InfoShow = user.InfoShow;
            this.OnlineTimesTotal = user.OnlineTimesTotal;
            this.HightestMissionLevel = user.BattleDatas.HightestMissionLevel;
            this.ArenaMatchCount = user.BattleDatas.ArenaMatchCount;
            this.ArenaWinTotal = user.BattleDatas.ArenaWinTotal;
            this.ArenaHightestScore = user.BattleDatas.ArenaHightestScore;
            this.BattleCards = user.BattleDatas.BattleCards;
            this.XingZuo=user.XingZuo;
        }
        //[CodeAnnotation("关卡历史最高等级")]
        public uint HightestMissionLevel { get; set; }
        //[CodeAnnotation("竞技场总场次")]
        public uint ArenaMatchCount { get; set; }
        //[CodeAnnotation("竞技场总胜场")]
        public uint ArenaWinTotal { get; set; }
        //[CodeAnnotation("竞技场历史最高分数")]
        public uint ArenaHightestScore { get; set; }
        //[CodeAnnotation("战斗卡牌")]
        public SDictionary<BattleType, SDictionary<ushort, Pos>> BattleCards { get; set; }
        //[CodeAnnotation("信息展示开关")]
        public SDictionary<UserInfoType, bool> InfoShow { get; set; }
        //[CodeAnnotation("此游戏的用户数据库唯一ID")]
        public uint UserID { get; set; }
        //[CodeAnnotation("昵称")]
        public string Name { get; set; }
        //[CodeAnnotation("头像URL 如果为空 为默认头像")]
        public string AvatarUrl { get; set; }
        //[CodeAnnotation("所有卡牌")]
        public SDictionary<ushort, Card> Cards { get; set; }
        public bool IsOnline { get; set; }
        public UserProperty UserProperty { get; set; }
        public double OnlineTimesTotal { get; set; }
        public byte XingZuo { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out UInt32 _HightestMissionLevel);
            this.HightestMissionLevel= _HightestMissionLevel;
            reader.Read(out UInt32 _ArenaMatchCount);
            this.ArenaMatchCount= _ArenaMatchCount;
            reader.Read(out UInt32 _ArenaWinTotal);
            this.ArenaWinTotal= _ArenaWinTotal;
            reader.Read(out UInt32 _ArenaHightestScore);
            this.ArenaHightestScore= _ArenaHightestScore;
            reader.Read(out SDictionary<BattleType,SDictionary<UInt16,Pos>> _BattleCards);
            this.BattleCards= _BattleCards;
            reader.Read(out SDictionary<UserInfoType,Boolean> _InfoShow);
            this.InfoShow= _InfoShow;
            reader.Read(out UInt32 _UserID);
            this.UserID= _UserID;
            reader.Read(out String _Name);
            this.Name= _Name;
            reader.Read(out String _AvatarUrl);
            this.AvatarUrl= _AvatarUrl;
            reader.Read(out SDictionary<UInt16,Card> _Cards);
            this.Cards= _Cards;
            reader.Read(out Boolean _IsOnline);
            this.IsOnline= _IsOnline;
            reader.Read(out UserProperty _UserProperty);
            this.UserProperty= _UserProperty;
            reader.Read(out Double _OnlineTimesTotal);
            this.OnlineTimesTotal= _OnlineTimesTotal;
            reader.Read(out Byte _XingZuo);
            this.XingZuo= _XingZuo;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(HightestMissionLevel);
            sender.Write(ArenaMatchCount);
            sender.Write(ArenaWinTotal);
            sender.Write(ArenaHightestScore);
            sender.Write(BattleCards);
            sender.Write(InfoShow);
            sender.Write(UserID);
            sender.Write(Name);
            sender.Write(AvatarUrl);
            sender.Write(Cards);
            sender.Write(IsOnline);
            sender.Write(UserProperty);
            sender.Write(OnlineTimesTotal);
            sender.Write(XingZuo);
        }
        #endregion

    }

    public class UserInfoTiny : IKHSerializable
    {
        public UserInfoTiny()
        {
        }
        public UserInfoTiny(uint userID, string name, string avatarUrl, string account,bool visible)
        {
            UserID = userID;
            Name = name;
            AvatarUrl = avatarUrl;
            Account = account;
            this.Visible = visible; 
        }
        public UserInfoTiny Refesh(User user)
        {
            this.UserID = user.UserID;
            this.Name = user.Name;
            this.AvatarUrl = user.AvatarUrl;
            this.Account = user.Account;
            this.Visible=user.Visible;
            return this;
        }
        //[CodeAnnotation("此游戏的用户数据库唯一ID")]
        public uint UserID { get; private set; }
        //[CodeAnnotation("昵称")]
        public string Name { get; private set; }
        //[CodeAnnotation("头像URL 如果为空 为默认头像")]
        public string AvatarUrl { get; private set; }


        public string Account { get; private set; }

        public bool Visible { get; private set; }
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out UInt32 _UserID);
            this.UserID= _UserID;
            reader.Read(out String _Name);
            this.Name= _Name;
            reader.Read(out String _AvatarUrl);
            this.AvatarUrl= _AvatarUrl;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(UserID);
            sender.Write(Name);
            sender.Write(AvatarUrl);
        }
        #endregion

    }
    public class EnumArray<T> where T : struct, Enum
    {
        public T[] Array { get; private set; }
        public EnumArray()
        {
            var arr = Enum.GetValues(typeof(T));
            Array = new T[arr.Length];
            var index = 0;
            foreach (var item in arr)
            {
                this.Array[index++] = (T)item;
            }
        }
        public EnumArray<T> Remove(T enumValue)
        {
            var lis = Array.ToList();
            lis.Remove(enumValue);
            Array = lis.ToArray();
            return this;
        }
        ///// <summary>
        ///// 获取所有<=mark 的枚举值
        ///// </summary>
        ///// <param name="mark"></param>
        ///// <returns></returns>
        //public List<T> GetUnderEq(T mark)
        //{
        //    var lis = new List<T>();
        //    foreach (var item in this.Array)
        //    {
        //        if (((int)(object)item)<=((int)(object)mark))
        //        {
        //            lis.Add(item);
        //        }
        //    }
        //    return lis;
        //}
        ///// <summary>
        ///// 获取所有>=mark 的枚举值
        ///// </summary>
        ///// <param name="mark"></param>
        ///// <returns></returns>
        //public List<T> GetUpEq(T mark)
        //{
        //    var lis = new List<T>();
        //    foreach (var item in this.Array)
        //    {
        //        if (((int)(object)item)>=((int)(object)mark))
        //        {
        //            lis.Add(item);
        //        }
        //    }
        //    return lis;
        //}
    }
    //[CodeAnnotation("全局属性变化")]
    public struct UserPropertyChange : IKHSerializable
    {
        public 种族 ZhongZu { get; set; }
        public 兵种职业 Zhiye { get; set; }
        public UserProeprtyType UserProeprtyType { get; set; }
        //[CodeAnnotation("当前值")]
        public double Value { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public void Deserialize(BufferReader reader)
        {
            reader.Read(out byte _ZhongZu);
            this.ZhongZu= (种族)_ZhongZu;
            reader.Read(out byte _Zhiye);
            this.Zhiye= (兵种职业)_Zhiye;
            reader.Read(out byte _UserProeprtyType);
            this.UserProeprtyType= (UserProeprtyType)_UserProeprtyType;
            reader.Read(out Double _Value);
            this.Value= _Value;
        }
        public void Serialize(BufferWriter sender)
        {
            sender.Write((byte)ZhongZu);
            sender.Write((byte)Zhiye);
            sender.Write((byte)UserProeprtyType);
            sender.Write(Value);
        }
        #endregion

    }

    public class UserProperty : IKHSerializable
    {

        public static EnumArray<UserProeprtyType> UserPropertise = new EnumArray<UserProeprtyType>();

        public static EnumArray<种族> ZhongZhuArray = new EnumArray<种族>().Remove(default);

        public static EnumArray<兵种职业> ZhiYeArray = new EnumArray<兵种职业>().Remove(default);
        public GameProperty GloabProperty { get; set; }
        public SDictionary<种族, GameProperty> ZhongZuPerproty { get; set; }
        public SDictionary<兵种职业, GameProperty> ZhiYePerproty { get; set; }
        public UserProperty(bool creat)
        {
            this.ZhiYePerproty = new SDictionary<兵种职业, GameProperty>();
            this.ZhongZuPerproty = new SDictionary<种族, GameProperty>();
            this.GloabProperty = new GameProperty();
        }
        public UserProperty(User user)
        {
            this.ZhiYePerproty = new SDictionary<兵种职业, GameProperty>();
            this.ZhongZuPerproty = new SDictionary<种族, GameProperty>();
            this.GloabProperty = new GameProperty();
            InitShengqi(user);
            InitTianfu(user);
            InitVip(user);
            InitXingzuo(user);
        }
        void InitShengqi(User user)
        {
            if (user.ShengQi != null)
            {
                foreach (var item in user.ShengQi)
                {
                    item.Value.AddProperty(this);
                }
            }
        }
        void InitTianfu(User user)
        {
            if (user.Tianfu != null)
            {
                foreach (var item in user.Tianfu)
                {
                    foreach (var rect in item.Value)
                    {
                        rect.InitProperty(item.Key, this);
                    }
                }
            }
        }
        void InitVip(User user)
        {
            if (user.VipData.VipYueKa > DateTime.Now)
            {
                this.SetYukaProperty(true);
            }
            if (user.VipData.VIPLevel >= 1)
            {
                SetVipProperty();
            }
        }
        void InitXingzuo(User user)
        {
            for (int i = 0; i <= user.XingZuo; i++)
            {
                if (i >= CardConfig.Instance.XingZuoModels.Count)
                {
                    return;
                }
                var model = CardConfig.Instance.XingZuoModels[i];
                if (i < user.XingZuo)
                {
                    model.AddAll(this);
                }
                else
                {
                    for (byte proIndex = 0; proIndex < user.XingZuoLevel; proIndex++)
                    {
                        model.AddProperty(proIndex, this);
                    }
                }
            }
        }
        public UserProperty() { }
        public void SetYukaProperty(bool on)
        {
            if (on)
            {
                foreach (var item in UserConfig.Instance.YueKaAward.Properties)
                {
                    this.SetGloabPropertyOffset(item.Key, item.Value);
                }
            }
            else
            {
                foreach (var item in UserConfig.Instance.YueKaAward.Properties)
                {
                    this.SetGloabPropertyOffset(item.Key, -item.Value);
                }
            }
        }
        public void SetVipProperty()
        {
            foreach (var item in UserConfig.Instance.VipAward.Properties)
            {
                this.SetGloabPropertyOffset(item.Key, item.Value);
            }
        }
        public double GetUerProperty(ushort? cardPid, UserProeprtyType proeprty)
        {
            double value = 0;
            this.GloabProperty?.TryGetValue(proeprty, out value);
            if (cardPid != null && CardConfig.Instance.AllCards.TryGetValue(cardPid.Value, out CardModel modle))
            {
                value += this.GetZhongZuPorperty(modle.ZhongZu, proeprty);
                value += this.GetZhiyePorperty(modle.ZhiYe, proeprty);
            }
            return value;
        }
        public double GetZhongZuPorperty(种族 zhongzu, UserProeprtyType proeprty)
        {
            double val = 0;
            GameProperty pro = null;
            ZhongZuPerproty?.TryGetValue(zhongzu, out pro);
            pro?.TryGetValue(proeprty, out val);
            return val;
        }
        public double GetZhiyePorperty(兵种职业 zhiye, UserProeprtyType proeprty)
        {
            double val = 0;
            GameProperty pro = null;
            ZhiYePerproty?.TryGetValue(zhiye, out pro);
            pro?.TryGetValue(proeprty, out val);
            return val;
        }
        public void SetZhiyePropertyOffset(兵种职业 zhiye, UserProeprtyType proeprty, double offset)
        {
            if (!this.ZhiYePerproty.TryGetValue(zhiye, out var pro))
            {
                pro = new GameProperty();
                this.ZhiYePerproty.Add(zhiye, pro);
            }
            if (!pro.ContainsKey(proeprty))
            {
                pro.Add(proeprty, 0);
            }
            this.ZhiYePerproty[zhiye][proeprty] += offset;
        }
        public void SetZhongZuPropertyOffset(种族 zhongzu, UserProeprtyType proeprty, double offset)
        {
            if (!this.ZhongZuPerproty.TryGetValue(zhongzu, out var pro))
            {
                pro = new GameProperty();
                this.ZhongZuPerproty.Add(zhongzu, pro);
            }
            if (!pro.ContainsKey(proeprty))
            {
                pro.Add(proeprty, 0);
            }
            this.ZhongZuPerproty[zhongzu][proeprty] += offset;
        }
        public void SetGloabPropertyOffset(UserProeprtyType proeprty, double offset)
        {
            if (!this.GloabProperty.ContainsKey(proeprty))
            {
                this.GloabProperty.Add(proeprty, 0);
            }
            this.GloabProperty[proeprty] += offset;
        }
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out GameProperty _GloabProperty);
            this.GloabProperty = _GloabProperty;
            reader.Read(out SDictionary<种族, GameProperty> _ZhongZuPerproty);
            this.ZhongZuPerproty = _ZhongZuPerproty;
            reader.Read(out SDictionary<兵种职业, GameProperty> _ZhiYePerproty);
            this.ZhiYePerproty = _ZhiYePerproty;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(GloabProperty);
            sender.Write(ZhongZuPerproty);
            sender.Write(ZhiYePerproty);
        }
        #endregion
    }
    public class GameProperty : SDictionary<UserProeprtyType, double>
    {
        public GameProperty()
        {
        }
   
   
   
   
   
   
   
        #region AutoProtocol
        public override void Deserialize(BufferReader reader)
        {
            base.Deserialize(reader);
        }
        public override void Serialize(BufferWriter sender)
        {
            base.Serialize(sender);
        }
        #endregion

    }
    /// <summary>
    /// 日结算数据
    /// </summary>
    public class DaySeaonData : IKHSerializable
    {
        public SDictionary<ushort, Award> DynamicAwardToday { get; set; }
        public SDictionary<ushort, Currency> DynamicPriceToday { get; set; }
        public SList<GameTask> TaskDatas { get; set; }
        public SList<GameTask> BuyTasks { get; set; }
        public SList<GameTask> DateTasks { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out SDictionary<UInt16,Award> _DynamicAwardToday);
            this.DynamicAwardToday= _DynamicAwardToday;
            reader.Read(out SDictionary<UInt16,Currency> _DynamicPriceToday);
            this.DynamicPriceToday= _DynamicPriceToday;
            reader.Read(out SList<GameTask> _TaskDatas);
            this.TaskDatas= _TaskDatas;
            reader.Read(out SList<GameTask> _BuyTasks);
            this.BuyTasks= _BuyTasks;
            reader.Read(out SList<GameTask> _DateTasks);
            this.DateTasks= _DateTasks;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(DynamicAwardToday);
            sender.Write(DynamicPriceToday);
            sender.Write(TaskDatas);
            sender.Write(BuyTasks);
            sender.Write(DateTasks);
        }
        #endregion

    }
    /// <summary>
    /// 周结算数据
    /// </summary>
    public class WeekSeaonData : IKHSerializable
    {
        public SList<GameTask> TaskDatas { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out SList<GameTask> _TaskDatas);
            this.TaskDatas= _TaskDatas;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(TaskDatas);
        }
        #endregion

    }
    /// <summary>
    /// 月结算数据
    /// </summary>
    public class MonthSeaonData : IKHSerializable
    {
   
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
        }
        public virtual void Serialize(BufferWriter sender)
        {
        }
        #endregion

    }
}
