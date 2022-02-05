using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ObjectsDefinition
{
    [Serializable]
    public class Highway
    {
        public Point[] polygon;
        private List<Vector3> highway = new List<Vector3>();
        static private uint highwayCount = 0;

        public Highway(Point[] points)
        {
            polygon = points;
            this.setup();
        }
        public void setup()
        {
            for (int i = 0; i < polygon.Length; i++)
                highway.Add(new Vector3(polygon[i].x, 0.0f, polygon[i].y));
        }
        public List<Vector3> getVertices()
        {
            return highway;
        }
        public uint getCount()
        {
            return highwayCount;
        }
        public void incrementCount()
        {
            highwayCount++;
        }
    }
}