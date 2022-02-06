using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class TreePlacement : MonoBehaviour
{
    public GameObject AllTrees;
    public static GameObject AllTreesS;
    public List<GameObject> TreesBasic;
    public static List<GameObject> TreesBasicS;
    private static List<GameObject> PlantedTrees;

    private const float TreeScale = 4.0f;
    private const float TreeDensity = 0.9f;

    private static int ChunkSize = TerrainGeneration.TerrainGenerator.chunkSize;
    private static List<Vector3> TreeMap = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(Resources.Load("enemy", typeof(GameObject))) as GameObject;
        //TreesBasic.Add(Instantiate(Resources.Load("tree01", typeof(GameObject))) as GameObject);
        PlantedTrees = new List<GameObject>();
        TreesBasicS = TreesBasic;
        AllTreesS = AllTrees;
    }
    // Update is called once per frame
    void Update()
    {

    }
    public static void GrowTrees(GameObject chunk, DataManagement.Response response)
    {
        Mesh mesh = chunk.GetComponent<MeshFilter>().mesh;
        //FreeBuildingSites(mesh, response);
        SetTreeMap(mesh);
        SetTrees();

    }
    static void FreeBuildingSites(Mesh mesh, DataManagement.Response response)
    {
        for (int i = 0; i < response.buildings.Length; i++)
        {
            for (int j = 0; j < response.buildings[i].polygon.Length; j++)
            {
                int x = (int)response.buildings[i].polygon[j].x;
                int z = (int)response.buildings[i].polygon[j].y;
                mesh.vertices[x * (ChunkSize + 1) + z].y = -1;
            }
        }
    }

    static void SetTreeMap(Mesh mesh)
    {
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            float x = mesh.vertices[i].x;
            float y = mesh.vertices[i].y;  
            float z = mesh.vertices[i].z;
            for (int a = 0; a < 1 / TreeDensity; a++)
            {
                for (int b = 0; b < 1 / TreeDensity; b++)
                {
                    float check = Mathf.PerlinNoise((x + a) * .3f, (z + b) * .3f);
                    if (check > 0.7)
                    {
                        TreeMap.Add(new Vector3(x + a, y, z + b));
                    }
                }
            }
        }
    }
    static void SetTrees() {
        for (int i = 0; i < TreeMap.Count; i++) {
            if(TreeMap[i][2] != -1) {
                Debug.Log(TreesBasicS.Count);
                GameObject TreeExample = TreesBasicS[(int)Mathf.Sqrt(2 * Random.Range(0, (int) ((TreesBasicS.Count - 1) * TreesBasicS.Count / 2)))];
                //Quaternion TornadoRotation = Quaternion.Euler(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
                Quaternion rotation = Quaternion.Euler(new Vector3(Random.Range(-5f, 5f), Random.Range(0f, 360f), Random.Range(-5f, 5f)));
                GameObject NewTree = Instantiate(TreeExample, TreeMap[i], rotation);

                float growth = Random.Range(-TreeScale * 0.3f, TreeScale * 0.5f);
                NewTree.transform.localScale = new Vector3(TreeScale + growth, TreeScale + growth, TreeScale + growth);

                NewTree.transform.SetParent(AllTreesS.transform);
                PlantedTrees.Add(NewTree);
            }
        }
    }
}
