

namespace DuiChongServerCommon.ClientProtocol
{
    //[CodeAnnotation("服务器派发事件协议")]
    public enum EventCode : byte
    {
        //[CodeAnnotation("强制退出游戏")]
        LogOut,
        //[CodeAnnotation("系统消息<string>")]
        SystemMessage,
        //[CodeAnnotation("货币变化 SDictionary<CurrencyType,Double> 玩家全部的最新货币")]
        CurrencyChanged,
        //[CodeAnnotation("卡片碎片变化 <CardNumber>（包括增加 减少）value 当前的数量")]
        CardDebrisChanged,
        //[CodeAnnotation("有人申请好友 <UserInfoTiny>")]
        FriendApply,
        //[CodeAnnotation("好友变化 <UserInfoTiny> (1申请好友通过 2通过别人的申请 3删除好友 4被别人删除好友 都会收到此事件 自行判断 是添加还是删除)")]
        FriendChange,
        //[CodeAnnotation("获得好友的赞<uint>id")]
        FriendZan,
        //[CodeAnnotation("卡片增加  发送Card")]
        AddCard,
        //[CodeAnnotation("成为终身会员")]
        VipChange,
        //[CodeAnnotation("月卡到期时间")]
        VipYueKa,
        //[CodeAnnotation("日赛季")]
        DaySeason,
        //[CodeAnnotation("周赛季")]
        WeekSeason,
        //[CodeAnnotation("月赛季")]
        MonthSeason,
        //[CodeAnnotation("提示客户端热更新")]
        HotFixClient,
        //[CodeAnnotation("收到邮件<Mail>")]
        Mail,
        //[CodeAnnotation("收到道具SList<Item>")]
        AddItem,
        //[CodeAnnotation("任务ID<ushort> <uint>最新完成数量")]
        TaskCount,
        //[CodeAnnotation("用户全局属性变化UserProperty")]
        UserPropertyChange,
        //[CodeAnnotation("监听客户端事件开关 <ClientEvent> <bool>")]
        ListeningEvent,
        //[CodeAnnotation("支付成功通知 发送商品1<ushort> 商品Pid 2<double> 真实充值金额")]
        PaySuccess,
        //[CodeAnnotation("查看客户端日志 发送1<int>条数(-1为则为所有)  2<Date> 哪一天的 如果为空 则为上传所有")]
        ClientLog,
        //[CodeAnnotation("刷新挂机数据 发送<GuaJiData>")]
        RefeshGuaJi,
        //[CodeAnnotation("获得新皮肤 发送<CardValue>")]
        CardSkin,
        //[CodeAnnotation("刷新自定义活动数据")]
        RefeshCustomActivityData,




    }

    //[CodeAnnotation("请求广告原因")]
    public enum WatchADReson
    {
        //[CodeAnnotation("广告区域购买")]
        Buy,
    }
    //[CodeAnnotation("请求协议")]

    public enum RequestCode : byte
    {
        //*****************************************************登录******************************************************
        //[RequestParamaters("网关握手 回复<GateHandShake>")]
        GateHandShake,
        //[RequestParamaters("账号登陆 成功回复<AccountLoginResponse>", true, true)]
        LoginAccount,
        //[RequestParamaters("登陆游戏", true, true)]
        LoginGame,
        //*****************************************************市场******************************************************
        //[RequestParamaters("购买商品请求 发送<BuyRequest> 成功回复<Award>", true, true)]
        BuyCommodity,
        //[RequestParamaters("领取广告观看宝箱奖励  无法送 成功回复<Award>")]
        GetAdBoxAward,
        //[RequestParamaters("发送次数<byte>次数 0为使用好友积分抽奖 成功回复<Award> ")]
        ChouJiang,
        //[RequestParamaters("发送次数<byte>  0就全开, 成功回复<Award>")]
        MangHe,
        //[RequestParamaters("领取每日月卡奖励 发送<AwardSelect>  回复<Award>")]
        GetYueKaAward,
        //[RequestParamaters("领取每日终身卡奖励 发送<AwardSelect>  回复<Award>")]
        GetVipAward,
        //[RequestParamaters("领取首充奖励 发送<AwardSelect> 回复<Award>")]
        GetShouchongAward,
        //[RequestParamaters("看广告请求 发送枚举<WatchADReson> 成功返回<bool> ture为需要连接SDK观看  false为消耗了广告卷可以直接跳过   返回失败则为不允许观看", true, true)]
        WatchAD,
        //[RequestParamaters("开启抽奖宝箱 回复 <Award>")]
        ChouJiangBaoXiang,
        //[RequestParamaters("装备大作战 (是否需要花费 自行计算) 发送 <byte>index  成功回复<Award>")]
        EQBigFight,




