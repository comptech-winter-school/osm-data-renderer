using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Здесь пилится здание по прямым линиям

namespace BuildingClass
{
    // Класс здания

    public class Building
    {
        // Высота этажа
        public const float heightPerLevel = 0.25f * 2.7f;
        public static uint BuildingsNumber = 0;
        private List<Vector3> roof = new List<Vector3>();
        private List<List<Vector3>> walls = new List<List<Vector3>>();
        public uint levels;

        public List<Vector3> GetRoofVertices()
        {
            return roof;
        }
        public List<List<Vector3>> GetWallsList()
        {
            return walls;
        }

        public Building(Point[] points, uint _levels)
        {
            _levels++;
            for (int i = 0; i < points.Length; i++)
            {
                roof.Add(new Vector3(points[i].x, heightPerLevel * _levels, points[i].y));
                List<Vector3> wall = new List<Vector3>();
                wall.Add(new Vector3(points[(i + 1) % points.Length].x, 0, points[(i + 1) % points.Length].y));
                wall.Add(new Vector3(points[(i + 1) % points.Length].x, heightPerLevel * _levels, points[(i + 1) % points.Length].y));
                wall.Add(new Vector3(points[i].x, heightPerLevel * _levels, points[i].y));
                wall.Add(new Vector3(points[i].x, 0, points[i].y));
                walls.Add(wall);
            }
            levels = _levels;
        }
    }
}
