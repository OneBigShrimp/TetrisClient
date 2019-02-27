using System;
using System.Collections.Generic;
using TetrisSupport;
using UnityEngine;

class LineShower : LineEventBinding
{
    static GameObject res;

    private RectTransform _rt;

    MapCell[] _line;

    int _rowIndex;

    int rowIndex
    {
        set
        {
            this._rowIndex = value;
            this._rt.anchoredPosition = new Vector2(0, value * MapCell.SideLength);
        }
    }


    static LineShower()
    {
        res = Resources.Load<GameObject>("Line");
    }

    public LineShower(int rowIndex, RectTransform parent)
    {
        _line = new MapCell[Map.ColumnCount];
        this._rt = GameObject.Instantiate<GameObject>(res).transform as RectTransform;
        this._rt.SetParent(parent, false);
        this.rowIndex = rowIndex;
    }

    public void Destroy()
    {
        ClearAllCell();
        GameObject.Destroy(this._rt.gameObject);
    }



    public void OnCellChange(int cellIndex, CellInfo newInfo)
    {
        RecycleCell(cellIndex);
        if (newInfo != CellInfo.None)
        {
            MapCell cell = MapCellPool.GetOne(newInfo);
            cell.RectTransform.SetParent(this._rt, false);
            cell.RectTransform.anchoredPosition = new Vector2(cellIndex * MapCell.SideLength, 0);
            this._line[cellIndex] = cell;
        }
    }

    public void OnErasureLine()
    {
        ClearAllCell();
    }

    public void OnRowIndexChange(int newIndex)
    {
        this.rowIndex = newIndex;
    }

    private void ClearAllCell()
    {
        for (int i = 0; i < _line.Length; i++)
        {
            RecycleCell(i);
        }
    }

    private void RecycleCell(int columnIndex)
    {
        if (this._line[columnIndex] != null)
        {
            MapCellPool.PutBack(this._line[columnIndex]);
            this._line[columnIndex] = null;
        }
    }
}