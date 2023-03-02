
using KHCore.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
namespace DuiChongServerCommon.ClientProtocol
{
    //[CodeAnnotation("支付信息的穿透参数")]
    public class PayExtra : IKHSerializable
    {
        public static PayExtra Creat(string base64Str)
        {
            return new BufferReader(Convert.FromBase64String(base64Str)).ReadSerializable<PayExtra>();
        }
        public uint UserID { get; set; }
        public int GameServerID { get; set; }
        public ushort CommodityID { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out UInt32 _UserID);
            this.UserID= _UserID;
            reader.Read(out Int32 _GameServerID);
            this.GameServerID= _GameServerID;
            reader.Read(out UInt16 _CommodityID);
            this.CommodityID= _CommodityID;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(UserID);
            sender.Write(GameServerID);
            sender.Write(CommodityID);
        }
        #endregion

    }
    //[CodeAnnotation("随机卡牌")]
    public class RandomCard : IKHSerializable
    {
        public RandomCard()
        {
        }
        public RandomCard(Range value, 品质 pinhi, 兵种职业 zhiye, 种族 zhongzu, ushort pid)
        {
            this.Value = value;
            this.PinZhi = pinhi;
            this.ZhongZu = zhongzu;
            this.Pid = pid;
            this.ZhiYe = zhiye;
        }
        //[CodeAnnotation("数量 如果Max=Min说明数量固定")]
        public Range Value { get; private set; }
        public 品质 PinZhi { get; private set; }
        public 兵种职业 ZhiYe { get; private set; }
        public 种族 ZhongZu { get; private set; }
        //[CodeAnnotation("如果不为0 说明卡牌固定 可以忽略 品质 职业 种族")]
        public ushort Pid { get; private set; }
        public RandomCard CreatCopy(double bili)
        {
            return new RandomCard()
            {
                Pid = this.Pid,
                PinZhi = this.PinZhi,
                Value = Value == null ? null : new Range(this.Value.Min * bili, this.Value.Min * bili),
                ZhiYe = this.ZhiYe,
                ZhongZu = this.ZhongZu,
            };
        }
        public bool SameLimit(RandomCard other)
        {
            if (other.Pid != this.Pid)
                return false;
            if (other.PinZhi != this.PinZhi)
                return false;
            if (other.ZhiYe != this.ZhiYe)
                return false;
            if (other.ZhongZu != this.ZhongZu)
                return false;
            return true;
        }
        public static RandomCard Plus(RandomCard org, RandomCard other)
        {
            var newCard = new RandomCard()
            {
                Pid = org.Pid,
                PinZhi = org.PinZhi,
                ZhongZu = org.ZhongZu,
                ZhiYe = org.ZhiYe,
            };
            if (other.Value != null)
            {
                newCard.Value = Range.Plus(other.Value, org.Value);
            }
            return newCard;
        }
        public override string ToString()
        {
            var str = "";
            foreach (var item in this.GetType().GetProperties())
            {
                if (!item.PropertyType.IsEnum)
                    continue;
                var value = item.GetValue(this).ToString();
                if (value != "None")
                {
                    str += $"[{value}]";
                }
            }
            str += $"={this.Value?.ToString()}";
            return str;
        }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out Range _Value);
            this.Value= _Value;
            reader.Read(out byte _PinZhi);
            this.PinZhi= (品质)_PinZhi;
            reader.Read(out byte _ZhiYe);
            this.ZhiYe= (兵种职业)_ZhiYe;
            reader.Read(out byte _ZhongZu);
            this.ZhongZu= (种族)_ZhongZu;
            reader.Read(out UInt16 _Pid);
            this.Pid= _Pid;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(Value);
            sender.Write((byte)PinZhi);
            sender.Write((byte)ZhiYe);
            sender.Write((byte)ZhongZu);
            sender.Write(Pid);
        }
        #endregion

    }

    //[CodeAnnotation("随机装备")]
    public class RandomEquip : IKHSerializable
    {
        public RandomEquip()
        {
        }
        public RandomEquip(byte star, byte intensify, params Enum[] enumLimits)
        {
            if (enumLimits != null && enumLimits.Length > 0)
            {
                Dictionary<Type, PropertyInfo> enumPro = new Dictionary<Type, PropertyInfo>();
                foreach (var item in this.GetType().GetProperties())
                {
                    if (item.PropertyType.IsEnum)
                    {
                        enumPro.Add(item.PropertyType, item);
                    }
                }
                foreach (var item in enumLimits)
                {
                    enumPro[item.GetType()].SetValue(this, item);
                }
            }
            this.Star = star == byte.MinValue ? (byte)1 : star;
            this.Intensify = intensify;
        }
        public RandomEquip(byte star, byte intensify, 品质 PinZhi)
        {
            this.Star = star == byte.MinValue ? (byte)1 : star;
            this.Intensify = intensify;
            this.PinZhi = PinZhi;
            if (this.PinZhi == 品质.神话)
            {
                this.Star = 30;
            }
        }
        //[CodeAnnotation("星级")]
        public byte Star { get; private set; }
        //[CodeAnnotation("强化等级")]
        public byte Intensify { get; private set; }
        public 品质 PinZhi { get; private set; }
        public EquipPos Pos { get; private set; }
        public 兵种职业 ZhiYe { get; private set; }
        public int Count { get; private set; } = 1;
        public bool Plus(RandomEquip other,int countOffset)
        {
            if (this.SameLimit(other))
            {
                this.Count+=(other.Count+countOffset);
                if (Count<=0)
                {
                    throw new Exception("RandomEquip.Count can not less or eq zero");
                }
                return true;
            }
            return false;
        }
        bool SameLimit(RandomEquip other)
        {
            return
                this.Star== other.Star&&
                this.Intensify==other.Intensify&&
                this.PinZhi==other.PinZhi&&
                this.Pos==other.Pos&&
                this.ZhiYe==other.ZhiYe;
        }
        public RandomEquip CreatCopy(int countoffset)
        {
            var eq= new RandomEquip() 
            {
                Star= this.Star,
                Count= this.Count+countoffset,
                Intensify= this.Intensify,
                PinZhi= this.PinZhi,
                Pos = this.Pos,
                ZhiYe= this.ZhiYe,
            };
            if (Count<=0)
            {
                throw new Exception("RandomEquip.Count can not less or eq zero");
            }
            return eq;
        }
  
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out Byte _Star);
            this.Star= _Star;
            reader.Read(out Byte _Intensify);
            this.Intensify= _Intensify;
            reader.Read(out byte _PinZhi);
            this.PinZhi= (品质)_PinZhi;
            reader.Read(out byte _Pos);
            this.Pos= (EquipPos)_Pos;
            reader.Read(out byte _ZhiYe);
            this.ZhiYe= (兵种职业)_ZhiYe;
            reader.Read(out Int32 _Count);
            this.Count= _Count;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(Star);
            sender.Write(Intensify);
            sender.Write((byte)PinZhi);
            sender.Write((byte)Pos);
            sender.Write((byte)ZhiYe);
            sender.Write(Count);
        }
        #endregion

    }

    //[CodeAnnotation("奖励类")]
    //[BsonSerializer(typeof(Award))]
    public class Award : IKHSerializable//, IBsonSerializer<Award>
    {
        public bool InPool
        {
            get
            {
                return true;
            }
            set
            {
                
            }
        }

        #region BsonSerializer
        //[DontAutoProtocol]
        //public Type ValueType => typeof(Award);
        //Award IBsonSerializer<Award>.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        //{
        //    return new BufferReader(context.Reader.ReadBinaryData().Bytes).ReadSerializable<Award>();
        //}
        //public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Award value)
        //{
        //    context.Writer.WriteBytes(new BufferWriter(128).Write(value).GetBuffer);
        //}
        //public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        //{
        //    return new BufferReader(context.Reader.ReadBinaryData().Bytes).ReadSerializable<Award>();
        //}
        //public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        //{
        //    context.Writer.WriteBytes(new BufferWriter(128).Write(value as Award).GetBuffer);
        //}
        #endregion
        //[CodeAnnotation("货币")]
        public SDictionary<CurrencyType, double> Currencies { get; set; }
        //[CodeAnnotation("随机货币")]
        public SDictionary<CurrencyType, Range> RandomCurrencies { get; set; }
        //[CodeAnnotation("卡牌碎片")]
        public SDictionary<ushort, uint> CardDebris { get; set; }
        //[CodeAnnotation("随机卡牌")]
        public SList<RandomCard> RandomCards { get; set; }
        //[CodeAnnotation("道具奖励  (目前道具只有装备<Equip> 保不齐将来会加 消耗品之类的 所以要判断下Item的类型)")]
        public SList<Item> Items { get; set; }
        //[CodeAnnotation("随机装备")]
        public SList<RandomEquip> RandomEquips { get; set; }
        //[CodeAnnotation("卡牌皮肤")]
        public SList<CardValue> CardSkins { get; set; }

        public int Count
        {
            get
            {
                int count = 0;
                count += Currencies == null ? 0 : Currencies.Count;
                count += RandomCurrencies == null ? 0 : RandomCurrencies.Count;
                count += CardDebris == null ? 0 : CardDebris.Count;
                count += RandomCards == null ? 0 : RandomCards.Count;
                count += Items == null ? 0 : Items.Count;
                count += RandomEquips == null ? 0 : RandomEquips.Count;
                count += CardSkins == null ? 0 : CardSkins.Count;
                return count;
            }
        }

        public Award AddItem(Item item)
        {
            this.Items ??= new SList<Item>();
            this.Items.Add(item);
            return this;
        }
        public Award AddItem<T>(IList<T> items) where T : Item
        {
            this.Items ??= new SList<Item>();
            this.Items.AddRange(items);
            return this;
        }
        public Award AddRandomEquip(RandomEquip limit,int countOffset=0)
        {
            this.RandomEquips ??= new SList<RandomEquip>();
            for (int i = 0; i < this.RandomEquips.Count; i++)
            {
                var org = this.RandomEquips[i];
                if (org.Plus(limit,countOffset))
                {
                    return this;
                }
            }
            this.RandomEquips.Add(limit.CreatCopy(countOffset));
            return this;
        }
        public Award AddRandomCurrency(CurrencyType type, Range range)
        {
            this.RandomCurrencies ??= new SDictionary<CurrencyType, Range>();
            if (!this.RandomCurrencies.TryGetValue(type, out var thisRange))
            {
                this.RandomCurrencies.Add(type, range);
            }
            else
            {
                this.RandomCurrencies[type] = Range.Plus(thisRange, range);
            }
            return this;
        }
        public Award AddCardSkin(ushort pid, byte skin)
        {
            this.CardSkins ??= new SList<CardValue>();
            this.CardSkins.Add(new CardValue() { Pid = pid, Value = skin });
            return this;
        }
        public Award Clear()
        {
            this.Currencies?.Clear();
            this.CardDebris?.Clear();
            this.Items?.Clear();
            this.RandomCards?.Clear();
            this.RandomEquips?.Clear();
            this.RandomCurrencies?.Clear();
            this.CardSkins?.Clear();
            return this;
        }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out SDictionary<CurrencyType,Double> _Currencies);
            this.Currencies= _Currencies;
            reader.Read(out SDictionary<CurrencyType,Range> _RandomCurrencies);
            this.RandomCurrencies= _RandomCurrencies;
            reader.Read(out SDictionary<UInt16,UInt32> _CardDebris);
            this.CardDebris= _CardDebris;
            reader.Read(out SList<RandomCard> _RandomCards);
            this.RandomCards= _RandomCards;
            reader.Read(out SList<Item> _Items);
            this.Items= _Items;
            reader.Read(out SList<RandomEquip> _RandomEquips);
            this.RandomEquips= _RandomEquips;
            reader.Read(out SList<CardValue> _CardSkins);
            this.CardSkins= _CardSkins;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(Currencies);
            sender.Write(RandomCurrencies);
            sender.Write(CardDebris);
            sender.Write(RandomCards);
            sender.Write(Items);
            sender.Write(RandomEquips);
            sender.Write(CardSkins);
        }
        #endregion

    }
    public class ChouJiangAward : Award, IRandomObject
    {
        //[CodeAnnotation("概率")]
        public double RandomNumber { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public override void Deserialize(BufferReader reader)
        {
            reader.Read(out Double _RandomNumber);
            this.RandomNumber= _RandomNumber;
            base.Deserialize(reader);
        }
        public override void Serialize(BufferWriter sender)
        {
            sender.Write(RandomNumber);
            base.Serialize(sender);
        }
        #endregion

    }
    //[CodeAnnotation("可选择的奖励")]
    /// <summary>
    /// 可选择的奖励
    /// </summary>
    public class OptionalAwards : IKHSerializable
    {
        /// <summary>
        /// 固定奖励
        /// </summary>
        //[CodeAnnotation("固定奖励 ")]
        public SList<Award> FixedAwards { get; set; }
        /// <summary>
        /// 单选奖励
        /// </summary>
        //[CodeAnnotation("单选奖励")]
        public SList<Award> SingleAwards { get; set; }
        //[CodeAnnotation("多选奖励可选择的数量")]
        public ushort MultipleCount { get; set; }
        /// <summary>
        /// 多选奖励
        /// </summary>
        //[CodeAnnotation("多选奖励")]
        public SList<Award> MultipleAwards { get; set; }
        public List<Award> GetAward(AwardSelect awardSelect)
        {
            var lis = new List<Award>();
            if (this.FixedAwards != null)
            {
                lis.AddRange(this.FixedAwards);
            }
            if (this.SingleAwards != null && this.SingleAwards.Count > 0)
            {
                lis.Add(SingleAwards[awardSelect.SingleSelect]);
            }
            if (this.MultipleAwards != null && this.MultipleAwards.Count > 0)
            {
                foreach (var item in awardSelect.MultipleSelect)
                {
                    lis.Add(this.MultipleAwards[item]);
                }
            }
            return lis;
        }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out SList<Award> _FixedAwards);
            this.FixedAwards= _FixedAwards;
            reader.Read(out SList<Award> _SingleAwards);
            this.SingleAwards= _SingleAwards;
            reader.Read(out UInt16 _MultipleCount);
            this.MultipleCount= _MultipleCount;
            reader.Read(out SList<Award> _MultipleAwards);
            this.MultipleAwards= _MultipleAwards;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(FixedAwards);
            sender.Write(SingleAwards);
            sender.Write(MultipleCount);
            sender.Write(MultipleAwards);
        }
        #endregion

    }
    //[CodeAnnotation("用户对可选奖励选择结果")]
    /// <summary>
    /// 用户对可选奖励选择结果
    /// </summary>
    public class AwardSelect : IKHSerializable
    {
        /// <summary>
        /// 单选奖励 index
        /// </summary>
        //[CodeAnnotation("单选奖励")]
        public ushort SingleSelect { get; set; }
        //[CodeAnnotation("多选奖励")]
        public SList<ushort> MultipleSelect { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out UInt16 _SingleSelect);
            this.SingleSelect= _SingleSelect;
            reader.Read(out SList<UInt16> _MultipleSelect);
            this.MultipleSelect= _MultipleSelect;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(SingleSelect);
            sender.Write(MultipleSelect);
        }
        #endregion

    }
}
