using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using ProceduralToolkit;

namespace TerrainGeneration
{
    public class TerrainGenerator : MonoBehaviour
    {
        public static float maxHeight = 0.0f;
        public static int chunkSize = 100;
        private const string HeightMapPath = "heightmap.jpg";

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public static GameObject generateTerrain(Vector3 pos)
        {
            GameObject chunkGo = Instantiate(new GameObject());
            MeshRenderer chunkMR = chunkGo.AddComponent<MeshRenderer>();
            MeshFilter chunkMF = chunkGo.AddComponent<MeshFilter>();
            MeshCollider chunkMC = chunkGo.AddComponent<MeshCollider>();
            Mesh chunk = CreateGrid();
            chunkMR.material = Resources.Load("Terrain", typeof(Material)) as Material;
            chunkMF.mesh = chunk;
            chunkMC.sharedMesh = chunk;
            chunkGo.transform.position = new Vector3(pos.x, 0.0f, pos.z);

            return chunkGo;
        }

        static Mesh CreateGrid()
        {
            int xSize = chunkSize;
            int zSize = chunkSize;

            if (xSize * zSize > 65536)
                Debug.LogWarning("Size of the grid exceeds Unity limit of 65536 vertices per mesh.");

            Vector3[] vertices = new Vector3[(xSize + 1) * (zSize + 1)];

            Texture2D heightMap = readHeightMap(HeightMapPath);

            for (int i = 0, z = 0; z < zSize + 1; z++)
            {
                for (int x = 0; x < xSize + 1; x++)
                {
                    Color pixel = heightMap.GetPixel(x, z);
                    float y = pixel.grayscale * 2;

                    //Debug.Log(y);
                    //float y = 0.0f;
                    vertices[i] = new Vector3(10*x, y, 10*z);
                    i++;

                    if (y > maxHeight)
                        maxHeight = y;
                }
            }

            int[] triangles = new int[xSize * zSize * 6];

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

            Mesh grid = new Mesh();
            grid.vertices = vertices;
            grid.triangles = triangles;
            grid.RecalculateNormals();
            grid.RecalculateUVDistributionMetrics();

            return grid;
        }

        public static GameObject[] generateChunks(Vector3 chunksOrigin)
        {
            GameObject[] chunks = new GameObject[9];

            chunks[0] = generateTerrain(chunksOrigin + new Vector3(-chunkSize * 10.0f, 0.0f, chunkSize * 10.0f));
            chunks[1] = generateTerrain(chunksOrigin + new Vector3(0, 0.0f, chunkSize * 10.0f));
            chunks[2] = generateTerrain(chunksOrigin + new Vector3(chunkSize * 10.0f, 0.0f, chunkSize * 10.0f));

            chunks[3] = generateTerrain(chunksOrigin + new Vector3(-chunkSize * 10.0f, 0.0f, 0.0f));
            chunks[4] = generateTerrain(chunksOrigin + new Vector3(0.0f, 0.0f, 0.0f));
            chunks[5] = generateTerrain(chunksOrigin + new Vector3(chunkSize * 10.0f, 0.0f, 0.0f));

            chunks[6] = generateTerrain(chunksOrigin + new Vector3(-chunkSize * 10.0f, 0.0f, -chunkSize * 10.0f));
            chunks[7] = generateTerrain(chunksOrigin + new Vector3(0, 0.0f, -chunkSize * 10.0f));
            chunks[8] = generateTerrain(chunksOrigin + new Vector3(chunkSize * 10.0f, 0.0f, -chunkSize * 10.0f));

            return chunks;
        }

        public static GameObject[] chunkChange(GameObject[] chunks, Direction direction)
        {
            switch (direction)
            {
                case Direction.LEFT:
                    for (int i = 0; i < 3; i++)
                    {
                        Destroy(chunks[i * 3 + 2]);
                        chunks[i * 3 + 2] = Instantiate(chunks[i * 3 + 1]);
                        Destroy(chunks[i * 3 + 1]);
                        chunks[i * 3 + 1] = Instantiate(chunks[i * 3]);
                        Destroy(chunks[i * 3]);
                        chunks[i * 3] = generateTerrain(chunks[i * 3 + 1].transform.position + new Vector3(-chunkSize * 10.0f, 0.0f, 0.0f));
                    }
                    break;
                case Direction.FORWARD:
                    for (int i = 0; i < 3; i++)
                    {
                        Destroy(chunks[i + 6]);
                        chunks[i + 6] = Instantiate(chunks[i + 3]);
                        Destroy(chunks[i + 3]);
                        chunks[i + 3] = Instantiate(chunks[i]);
                        Destroy(chunks[i]);
                        chunks[i] = generateTerrain(chunks[i + 3].transform.position + new Vector3(0.0f, 0.0f, chunkSize * 10.0f));
                    }
                    break;
                case Direction.RIGHT:
                    for (int i = 0; i < 3; i++)
                    {
                        Destroy(chunks[i * 3]);
                        chunks[i * 3] = Instantiate(chunks[i * 3 + 1]);
                        Destroy(chunks[i * 3 + 1]);
                        chunks[i * 3 + 1] = Instantiate(chunks[i * 3 + 2]);
                        Destroy(chunks[i * 3 + 2]);
                        chunks[i * 3 + 2] = generateTerrain(chunks[i * 3 + 1].transform.position + new Vector3(chunkSize * 10.0f, 0.0f, 0.0f));
                    }
                    break;
                case Direction.BACKWARD:
                    for (int i = 0; i < 3; i++)
                    {
                        Destroy(chunks[i]);
                        chunks[i] = Instantiate(chunks[i + 3]);
                        Destroy(chunks[i + 3]);
                        chunks[i + 3] = Instantiate(chunks[i + 6]);
                        Destroy(chunks[i + 6]);
                        chunks[i + 6] = generateTerrain(chunks[i + 3].transform.position + new Vector3(0.0f, 0.0f, -chunkSize * 10.0f));
                    }
                    break;
            }

            return chunks;
        }

        public static Texture2D readHeightMap(string name)
        {
            Texture2D texture = new Texture2D(128, 128);

            string path = "Resources/heightmap.jpg"; // "Resources /" + name;
            byte[] binaryImageData = File.ReadAllBytes(Path.Combine(Application.dataPath, path));
            texture.LoadImage(binaryImageData);

            return texture;
        }
    }

    public enum Direction { LEFT, FORWARD, RIGHT, BACKWARD};
}