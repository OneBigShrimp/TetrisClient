using UnityEngine;
using System.Collections;
using MyNetManager;


public class SEnter : IProtocol
{
    public byte TbIndex;
    public void Process(ILinker linker, object args)
    {
        if (TbIndex != byte.MaxValue)
        {
            PlayerManager.Instance.EnterGame(TbIndex);
        }
        else
        {
            Debug.LogError("进入失败");
        }
    }
}

public class SAddPlayer : IProtocol
{
    public byte TbIndex;
    public string Name;
    public byte TeamId;
    public void Process(ILinker linker, object args)
    {
        Debug.Log("SPlayerReady  " + TbIndex);
        PlayerManager.Instance.AddPlayer(this);
    }
}

public class SSetName : IProtocol
{
    public byte TbIndex;
    public string Name;
    public void Process(ILinker linker, object args)
    {
        PlayerManager.Instance.SetName(this);
    }
}

public class SChangeTeam : IProtocol
{
    public byte TbIndex;
    public byte TeamId;

    public void Process(ILinker linker, object args)
    {
        PlayerManager.Instance.PlayerChangeTeam(TbIndex, TeamId);
    }
}

public class SPlayerReady : IProtocol
{
    public byte TbIndex;
    public byte IsReady;
    public void Process(ILinker linker, object args)
    {
        Debug.Log("SPlayerReady");
        PlayerManager.Instance.PlayerReadyOrCancel(TbIndex, IsReady == 1);
    }
}


public class SGameReady : IProtocol
{
    public void Process(ILinker linker, object args)
    {
        Debug.Log("SGameReady");
        PlayerManager.Instance.GameReady();
    }
}

public class SGameStart : IProtocol
{
    public AllShapeInfo AllShape;
    public void Process(ILinker linker, object args)
    {
        Debug.Log("GameStart");
        PlayerManager.Instance.GameStart(this.AllShape);
    }
}


public class SPutShape : IProtocol
{
    public byte TbIndex;
    public byte ShapeFlag;
    public Pos PutPos;
    public byte NextNextShapeFlag;
    public byte ItemFlag;
    public void Process(ILinker linker, object args)
    {
        //Debug.Log("SPutShape : " + ItemFlag);
        PlayerManager.Instance.PlayerPutShape(this);
    }
}

public class SRefreshHoleMap : IProtocol
{
    public int[] AllLines;
    public ItemDetail[] Items;
    public AllShapeInfo AllShape;
    public void Process(ILinker linker, object args)
    {
        PlayerManager.Instance.RefreshHoleMap(this);
    }
}

public class SUseItem : IProtocol
{
    public byte FromTbIndex;
    public byte TargetTbIndex;
    public byte ItemType;
    public void Process(ILinker linker, object args)
    {
        PlayerManager.Instance.SUseItem(this);
    }
}

public class SEatItems : IProtocol
{
    public byte[] Items;
    public void Process(ILinker linker, object args)
    {
        PlayerManager.Instance.EatItem(this.Items);
    }
}

public class SUnderAttack : IProtocol
{
    public byte[] TbIndexs;

    public byte AddLine;
    public void Process(ILinker linker, object args)
    {
        PlayerManager.Instance.UnderAttack(this);
    }
}

public class SPlayerFailure : IProtocol
{
    public byte TbIndex;
    public void Process(ILinker linker, object args)
    {
        PlayerManager.Instance.PlayerFailure(TbIndex);
    }
}


public class SPlayerLeave : IProtocol
{
    public byte TbIndex;
    public void Process(ILinker linker, object args)
    {
        PlayerManager.Instance.PlayerLeave(this.TbIndex);
    }
}

public class SGameOver : IProtocol
{
    /// <summary>
    /// 胜利者,
    /// 如果胜利者是自由人,则把胜利者的座位号放在低四位
    /// 如果胜利者是一个队伍,则把胜利队伍Id放在高四位
    /// </summary>
    public byte Winner;

    public void Process(ILinker linker, object args)
    {
        PlayerManager.Instance.GameOver(this.Winner);
        Debug.Log("GameOver");
    }
}

