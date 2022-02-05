using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProceduralToolkit;
using ObjectsDefinition;

namespace Generation
{
    public class GenerateHighway : MonoBehaviour
    {
        public static GameObject createHighway(Highway highway)
        {
            GameObject highwayGo = Instantiate(new GameObject());
            highwayGo.name = "highway" + highway.getCount();
            if (highway.getCount() == 142)
                highway.getCount();
            highway.incrementCount();

            var highwayRenderer = highwayGo.AddComponent<MeshRenderer>();
            var highwayFilter = highwayGo.AddComponent<MeshFilter>();
            var highwayCollider = highwayGo.AddComponent<MeshCollider>();

            highwayRenderer.material = Resources.Load("HighwayMaterial", typeof(Material)) as Material;

            Mesh highwayMesh = new Mesh();

            // Здесь создаётся полигон из N точек с помощью ProceduralToolkit
            Tessellator tessellator = new Tessellator();
            tessellator.AddContour(highway.getVertices(), ProceduralToolkit.LibTessDotNet.ContourOrientation.Clockwise);
            tessellator.Tessellate();
            highwayMesh = tessellator.ToMesh();
            highwayMesh.RecalculateNormals();
            highwayMesh.RecalculateUVDistributionMetrics();

            highwayFilter.mesh = highwayMesh;
            highwayCollider.sharedMesh = highwayMesh;

            float minDistance = 100.0f;
            highwayFilter.mesh.Move(new Vector3(0.0f, minDistance, 0.0f));
            RaycastHit hit;
            for (int i = 0; i < highwayFilter.mesh.vertices.Length; i++)
            {
                Ray ray = new Ray(highwayFilter.mesh.vertices[i], Vector3.down);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.distance < minDistance)
                        minDistance = hit.distance;
                }
            }
            highwayFilter.mesh.Move(new Vector3(0.0f, -minDistance + 0.1f, 0.0f));

            //highwayGo.transform.Translate(new Vector3(0.0f, 0.01f, 0.0f));

            return highwayGo;
        }
    }
}