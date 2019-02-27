using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyNetManager;
using TetrisSupport;
using UnityEngine;

public class ProtocolsSender
{
    static CPutShape cps = new CPutShape();

    static CUseItem cui = new CUseItem();
    static ProtocolsSender()
    {

    }


    public static void Ready(bool isReady)
    {
        NetManager.Instance.SendMsg(new CPlayerReady() { IsReady = isReady ? (byte)1 : (byte)0 });
    }

    public static void ChangeTeam(byte teamId)
    {
        NetManager.Instance.SendMsg(new CChangeTeam() {TeamId = teamId });
    }

    public static void PutShape(byte turnIndex, Position pos)
    {
        cps.TurnIndex = turnIndex;
        cps.PutPos = CreatePosByPosition(pos);
        NetManager.Instance.SendMsg(cps);
    }

    public static void UseItem(byte targetTbIndex, byte itemType)
    {
        cui.TargetTbIndex = targetTbIndex;
        cui.ItemType = itemType;
        NetManager.Instance.SendMsg(cui);
    }

    public static void SendFailure()
    {
        NetManager.Instance.SendMsg(new CReqFailure());
    }

    public static Position CreatePositionByPos(Pos pos)
    {
        return new Position(pos.X, pos.Y);
    }

    static Pos CreatePosByPosition(Position pos)
    {
        return new Pos() { X = (byte)pos.X, Y = (byte)pos.Y };
    }

    static string Pos2String(Pos pos)
    {
        return string.Format("[{0},{1}]", pos.X, pos.Y);
    }
}
