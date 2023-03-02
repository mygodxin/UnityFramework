
using DuiChongServerCommon.ClientProtocol;
using UnityEngine;
using UnityWebSocket;

public class LoginCommond
{
    public void OnMessage(object sender, MessageEventArgs e)
    {
        if (e.IsBinary)
        {
            var stream = new BufferReader(e.RawData);
            var code = stream.ReadByte();
            if (code == 0)
            {
                var requestCode = stream.ReadByte();
                //var callback =
            }
            else if (code == 1)
            {
                code = stream.ReadByte();
                onServerEvent((EventCode)code, stream);
            }
        }
        else if (e.IsText)
        {
            Debug.Log(string.Format("Receive: {0}", e.Data));
        }
    }
    private void onServerEvent(EventCode code, BufferReader buffer)
    {
        switch (code)
        {
            case EventCode.LogOut:
                this.onLogOut(buffer);
                break;
        }
    }

    private void onLogOut(BufferReader buffer)
    {
        var mes = buffer.ReadString();
        Debug.Log(mes);
    }
}