        //*****************************************************神器******************************************************
        //[RequestParamaters("神器锻造 发送Pid<ushort>  成功回复<ShenQi>")]
        SQCreat,
        //[RequestParamaters("神器升级 Pid<ushort>  成功无数据")]
        SQLevelUp,
        //[RequestParamaters("神器重置 Pid<ushort> 成功返回<Award>")]
        SQRest,


        //*****************************************************天赋******************************************************

        //[RequestParamaters("天赋解锁 <TFReq>   成功只回复<UserPropertyType>")]
        TFJieSuo,
        //[RequestParamaters("天赋洗练 <TFReq>    成功只回复<UserPropertyType>")]
        TFXiLian,
        //[RequestParamaters("天赋重置一个面  <TFReq>     只回复成功失败")]
        TFXiLianAll,
        //*****************************************************装备******************************************************

        //[RequestParamaters("装备升星 <uint>ID  回复<Equip> 如果不是undefine或者null说明成功了需要更新数据")]
        EQStarUp,
        //[RequestParamaters("装备强化 <uint>ID  回复<Equip> 如果不是undefine或者null说明成功了需要更新数据")]
        EQQiangHua,
        //[RequestParamaters("装备分解 <SList<>>ID  回复<Award>")]
        EQFenJie,
        //[RequestParamaters("装备成长洗练 <uint>ID  回复<Equip> ")]
        EQXLGropUp,
        //[RequestParamaters("装备强化洗练 1 <uint>ID 2<UsrporpertyType>  回复<Equip> ")]
        EQXLQiangHua,
        //[RequestParamaters("装备技能洗练 1 <uint>ID   回复<Equip> ")]
        EQXLSkill,
        //[RequestParamaters("装备资质洗练 1 <uint>ID   回复<Equip>  ")]
        EQZZRest,

        //[RequestParamaters("发送<ushort> 卡牌PId, <uint> 装备ID 成功无回复")]
        TakeOnEquip,
        //[RequestParamaters("发送<ushort> 卡牌PId, <EquipPos> pos 成功无回复")]
        TakeOffEquip,
        //[RequestParamaters("装备锁 发送 <uint>ID 无任何回复 直接Equip.Lock取反")]
        EQLock,


        //*****************************************************卡牌******************************************************
        //[RequestParamaters("升级卡牌等级请求 发送1<ushort>pid 生几级<int> 成功回复<int>当前等级")]
        CardLevelup,
        //[RequestParamaters("多卡牌升级 发送SDictionary<ushrot,int> key卡牌ID value 要升到级数  ")]
        CardLevelupMut,

        //[RequestParamaters("重置卡牌等级 <ushort>cardPid")]
        RestCardLevel,
        //[RequestParamaters("卡牌进阶 发送卡牌<ushort>pid ")]
        CardJinjie,
        //[RequestParamaters("卡牌品质升级 发送卡牌<ushort>pid ")]
        CardPZLevelup,
        //[RequestParamaters("星座Index<byte>")]
        XingZuoLevelUp,
        //[RequestParamaters("改变卡牌皮肤 发送 <CardValue>")]
        ChangeCardSkin,


        //*****************************************************战斗******************************************************
        //[RequestParamaters("更新战斗卡牌 发送<BattleType> <SDictionary<ushort, Pos>> 无任何返回", true, true)]
        RefeshBattleCard,

        ////[RequestParamaters("记录怪物 发送1<BattleType> 2 SList<ushort>卡牌PID 无任何返回", true, false)]
        //MarkMonster,

