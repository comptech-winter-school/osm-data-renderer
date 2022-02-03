using System.Collections.Generic;
using UnityEngine;
using ProceduralToolkit;
using BuildingClass;
using TerrainGeneration;

// Этот скрипт создаёт одно здание и рисует его

namespace Generation
{
    public class GenerateBuilding : MonoBehaviour
    {
        List<GameObject> createWalls(Building building)
        {
            List<GameObject> walls = new List<GameObject>();

            for (int i = 0; i < building.GetWallsList().Count; i++)
            {
                GameObject wallGo = new GameObject();
                wallGo.name = "wall" + i;

                MeshRenderer wallRenderer;
                wallRenderer = wallGo.AddComponent<MeshRenderer>();
                var wallFilter = wallGo.AddComponent<MeshFilter>();
                wallRenderer.material = Resources.Load("BuildingMaterial", typeof(Material)) as Material;

                Mesh wall = new Mesh();
                Tessellator tessellator = new Tessellator();
                tessellator.AddContour(building.GetWallsList()[i]);
                tessellator.Tessellate();
                wall = tessellator.ToMesh();
                wall.RecalculateNormals();
                wall.RecalculateUVDistributionMetrics();

                wallFilter.mesh = wall;

                walls.Add(wallGo);
            }

            return walls;
        }

        GameObject createRoof(Building building)
        {
            GameObject roofGo = new GameObject();
            roofGo.name = "roof";

            var roofRenderer = roofGo.AddComponent<MeshRenderer>();
            var roofFilter = roofGo.AddComponent<MeshFilter>();

            roofRenderer.material = Resources.Load("BuildingMaterial", typeof(Material)) as Material;

            Mesh roof = new Mesh();

            // Здесь создаётся полигон из N точек с помощью ProceduralToolkit
            Tessellator tessellator = new Tessellator();
            tessellator.AddContour(building.GetRoofVertices());
            tessellator.Tessellate();
            roof = tessellator.ToMesh();
            roof.RecalculateNormals();
            roof.RecalculateUVDistributionMetrics();

            roofFilter.mesh = roof;

            return roofGo;
        }

        GameObject createBuilding(Building building)
        {
            // Ниже создаётся уже сам GameObject здания и также проводится объединение мешей для этого

            List<GameObject> buildingMeshes = createWalls(building);
            buildingMeshes.Add(createRoof(building));
            CombineInstance[] combine = new CombineInstance[buildingMeshes.Count];

            for (int i = 0; i < buildingMeshes.Count; i++)
            {
                MeshFilter mf = buildingMeshes[i].GetComponent<MeshFilter>();
                combine[i].mesh = mf.mesh;
                combine[i].transform = mf.transform.localToWorldMatrix;
                Destroy(mf.gameObject);
            }

            GameObject buildingGo = Instantiate(new GameObject());
            MeshRenderer buildingMR = buildingGo.AddComponent<MeshRenderer>();
            buildingMR.material = Resources.Load("BuildingMaterial", typeof(Material)) as Material;
            MeshFilter buildingMF = buildingGo.AddComponent<MeshFilter>();
            buildingMF.mesh = new Mesh();
            buildingMF.mesh.CombineMeshes(combine);
            buildingMF.mesh.RecalculateNormals();
            buildingMF.mesh.RecalculateUVDistributionMetrics();
            Building.BuildingsNumber++;
            buildingGo.name = "building" + Building.BuildingsNumber;
            buildingGo.SetActive(true);

            float minDistance = Building.heightPerLevel * levels + TerrainGenerator.maxHeight;
            buildingMF.mesh.Move(new Vector3(0.0f, minDistance, 0.0f));
            RaycastHit hit;
            for (int i = 0; i < buildingMF.mesh.vertices.Length; i++)
            {
                Ray ray = new Ray(buildingMF.mesh.vertices[i], Vector3.down);
                if (Physics.Raycast(ray, out hit))
                    {
                    if (hit.distance < minDistance)
                        minDistance = hit.distance;
                }
            }
            buildingMF.mesh.Move(new Vector3(0.0f, -minDistance - Building.heightPerLevel, 0.0f));

            return buildingGo;
        }

        // Поля для упрощения тестинга (в редакторе можно настроить координаты точек и кол-во этажей)
        [SerializeField] public uint levels;

        // Start is called before the first frame update
        void Start()
        {

            Point[] pnts = new Point[4];
            pnts[0] = new Point(20.0f, 30.0f);
            pnts[1] = new Point(20.0f, 35.0f);
            pnts[2] = new Point(25.0f, 35.0f);
            pnts[3] = new Point(25.0f, 30.0f);

            Building b1 = new Building(pnts, levels);

            createBuilding(b1).transform.parent = gameObject.transform;
        }



        // Update is called once per frame
        void Update()
        {

        }

    }
}