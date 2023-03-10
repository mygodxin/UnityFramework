using UnityEngine;
using UnityWebSocket;

public class WSClient
{
    private WebSocket _webSocket;
    //private int _receiveCount = 0;

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
    }
    private void OnClose(object sender, CloseEventArgs e)
    {

    }
    private void OnMessage(object sender, MessageEventArgs e)
    {
        Facade.inst.ExcuteServerCommond(sender, e);
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
        if(_webSocket.ReadyState == WebSocketState.Closed || _webSocket.ReadyState == WebSocketState.Closing)
        {
            Debug.LogError("web socket is Closed or Closing");
        }
        _webSocket.CloseAsync();
    }
}