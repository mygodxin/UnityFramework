
using System;
namespace DuiChongServerCommon.ClientProtocol
{
    //[CodeAnnotation("商城数据")]
    public class MarktData : IKHSerializable
    {
        public static MarktData CreatNew()
        {
            return new MarktData()
            {
                BuyTime = new SDictionary<ushort, uint>(),
                BuyCuntToday = new SDictionary<ushort, uint>(),
                DynamicAwardToday = new SDictionary<ushort, Award>(),
                DynamicPriceToday = new SDictionary<ushort, Currency>(),
                EQFightDate = new SDictionary<byte, DateTime>(),
            };
        }
        /// <summary>
        /// 今日购买商品次数
        /// </summary>
        //[CodeAnnotation("今日购买商品次数")]
        public SDictionary<ushort, uint> BuyCuntToday { get; set; }
        /// <summary>
        /// 商品总购买次数
        /// </summary>
        //[CodeAnnotation("商品总购买次数")]
        public SDictionary<ushort, uint> BuyTime { get; set; }
        /// <summary>
        /// DynamicPrice
        /// </summary>
        //[CodeAnnotation("商品的动态价格")]
        public SDictionary<ushort, Currency> DynamicPriceToday { get; set; }
        /// <summary>
        /// DynamicAward
        /// </summary>
        //[CodeAnnotation("商品的动态货物")]
        public SDictionary<ushort, Award> DynamicAwardToday { get; set; }
        /// <summary>
        /// 真实人民币累计充值数额
        /// </summary>
        //[CodeAnnotation("真实人民币累计充值数额")]
        public uint RMBCost { get; set; }
        /// <summary>
        /// 抽奖累计次数(用于计算抽奖宝箱的开启)
        /// </summary>
        //[CodeAnnotation("抽奖累计次数(用于计算抽奖宝箱的开启)")]
        public int ChouJiangCount { get; set; }
        //[CodeAnnotation("今日累计抽奖次数  用于抽奖限制  日结算清除")]
        public int ChouJiangToday { get; set; }
        /// <summary>
        /// 今日广告观看次数
        /// </summary>
        //[CodeAnnotation("今日广告观看次数")]
        public ushort WatchAdCountToday { get; set; }
        /// <summary>
        /// 真实看广告的次数(用于计算反广告券)
        /// </summary>

        public byte RealADCount { get; set; }
        /// <summary>
        /// 今日广告宝箱开启次数
        /// </summary>
        //[CodeAnnotation("今日广告宝箱开启次数")]
        public ushort ADBoxGetCountToday { get; set; }

        //[CodeAnnotation("装备大作战最后开启时间 key 为index 如果没有 则为还没开启过")]
        public SDictionary<byte, DateTime> EQFightDate { get; set; }
   
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out SDictionary<UInt16,UInt32> _BuyCuntToday);
            this.BuyCuntToday= _BuyCuntToday;
            reader.Read(out SDictionary<UInt16,UInt32> _BuyTime);
            this.BuyTime= _BuyTime;
            reader.Read(out SDictionary<UInt16,Currency> _DynamicPriceToday);
            this.DynamicPriceToday= _DynamicPriceToday;
            reader.Read(out SDictionary<UInt16,Award> _DynamicAwardToday);
            this.DynamicAwardToday= _DynamicAwardToday;
            reader.Read(out UInt32 _RMBCost);
            this.RMBCost= _RMBCost;
            reader.Read(out Int32 _ChouJiangCount);
            this.ChouJiangCount= _ChouJiangCount;
            reader.Read(out Int32 _ChouJiangToday);
            this.ChouJiangToday= _ChouJiangToday;
            reader.Read(out UInt16 _WatchAdCountToday);
            this.WatchAdCountToday= _WatchAdCountToday;
            reader.Read(out UInt16 _ADBoxGetCountToday);
            this.ADBoxGetCountToday= _ADBoxGetCountToday;
            reader.Read(out SDictionary<Byte,DateTime> _EQFightDate);
            this.EQFightDate= _EQFightDate;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(BuyCuntToday);
            sender.Write(BuyTime);
            sender.Write(DynamicPriceToday);
            sender.Write(DynamicAwardToday);
            sender.Write(RMBCost);
            sender.Write(ChouJiangCount);
            sender.Write(ChouJiangToday);
            sender.Write(WatchAdCountToday);
            sender.Write(ADBoxGetCountToday);
            sender.Write(EQFightDate);
        }
        #endregion

    }
}
