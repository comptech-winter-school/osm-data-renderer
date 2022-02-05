using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Здесь пилится здание по прямым линиям

namespace ObjectsDefinition
{
    // Класс здания
    [Serializable]
    public class Building
    {
        // Высота этажа
        private static float heightPerLevel = 10f * 2.7f;
        private static uint BuildingsNumber = 0;
        private List<Vector3> roof = new List<Vector3>();
        private List<List<Vector3>> walls = new List<List<Vector3>>();
        public uint levels;
        public Point[] polygon;

        public List<Vector3> GetRoofVertices()
        {
            return roof;
        }
        public List<List<Vector3>> GetWallsList()
        {
            return walls;
        }
        public static float getHeight()
        {
            return heightPerLevel;
        }
        public static uint getBuildingsNumber()
        {
            return BuildingsNumber;
        }
        public static void incrementBuildingsNumber()
        {
            BuildingsNumber++;
        }

        public Building(Point[] points, uint _levels)
        {
            polygon = points;
            levels = _levels;

            this.setup();
        }

        public void setup()
        {
            if (levels == 0)
            {
                levels++;
            }
            //this = new Building(polygon, levels);
            levels++;
            for (int i = 0; i < polygon.Length; i++)
            {
                roof.Add(new Vector3(polygon[i].x, heightPerLevel * levels, polygon[i].y));
                List<Vector3> wall = new List<Vector3>();
                wall.Add(new Vector3(polygon[(i + 1) % polygon.Length].x, 0, polygon[(i + 1) % polygon.Length].y));
                wall.Add(new Vector3(polygon[(i + 1) % polygon.Length].x, heightPerLevel * levels, polygon[(i + 1) % polygon.Length].y));
                wall.Add(new Vector3(polygon[i].x, heightPerLevel * levels, polygon[i].y));
                wall.Add(new Vector3(polygon[i].x, 0, polygon[i].y));
                walls.Add(wall);
            }
        }
    }
}
