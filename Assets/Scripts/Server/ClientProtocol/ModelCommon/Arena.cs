using System;

namespace DuiChongServerCommon.ClientProtocol
{
    public struct MyRank : IKHSerializable
    {
        public double Score { get; set; }
        public uint Rank { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public void Deserialize(BufferReader reader)
        {
            reader.Read(out Double _Score);
            this.Score= _Score;
            reader.Read(out UInt32 _Rank);
            this.Rank= _Rank;
        }
        public void Serialize(BufferWriter sender)
        {
            sender.Write(Score);
            sender.Write(Rank);
        }
        #endregion

    }
    public class UserRanking : IKHSerializable
    {
        //[CodeAnnotation("分数")]
        public double Score { get; set; }
        public UserInfoTiny Info { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out Double _Score);
            this.Score= _Score;
            reader.Read(out UserInfoTiny _Info);
            this.Info= _Info;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(Score);
            sender.Write(Info);
        }
        #endregion

    }
}
