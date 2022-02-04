using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class tree_placement : MonoBehaviour
{

    public GameObject AllTrees;
    public List<GameObject> TreesBasic;

    private const float TreeScale = 0.1f;
    private const float TreeDensity = 0.4f;
    private const string HeightMapPath = "Assets/Resources/heightmap.jpg";

    private int ChunkSizeX = 5;
    private int ChunkSizeZ = 5;
    private int num = 0;
    private List<Vector3> TreeMap = new List<Vector3>();
    private Texture2D HeightMap;
    // Start is called before the first frame update
    void Start()
    {
        HeightMap = (Texture2D)AssetDatabase.LoadAssetAtPath(HeightMapPath, typeof(Texture2D));
        ChunkSizeX = HeightMap.width;
        ChunkSizeZ = HeightMap.height;
        SetTreeMap();
        SetTrees();
    }

    void SetTreeMap()
    {
        for (float x = 0; x < ChunkSizeX / TreeDensity; x++)
        {
            for (float z = 0; z < ChunkSizeZ / TreeDensity; z++)
            {
                float check = Mathf.PerlinNoise(x * .3f, z * .3f);
                float y = GetHight(x * TreeDensity, z * TreeDensity);
                if (y > 0.3 && check > 0.7)
                {
                    TreeMap.Add(new Vector3(x * TreeDensity, y, z * TreeDensity));
                }
            }
        }
    }
    float GetHight(float x, float z)
    {
        /*int x1 = Mathf.CeilToInt(x);
        int x2 = Mathf.FloorToInt(x);
        float dx = x - x1;
        int z1 = Mathf.CeilToInt(z);
        int z2 = Mathf.FloorToInt(z);
        float dz = z - z1;

        float h11 = heightMap.GetPixel(x1, z1).grayscale * 2;
        float h12 = heightMap.GetPixel(x1, z2).grayscale * 2;
        float h21 = heightMap.GetPixel(x2, z1).grayscale * 2;
        float h22 = heightMap.GetPixel(x2, z2).grayscale * 2;

        float h = h11 * (1 - dx) * (1 - dz) + h12 * (1 - dx) * dz + h21 * dx * (1 - dz) + h22 * dx * dz;
        */

        return HeightMap.GetPixel((int)x, (int)z).grayscale * 2;
    }
    void SetTrees() {
        for (int i = 0; i < TreeMap.Count; i++) {
            if(TreeMap[i][2] != -1) {
                GameObject TreeExample = TreesBasic[(int)Mathf.Sqrt(2 * Random.Range(0, (int) ((TreesBasic.Count - 1) * TreesBasic.Count / 2)))];
                //Quaternion TornadoRotation = Quaternion.Euler(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
                Quaternion rotation = Quaternion.Euler(new Vector3(Random.Range(-5f, 5f), Random.Range(0f, 360f), Random.Range(-5f, 5f)));
                GameObject NewTree = Instantiate(TreeExample, TreeMap[i], rotation);

                float growth = Random.Range(-TreeScale * 0.3f, TreeScale * 0.5f);
                NewTree.transform.localScale = new Vector3(TreeScale + growth, TreeScale + growth, TreeScale + growth);

                NewTree.transform.SetParent(AllTrees.transform);
                num++;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        //SetTrees();
    }
}
