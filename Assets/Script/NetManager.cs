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
        ClientNetManager.Instance.Regist(typeof(CPlayerReady), 1);
        ClientNetManager.Instance.Regist(typeof(CChangeTeam), 3);
        ClientNetManager.Instance.Regist(typeof(CPutShape), 4);
        ClientNetManager.Instance.Regist(typeof(CUseItem), 5);
        ClientNetManager.Instance.Regist(typeof(CReqFailure), 6);
        ClientNetManager.Instance.Regist(typeof(CSetName), 7);

        ClientNetManager.Instance.Regist(typeof(SEnter), 20);
        ClientNetManager.Instance.Regist(typeof(SAddPlayer), 21);
        ClientNetManager.Instance.Regist(typeof(SChangeTeam), 22);
        ClientNetManager.Instance.Regist(typeof(SPlayerReady), 23);
        ClientNetManager.Instance.Regist(typeof(SGameReady), 25);
        ClientNetManager.Instance.Regist(typeof(SGameStart), 26);
        ClientNetManager.Instance.Regist(typeof(SPutShape), 27);
        ClientNetManager.Instance.Regist(typeof(SRefreshHoleMap), 28);
        ClientNetManager.Instance.Regist(typeof(SUseItem), 29);
        ClientNetManager.Instance.Regist(typeof(SPlayerFailure), 30);
        ClientNetManager.Instance.Regist(typeof(SPlayerLeave), 31);
        ClientNetManager.Instance.Regist(typeof(SEatItems), 32);
        ClientNetManager.Instance.Regist(typeof(SGameOver), 33);
        ClientNetManager.Instance.Regist(typeof(SUnderAttack), 34);
        ClientNetManager.Instance.Regist(typeof(SSetName), 35);
    }


    public void Close()
    {
        ClientNetManager.Instance.Close();
    }
}
