using UnityEngine;
using System.Collections.Generic;
using TetrisSupport;

public class MapCellPool
{
    const int itemCacheCount = 16;
    const int noItemCacheCount = 64;
    const string resPath = "Cube";
    static Dictionary<CellInfo, GameObject> info2Res = new Dictionary<CellInfo, GameObject>();

    static Dictionary<CellInfo, Queue<MapCell>> caches = new Dictionary<CellInfo, Queue<MapCell>>();

    static Transform cellPoolParent;

    static MapCellPool()
    {
        info2Res.Add(CellInfo.Blue, Resources.Load<GameObject>("Blue"));
        info2Res.Add(CellInfo.Green, Resources.Load<GameObject>("Green"));
        info2Res.Add(CellInfo.Purple, Resources.Load<GameObject>("Purple"));
        info2Res.Add(CellInfo.Red, Resources.Load<GameObject>("Red"));
        info2Res.Add(CellInfo.Gray, Resources.Load<GameObject>("Gray"));

        info2Res.Add(CellInfo.SpeedUp, Resources.Load<GameObject>("SpeedUp"));
        info2Res.Add(CellInfo.AddOneLine, Resources.Load<GameObject>("AddOneLine"));
        info2Res.Add(CellInfo.AddTwoLine, Resources.Load<GameObject>("AddTwoLine"));
        info2Res.Add(CellInfo.AddThreeLine, Resources.Load<GameObject>("AddThreeLine"));
        info2Res.Add(CellInfo.SpeedDown, Resources.Load<GameObject>("SpeedDown"));
        info2Res.Add(CellInfo.ReduceOneLine, Resources.Load<GameObject>("ReduceOneLine"));
        info2Res.Add(CellInfo.ReduceTwoLine, Resources.Load<GameObject>("ReduceTwoLine"));
        info2Res.Add(CellInfo.ReduceThreeLine, Resources.Load<GameObject>("ReduceThreeLine"));

        foreach (var item in info2Res.Keys)
        {
            caches.Add(item, new Queue<MapCell>());
        }

        GameObject poolGo = new GameObject("CellPoolParent");
        cellPoolParent = poolGo.transform;
        cellPoolParent.transform.position = Vector3.one * 9999;
        GameObject.DontDestroyOnLoad(poolGo);
    }

    public static MapCell GetOne(CellInfo info)
    {
        if (caches[info].Count > 0)
        {
            return caches[info].Dequeue();
        }
        else
        {
            GameObject go = GameObject.Instantiate(info2Res[info]) as GameObject;
            return go.GetComponent<MapCell>();
        }
    }

    public static void PutBack(MapCell cell)
    {
        if (caches[cell.Info].Count >= (cell.IsItem ? itemCacheCount : noItemCacheCount))
        {
            GameObject.Destroy(cell.gameObject);
        }
        else
        {
            caches[cell.Info].Enqueue(cell);
            cell.transform.SetParent(cellPoolParent, false);
        }
    }

}
