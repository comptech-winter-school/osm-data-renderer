using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  Здесь пилится здание по прямым линиям
*/

/*  Структура, представляющая прямую линию
 *  Я использую Vector2, потому что нам не нужна высота (Y) вершины, ибо это можно вычислить
 *  К тому же координаты точек на картах состоят из 2 чисел
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

/*  Класс здания
 */
public class Building
{
    // Высота этажа
    private const float heightPerLevel = 1.0f;

    // Цвет пока что захардкожен, в случае чего это можно изменить
    public static Color Color = new Color(0.3f, 0.3f, 0.3f);
    public static uint BuilingsNumber = 0;

    public List<Line> lines;
    public int levels;
    private List<Vector3> roofVertices = new List<Vector3>();

    public List<Vector3> GetRoofVertices()
    {
        return roofVertices;
    }

    public Building(List<Line> _lines, int _levels)
    {
        for (int i = 0; i < _lines.Count; i++)
        {
            roofVertices.Add(new Vector3(_lines[i].start.x, heightPerLevel * _levels, _lines[i].start.y));
        }
        levels = _levels;
        lines = _lines;
    }

    //public static Building CreateFromJSON(string json)
    //{

    //}

    /*  Генерируем координаты вершин прямоугольника на основании линии и количества этажей
    */
    public Vector3[] generateRect(Line inputLine, int levels)
    {
        float height = levels * heightPerLevel;
        Vector3[] vertices = new Vector3[8];
        /*  Вершины генерируются не абы как, а в определённом порядке (это нужно чтобы треугольники задавались нормально)
         *  Вот как это прмерно выглядит:
         *  
         *  2--------3
         *  |        |
         *  |        |
         *  1--------4
         *  
         *  Тут цифры означают их порядок в массиве, он дожлен быть строго таким, чтобы не сбить вычисление нормалей
         *  Точки 1 и 4 - это концы прямой линии (Line.start и Line.end) соответственно
         */
        vertices[0].x = inputLine.start.x; vertices[0].y = 0.0f; vertices[0].z = inputLine.start.y;
        vertices[1].x = inputLine.start.x; vertices[1].y = height; vertices[1].z = inputLine.start.y;
        vertices[2].x = inputLine.end.x; vertices[2].y = height; vertices[2].z = inputLine.end.y;
        vertices[3].x = inputLine.end.x; vertices[3].y = 0.0f; vertices[3].z = inputLine.end.y;

        /*  Создаём вторую пачку вершин с такими же координатам, они будут использоваться
         *  для того чтобы прямоугольник был виден с двух сторон (если это убрать то прямоугольник
         *  будет отображаться лишь с одной стороны, в случае если приложение не выдаст ошибку)
         *  Это происходит из-за т.н. Backface Culling
        */
        vertices[4] = vertices[0];
        vertices[5] = vertices[1];
        vertices[6] = vertices[2];
        vertices[7] = vertices[3];

        return vertices;
    }
}
