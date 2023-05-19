using System;
using System.Text;

namespace DuiChongServerCommon.ClientProtocol
{
    //[CodeAnnotation("网关握手消息")]
    public class GateHandShake : IKHSerializable
    {
        //[CodeAnnotation("请求加密密匙")]
        public SList<int> EncryptKeys { get; set; }
        //[CodeAnnotation("请求加密签名")]
        public SDictionary<byte, int> RequestEncrytSign { get; set; }
   
   
   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out SList<Int32> _EncryptKeys);
            this.EncryptKeys= _EncryptKeys;
            reader.Read(out SDictionary<Byte,Int32> _RequestEncrytSign);
            this.RequestEncrytSign= _RequestEncrytSign;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(EncryptKeys);
            sender.Write(RequestEncrytSign);
        }
        #endregion

    }
    /// <summary>
    /// 账号登录成功返回
    /// </summary>
    //[CodeAnnotation("账号登录成功返回")]
    public class AccountLoginResponse : IKHSerializable
    {
        //[CodeAnnotation("上次登录的时间")]
        /// <summary>
        /// 上次登录的时间
        /// </summary>
        public DateTime LastLoginDate { get; set; }
        /// <summary>
        /// 历史总登录次数(如果为1表明是新玩家)
        /// </summary>
        //[CodeAnnotation("历史总登录次数(如果为1表明是新玩家)")]
        public uint HsitoryLoginCount { get; set; }
        //[CodeAnnotation("游戏服务器信息")]
        public SList<GameServerInfo> GameServers { get; set; }


   
   
   
   
        #region AutoProtocol
        public virtual void Deserialize(BufferReader reader)
        {
            reader.Read(out DateTime _LastLoginDate);
            this.LastLoginDate= _LastLoginDate;
            reader.Read(out UInt32 _HsitoryLoginCount);
            this.HsitoryLoginCount= _HsitoryLoginCount;
            reader.Read(out SList<GameServerInfo> _GameServers);
            this.GameServers= _GameServers;
        }
        public virtual void Serialize(BufferWriter sender)
        {
            sender.Write(LastLoginDate);
            sender.Write(HsitoryLoginCount);
            sender.Write(GameServers);
        }
        #endregion

    }
    public struct LoginAccount : IKHSerializable
    {
        //[CodeAnnotation("邀请者的邀请码 为空则没有邀请者")]
        public string InvitationCode { get; set; }
        //[CodeAnnotation("账号  如果是微信平台为 微信code")]
        public string Account { get; set; }
        //[CodeAnnotation("密码")]
        public string Password { get; set; }
        //[CodeAnnotation("登陆平台")]
        public Platform Platform { get; set; }
        //[CodeAnnotation("平台用户的昵称")]
        public string Name { get; set; }
        //[CodeAnnotation("平台用户头像URL")]
        public string AvatarUrl { get; set; }
   
   
   
   
   
   
        #region AutoProtocol
        public void Deserialize(BufferReader reader)
        {
            reader.Read(out String _InvitationCode);
            this.InvitationCode= _InvitationCode;
            reader.Read(out String _Account);
            this.Account= _Account;
            reader.Read(out String _Password);
            this.Password= _Password;
            reader.Read(out byte _Platform);
            this.Platform= (Platform)_Platform;
            reader.Read(out String _Name);
            this.Name= _Name;
            reader.Read(out String _AvatarUrl);
            this.AvatarUrl= _AvatarUrl;
        }
        public void Serialize(BufferWriter sender)
        {
            sender.Write(InvitationCode);
            sender.Write(Account);
            sender.Write(Password);
            sender.Write((byte)Platform);
            sender.Write(Name);
            sender.Write(AvatarUrl);
        }
        #endregion

    }
    public struct InvitationCode
    {
        public uint ID { get; set; }
        public Platform Platform { get; set; }
        public override string ToString()
        {
            return string.Format("ID={0}\nPlatform={1}\n", this.ID, this.Platform);
        }
    }
    public struct WeChatObjcet
    {
        //[CodeAnnotation("用户唯一标识")]
        public string openid;
        //[CodeAnnotation("会话密钥")]
        public string session_key;
        //[CodeAnnotation("用户在开放平台的唯一标识符，若当前小程序已绑定到微信开放平台帐号下会返回，详见 https://developers.weixin.qq.com/minigame/dev/guide/open-ability/union-id.html")]
        public string unionid;
        //[CodeAnnotation("错误码  0=请求成功|-1=系统繁忙，此时请开发者稍候再试|40029=code无效|45011=频率限制，每个用户每分钟100次   ")]
        public int errcode;
        //[CodeAnnotation("错误信息")]
        public string errmsg;
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var item in this.GetType().GetFields())
            {
                sb.Append(string.Format("{0} = {1}\n", item.Name, item.GetValue(this)));
            }
            return sb.ToString();
        }
    }
    public struct TapTapObject
    {
        public String kid;
        public String access_token;
        public String token_type;
        public String mac_key;
        public String mac_algorithm;
        public String expire_in;
    }
}
