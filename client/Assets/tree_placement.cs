using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class tree_placement : MonoBehaviour
{
    public GameObject AllTrees;
    public List<GameObject> Trees_basic;
    int chunk_size_x = 5;
    int chunk_size_z = 5;
    float tree_scale = 0.1f;
    float tree_density = 0.4f;
    private int num = 0;
    private List<Vector3> tree_map = new List<Vector3>();
    Texture2D heightMap;
    // Start is called before the first frame update
    void Start()
    {
        heightMap = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Resources/heightmap.jpg", typeof(Texture2D));
        chunk_size_x = heightMap.width;
        chunk_size_z = heightMap.height;
        Set_Tree_Map();
        Set_Trees();
    }

    void Set_Tree_Map()
    {
        for (float x = 0; x < chunk_size_x / tree_density; x++)
        {
            for (float z = 0; z < chunk_size_z / tree_density; z++)
            {
                float do_tree = Mathf.PerlinNoise(x * .3f, z * .3f);
                float y = Get_Hight(x * tree_density, z * tree_density);
                if (y > 0.3 && do_tree > 0.7)
                {
                    tree_map.Add(new Vector3(x * tree_density, y, z * tree_density));
                }
            }
        }
    }
    float Get_Hight(float x, float z)
    {
        int x1 = Mathf.CeilToInt(x);
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


        return heightMap.GetPixel((int)x, (int)z).grayscale * 2;
    }
    void Set_Trees() {
        for (int i = 0; i < tree_map.Count; i++) {
            if(tree_map[i][2] != -1) {
                GameObject Tree_base = Trees_basic[(int)Mathf.Sqrt(2 * Random.Range(0, (int) ((Trees_basic.Count - 1) * Trees_basic.Count / 2)))];
                //Quaternion tornado_rotation = Quaternion.Euler(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
                Quaternion rotation = Quaternion.Euler(new Vector3(Random.Range(-5f, 5f), Random.Range(0f, 360f), Random.Range(-5f, 5f)));
                GameObject new_tree = Instantiate(Tree_base, tree_map[i], rotation);

                float growth = Random.Range(-tree_scale * 0.3f, tree_scale * 0.5f);
                new_tree.transform.localScale = new Vector3(tree_scale + growth, tree_scale + growth, tree_scale + growth);

                new_tree.transform.SetParent(AllTrees.transform);
                num++;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Set_Trees();
    }
}
