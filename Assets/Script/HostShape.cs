using UnityEngine;
using System.Collections;
using TetrisSupport;

public class HostShape
{
    Shape _shape;

    public Shape Shape
    {
        get
        {
            return this._shape;
        }
        set
        {
            if (this._shape != null)
            {
                for (int i = 0; i < cells.Length; i++)
                {
                    MapCellPool.PutBack(cells[i]);
                    cells[i] = null;
                }
            }

            this._shape = value;
            if (value != null)
            {
                for (int i = 0; i < cells.Length; i++)
                {
                    cells[i] = MapCellPool.GetOne(_shape.Info);
                    cells[i].transform.SetParent(_root, false);
                }
                RefreshRootPos();
                RefreshCellPos();
            }
        }
    }

    RectTransform _root;

    MapCell[] cells = new MapCell[4];

    public HostShape(RectTransform root)
    {
        this._root = root;
    }

    public void RefreshCellPos()
    {
        Position[] localPoses = this._shape.LocalOccupyPoses.Poses;
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].RectTransform.anchoredPosition = localPoses[i].GetVecPos();
        }
    }

    public void RefreshRootPos()
    {
        if (_shape == null)
        {
            Debug.LogError("RefreshPos but no _shape!");
            return;
        }
        _root.anchoredPosition = _shape.Pos.GetVecPos();
    }
}

