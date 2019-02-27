using UnityEngine;
using System.Collections;
using TetrisSupport;

public class MapCell : MonoBehaviour {

    public static float SideLength = 32;

    public CellInfo Info;

    public bool IsItem { get; private set; }

    public RectTransform RectTransform;

    void Awake()
    {
        this.IsItem = (int)Info > 10;
        this.RectTransform = this.transform as RectTransform;
        //this.RectTransform.sizeDelta = new Vector2(SideLength, SideLength);
    }

}