        //[RequestParamaters("进入大秘境 发送<int>进入的层数 只回复成功失败")]
        EnterDMJ,
        //[RequestParamaters("闯关战斗结果 只有胜利时发送 无参数 返回1<uint>当前关卡 2<Award>当前所有关卡奖励 ", true, true)]
        MatchResult_Mission,
        //[RequestParamaters("竞技场战斗结果 发送<bool>是否胜利 返回1<int>当前分数 2<uint>积分奖励  ", true, true)]
        MatchResult_Arena,
        //[RequestParamaters("日常副本战斗结果  发送1<ushort>Pid    2<double>计算血量比例  返回<Award>", true, true)]
        MatchResult_RCFB,
        ////[RequestParamaters("种族副本战斗结果 发送1<ushort>Pid    返回<Award> ", true, true)]
        //MatchResult_ZZFB,
        //[RequestParamaters("征战讨伐副本战斗结果 发送1<ushort>Pid    返回<Award> ", true, true)]
        MatchResult_ZZTFFB,
        //[RequestParamaters("世界BOSS战斗结果 发送<double>伤害值 只返回成功失败", true, true)]
        MatchResult_WorldBoss,
        //[RequestParamaters("限时闯关战斗结果  发送<bool>胜利失败 胜利无数据 失败返回<Award>参与奖", true, true)]
        MatchResult_TimingMission,
        //[RequestParamaters("切磋战斗结果  发送<bool>胜利失败 无任何返回 ", false, false)]
        MatchResult_QieCuo,

        //[RequestParamaters("大秘境胜利  发送大秘境通过的关卡<int>  返回<Award> ", true, true)]
        MatchResult_DMJ,


        //[RequestParamaters("星座战斗胜利 回复<Award>", true, true)]
        MatchResult_XingZuo,



        //[RequestParamaters("领取关卡奖励 无法送 返回1 <Award>  2<ushort> MissionAwardIndex, ")]
        GetMissionAward,



        ////[RequestParamaters("领取关卡奖励")]
        //GetMissionAward,
        ////[RequestParamaters("关卡重生")]
        //MissionBack,
        ////[RequestParamaters("取消重生")]
        //CancleMissionBack,
        //[RequestParamaters("竞技场匹配请求 发送<BattleType> 回复<bool> ture 为真人<UserInfo> false 为机器人<RobotInfo> ")]
        ArenaMatch,
        //[RequestParamaters("领取竞技场奖励 无法送 成功回复<Award>")]
        GetArenaAward,
        //[RequestParamaters("进入活动 发送活动 枚举<活动类型> 失败<string>")]
        EnterActivity,

        //[RequestParamaters("进入星座战斗")]
        EnterXingZuoBattle,




        //*****************************************************社交******************************************************
        //[RequestParamaters("添加好友 <uint> ID")]
        AddFriend,
        //[RequestParamaters("移除好友<uint ID>")]
        RemoveFriend,
        //[RequestParamaters("请求好友的结果 <unit>ID  <bool>是否允许")]
        AgreeFriend,
        //[RequestParamaters("给好友点赞 <uint>ID")]
        FriendZan,
        //[RequestParamaters("推荐好友 返回 SList<UserInfoTiny>")]
        RecommendFriend,
        //[RequestParamaters("删除邮件 发送ID SList<uint>  无任何回复", false, false)]
        DeleteMalil,
        //[RequestParamaters("读邮件 发送ID <uint>  无任何回复", false, false)]
        ReadMail,
        //[RequestParamaters("获取邮件奖励 发送ID SList<uint> 回复Award")]
        GetMailAward,
        //[RequestParamaters("刷新排行榜 发公诉 <RankingType>  回复 <MyRank>为空则未上榜  SList<UserRanking>  ")]
        RefeshRanking,









        //*****************************************************任务******************************************************
        //[RequestParamaters("获取任务奖励 pid<ushort>  成功返回<Award>  错误返回<string> ")]
        GetTaskAward,

        //[RequestParamaters("获取购买任务奖励  1pid<ushort> 2<BuyExtraAwardSelect> 回复Award ")]
        GetBuyTaskAward,

        //[RequestParamaters("获取限时任务奖励  1pid<ushort> 2<BuyExtraAwardSelect> 回复Award ")]
        GetDateTaskAward,


        //[RequestParamaters("无法送 只回复成功失败")]
        SignIn,
        //[RequestParamaters("领取普通通行证奖励 无发送  回复<Award>")]
        GetTXZAward,
        //[RequestParamaters("领取高级通行证奖励 发送<AwardSelect>  回复<Award> ")]
        GetVipTXZAward,




