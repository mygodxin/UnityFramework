






//************************************************************************************************AutoProtocol**********************************************************************************
/**服务器派发事件协议 */
public enum EventCode
{
    /**强制退出游戏 */
    LogOut = 0,
    /**系统消息<string> */
    SystemMessage = 1,
    /**货币变化 SDictionary<CurrencyType,Double> 玩家全部的最新货币 */
    CurrencyChanged = 2,
    /**卡片碎片变化 <CardNumber>（包括增加 减少）value 当前的数量 */
    CardDebrisChanged = 3,
    /**有人申请好友 <UserInfoTiny> */
    FriendApply = 4,
    /**好友变化 <UserInfoTiny> (1申请好友通过 2通过别人的申请 3删除好友 4被别人删除好友 都会收到此事件 自行判断 是添加还是删除) */
    FriendChange = 5,
    /**获得好友的赞<uint>id */
    FriendZan = 6,
    /**卡片增加  发送Card */
    AddCard = 7,
    /**成为终身会员 */
    VipChange = 8,
    /**月卡到期时间 */
    VipYueKa = 9,
    /**日赛季 */
    DaySeason = 10,
    /**周赛季 */
    WeekSeason = 11,
    /**月赛季 */
    MonthSeason = 12,
    /**提示客户端热更新 */
    HotFixClient = 13,
    /**收到邮件<Mail> */
    Mail = 14,
    /**收到道具SList<Item> */
    AddItem = 15,
    /**任务ID<ushort> <uint>最新完成数量 */
    TaskCount = 16,
    /**用户全局属性变化UserProperty */
    UserPropertyChange = 17,
    /**监听客户端事件开关 <ClientEvent> <bool> */
    ListeningEvent = 18,
    /**支付成功通知 发送商品1<ushort> 商品Pid 2<double> 真实充值金额 */
    PaySuccess = 19,
    /**查看客户端日志 发送1<int>条数(-1为则为所有)  2<Date> 哪一天的 如果为空 则为上传所有 */
    ClientLog = 20,
    /**刷新挂机数据 发送<GuaJiData> */
    RefeshGuaJi = 21,
    /**获得新皮肤 发送<CardValue> */
    CardSkin = 22,
    /**刷新自定义活动数据 */
    RefeshCustomActivityData = 23,
}

/********************************************************************/

/**请求广告原因 */
public enum WatchADReson
{
    /**广告区域购买 */
    Buy = 0,
}

/********************************************************************/

