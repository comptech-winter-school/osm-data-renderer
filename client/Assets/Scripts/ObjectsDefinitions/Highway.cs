using System.Collections;
using System;
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
            this.sortVertices();
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

        public void sortVertices()
        {
            float xAvg = 0;
            float yAvg = 0;
            for (int i = 0; i < polygon.Length; i++)
            {
                xAvg += polygon[i].x;
                yAvg += polygon[i].y;
            }
            xAvg /= polygon.Length;
            yAvg /= polygon.Length;
            //Quaternion toPlaneSpace = Quaternion.Inverse(Quaternion.LookRotation())
            List<Tuple<float, Point>> pairs = new List<Tuple<float, Point>>(polygon.Length);
            for (int i = 0; i < polygon.Length; i++)
            {
                pairs.Add(Tuple.Create<float, Point>(Mathf.Atan2(polygon[i].y, polygon[i].x), polygon[i]));
            }
            pairs.Sort((a, b) => b.Item1.CompareTo(a.Item1));
            for (int i = 0; i < polygon.Length; i++)
            {
                polygon[i] = pairs[i].Item2;
            }
        }
    }
}