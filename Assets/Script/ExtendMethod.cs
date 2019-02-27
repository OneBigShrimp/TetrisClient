using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class ExtendMethod
{

    public static Vector2 GetVecPos(this TetrisSupport.Position pos)
    {
        return new Vector2(pos.X * MapCell.SideLength, pos.Y * MapCell.SideLength);
    }

}