/**请求协议 */
public enum RequestCode
{
    /**网关握手 回复<GateHandShake> */
    GateHandShake = 0,
    /**账号登陆 成功回复<AccountLoginResponse> */
    LoginAccount = 1,
    /**登陆游戏 */
    LoginGame = 2,
    /**购买商品请求 发送<BuyRequest> 成功回复<Award> */
    BuyCommodity = 3,
    /**领取广告观看宝箱奖励  无法送 成功回复<Award> */
    GetAdBoxAward = 4,
    /**发送次数<byte>次数 0为使用好友积分抽奖 成功回复<Award>  */
    ChouJiang = 5,
    /**发送次数<byte>  0就全开, 成功回复<Award> */
    MangHe = 6,
    /**领取每日月卡奖励 发送<AwardSelect>  回复<Award> */
    GetYueKaAward = 7,
    /**领取每日终身卡奖励 发送<AwardSelect>  回复<Award> */
    GetVipAward = 8,
    /**领取首充奖励 发送<AwardSelect> 回复<Award> */
    GetShouchongAward = 9,
    /**看广告请求 发送枚举<WatchADReson> 成功返回<bool> ture为需要连接SDK观看  false为消耗了广告卷可以直接跳过   返回失败则为不允许观看 */
    WatchAD = 10,
    /**开启抽奖宝箱 回复 <Award> */
    ChouJiangBaoXiang = 11,
    /**装备大作战 (是否需要花费 自行计算) 发送 <byte>index  成功回复<Award> */
    EQBigFight = 12,
    /**神器锻造 发送Pid<ushort>  成功回复<ShenQi> */
    SQCreat = 13,
    /**神器升级 Pid<ushort>  成功无数据 */
    SQLevelUp = 14,
    /**神器重置 Pid<ushort> 成功返回<Award> */
    SQRest = 15,
    /**天赋解锁 <TFReq>   成功只回复<UserPropertyType> */
    TFJieSuo = 16,
    /**天赋洗练 <TFReq>    成功只回复<UserPropertyType> */
    TFXiLian = 17,
    /**天赋重置一个面  <TFReq>     只回复成功失败 */
    TFXiLianAll = 18,
    /**装备升星 <uint>ID  回复<Equip> 如果不是undefine或者null说明成功了需要更新数据 */
    EQStarUp = 19,
    /**装备强化 <uint>ID  回复<Equip> 如果不是undefine或者null说明成功了需要更新数据 */
    EQQiangHua = 20,
    /**装备分解 <SList<>>ID  回复<Award> */
    EQFenJie = 21,
    /**装备成长洗练 <uint>ID  回复<Equip>  */
    EQXLGropUp = 22,
    /**装备强化洗练 1 <uint>ID 2<UsrporpertyType>  回复<Equip>  */
    EQXLQiangHua = 23,
    /**装备技能洗练 1 <uint>ID   回复<Equip>  */
    EQXLSkill = 24,
    /**装备资质洗练 1 <uint>ID   回复<Equip>   */
    EQZZRest = 25,
    /**发送<ushort> 卡牌PId, <uint> 装备ID 成功无回复 */
    TakeOnEquip = 26,
    /**发送<ushort> 卡牌PId, <EquipPos> pos 成功无回复 */
    TakeOffEquip = 27,
    /**装备锁 发送 <uint>ID 无任何回复 直接Equip.Lock取反 */
    EQLock = 28,
    /**升级卡牌等级请求 发送1<ushort>pid 生几级<int> 成功回复<int>当前等级 */
    CardLevelup = 29,
    /**多卡牌升级 发送SDictionary<ushrot,int> key卡牌ID value 要升到级数   */
    CardLevelupMut = 30,
    /**重置卡牌等级 <ushort>cardPid */
    RestCardLevel = 31,
    /**卡牌进阶 发送卡牌<ushort>pid  */
    CardJinjie = 32,
    /**卡牌品质升级 发送卡牌<ushort>pid  */
    CardPZLevelup = 33,
    /**星座Index<byte> */
    XingZuoLevelUp = 34,
    /**改变卡牌皮肤 发送 <CardValue> */
    ChangeCardSkin = 35,
    /**更新战斗卡牌 发送<BattleType> <SDictionary<ushort, Pos>> 无任何返回 */
    RefeshBattleCard = 36,
    /**进入大秘境 发送<int>进入的层数 只回复成功失败 */
    EnterDMJ = 37,
    /**闯关战斗结果 只有胜利时发送 无参数 返回1<uint>当前关卡 2<Award>当前所有关卡奖励  */
    MatchResult_Mission = 38,
    /**竞技场战斗结果 发送<bool>是否胜利 返回1<int>当前分数 2<uint>积分奖励   */
    MatchResult_Arena = 39,
    /**日常副本战斗结果  发送1<ushort>Pid    2<double>计算血量比例  返回<Award> */
    MatchResult_RCFB = 40,
    /**征战讨伐副本战斗结果 发送1<ushort>Pid    返回<Award>  */
    MatchResult_ZZTFFB = 41,
    /**世界BOSS战斗结果 发送<double>伤害值 只返回成功失败 */
    MatchResult_WorldBoss = 42,
    /**限时闯关战斗结果  发送<bool>胜利失败 胜利无数据 失败返回<Award>参与奖 */
    MatchResult_TimingMission = 43,
    /**切磋战斗结果  发送<bool>胜利失败 无任何返回  */
    MatchResult_QieCuo = 44,
    /**大秘境胜利  发送大秘境通过的关卡<int>  返回<Award>  */
    MatchResult_DMJ = 45,
    /**星座战斗胜利 回复<Award> */
    MatchResult_XingZuo = 46,
    /**领取关卡奖励 无法送 返回1 <Award>  2<ushort> MissionAwardIndex,  */
    GetMissionAward = 47,
    /**竞技场匹配请求 发送<BattleType> 回复<bool> ture 为真人<UserInfo> false 为机器人<RobotInfo>  */
    ArenaMatch = 48,
    /**领取竞技场奖励 无法送 成功回复<Award> */
    GetArenaAward = 49,
    /**进入活动 发送活动 枚举<活动类型> 失败<string> */
    EnterActivity = 50,
    /**进入星座战斗 */
    EnterXingZuoBattle = 51,
    /**添加好友 <uint> ID */
    AddFriend = 52,
    /**移除好友<uint ID> */
    RemoveFriend = 53,
    /**请求好友的结果 <unit>ID  <bool>是否允许 */
    AgreeFriend = 54,
    /**给好友点赞 <uint>ID */
    FriendZan = 55,
    /**推荐好友 返回 SList<UserInfoTiny> */
    RecommendFriend = 56,
    /**删除邮件 发送ID SList<uint>  无任何回复 */
    DeleteMalil = 57,
    /**读邮件 发送ID <uint>  无任何回复 */
    ReadMail = 58,
    /**获取邮件奖励 发送ID SList<uint> 回复Award */
    GetMailAward = 59,
    /**刷新排行榜 发公诉 <RankingType>  回复 <MyRank>为空则未上榜  SList<UserRanking>   */
    RefeshRanking = 60,
    /**获取任务奖励 pid<ushort>  成功返回<Award>  错误返回<string>  */
    GetTaskAward = 61,
    /**获取购买任务奖励  1pid<ushort> 2<BuyExtraAwardSelect> 回复Award  */
    GetBuyTaskAward = 62,
    /**获取限时任务奖励  1pid<ushort> 2<BuyExtraAwardSelect> 回复Award  */
    GetDateTaskAward = 63,
    /**无法送 只回复成功失败 */
    SignIn = 64,
    /**领取普通通行证奖励 无发送  回复<Award> */
    GetTXZAward = 65,
    /**领取高级通行证奖励 发送<AwardSelect>  回复<Award>  */
    GetVipTXZAward = 66,
    /**技能升级  发送卡牌pid<ushort> 和技能pid<ushort> */
    SkillLevelUp = 67,
    /**重置技能  发送卡牌pid<ushort> */
    RestSkill = 68,
    /**职业专精升级  发送卡牌pid<ushort> 专精<ZJDir> 和 Index <byte>  */
    ZhiyeSkillLevelUp = 69,
    /**职业专精重置  发送卡牌pid<ushort> */
    RestZhiyeSkill = 70,
    /**开始元宵节问题回答 成功无数据 失败<string> */
    YuanXiaoAnswerStart = 71,
    /**元宵节答题结束发送<BuyExtraAwardSelect> 如果发送 undefine 则是回答错误了 否则回复<Award> */
    YuanXiaoAnswerEnd = 72,
    /**购买元宵奖励<BuyExtraAwardSelect>  回复<Award> */
    YuanXiaoBuy = 73,
    /**改名请求 成功失败恢复<string> */
    ChangeName = 74,
    /**修改头像请求 发送<ushort>id 成功失败回复<string> */
    ChangeAvatar = 75,
    /**发送SList<uint> userID 成功回复SList<UserInfo> */
    GetUserData = 76,
    /**领取兑换码 发送string 输入的兑换码 成功回复<Award> 失败回复string */
    Duihuanma = 77,
    /**离开游戏 */
    ExitGame = 78,
    /**设置信息展示开关 发送 <UserInfoType> 无任何回复 */
    SetInfoShow = 79,
    /**客户端事件触发 发送<ClientEvent> 具体其他发送数据以及回复 查看枚举<ClientEvent>的注释 */
    OnClientEvent = 80,
    /**作弊检查 发送 <BattleCheck> */
    CheatCheck = 81,
    /**日志上传 发送<string> 最大2MB */
    ClientLog = 82,
    /**领取挂机奖励 返回1<Award>2 <int>剩余挂机时间  (同时会派发事件 RefeshGuaJi)  */
    GetGuajiAward = 83,
    /**获取战斗支援列表 返回SList<UserRanking> Score为此人今天援助的次数 */
    UserHelpRank = 84,
    /**战斗支援 发送 <uint> UserID, 成返回<UserInfo> 失败返回<HelpErrorCode>  */
    GetUserHelp = 85,
    /**请求测试专用 服务器非Dubug模式不管用 */
    Test = 255,
}

/********************************************************************/

/**请求统一回复 */
public enum ReturnCode
{
    Filed = 0,
    Scuess = 1,
}