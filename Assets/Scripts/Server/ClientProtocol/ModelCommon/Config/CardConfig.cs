using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using KHCore.Utils;

namespace DuiChongServerCommon.ClientProtocol
{
    //[CodeAnnotation("升级消耗")]
    public class EcoCostData : IKHSerializable
    {
        //[CodeAnnotation("目标等级对应消耗")]
        public SDictionary<int, double> Cost { get; set; }
        //[CodeAnnotation("最大等级限制")]
        public int MaxLevel { get; set; }
        public override string ToString()
        {
            var s = new StringBuilder();
            s.AppendLine($"最大等级:{MaxLevel}");
            var minlevel = int.MaxValue;
            foreach (var item in Cost)
            {
                if (item.Key < minlevel)
                {
                    minlevel = item.Key;
                }
            }
            s.AppendLine($"最小等级:{minlevel - 1}");
            double totalCast = 0;
            for (int i = minlevel; i <= MaxLevel; i++)
            {
                var cast = Cost[i];
                totalCast += cast;
                s.AppendLine($"{i - 1}->{i}级 消耗{cast}");
            }
            s.AppendLine($"总消耗{totalCast}");
            return s.ToString();
        }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out SDictionary<Int32,Double> _Cost);
            this.Cost= _Cost;
            reader.Read(out Int32 _MaxLevel);
            this.MaxLevel= _MaxLevel;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(Cost);
            sender.Write(MaxLevel);
        }
        #endregion

    }
    public class ShenQiModle : AMoldeDataBase
    {

        //[CodeAnnotation("属性全局占比")]
        public decimal 属性全局占比 { get; set; }
        //[CodeAnnotation("属性")]
        public UserProeprtyType PropertyType { get; private set; }
        public SDictionary<int, double> LevelProperties { get; set; }
        //[CodeAnnotation("品质")]
        public 品质 PinZhi { get; set; }
        //[CodeAnnotation("打造花费")]
        public Currency CreatCost { get; set; }
   
   
   
   
   
   
   
   
        #region AutoProtocol
        public override void Deserialize(BufferReader reader)
        {
            reader.Read(out byte _PropertyType);
            this.PropertyType= (UserProeprtyType)_PropertyType;
            reader.Read(out SDictionary<Int32,Double> _LevelProperties);
            this.LevelProperties= _LevelProperties;
            reader.Read(out byte _PinZhi);
            this.PinZhi= (品质)_PinZhi;
            reader.Read(out Currency _CreatCost);
            this.CreatCost= _CreatCost;
            base.Deserialize(reader);
        }
        public override void Serialize(BufferWriter sender)
        {
            sender.Write((byte)PropertyType);
            sender.Write(LevelProperties);
            sender.Write((byte)PinZhi);
            sender.Write(CreatCost);
            base.Serialize(sender);
        }
        #endregion

    }
    public class ShenQi : IKHSerializable
    {
        public void AddProperty(UserProperty property)
        {
            var shenQiModle = CardConfig.Instance.ShenQiModles[this.Pid];
            property.SetGloabPropertyOffset(shenQiModle.PropertyType, shenQiModle.LevelProperties[this.Level]);
        }
        public void RemoveProperty(UserProperty property)
        {
            var shenQiModle = CardConfig.Instance.ShenQiModles[this.Pid];
            property.SetGloabPropertyOffset(shenQiModle.PropertyType, -shenQiModle.LevelProperties[this.Level]);//* this.Level);
        }
        public ushort Pid { get; set; }
        public ushort Level { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out UInt16 _Pid);
            this.Pid= _Pid;
            reader.Read(out UInt16 _Level);
            this.Level= _Level;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(Pid);
            sender.Write(Level);
        }
        #endregion

    }
    public class Pos : IKHSerializable
    {


        static List<Pos> 五人五角星占位 = new List<Pos>()
{
new Pos() {X=5,Y=4},
new Pos() {X=2,Y=3},
new Pos() {X=8,Y=3},
new Pos() {X=7,Y=2},
new Pos() {X=3,Y=2},
};


        static List<Pos> 五人方形占位 = new List<Pos>()
{
new Pos() {X=3,Y=2},
new Pos() {X=7,Y=2},
new Pos() {X=5,Y=3},
new Pos() {X=7,Y=4},
new Pos() {X=3,Y=4},
};


        static List<Pos> 五人V形占位 = new List<Pos>()
{
new Pos() {X=5,Y=2},
new Pos() {X=4,Y=3},
new Pos() {X=6,Y=3},
new Pos() {X=3,Y=4},
new Pos() {X=7,Y=4},
};


        static List<Pos> 五人A形占位 = new List<Pos>()
{
new Pos() {X=5,Y=4},
new Pos() {X=4,Y=3},
new Pos() {X=6,Y=3},
new Pos() {X=3,Y=2},
new Pos() {X=7,Y=2},
};


        static List<Pos> 五人一字行占位 = new List<Pos>()
{
new Pos() {X=3,Y=2},
new Pos() {X=7,Y=2},
new Pos() {X=4,Y=2},
new Pos() {X=6,Y=2},
new Pos() {X=5,Y=2},
};


        static List<Pos> 十人五角星占位 = new List<Pos>()
{
new Pos() {X=5,Y=5},
new Pos() {X=1,Y=4},
new Pos() {X=9,Y=4},
new Pos() {X=2,Y=3},
new Pos() {X=8,Y=3},
new Pos() {X=7,Y=2},
new Pos() {X=3,Y=2},
new Pos() {X=5,Y=2},
new Pos() {X=6,Y=1},
new Pos() {X=4,Y=1},
};


        static List<Pos> 十人方形占位 = new List<Pos>()
{
new Pos() {X=5,Y=5},
new Pos() {X=4,Y=4},
new Pos() {X=5,Y=4},
new Pos() {X=6,Y=4},
new Pos() {X=4,Y=3},
new Pos() {X=5,Y=3},
new Pos() {X=6,Y=3},
new Pos() {X=4,Y=2},
new Pos() {X=5,Y=2},
new Pos() {X=6,Y=2},
};


        static List<Pos> 十人V形占位 = new List<Pos>()
{
new Pos() {X=5,Y=1},
new Pos() {X=4,Y=2},
new Pos() {X=6,Y=2},
new Pos() {X=3,Y=3},
new Pos() {X=7,Y=3},
new Pos() {X=2,Y=4},
new Pos() {X=8,Y=4},
new Pos() {X=1,Y=5},
new Pos() {X=9,Y=5},
new Pos() {X=5,Y=6},
};


        static List<Pos> 十人A形占位 = new List<Pos>()
{
new Pos() {X=5,Y=6},
new Pos() {X=4,Y=5},
new Pos() {X=6,Y=5},
new Pos() {X=3,Y=4},
new Pos() {X=7,Y=4},
new Pos() {X=2,Y=3},
new Pos() {X=8,Y=3},
new Pos() {X=1,Y=2},
new Pos() {X=9,Y=2},
new Pos() {X=5,Y=1},
};


        static List<Pos> 十人一字行占位 = new List<Pos>()
        {
            new Pos() {X=5,Y=3},
            new Pos() {X=4,Y=3},
            new Pos() {X=6,Y=3},
            new Pos() {X=3,Y=3},
            new Pos() {X=7,Y=3},
            new Pos() {X=5,Y=4},
            new Pos() {X=4,Y=4},
            new Pos() {X=6,Y=4},
            new Pos() {X=3,Y=4},
            new Pos() {X=7,Y=4},
        };


        public static List<List<Pos>> CardPostionFunc5 = new List<List<Pos>>()
        {
            五人五角星占位,
            五人方形占位,
            五人V形占位,
            五人A形占位,
            五人一字行占位,
        };


        public static List<List<Pos>> CardPostionFunc10 = new List<List<Pos>>()
        {
            十人五角星占位,
            十人方形占位,
            十人V形占位,
            十人A形占位,
            十人一字行占位,
        };
        public static List<Pos> 五人随机队形()
        {
            return CardPostionFunc5.RandomElement();
        }
        public static List<Pos> 十人随机队形()
        {
            return CardPostionFunc10.RandomElement();
        }
        public byte X { get; private set; }
        public byte Y { get; private set; }
        public Pos()
        {
        }
        public Pos(byte x, byte y)
        {
            this.X = x;
            this.Y = y;
        }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out Byte _X);
            this.X= _X;
            reader.Read(out Byte _Y);
            this.Y= _Y;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(X);
            sender.Write(Y);
        }
        #endregion

    }
    public class JinjieLimit : IKHSerializable
    {
        //[CodeAnnotation("进阶等级")]
        public int JinjieLevel { get; set; }
        //[CodeAnnotation("等级限制")]
        public int LevelLimit { get; set; }
        //[CodeAnnotation("技能最大等级")]
        public int SkillLimit { get; set; }
        //[CodeAnnotation("最低品质要求")]
        public 品质 MinPinzhi { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out Int32 _JinjieLevel);
            this.JinjieLevel= _JinjieLevel;
            reader.Read(out Int32 _LevelLimit);
            this.LevelLimit= _LevelLimit;
            reader.Read(out Int32 _SkillLimit);
            this.SkillLimit= _SkillLimit;
            reader.Read(out byte _MinPinzhi);
            this.MinPinzhi= (品质)_MinPinzhi;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(JinjieLevel);
            sender.Write(LevelLimit);
            sender.Write(SkillLimit);
            sender.Write((byte)MinPinzhi);
        }
        #endregion

    }

    public class Card : IKHSerializable
    {
        public static Card CreatNew(ushort pid)
        {
            var molde = CardConfig.Instance.AllCards[pid];
            var card = new Card()
            {
                Pid = pid,
                Level = 1,
                Jinjie = 1,
                SkillLevel = new SDictionary<ushort, byte>(),
                Equips = new SDictionary<EquipPos, Equip>(),
            };
            for (byte i = 0; i < molde.Skills.Count; i++)
            {
                card.SkillLevel.Add(molde.Skills[i], 1);
            }
            if (SkillConfig.Instance.ZhiyeSkillModels.TryGetValue(molde.ZhiYe, out var zhiyeSkills))
            {
                card.ZhiyeSkillLevel = new SList<byte>();
            }
            return card;
        }
        public ushort Pid { get; set; }
        public byte Jinjie { get; set; } = 1;
        public ushort Level { get; set; } = 1;
        public byte ShengPinLevel { get; set; } = 0;
        public byte PinZhiPlus { get; set; } = 0;
        public byte Skin { get; set; }

        public int ZhuangjingLevel => (ZhiyeSkillLevel == null || ZhiyeSkillLevel.Count == 0) ? 0 : ZhiyeSkillLevel.Sum((x) => { return (int)x; });

        public 品质 PinZhi => (品质)((byte)CardConfig.Instance.AllCards[this.Pid].PinZhi + this.PinZhiPlus);
        //[CodeAnnotation("专精方向 默认为None")]
        public ZJDir ZJDir { get; set; }
        //[CodeAnnotation("专精 技能树")]
        public SList<byte> ZhiyeSkillLevel { get; set; }
        //[CodeAnnotation("技能等级")]
        public SDictionary<ushort, byte> SkillLevel { get; set; }
        //[CodeAnnotation("装备")]
        public SDictionary<EquipPos, Equip> Equips { get; set; }
        public override string ToString()
        {
            var s = new StringBuilder();
            foreach (var item in this.GetType().GetProperties())
            {
                s.Append($"{item.Name} = {item.GetValue(this)}\n");
            }
            return s.ToString();
        }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out UInt16 _Pid);
            this.Pid= _Pid;
            reader.Read(out Byte _Jinjie);
            this.Jinjie= _Jinjie;
            reader.Read(out UInt16 _Level);
            this.Level= _Level;
            reader.Read(out Byte _ShengPinLevel);
            this.ShengPinLevel= _ShengPinLevel;
            reader.Read(out Byte _PinZhiPlus);
            this.PinZhiPlus= _PinZhiPlus;
            reader.Read(out Byte _Skin);
            this.Skin= _Skin;
            reader.Read(out byte _ZJDir);
            this.ZJDir= (ZJDir)_ZJDir;
            reader.Read(out SList<Byte> _ZhiyeSkillLevel);
            this.ZhiyeSkillLevel= _ZhiyeSkillLevel;
            reader.Read(out SDictionary<UInt16,Byte> _SkillLevel);
            this.SkillLevel= _SkillLevel;
            reader.Read(out SDictionary<EquipPos,Equip> _Equips);
            this.Equips= _Equips;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(Pid);
            sender.Write(Jinjie);
            sender.Write(Level);
            sender.Write(ShengPinLevel);
            sender.Write(PinZhiPlus);
            sender.Write(Skin);
            sender.Write((byte)ZJDir);
            sender.Write(ZhiyeSkillLevel);
            sender.Write(SkillLevel);
            sender.Write(Equips);
        }
        #endregion

    }
    //[CodeAnnotation("天赋正方形")]
    public class TianfuRect : IKHSerializable
    {
        //[CodeAnnotation("天赋数据 key Index")]
        public SDictionary<byte, UserProeprtyType> TianfuDatas { get; set; }
        public void InitProperty(种族 zhongZu, UserProperty property)
        {
            foreach (var item in TianfuDatas)
            {
                var proValue = CardConfig.Instance.TianfuProperties[item.Value];
                property.SetZhongZuPropertyOffset(zhongZu, item.Value, proValue);
            }
        }
        public void AddTianfu(种族 zhongZu, UserProperty property, byte index, UserProeprtyType proeprtyType)
        {
            this.RemoveTinfu(zhongZu, property, index);
            this.TianfuDatas.AddOrUpdate(index, proeprtyType);
            var proValue = CardConfig.Instance.TianfuProperties[proeprtyType];
            property.SetZhongZuPropertyOffset(zhongZu, proeprtyType, proValue);
        }
        public void RemoveTinfu(种族 zhongZu, UserProperty property, byte index)
        {
            if (this.TianfuDatas.Remove(index, out var type))
            {
                var proValue = CardConfig.Instance.TianfuProperties[type];
                property.SetZhongZuPropertyOffset(zhongZu, type, -proValue);
            }
        }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out SDictionary<Byte,UserProeprtyType> _TianfuDatas);
            this.TianfuDatas= _TianfuDatas;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(TianfuDatas);
        }
        #endregion

    }
    //[CodeAnnotation("卡牌数据变化")]
    public class CardValue : IKHSerializable
    {
        //[CodeAnnotation("卡片的PID")]
        public ushort Pid { get; set; }
        public int Value { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out UInt16 _Pid);
            this.Pid= _Pid;
            reader.Read(out Int32 _Value);
            this.Value= _Value;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(Pid);
            sender.Write(Value);
        }
        #endregion

    }
    public class CardSkin : AMoldeDataBase
    {
        public static CardSkin Parse(string str)
        {
            return new CardSkin() { Name = str.Trim() };
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
    public class JiBanProperty : IKHSerializable
    {
        public byte Level { get; set; }
        public SDictionary<UserProeprtyType, double> JibanProperty { get; set; }
        public byte CountLimit { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out Byte _Level);
            this.Level= _Level;
            reader.Read(out SDictionary<UserProeprtyType,Double> _JibanProperty);
            this.JibanProperty= _JibanProperty;
            reader.Read(out Byte _CountLimit);
            this.CountLimit= _CountLimit;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(Level);
            sender.Write(JibanProperty);
            sender.Write(CountLimit);
        }
        #endregion

    }
    public class CardModel : AMoldeDataBase
    {
        //[CodeAnnotation("兵种职业")]
        public 兵种职业 ZhiYe { get; set; }
        //[CodeAnnotation("品质")]
        public 品质 PinZhi { get; set; }
        //[CodeAnnotation("种族")]
        public 种族 ZhongZu { get; set; }
        //[CodeAnnotation("攻击成长")]
        public double GUDamage { get; set; }
        //[CodeAnnotation("穿刺成长")]
        public double GUChuanci { get; set; }
        //[CodeAnnotation("护甲成长")]
        public double GUHujia { get; set; }
        //[CodeAnnotation("生命成长")]
        public double GUHP { get; set; }
        //[CodeAnnotation("攻击速度")]
        public double AttackSpeed { get; set; } = 1;
        //[CodeAnnotation("攻击范围")]
        public byte AttackRange { get; set; } = 1;
        //[CodeAnnotation("移动速度")]
        public double MoveSpeed { get; set; } = 1;
        //[CodeAnnotation("成长修正系数")]
        public double GUCoefficient { get; set; } = 1;
        //[CodeAnnotation("攻击速度修正系数")]
        public double AttackSpeedCoefficient { get; set; } = 1;
        //[CodeAnnotation("携带技能")]
        public SList<ushort> Skills { get; set; }
        //[CodeAnnotation("皮肤")]
        public SDictionary<byte, CardSkin> AllSkins { get; set; }
        public override string ToString()
        {
            var s = new StringBuilder();
            foreach (var item in this.GetType().GetProperties())
            {
                s.Append($"{item.Name} = {item.GetValue(this)}\n");
            }
            return s.ToString();
        }
   
   
   
   
   
   
   
        #region AutoProtocol
        public override void Deserialize(BufferReader reader)
        {
            reader.Read(out byte _ZhiYe);
            this.ZhiYe= (兵种职业)_ZhiYe;
            reader.Read(out byte _PinZhi);
            this.PinZhi= (品质)_PinZhi;
            reader.Read(out byte _ZhongZu);
            this.ZhongZu= (种族)_ZhongZu;
            reader.Read(out Double _GUDamage);
            this.GUDamage= _GUDamage;
            reader.Read(out Double _GUChuanci);
            this.GUChuanci= _GUChuanci;
            reader.Read(out Double _GUHujia);
            this.GUHujia= _GUHujia;
            reader.Read(out Double _GUHP);
            this.GUHP= _GUHP;
            reader.Read(out Double _AttackSpeed);
            this.AttackSpeed= _AttackSpeed;
            reader.Read(out Byte _AttackRange);
            this.AttackRange= _AttackRange;
            reader.Read(out Double _MoveSpeed);
            this.MoveSpeed= _MoveSpeed;
            reader.Read(out Double _GUCoefficient);
            this.GUCoefficient= _GUCoefficient;
            reader.Read(out Double _AttackSpeedCoefficient);
            this.AttackSpeedCoefficient= _AttackSpeedCoefficient;
            reader.Read(out SList<UInt16> _Skills);
            this.Skills= _Skills;
            reader.Read(out SDictionary<Byte,CardSkin> _AllSkins);
            this.AllSkins= _AllSkins;
            base.Deserialize(reader);
        }
        public override void Serialize(BufferWriter sender)
        {
            sender.Write((byte)ZhiYe);
            sender.Write((byte)PinZhi);
            sender.Write((byte)ZhongZu);
            sender.Write(GUDamage);
            sender.Write(GUChuanci);
            sender.Write(GUHujia);
            sender.Write(GUHP);
            sender.Write(AttackSpeed);
            sender.Write(AttackRange);
            sender.Write(MoveSpeed);
            sender.Write(GUCoefficient);
            sender.Write(AttackSpeedCoefficient);
            sender.Write(Skills);
            sender.Write(AllSkins);
            base.Serialize(sender);
        }
        #endregion

    }
    public class XingZuoModel : IKHSerializable
    {
        //[CodeAnnotation("开启等级")]
        public int OpenLevel { get; set; }
        public XingZuo XingZuo { get; set; }
        public SList<UserPorpertyValue> Properties { get; set; }
        public SList<Currency> Price { get; set; }
        /// <summary>
        /// 最终技能属性
        /// </summary>
        public UserPorpertyValue EndSkill { get; set; }
        /// <summary>
        /// 最终属性奖励
        /// </summary>
        public SList<UserPorpertyValue> EndProperties { get; set; }
        public void AddProperty(byte currentXingzuoLevel, UserProperty userProperty)
        {
            var pro = this.Properties[currentXingzuoLevel];
            userProperty.SetGloabPropertyOffset(pro.UserProeprtyType, pro.Value);
            if (currentXingzuoLevel == this.Properties.Count - 1)
            {
                this.AddEnd(userProperty);
            }
        }
        void AddEnd(UserProperty userProperty)
        {
            for (int i = 0; i < EndProperties.Count; i++)
            {
                var pro = EndProperties[i];
                userProperty.SetGloabPropertyOffset(pro.UserProeprtyType, pro.Value);
            }
            userProperty.SetGloabPropertyOffset(this.EndSkill.UserProeprtyType, this.EndSkill.Value);
        }
        public void AddAll(UserProperty userProperty)
        {
            for (int i = 0; i < Properties.Count; i++)
            {
                var pro = Properties[i];
                userProperty.SetGloabPropertyOffset(pro.UserProeprtyType, pro.Value);
            }
            this.AddEnd(userProperty);
        }
   
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out Int32 _OpenLevel);
            this.OpenLevel= _OpenLevel;
            reader.Read(out byte _XingZuo);
            this.XingZuo= (XingZuo)_XingZuo;
            reader.Read(out SList<UserPorpertyValue> _Properties);
            this.Properties= _Properties;
            reader.Read(out SList<Currency> _Price);
            this.Price= _Price;
            reader.Read(out UserPorpertyValue _EndSkill);
            this.EndSkill= _EndSkill;
            reader.Read(out SList<UserPorpertyValue> _EndProperties);
            this.EndProperties= _EndProperties;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(OpenLevel);
            sender.Write((byte)XingZuo);
            sender.Write(Properties);
            sender.Write(Price);
            sender.Write(EndSkill);
            sender.Write(EndProperties);
        }
        #endregion

    }

    //[CodeAnnotation("卡片配置文件")]
    public class CardConfig : ConfigBase<CardConfig>
    {


        public ShenQiModle[] ShenQiList { get; private set; }

        /// <summary>
        /// 宝石兑换其它货币
        /// </summary>
        /// <param name="baoshi"></param>
        /// <param name="currencyType"></param>
        /// <returns></returns>
        public double Baoshi_2_Currency(double baoshi, CurrencyType currencyType)
        {
            return baoshi * Baoshi2Currency[currencyType];
        }
        /// <summary>
        /// 其它货币兑换宝石
        /// </summary>
        /// <param name="currencyValue"></param>
        /// <param name="currencyType"></param>
        /// <returns></returns>
        public double Currency_2_BaoShi(double currencyValue, CurrencyType currencyType)
        {
            return currencyValue / this.Baoshi2Currency[currencyType];
        }

        public double 移动速度百分之一修正系数 { get; set; }

        public double 攻击范围一格修正系数 { get; set; }

        public int TianfuMaxIndex { get; private set; }

        public UserProeprtyType[] TianfuProperyRandomArr { get; private set; }
        public int 升品最大等级 { get; set; }
        public int 进阶最大等级 { get; set; }
        public int 天赋正方形尺寸 { get; set; }
        public int 神器最大等级 { get; set; }
        public int 星座最大等级 { get; set; }

        public CardModel[] AllCardArray { get; private set; }
        #region 金币等级数据
        public int 等级开放跨度 { get; set; }
        //[CodeAnnotation("key 所有卡牌的评级星级(如果没有这张卡 按1星计算)")]
        public SDictionary<int, double> 总进阶影响金币产出系数 { get; set; }
        //[CodeAnnotation("key 所有卡牌的评级星级(如果没有这张卡 按1星计算)")]
        public SDictionary<int, double> 总关卡影响金币产出系数 { get; set; }
        /// <summary>
        /// 卡牌升级金币消耗
        /// </summary>
        //[CodeAnnotation("卡牌升级消耗")]
        public EcoCostData CardLevelUp { get; set; }
        //[CodeAnnotation("每秒金币增加 key 最高关卡等级 value 每秒金币增加值")]
        /// <summary>
        /// 每秒金币增加
        /// </summary>
        public double JinBiAwardSec { get; set; }
        #endregion
        //[CodeAnnotation("天赋洗练价格")]
        public Currency TianFuResetPrice { get; set; }
        //[CodeAnnotation("天赋洗练一个面的 价格 key 为当前面开启数量 value 为价格")]
        public SDictionary<int, Currency> TianFuResetAllPrice { get; set; }
        //[CodeAnnotation("进阶限制")]
        public SDictionary<int, JinjieLimit> JinjieLimits { get; set; }
        //[CodeAnnotation("所有的卡片信息 key 卡片PID  value 卡片信息")]
        public SDictionary<ushort, CardModel> AllCards { get; set; }
        //[CodeAnnotation("额外的特殊卡片信息 key 卡片PID  value 卡片信息")]
        public SDictionary<ushort, CardModel> ExtraCards { get; set; }
        //[CodeAnnotation("神器数据")]
        public SDictionary<ushort, ShenQiModle> ShenQiModles { get; set; }
        //[CodeAnnotation("神器重置货币返还")]
        public SDictionary<uint, Currency> ShenQiBack { get; set; }
        //[CodeAnnotation("天赋属性数据")]
        public SDictionary<UserProeprtyType, double> TianfuProperties { get; set; }
        /// <summary>
        /// 神器升级水晶消耗
        /// </summary>
        //[CodeAnnotation("神器升级消耗")]
        public SDictionary<品质, EcoCostData> ShengQiLevelUp { get; set; }
        /// <summary>
        /// 天赋药水消耗
        /// </summary>
        //[CodeAnnotation("天赋升级消耗")]
        public EcoCostData TianfuLevelUp { get; set; }
        /// <summary>
        /// 兵种进阶消耗
        /// </summary>
        //[CodeAnnotation("兵种进阶消耗")]
        public SDictionary<品质, EcoCostData> CardJinJie { get; set; }
        /// <summary>
        /// 兵种品质消耗
        /// </summary>
        //[CodeAnnotation("兵种品质消耗")]
        public SDictionary<品质, EcoCostData> CardPinZhi { get; set; }
        /// <summary>
        /// 宝石兑换碎片的比例
        /// </summary>
        //[CodeAnnotation("宝石兑换碎片的比例")]
        public SDictionary<品质, double> Baoshi2SuiPian { get; set; }
        /// <summary>
        /// 货币对应宝石兑换比例
        /// </summary>
        //[CodeAnnotation("宝石兑换各种货币的比例")]
        public SDictionary<CurrencyType, double> Baoshi2Currency { get; set; }
        //[CodeAnnotation("全局系数最大增加")]
        public SDictionary<UserProeprtyType, double> MaxCoefficientLimit { get; set; }
        //[CodeAnnotation("")]
        public SList<XingZuoModel> XingZuoModels { get; set; }
        //[CodeAnnotation("星座闯关奖励")]
        public SList<Award> XingZuoAward { get; set; }
        public SList<SList<ushort>> XingZuoMonsters { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public override void Deserialize(BufferReader reader)
        {
            reader.Read(out Int32 _升品最大等级);
            this.升品最大等级= _升品最大等级;
            reader.Read(out Int32 _进阶最大等级);
            this.进阶最大等级= _进阶最大等级;
            reader.Read(out Int32 _天赋正方形尺寸);
            this.天赋正方形尺寸= _天赋正方形尺寸;
            reader.Read(out Int32 _神器最大等级);
            this.神器最大等级= _神器最大等级;
            reader.Read(out Int32 _星座最大等级);
            this.星座最大等级= _星座最大等级;
            reader.Read(out Int32 _等级开放跨度);
            this.等级开放跨度= _等级开放跨度;
            reader.Read(out SDictionary<Int32,Double> _总进阶影响金币产出系数);
            this.总进阶影响金币产出系数= _总进阶影响金币产出系数;
            reader.Read(out SDictionary<Int32,Double> _总关卡影响金币产出系数);
            this.总关卡影响金币产出系数= _总关卡影响金币产出系数;
            reader.Read(out EcoCostData _CardLevelUp);
            this.CardLevelUp= _CardLevelUp;
            reader.Read(out Double _JinBiAwardSec);
            this.JinBiAwardSec= _JinBiAwardSec;
            reader.Read(out Currency _TianFuResetPrice);
            this.TianFuResetPrice= _TianFuResetPrice;
            reader.Read(out SDictionary<Int32,Currency> _TianFuResetAllPrice);
            this.TianFuResetAllPrice= _TianFuResetAllPrice;
            reader.Read(out SDictionary<Int32,JinjieLimit> _JinjieLimits);
            this.JinjieLimits= _JinjieLimits;
            reader.Read(out SDictionary<UInt16,CardModel> _AllCards);
            this.AllCards= _AllCards;
            reader.Read(out SDictionary<UInt16,CardModel> _ExtraCards);
            this.ExtraCards= _ExtraCards;
            reader.Read(out SDictionary<UInt16,ShenQiModle> _ShenQiModles);
            this.ShenQiModles= _ShenQiModles;
            reader.Read(out SDictionary<UInt32,Currency> _ShenQiBack);
            this.ShenQiBack= _ShenQiBack;
            reader.Read(out SDictionary<UserProeprtyType,Double> _TianfuProperties);
            this.TianfuProperties= _TianfuProperties;
            reader.Read(out SDictionary<品质,EcoCostData> _ShengQiLevelUp);
            this.ShengQiLevelUp= _ShengQiLevelUp;
            reader.Read(out EcoCostData _TianfuLevelUp);
            this.TianfuLevelUp= _TianfuLevelUp;
            reader.Read(out SDictionary<品质,EcoCostData> _CardJinJie);
            this.CardJinJie= _CardJinJie;
            reader.Read(out SDictionary<品质,EcoCostData> _CardPinZhi);
            this.CardPinZhi= _CardPinZhi;
            reader.Read(out SDictionary<品质,Double> _Baoshi2SuiPian);
            this.Baoshi2SuiPian= _Baoshi2SuiPian;
            reader.Read(out SDictionary<CurrencyType,Double> _Baoshi2Currency);
            this.Baoshi2Currency= _Baoshi2Currency;
            reader.Read(out SDictionary<UserProeprtyType,Double> _MaxCoefficientLimit);
            this.MaxCoefficientLimit= _MaxCoefficientLimit;
            reader.Read(out SList<XingZuoModel> _XingZuoModels);
            this.XingZuoModels= _XingZuoModels;
            reader.Read(out SList<Award> _XingZuoAward);
            this.XingZuoAward= _XingZuoAward;
            reader.Read(out SList<SList<UInt16>> _XingZuoMonsters);
            this.XingZuoMonsters= _XingZuoMonsters;
            base.Deserialize(reader);
        }
        public override void Serialize(BufferWriter sender)
        {
            sender.Write(升品最大等级);
            sender.Write(进阶最大等级);
            sender.Write(天赋正方形尺寸);
            sender.Write(神器最大等级);
            sender.Write(星座最大等级);
            sender.Write(等级开放跨度);
            sender.Write(总进阶影响金币产出系数);
            sender.Write(总关卡影响金币产出系数);
            sender.Write(CardLevelUp);
            sender.Write(JinBiAwardSec);
            sender.Write(TianFuResetPrice);
            sender.Write(TianFuResetAllPrice);
            sender.Write(JinjieLimits);
            sender.Write(AllCards);
            sender.Write(ExtraCards);
            sender.Write(ShenQiModles);
            sender.Write(ShenQiBack);
            sender.Write(TianfuProperties);
            sender.Write(ShengQiLevelUp);
            sender.Write(TianfuLevelUp);
            sender.Write(CardJinJie);
            sender.Write(CardPinZhi);
            sender.Write(Baoshi2SuiPian);
            sender.Write(Baoshi2Currency);
            sender.Write(MaxCoefficientLimit);
            sender.Write(XingZuoModels);
            sender.Write(XingZuoAward);
            sender.Write(XingZuoMonsters);
            base.Serialize(sender);
        }
        #endregion

    }
}
