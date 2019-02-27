using System.Collections.Generic;
using UnityEngine;
using TetrisSupport;

public class PlayerManager : IEnumerable<BasePlayer>
{
    public static PlayerManager Instance = new PlayerManager();

    BasePlayer[] allPlayers = new BasePlayer[ConstData.PlayerCount];

    HostPlayer host;

    InputControl iControl;

    bool isGameReady = false;

    public void Init(Transform uiRoot)
    {
        host = new HostPlayer(uiRoot.Find("Host").transform as RectTransform, 1);
        iControl = new InputControl();
        allPlayers[0] = host;
        Transform otherRootTran = uiRoot.Find("Other").transform;
        for (int i = 1; i < allPlayers.Length; i++)
        {
            allPlayers[i] = new OtherPlayer(otherRootTran.Find(string.Format("Map_pt{0}/PlayerPanel", i)) as RectTransform, i + 1);
        }
    }

    public void Tick()
    {
        if (host == null)
        {
            return;
        }
        host.Tick(iControl.GetCommand());
        if (isGameReady)
        {
            host.TickReadyTime();
        }
    }

    public void EnterGame(byte tbIndex)
    {
        for (byte i = 0; i < tbIndex; i++)
        {
            allPlayers[i] = allPlayers[i + 1];
        }
        allPlayers[tbIndex] = host;

        host.InitOnePlayer(tbIndex, "", 0);
    }

    public void AddPlayer(SAddPlayer sap)
    {
        allPlayers[sap.TbIndex].InitOnePlayer(sap.TbIndex, sap.Name, sap.TeamId);
    }

    public void SetName(SSetName sspn)
    {
        allPlayers[sspn.TbIndex].SetName(sspn.Name);
    }

    public void GameReady()
    {
        foreach (var item in this)
        {
            item.GameReady();
        }
        isGameReady = true;
    }

    public void GameStart(AllShapeInfo allShape)
    {
        foreach (var item in this)
        {
            item.GameStart(allShape);
        }
        isGameReady = false;
    }

    public void PlayerChangeTeam(byte tbIndex, byte teamId)
    {
        allPlayers[tbIndex].SetTeamId(teamId);
    }

    public void PlayerReadyOrCancel(byte tbIndex, bool isReady)
    {
        allPlayers[tbIndex].PlayerReadyOrCancel(isReady);
    }

    public void PlayerPutShape(SPutShape sps)
    {
        allPlayers[sps.TbIndex].SPutShape(sps);
    }

    public void UnderAttack(SUnderAttack sua)
    {
        for (byte i = 0; i < sua.TbIndexs.Length; i++)
        {
            allPlayers[sua.TbIndexs[i]].UnderAttack(sua.AddLine);
        }
    }

    public void RefreshHoleMap(SRefreshHoleMap sfhm)
    {
        host.RefreshHoleMap(sfhm);
    }

    public void EatItem(byte[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            host.EatItem(items[i]);
        }
    }

    public void ReqUseItem(int relativeIndex, byte itemType)
    {
        byte tbIndex = ConstData.PlayerCount;
        foreach (var item in this)
        {
            if (item.RelativeIndex == relativeIndex)
            {
                tbIndex = item.TbIndex;
                break;
            }
        }
        ProtocolsSender.UseItem(tbIndex, itemType);
    }


    public void SUseItem(SUseItem sui)
    {
        allPlayers[sui.TargetTbIndex].ReceiveItem(sui.ItemType);
    }

    public void PlayerFailure(byte tbIndex)
    {
        allPlayers[tbIndex].Failure();
    }

    public void GameOver(byte winner)
    {
        if (winner != byte.MaxValue)
        {
            if (winner < 16)
            {

            }
        }
        else
        {
            Debug.Log("无人获胜");
        }
        isGameReady = false;
        host.GameOver();
    }

    public void PlayerLeave(byte tbIndex)
    {
        allPlayers[tbIndex].ClearPlayer();
    }

    public IEnumerator<BasePlayer> GetEnumerator()
    {
        for (int i = 0; i < allPlayers.Length; i++)
        {
            if (allPlayers[i] != null)
            {
                yield return allPlayers[i];
            }
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}