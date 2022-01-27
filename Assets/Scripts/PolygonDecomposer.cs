using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  Начинал тут делать триангуляцию полигона,
 *  нужно вместо этого впихнуть алгоритм триангуляцию с интернета
 *  Можно смело игнорировать этот класс
 */
public static class PolygonDecomposer
{
    //Сделать функцию которая будет проводить декомпозицию полигона (разбирать полигон на треугольники)
    public static int[] decompose(List<Vector3> vertices)
    {
        List<int> triangles = new List<int>();

        //Пока что вершины сортируются пузырьком, потом придумать что-то получше
        for (int i = 0; i < vertices.Count - 1; i++)
        {
            for (int j = i + 1; j < vertices.Count; j++)
            {
                if (vertices[j].x < vertices[i].x)
                {
                    var t = vertices[j];
                    vertices[j] = vertices[i];
                    vertices[i] = t;
                }
            }
        }

        return new int[0];
    }
}
