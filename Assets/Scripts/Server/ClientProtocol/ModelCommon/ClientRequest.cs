
using System;

namespace DuiChongServerCommon.ClientProtocol
{
    //[CodeAnnotation("天赋更改请求")]
    public struct TFReq : IKHSerializable
    {
        public 种族 ZZ { get; set; }
        //[CodeAnnotation("立方体6个面 PageIndex <= 5")]
        public byte Page { get; set; }
        //[CodeAnnotation("Index <=Math.Pow(CardConfig.Instance.天赋正方形尺寸, 2) - 1")]
        public byte Index { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public void Deserialize(BufferReader reader)
        {
            reader.Read(out byte _ZZ);
            this.ZZ= (种族)_ZZ;
            reader.Read(out byte _Page);
            this.Page= _Page;
            reader.Read(out byte _Index);
            this.Index= _Index;
        }
        public void Serialize(BufferWriter sender)
        {
            sender.Write((byte)ZZ);
            sender.Write(Page);
            sender.Write(Index);
        }
        #endregion

    }
    public struct BuyRequest : IKHSerializable
    {
        //[CodeAnnotation("购买商品的Pid")]
        public ushort Pid { get; set; }
        //[CodeAnnotation("购买数量")]
        public ushort BuyCount { get; set; }
        //[CodeAnnotation("购买价格的下标 (-1 表示使用主价格 就是 CommodityModel.Price >-1 表示使用CommodityModel.OtherPrice[index] 购买)")]
        public sbyte PriceIndex { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public void Deserialize(BufferReader reader)
        {
            reader.Read(out UInt16 _Pid);
            this.Pid= _Pid;
            reader.Read(out UInt16 _BuyCount);
            this.BuyCount= _BuyCount;
            reader.Read(out sbyte _PriceIndex);
            this.PriceIndex= _PriceIndex;
        }
        public void Serialize(BufferWriter sender)
        {
            sender.Write(Pid);
            sender.Write(BuyCount);
            sender.Write(PriceIndex);
        }
        #endregion

    }
}
