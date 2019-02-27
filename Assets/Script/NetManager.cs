using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyNetManager;
using TetrisSupport;
using System;

public class NetManager
{
    public static readonly NetManager Instance = new NetManager();

    bool waitConnect = false;

    Action _onConnectSuccess;

    public NetManager()
    {
        Regist();
    }

    public void Connect(string ip, Action onConnectSuccess)
    {
        if (waitConnect)
        {
            return;
        }
        waitConnect = true;
        this._onConnectSuccess = onConnectSuccess;
        ClientNetManager.Instance.Connect(ip, ConstData.GamePort, 2048, 2048, this.OnConnect);
    }

    public void Tick()
    {
        ClientNetManager.Instance.Tick();
        ClientNetManager.Instance.Tick();
    }

    public void SendMsg(IProtocol p)
    {
        ClientNetManager.Instance.SendMsg(p);
    }


    void OnConnect(bool success)
    {
        waitConnect = false;
        if (success)
        {
            if (this._onConnectSuccess != null)
            {
                this._onConnectSuccess.Invoke();
            }
        }
    }


    void Regist()
    {
        ClientNetManager.Instance.Regist(typeof(CPlayerReady));
        ClientNetManager.Instance.Regist(typeof(CChangeTeam));
        ClientNetManager.Instance.Regist(typeof(CPutShape));
        ClientNetManager.Instance.Regist(typeof(CUseItem));
        ClientNetManager.Instance.Regist(typeof(CReqFailure));
        ClientNetManager.Instance.Regist(typeof(CSetName));

        ClientNetManager.Instance.Regist(typeof(SEnter));
        ClientNetManager.Instance.Regist(typeof(SAddPlayer));
        ClientNetManager.Instance.Regist(typeof(SChangeTeam));
        ClientNetManager.Instance.Regist(typeof(SPlayerReady));
        ClientNetManager.Instance.Regist(typeof(SGameReady));
        ClientNetManager.Instance.Regist(typeof(SGameStart));
        ClientNetManager.Instance.Regist(typeof(SPutShape));
        ClientNetManager.Instance.Regist(typeof(SRefreshHoleMap));
        ClientNetManager.Instance.Regist(typeof(SUseItem));
        ClientNetManager.Instance.Regist(typeof(SPlayerFailure));
        ClientNetManager.Instance.Regist(typeof(SPlayerLeave));
        ClientNetManager.Instance.Regist(typeof(SEatItems));
        ClientNetManager.Instance.Regist(typeof(SGameOver));
        ClientNetManager.Instance.Regist(typeof(SUnderAttack));
        ClientNetManager.Instance.Regist(typeof(SSetName));
    }


    public void Close()
    {
        ClientNetManager.Instance.Close();
    }
}
