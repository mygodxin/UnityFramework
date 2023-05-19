using KHCore.Utils;
using System.Collections;
using UnityEngine;
using HS;
using UnityWebSocket;
using DuiChongServerCommon.ClientProtocol;

public class WSClient
{
    private WebSocket _webSocket;
    //private int _receiveCount = 0;
    private Queue msgQueue = new Queue();

    private static WSClient _inst = null;
    public static WSClient inst
    {
        get
        {
            if (_inst == null)
                _inst = new WSClient();
            return _inst;
        }
    }
    public void Connect(string address)
    {
        if (_webSocket == null)
            _webSocket = new WebSocket(address);
        _webSocket.OnOpen += OnOpen;
        _webSocket.OnClose += OnClose;
        _webSocket.OnMessage += OnMessage;
        _webSocket.OnError += OnError;
        _webSocket.ConnectAsync();
    }
    private void OnOpen(object sender, OpenEventArgs e)
    {
        Debug.Log("websocket open");
        Timer.Inst.Add(0, -1, this.SendMessage);
        LoginManager.Inst.GateHandHake();
    }
    private void OnClose(object sender, CloseEventArgs e)
    {
        Debug.Log("websocket close");
        Timer.Inst.Remove(this.SendMessage);
    }
    private void OnMessage(object sender, MessageEventArgs e)
    {
        if (e.IsBinary)
        {
            var stream = new BufferReader(e.RawData);
            var code = stream.ReadByte();
            if (code == 0)
            {
                var requestCode = (RequestCode)stream.ReadByte();
                this.OnServerResponse(new ResponseData((ReturnCode)stream.ReadByte(), stream, requestCode));
            }
            else if (code == 1)
            {
                this.OnServerEvent((EventCode)stream.ReadByte(), stream);
            }
        }
        else if (e.IsText)
        {
            Debug.Log(string.Format("Receive: {0}", e.Data));
        }
    }

    // 在链接阶段出现错误时调用，比如以下情形：
    /**
     * 1. 无网络
     * 2. 服务器域名解析失败
     * 3. 服务器端口拒绝访问
     * 4. 数据包路由失败
     * 5. 服务器没有websocket服务
     * 6. 如果是wss协议，证书无效
     * 7. 服务器完成握手后立即关闭连接
     */
    // 以上场景均可能触发onerror，但是HTML5规范中不允许ErrorEvent对象携带上述信息，防止服务器被蓄意攻击
    private void OnError(object sender, ErrorEventArgs e)
    {

    }
    public void Close()
    {
        if (_webSocket.ReadyState == WebSocketState.Closed || _webSocket.ReadyState == WebSocketState.Closing)
        {
            Debug.LogError("web socket is Closed or Closing");
        }
        _webSocket.CloseAsync();
    }

    private void SendMessage()
    {
        if (this._webSocket != null && this._webSocket.ReadyState == WebSocketState.Open)
        {
            if (this.msgQueue.Count > 0)
            {
                this._webSocket.SendAsync((byte[])this.msgQueue.Dequeue());
            }
        }
    }
    public void SendPackage(RequestData requestData)
    {
        if (RequestCode.GateHandShake != requestData.code)
        {
            var buffer = EncryptTool.EncryptBinary(requestData.Buffer, RequestData.keys);
            var writer = new BufferWriter(buffer.Length + 1);
            writer.Write((byte)requestData.code);
            writer.Joint(buffer);
            this.msgQueue.Enqueue(writer.GetBuffer);
        }
        else
        {
            this.msgQueue.Enqueue(requestData.Buffer);
        }
    }

    private void OnServerResponse(ResponseData responseData)
    {
        var code = responseData.requestCode;
        switch (code)
        {
            case RequestCode.GateHandShake:
                LoginManager.Inst.OnGateHandShake(responseData);
                break;
            case RequestCode.LoginGame:
                LoginManager.Inst.OnLoginGame(responseData);
                break;
        }
    }
    private void OnServerEvent(EventCode code, BufferReader buffer)
    {
        switch (code)
        {
            case EventCode.LogOut:
                this.OnLogOut(buffer);
                break;
        }
    }

    private void OnLogOut(BufferReader buffer)
    {
        var mes = buffer.ReadString();
        Debug.Log(mes);
    }
}