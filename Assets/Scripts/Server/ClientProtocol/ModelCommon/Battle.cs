using System;

namespace DuiChongServerCommon.ClientProtocol
{
    public struct MonsterInfo : IKHSerializable
    {
        //[CodeAnnotation("属于哪场战斗的怪物")]
        public BattleType BattleType { get; set; }
        //[CodeAnnotation("怪物的卡牌PID")]
        public ushort Pid { get; set; }
        //[CodeAnnotation("是否是BOSS")]
        public bool IsBoss { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public void Deserialize(BufferReader reader)
        {
            reader.Read(out byte _BattleType);
            this.BattleType= (BattleType)_BattleType;
            reader.Read(out UInt16 _Pid);
            this.Pid= _Pid;
            reader.Read(out Boolean _IsBoss);
            this.IsBoss= _IsBoss;
        }
        public void Serialize(BufferWriter sender)
        {
            sender.Write((byte)BattleType);
            sender.Write(Pid);
            sender.Write(IsBoss);
        }
        #endregion

    }
    //[CodeAnnotation("机器人")]
    public struct RobotInfo : IKHSerializable
    {
        public double Power { get; set; }
        public string AvatarUrl { get; set; }
        public string Name { get; set; }
        public int HightestMissionLevel { get; set; }
        public int HightestArenaScore { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public void Deserialize(BufferReader reader)
        {
            reader.Read(out Double _Power);
            this.Power= _Power;
            reader.Read(out String _AvatarUrl);
            this.AvatarUrl= _AvatarUrl;
            reader.Read(out String _Name);
            this.Name= _Name;
            reader.Read(out Int32 _HightestMissionLevel);
            this.HightestMissionLevel= _HightestMissionLevel;
            reader.Read(out Int32 _HightestArenaScore);
            this.HightestArenaScore= _HightestArenaScore;
        }
        public void Serialize(BufferWriter sender)
        {
            sender.Write(Power);
            sender.Write(AvatarUrl);
            sender.Write(Name);
            sender.Write(HightestMissionLevel);
            sender.Write(HightestArenaScore);
        }
        #endregion

    }
}
