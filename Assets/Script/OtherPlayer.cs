using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TetrisSupport;
using UnityEngine;

public class OtherPlayer : BasePlayer
{

    public OtherPlayer(RectTransform playerPanel, int relativeTbIndex)
        : base(playerPanel, relativeTbIndex)
    {

    }

    public override void SPutShape(SPutShape sps)
    {
        Shape shape = Shape.Create(sps.ShapeFlag);
        shape.Pos = ProtocolsSender.CreatePositionByPos(sps.PutPos);
        this.map.PutOneShapeOnMap(shape);
        this.map.ApplyItemFlagOnMap(sps.ItemFlag);
    }
}