        //*****************************************************技能******************************************************
        //[RequestParamaters("技能升级  发送卡牌pid<ushort> 和技能pid<ushort>")]
        SkillLevelUp,
        //[RequestParamaters("重置技能  发送卡牌pid<ushort>")]
        RestSkill,
        //[RequestParamaters("职业专精升级  发送卡牌pid<ushort> 专精<ZJDir> 和 Index <byte> ")]
        ZhiyeSkillLevelUp,
        //[RequestParamaters("职业专精重置  发送卡牌pid<ushort>")]
        RestZhiyeSkill,




        //*****************************************************自定义活动请求******************************************************
        //[RequestParamaters("开始元宵节问题回答 成功无数据 失败<string>")]
        YuanXiaoAnswerStart,
        //[RequestParamaters("元宵节答题结束发送<BuyExtraAwardSelect> 如果发送 undefine 则是回答错误了 否则回复<Award>")]
        YuanXiaoAnswerEnd,
        //[RequestParamaters("购买元宵奖励<BuyExtraAwardSelect>  回复<Award>")]
        YuanXiaoBuy,








        //*****************************************************用户******************************************************
        //[RequestParamaters("改名请求 成功失败恢复<string>")]
        ChangeName,
        //[RequestParamaters("修改头像请求 发送<ushort>id 成功失败回复<string>")]
        ChangeAvatar,
        //[RequestParamaters("发送SList<uint> userID 成功回复SList<UserInfo>", true, true)]
        GetUserData,
        //[RequestParamaters("领取兑换码 发送string 输入的兑换码 成功回复<Award> 失败回复string", true, true)]
        Duihuanma,
        //[RequestParamaters("离开游戏", false, false)]
        ExitGame,
        //[RequestParamaters("设置信息展示开关 发送 <UserInfoType> 无任何回复", false, true)]
        SetInfoShow,
        //[RequestParamaters("客户端事件触发 发送<ClientEvent> 具体其他发送数据以及回复 查看枚举<ClientEvent>的注释", true, false)]
        OnClientEvent,
        //[RequestParamaters("作弊检查 发送 <BattleCheck>")]
        CheatCheck,
        //[RequestParamaters("日志上传 发送<string> 最大2MB", false, false, 1024 * 1024 * 2)]
        ClientLog,
        //[RequestParamaters("领取挂机奖励 返回1<Award>2 <int>剩余挂机时间  (同时会派发事件 RefeshGuaJi) ")]
        GetGuajiAward,
        //[RequestParamaters("获取战斗支援列表 返回SList<UserRanking> Score为此人今天援助的次数")]
        UserHelpRank,
        //[RequestParamaters("战斗支援 发送 <uint> UserID, 成返回<UserInfo> 失败返回<HelpErrorCode> ")]
        GetUserHelp,



        //[RequestParamaters("请求测试专用 服务器非Dubug模式不管用")]
        Test = 255,
    }


    //[CodeAnnotation("请求统一回复")]

    public enum ReturnCode : byte
    {
        Scuess = 1,
        Filed = 0,
    }



    //[CodeAnnotation("星座枚举")]

    public enum XingZuo : byte
    {
        白羊座,
        金牛座,
        双子座,
        巨蟹座,
        狮子座,
        处女座,
        天秤座,
        天蝎座,
        射手座,
        摩羯座,
        水瓶座,
        双鱼座
    }




    //[CodeAnnotation("排行榜类型")]

    public enum RankingType : byte
    {
        //[CodeAnnotation("闯关排行榜")]
        MissionRanking,
        //[CodeAnnotation("竞技场排行榜")]
        ArenaRanking,
        //[CodeAnnotation("世界BOSS排行榜")]
        WordBossRanking,
        //[CodeAnnotation("限时闯关排行榜")]
        TimingMissionRanking,
        //[CodeAnnotation("成就排行榜")]
        ChengJiuRanking,
    }


    //[CodeAnnotation("")]
    public enum HelpErrorCode : byte
    {
        //[CodeAnnotation("自己的援助次数使用完毕,没有气体")]
        援助已使用完毕,
        //[CodeAnnotation("返回新的榜单数据<SList<UserRanking>>")]
        宝石不足,
        //[CodeAnnotation("返回新的榜单数据<SList<UserRanking>>")]
        此人暂未上榜,
        //[CodeAnnotation("返回最新的援助次数<uint>")]
        此人今日援助次数已达上限,
    }




