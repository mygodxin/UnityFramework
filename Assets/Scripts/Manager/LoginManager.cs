using DuiChongServerCommon.ClientProtocol;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

public class ClientServerInfo : GameServerInfo
{
    int UserId;
    public ClientServerInfo(GameServerInfo info) : base()
    {
        this.GateIp = info.GateIp;
        this.ConnectCode = info.ConnectCode;
        this.OpenDate = info.OpenDate;
        this.ServerID = info.ServerID;
        this.ServerName = info.ServerName;
        this.ServerState = info.ServerState;
        this.UserInfo = info.UserInfo;
    }
}
/// <summary>
/// 登录管理
/// </summary>
public class LoginManager
{
    private LoginAccount _loginAccount;
    public LoginAccount loginAccount
    {
        get
        {
            return _loginAccount;
        }
    }
    private List<GameServerInfo> _serverList;
    private GameServerInfo _curServerInfo;
    private static LoginManager _inst = null;
    public static LoginManager inst
    {
        get
        {
            if (_inst == null)
                _inst = new LoginManager();
            return _inst;
        }
    }

    public async Task<string> Login(LoginAccount data,bool switchServer = false)
    {
        this._loginAccount = data;
        if (this._serverList == null)
        {
            //校验sdk登录账号
            var loginResult = await this.LoginAccount(data);
            if (loginResult.returnCode == ReturnCode.Filed)
            {
                var login = loginResult.bufferReader.ReadSerializable<AccountLoginResponse>();
                //新玩家
                if (login.HsitoryLoginCount == 1)
                {
                    //发送埋点
                }

                this._serverList = login.GameServers;
            }
            else
            {
                return "获取服务器列表失败！";
            }
        }
        var info = await this.GetLoginServer(switchServer);
        if(info != null)
        {
            ConnectToGate(info.GateIp);
        }
        return null;
    }
    //获取登录服务器
    private async Task<ClientServerInfo> GetLoginServer(bool switchServer)
    {
        ClientServerInfo info = null;
        if (switchServer)
        {
            //等待用户UI选择服务器
            info = await Task.Run( () =>
            {
                ClientServerInfo server = null;
                Action<ClientServerInfo> action = (ClientServerInfo s) =>
                {
                    server = s;
                };
                UIManager.inst.ShowWindow<ServerWin>(action);
                return server;
            });
        }
        else
        {
            ClientServerInfo lastServer = LocalStorage.Read<ClientServerInfo>(StorageDef.LastServer);
            if (lastServer != null)
            {
                for (int i = 0; i < this._serverList.Count; i++)
                {
                    var cur = this._serverList[i];
                    if (cur.ServerID == lastServer.ServerID)
                    {
                        if (cur.ServerState != ServerStatus.关闭 && cur.ServerState != ServerStatus.拒绝服务)
                            info = new ClientServerInfo(cur);
                        break;
                    }
                }
            }
            if (info == null)
            {
                this._serverList.Sort((a, b) =>
                {
                    return DateTime.Compare(b.OpenDate, a.OpenDate);
                });
                for (int i = 0; i < this._serverList.Count; i++)
                {
                    var s = this._serverList[i];
                    if (s.ServerState != ServerStatus.关闭 && s.ServerState != ServerStatus.拒绝服务)
                    {
                        info = new ClientServerInfo(s);
                        break;
                    }
                }
            }
        }
        if (info != null)
            LocalStorage.Save(StorageDef.LastServer, info);
        return info;
    }
    private async Task<ResponseData> LoginAccount(LoginAccount data)
    {
        var req = new RequestData(RequestCode.LoginAccount, data);
        var result = await HttpRequest.inst.Post(ConfigManager.inst.connectURL, req.data);
        if (result == null)
        {
            var buffer1 = new BufferWriter(128);
            buffer1.Write("服务器维护中，请稍后再试！");
            var reader = new BufferReader(buffer1.GetBuffer);
            return new ResponseData(ReturnCode.Filed, reader, req.code);
        }
        else
        {
            var reader = new BufferReader(result);
            reader.ReadByte();
            var requestCode = (RequestCode)reader.ReadByte();
            var returnCode = (ReturnCode)reader.ReadByte();
            return new ResponseData(returnCode, reader, requestCode);
        }
    }
    private void ConnectToGate(string ip)
    {
        WSClient.inst.Connect(ip);
    }
}
