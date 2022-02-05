using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BuildingOther
{
    Tags tags;
    Point[] polygon;
    public int GetNumOfPoints()
    {
        return polygon.Length;
    }

    public (float, float) GetPointAt(int i)
    {
        return (polygon[i].x, polygon[i].y);
    }
}
