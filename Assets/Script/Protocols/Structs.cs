using UnityEngine;
using System.Collections;
using MyNetManager;

public class Pos : ISerObj
{
    public byte X;
    public byte Y;
}
public class MapCellInfo : ISerObj
{
    public Pos CellPos;
    public byte CellInfo;
}

public class AllShapeInfo : ISerObj
{
    public byte CurShapeFlag;
    public byte NextShapeFlag;
    public byte NextNextShapeFlag;
}

public enum EnterFailReason
{
    None = 0,
    IsGaming = 1,
    TableIsFull = 2,
}

public class ItemDetail : ISerObj
{
    public byte RowIndex;
    public byte ColIndex;
    public byte ItemCellInfo;
}