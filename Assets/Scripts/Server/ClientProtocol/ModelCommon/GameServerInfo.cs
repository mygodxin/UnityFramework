using System;

namespace DuiChongServerCommon.ClientProtocol
{
    public class GameServerInfo : IKHSerializable
    {
        public int ServerID { get; set; }
        public string ServerName { get; set; }
        public string GateIp { get; set; }
        public string ConnectCode { get; set; }
        public ServerStatus ServerState { get; set; }
        //[CodeAnnotation("角色信息")]
        public SDictionary<uint, UserInfoTiny> UserInfo { get; set; }
        //[CodeAnnotation("开服时间")]
        public DateTime OpenDate { get; set; }
   
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out Int32 _ServerID);
            this.ServerID= _ServerID;
            reader.Read(out String _ServerName);
            this.ServerName= _ServerName;
            reader.Read(out String _GateIp);
            this.GateIp= _GateIp;
            reader.Read(out String _ConnectCode);
            this.ConnectCode= _ConnectCode;
            reader.Read(out byte _ServerState);
            this.ServerState= (ServerStatus)_ServerState;
            reader.Read(out SDictionary<UInt32,UserInfoTiny> _UserInfo);
            this.UserInfo= _UserInfo;
            reader.Read(out DateTime _OpenDate);
            this.OpenDate= _OpenDate;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(ServerID);
            sender.Write(ServerName);
            sender.Write(GateIp);
            sender.Write(ConnectCode);
            sender.Write((byte)ServerState);
            sender.Write(UserInfo);
            sender.Write(OpenDate);
        }
        #endregion

    }
}
