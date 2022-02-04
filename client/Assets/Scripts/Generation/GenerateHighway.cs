using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingClass;
using ProceduralToolkit;

public class GenerateHighway : MonoBehaviour
{
    public static GameObject createHighway(Highway highway)
    {
        GameObject highwayGo = Instantiate(new GameObject());
        highwayGo.name = "highway" + highway.getCount();
        highway.incrementCount();

        var highwayRenderer = highwayGo.AddComponent<MeshRenderer>();
        var highwayFilter = highwayGo.AddComponent<MeshFilter>();

        highwayRenderer.material = Resources.Load("BuildingMaterial", typeof(Material)) as Material;

        Mesh highwayMesh = new Mesh();

        // Здесь создаётся полигон из N точек с помощью ProceduralToolkit
        Tessellator tessellator = new Tessellator();
        tessellator.AddContour(highway.getVertices());
        tessellator.Tessellate();
        highwayMesh = tessellator.ToMesh();
        highwayMesh.RecalculateNormals();
        highwayMesh.RecalculateUVDistributionMetrics();

        highwayFilter.mesh = highwayMesh;
        highwayGo.transform.Translate(new Vector3(0.0f, 0.01f, 0.0f));

        return highwayGo;
    }
}