    //[CodeAnnotation("")]
    public enum UserInfoType : byte
    {
        //[CustomParamater(true)]
        个人信息,
        //[CustomParamater(true)]
        竞技信息,
        //[CustomParamater(true)]
        卡牌信息,
        //[CustomParamater(false)]
        普通品质自动分解,
        //[CustomParamater(false)]
        稀有品质自动分解,
        //[CustomParamater(false)]
        史诗品质自动分解,
    }

    //[CodeAnnotation("需要监听的客户端事件")]

    public enum ClientEvent : byte
    {
        //[CodeAnnotation("发送 <MonsterInfo> 无任何回复")]
        KillMonster,
    }


    //[CodeAnnotation("货币种类")]

    public enum CurrencyType : byte
    {
        人民币 = 0,
        宝石 = 1,
        免广告券 = 2,
        金币 = 3,
        神器水晶 = 4,
        技能石 = 5,
        盲盒币 = 7,
        装备强化石 = 8,
        魔方残片 = 9,
        通行证积分 = 10,
        装备洗练石 = 11,
        竞技积分 = 12,
        好友积分 = 13,
        星源石 = 14,
    }


    //[CodeAnnotation("用户行为")]

    public enum UserBehavior : byte
    {
        //[CodeAnnotation("登录,如果Value值为1 则表示是新用户")]
        Login,
        //[CodeAnnotation("改名")]
        ChangeName,
    }


    public enum CheatType : byte
    {
        数值作弊,
        加速作弊,
    }



    //[CodeAnnotation("游戏玩法功能")]
    public enum Functional : byte
    {
        Shop_每日精选,
        Shop_装备宝箱,
        Shop_充值,
        Shop_通行证,
        Battle_竞技场,
        Battle_日常副本,
        Battle_征战副本,
        Battle_限时闯关,
        Battle_世界BOSS,
        Battle_排行榜,
        Battle_大秘境,
        Battle_援助,
        Card_天赋,
        Card_星座,
    }

    //[CodeAnnotation("登录平台")]

    public enum Platform : byte
    {
        Web,
        WeChat,
        IOS,
        Android,
        TapTap,
        /// <summary>
        /// 大象
        /// </summary>
        DaXiang,
    }
    //[CodeAnnotation("商城购买失败回复")]

    public enum 兵种职业 : byte
    {
        兵种职业None,
        刺客,
        法师,
        游侠,
        坦克,
    }

    public enum 套装类型 : byte
    {
        套装类型None,
        亡语,
        冲锋,
        手刃,
    }
    //[CodeAnnotation("商品区域")]

    public enum CommodityPos : byte
    {
        广告区域,
        竞技兑换,
        每日精选,
        兵种招募,
        首充豪礼,
        特权奖励,
        恶魔宝库,
        通行证,
    }

    public enum 品质 : byte
    {
        品质None,
        普通,
        稀有,
        史诗,
        传奇,
        神话,
    }

    public enum 竞技段位 : byte
    {
        青铜五阶,
        青铜四阶,
        青铜三阶,
        青铜二阶,
        青铜一阶,
        白银五阶,
        白银四阶,
        白银三阶,
        白银二阶,
        白银一阶,
        黄金五阶,
        黄金四阶,
        黄金三阶,
        黄金二阶,
        黄金一阶,
        钻石五阶,
        钻石四阶,
        钻石三阶,
        钻石二阶,
        钻石一阶,
        皇冠五阶,
        皇冠四阶,
        皇冠三阶,
        皇冠二阶,
        皇冠一阶,
        王者,
    }

    public enum 种族 : byte
    {
        种族None,
        高山据点,
        魔法塔楼,
        平原城堡,
        森林壁垒,
        元素生物,
        地狱军团,
    }

    public enum UserProeprtyType : byte
    {
        /// <summary>
        /// 伤害=攻击力*(穿刺/护甲)
        /// </summary>
        攻击力 = 0,
        生命值 = 1,
        穿刺 = 2,
        护甲 = 3,


