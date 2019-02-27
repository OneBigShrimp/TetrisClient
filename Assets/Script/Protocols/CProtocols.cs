using UnityEngine;
using System.Collections;
using MyNetManager;

public class CSetName : IProtocol
{
    public string Name;
    public void Process(ILinker linker, object args)
    {
    }
}

public class CPlayerReady : IProtocol
{
    public byte IsReady;
    public void Process(ILinker linker, object args)
    {
        throw new System.NotImplementedException();
    }
}


public class CChangeTeam : IProtocol
{
    public byte TeamId;

    public void Process(ILinker linker, object args)
    {
        throw new System.NotImplementedException();
    }
}


public class CPutShape : IProtocol
{
    public byte TurnIndex;
    public Pos PutPos;
    public void Process(ILinker linker, object args)
    {
        throw new System.NotImplementedException();
    }
}

public class CUseItem : IProtocol
{
    public byte TargetTbIndex;
    public byte ItemType;
    public void Process(ILinker linker, object args)
    {
        throw new System.NotImplementedException();
    }
}

public class CReqFailure : IProtocol
{
    public void Process(ILinker linker, object args)
    {
    }
}