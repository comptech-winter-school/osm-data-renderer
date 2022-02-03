using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace TerrainGeneration
{
    public class TerrainGenerator : MonoBehaviour
    {
        Texture2D tex;
        public int xSize;
        public int zSize;
        public static float maxHeight = 0.0f;

        Vector3[] vertices;
        int[] triangles;
        Mesh mesh;
        Texture2D heightMap;

        // Start is called before the first frame update
        void Start()
        {
            mesh = new Mesh();
            gameObject.GetComponent<MeshFilter>().mesh = mesh;

            heightMap = readHeightMap("Australia/row-12-column-15.jpg");
            CreateShape();

            MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
            Debug.Log("Max Height: " + maxHeight);
        }

        // Update is called once per frame
        void Update()
        {
            UpdateMesh();
        }

        void CreateShape()
        {
            xSize = heightMap.width;
            zSize = heightMap.height;

            if (xSize * zSize > 65536)
                Debug.LogWarning("Size of the grid exceeds Unity limit of 65536 vertices per mesh.");

            vertices = new Vector3[(xSize + 1) * (zSize + 1)];

            for (int i = 0, z = 0; z < zSize + 1; z++)
            {
                for (int x = 0; x < xSize + 1; x++)
                {
                    Color pixel = heightMap.GetPixel(x, z);

                    float y = pixel.grayscale * 2;
                    vertices[i] = new Vector3(x, y, z);
                    i++;

                    if (y > maxHeight)
                        maxHeight = y;
                }
            }

            triangles = new int[xSize * zSize * 6];

            int vert = 0;
            int tris = 0;

            for (int z = 0; z < zSize; z++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    triangles[tris + 0] = vert + 0;
                    triangles[tris + 1] = vert + xSize + 1;
                    triangles[tris + 2] = vert + 1;
                    triangles[tris + 3] = vert + 1;
                    triangles[tris + 4] = vert + xSize + 1;
                    triangles[tris + 5] = vert + xSize + 2;

                    vert++;
                    tris += 6;
                }
                vert++;
            }

            UpdateMesh();
        }

        void UpdateMesh()
        {
            mesh.Clear();

            mesh.vertices = vertices;
            mesh.triangles = triangles;

            mesh.RecalculateNormals();
        }

        public Texture2D readHeightMap(string name)
        {
            Texture2D texture = new Texture2D(128, 128);

            string path = "Resources/" + name;
            byte[] binaryImageData = File.ReadAllBytes(Path.Combine(Application.dataPath, path));
            texture.LoadImage(binaryImageData);

            return texture;
        }
    }
}