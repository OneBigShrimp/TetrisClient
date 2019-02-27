using System;
using System.Collections.Generic;
using UnityEngine;
using TetrisSupport;

public class ItemContainer
{
    private Line data;
    private LineShower shower;

    int itemCount = 0;

    public int ItemCount
    {
        get
        {
            return this.itemCount;
        }
    }

    public ItemContainer(RectTransform itemRectTran)
    {
        data = new Line(0);
        shower = new LineShower(0, itemRectTran);
        data.SetEventBinding(shower);
    }

    public void AddItem(byte itemType)
    {
        AddOneCell((CellInfo)(itemType + 10));
    }

    public void SwitchItem()
    {
        CellInfo firstItem = PullFirstCell();
        if (firstItem == CellInfo.None)
        {
            return;
        }
        AddOneCell(firstItem);
    }

    public byte PullFirstItem()
    {
        return (byte)((byte)PullFirstCell() - 10);
    }

    public void Clear()
    {
        for (int i = 0; i < this.itemCount; i++)
        {
            data[i] = CellInfo.None;
        }
        this.itemCount = 0;
    }

    CellInfo PullFirstCell()
    {
        CellInfo result = data[0];
        if (itemCount > 0)
        {
            itemCount--;
            for (int i = 0; i < itemCount; i++)
            {
                data[i] = data[i + 1];
            }
            data[itemCount] = CellInfo.None;
        }
        return result;
    }


    void AddOneCell(CellInfo info)
    {
        if (itemCount >= Map.ColumnCount)
        {
            Debug.LogError("添加道具超限了!!!");
            return;
        }
        data[itemCount++] = info;
    }

}
