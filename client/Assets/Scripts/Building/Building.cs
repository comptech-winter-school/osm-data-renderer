using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  «десь пилитс€ здание по пр€мым лини€м
*/

namespace OSMDataRenderer
{
    /*  —труктура, представл€юща€ пр€мую линию
    *  я использую Vector2, потому что нам не нужна высота (Y) вершины, ибо это можно вычислить
    *    тому же координаты точек на картах состо€т из 2 чисел
    */
    public struct Line
    {
        public Vector2 start;
        public Vector2 end;
        public Line(Vector2 _start, Vector2 _end)
        {
            start = _start;
            end = _end;
        }
    }

    /*   ласс здани€
     */
    public class Building
    {
        // ¬ысота этажа
        private const float heightPerLevel = 1.0f;

        // ÷вет пока что захардкожен, в случае чего это можно изменить
        public static Color GlobalColor = new Color(0.4f, 0.4f, 0.4f);
        public static uint BuildingsNumber = 0;

        public uint levels;
        private List<Vector3> roofVertices = new List<Vector3>();
        private List<List<Vector3>> walls = new List<List<Vector3>>();

        public List<Vector3> GetRoofVertices()
        {
            return roofVertices;
        }
        public List<List<Vector3>> GetWallsList()
        {
            return walls;
        }

        public Building(List<Line> _lines, uint _levels)
        {
            for (int i = 0; i < _lines.Count; i++)
            {
                roofVertices.Add(new Vector3(_lines[i].start.x, heightPerLevel * _levels, _lines[i].start.y));
                List<Vector3> wall = new List<Vector3>();
                wall.Add(new Vector3(_lines[i].end.x, 0, _lines[i].end.y));
                wall.Add(new Vector3(_lines[i].end.x, heightPerLevel * _levels, _lines[i].end.y));
                wall.Add(new Vector3(_lines[i].start.x, heightPerLevel * _levels, _lines[i].start.y));
                wall.Add(new Vector3(_lines[i].start.x, 0, _lines[i].start.y));
                
                walls.Add(wall);
            }
            levels = _levels;
        }
    }
}
