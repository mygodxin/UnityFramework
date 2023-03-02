using System;

namespace DuiChongServerCommon.ClientProtocol
{
    public class BattleCheck : IKHSerializable
    {
        public SList<Card> Cards { get; set; }
        //[CodeAnnotation("加速检查(客户端真实经过时间 毫秒)")]
        public double ClientRunTimes { get; set; }
        public UserProperty UserProperty { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out SList<Card> _Cards);
            this.Cards= _Cards;
            reader.Read(out Double _ClientRunTimes);
            this.ClientRunTimes= _ClientRunTimes;
            reader.Read(out UserProperty _UserProperty);
            this.UserProperty= _UserProperty;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(Cards);
            sender.Write(ClientRunTimes);
            sender.Write(UserProperty);
        }
        #endregion

    }
}