        精通 = 10,
        韧性 = 11,
        暴击 = 12,
        暴击伤害 = 13,
        闪避 = 14,
        命中 = 15,
        吸血 = 16,
        技能效果 = 17,


        攻击速度 = 100,
        攻击范围 = 101,
        移动速度 = 102,


        战斗加速 = 200,


        ////删除
        //重生跳关概率 = 201,


        怪物攻击减少 = 202,
        怪物血量减少 = 203,
        怪物护甲减少 = 204,
        怪物穿刺减少 = 205,




        ////删除
        //金币产出 = 206,


        ////删除
        //离线闯关速度 = 207,


        PVP伤害加成 = 208,
        PVP伤害减免 = 209,
        羁绊属性效果提升 = 210,

        治疗效果提高 = 211,
        控制效果时间延长 = 212,
        召唤物基础属性提升 = 213,
        日常副本收益增加 = 214,
        挂机收益提升 = 215,



        技能伤害,
        攻击伤害,
        技能免伤,
        攻击免伤,



        攻击基础值增加,
        穿刺基础值增加,
        生命基础值增加,
        护甲基础值增加,

    }

    public enum 副本类型 : byte
    {
        日常副本,
        //种族副本,
        征战讨伐,
    }


    public enum 活动类型 : byte
    {
        世界BOSS,
        限时闯关,
    }

    public enum BUFFType : byte
    {
        //[CodeAnnotation("")]
        HOT,
        //[CodeAnnotation("")]
        BUFF,
        //[CodeAnnotation("")]
        DEBUFF,
        //[CodeAnnotation("")]
        DOT,
        //[CodeAnnotation("")]
        CTRL,
    }

    public enum ItemType : byte
    {
        Item,
        Equip,
        Consumption,
    }

    public enum EquipPos : byte
    {
        EquipPosNone,
        头盔,
        武器,
        项链,
        盔甲,
    }

    public enum TaskType : byte
    {
        每日任务,
        每周任务,
        成就任务,
        购买任务,
        限时任务,
    }

    //[CodeAnnotation("职业专精方向")]
    public enum ZJDir : byte
    {
        ZJDirNone,
        Left,
        Right,
    }

    public enum BattleType : byte
    {
        /// <summary>
        /// 关卡
        /// </summary>
        //[CodeAnnotation("关卡  ")]
        MissionBattle,
        //[CodeAnnotation("日常副本")]
        Richangfuben,
        //[CodeAnnotation("征战讨伐 ")]
        ZhengZhanFuben,
        //[CodeAnnotation("竞技场")]
        Jingjichang,
        //[CodeAnnotation("世界BOSS")]
        WordBoss,
        //[CodeAnnotation("限时闯关")]
        TimeMissionRank,
        /// <summary>
        /// 切磋
        /// </summary>
        //[CodeAnnotation("切磋")]
        QieCuo,
        //[CodeAnnotation("征战讨伐 ")]
        ZhengZhanFuben1,
        //[CodeAnnotation("征战讨伐 ")]
        ZhengZhanFuben2,
        //[CodeAnnotation("征战讨伐 ")]
        ZhengZhanFuben3,
        //[CodeAnnotation("征战讨伐 ")]
        ZhengZhanFuben4,
        //[CodeAnnotation("征战讨伐 ")]
        ZhengZhanFuben5,
        //[CodeAnnotation("征战讨伐 ")]
        ZhengZhanFuben6,
        //[CodeAnnotation("星座")]
        XingZuoBattle,
    }
    //[CodeAnnotation("自定义活动类型")]

    public enum CustomActivity:byte
    { 
        元宵节,


    }




    public enum ServerStatus : byte
    {
        关闭,
        流畅,
        拥挤,
        爆满,
        拒绝服务,
    }

    public enum KNumber : byte
    {
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        K,
        L,
        M,
        N,
        O,
        P,
        Q,
        R,
        S,
        T,
        U,
        V,
        W,
        X,
        Y,
        Z,
    }



    public enum CustomActivityDataType : byte
    {
        CustomActivityData,
        YuanXiaoData,
    }

    public enum CustomActivityModelType : byte
    {
        CustomActivityModel,
        YuanXiaoModelList,
    }


    public enum AnswerState : byte
    {
        None = 0,
        AnswerWrong,
    }

}




