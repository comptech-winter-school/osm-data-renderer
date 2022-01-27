using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polygon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Создание линий из 4 вершин
        List<Line> myLines = new List<Line>();
        Vector2[] points =
        {
            new Vector2(0.0f, 3.0f),
            new Vector2(5.0f, 3.0f),
            new Vector2(1.0f, 2.0f),
            new Vector2(0.0f, 1.0f)
        };
        myLines.Add(new Line(points[0], points[1]));
        myLines.Add(new Line(points[1], points[2]));
        myLines.Add(new Line(points[2], points[3]));
        myLines.Add(new Line(points[3], points[0]));

        // Второй аргумент это количество этажей
        Building building = new Building(myLines, 1);

        // Здесь инициализируется здание и задаются все нужные параметры
        // Пока что приходится это делать тут, поскольку из-за ограничений Unity (нельзя создавать класс наследующий MonoBehavior)
        // нельзя провести инициализацию с помощью метода класса
        // Желательно придумать способ инициализировать объект класса с помощью метода
        // gameObject.Count по сути равно количеству мешей (прямоугольников)
        for (int i = 0; i < building.gameObjects.Count; i++)
        {
            Instantiate<GameObject>(building.gameObjects[i]);   //Из-за этой функции не получается инициализировать класс, т.к. она только в MonoBehavior
            building.meshRenderers.Add(new MeshRenderer());
            building.meshRenderers[i] = building.gameObjects[i].AddComponent<MeshRenderer>();
            building.meshFilters.Add(new MeshFilter());
            building.meshFilters[i] = building.gameObjects[i].AddComponent<MeshFilter>();

            building.meshRenderers[i].sharedMaterial = new Material(Shader.Find("Standard"));
            building.meshRenderers[i].sharedMaterial.SetColor("_Color", building.color);

            Mesh rect = new Mesh();
            rect.vertices = building.generateRect(building.lines[i], building.levels);
            int[] rectTriangles =
            {
                0, 1, 2,
                0, 2, 3,
                4, 6, 5,
                4, 7, 6
            };
            rect.triangles = rectTriangles;

            rect.RecalculateBounds();
            rect.RecalculateNormals();

            building.meshFilters[i].mesh = rect;

            //Пока что ничего не делает, ибо не была запилена генерация крыши
            //building.roofVertices.Add(rect.vertices[1]);
        }

        //int[] roofTriangles = PolygonDecomposer.decompose(building.roofVertices);
    }

    

    // Update is called once per frame
    void Update()
    {

    }

}
