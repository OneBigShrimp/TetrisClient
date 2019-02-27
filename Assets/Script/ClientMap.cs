using System.Collections.Generic;
using System.Linq;
using TetrisSupport;
using UnityEngine;

public class ClientMap
{
    RectTransform _mapPanel;
    Map _map;
    LineShower[] _lines;

    public ClientMap(RectTransform mapPanel)
    {
        this._mapPanel = mapPanel;
        this._map = new Map();
        this._lines = new LineShower[Map.RowCount];
        for (int i = 0; i < this._lines.Length; i++)
        {
            this._lines[i] = new LineShower(i, mapPanel);
            _map[i].SetEventBinding(this._lines[i]);
        }
    }

    public void PutOneShapeOnMap(Shape shape)
    {
        _map.ForcePutShape(shape, null);
    }

    public bool CheckShapeLegal(Shape shape)
    {
        return shape != null && _map.CheckShapeLegal(shape);
    }

    public bool MoveByOffset(HostShape hShape, Position offset)
    {
        Position oldPos = hShape.Shape.Pos;
        hShape.Shape.Pos += offset;
        if (_map.CheckShapeLegal(hShape.Shape))
        {
            hShape.RefreshRootPos();
            return true;
        }
        else
        {
            hShape.Shape.Pos = oldPos;
            return false;
        }
    }

    public bool Turn(HostShape hShape, int turnCount)
    {
        hShape.Shape.CurTurnIndex += turnCount;
        if (_map.CheckShapeLegal(hShape.Shape))
        {
            hShape.RefreshCellPos();
            return true;
        }
        else
        {
            hShape.Shape.CurTurnIndex -= turnCount;
            return false;
        }

    }

    public void RefreshHoleMap(int[] allLines, ItemDetail[] items)
    {
        for (int i = 0; i < Map.RowCount; i++)
        {
            if (!this._map[i].IsEmpty)
            {
                this._map[i].Erasure();
            }
        }

        for (int i = 0; i < allLines.Length; i++)
        {
            this._map[i].ParseByInt(allLines[i]);
        }

        foreach (var item in items)
        {
            this._map[item.RowIndex][item.ColIndex] = (CellInfo)item.ItemCellInfo;
        }
    }

    public void ApplyItemFlagOnMap(byte itemFlag)
    {
        this._map.ApplyItemFlagOnMap(ref itemFlag);
    }

    public void ChangeLine(int changeCount)
    {
        this._map.ChangeLine(changeCount);
    }

    public void ReceiveItem(byte itemType)
    {
        this._map.ReceiveItem(itemType);
    }

    public void GrayMap()
    {
        for (int i = 0; i < Map.RowCount; i++)
        {
            for (int j = 0; j < Map.ColumnCount; j++)
            {
                if (_map[i][j] != CellInfo.None)
                {
                    _map[i][j] = CellInfo.Gray;
                }
            }
        }
    }

    public void ClearMap()
    {
        this._map.ClearMap();
    }




}

