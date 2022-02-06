using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrainGeneration;
using HTTPClientNamespace;
using DataManagement;
using ObjectsDefinition;

namespace ChunkSystem
{
    public class Chunks : MonoBehaviour
    {
        public GameObject[] chunks = new GameObject[9];
        Transform camera;
        public static Response response = new Response();
        public static Request request = new Request();
        const uint radius = 500;
        public static bool objectsBuilt = false;

        // Start is called before the first frame update
        void Start()
        {
            camera = gameObject.transform.Find("Camera");
            Vector3 chunksOrigin = findClosestPoint(camera.transform.position);

            chunks = TerrainGenerator.generateChunks(chunksOrigin);
            //request.position = new Point(camera.position.x, camera.position.z);
            request.position = Response.convertToWGS84(new Point(camera.position.x, camera.position.z));
            request.radius = radius;
            StartCoroutine(HTTPClient.SendRequest(request));
        }

        // Update is called once per frame
        void Update()
        {
            if (!objectsBuilt)
            {
                response.generateObjects();
                TreePlacement.GrowTrees(chunks[4], response);
            }
            bool moved = false;
            if (camera.position.x < chunks[4].transform.position.x)
            {
                chunks = TerrainGenerator.chunkChange(chunks, Direction.LEFT);
                moved = true;
            }
            if (camera.position.z > (chunks[4].transform.position.z + TerrainGenerator.chunkSize * 10.0f))
            {
                chunks = TerrainGenerator.chunkChange(chunks, Direction.FORWARD);
                moved = true;
            }
            if (camera.position.x > (chunks[4].transform.position.x + TerrainGenerator.chunkSize * 10.0f))
            {
                chunks = TerrainGenerator.chunkChange(chunks, Direction.RIGHT);
                moved = true;
            }
            if (camera.position.z < (chunks[4].transform.position.z))
            {
                chunks = TerrainGenerator.chunkChange(chunks, Direction.BACKWARD);
                moved = true;
            }
            if (moved)
            {
                //request.position = new Point(camera.position.x, camera.position.z);
                request.position = Response.convertToWGS84(new Point(camera.position.x, camera.position.z));
                StartCoroutine(HTTPClient.SendRequest(request));
                response.generateObjects();
                TreePlacement.GrowTrees(chunks[4], response);
            }
        }

        Vector3 findClosestPoint(Vector3 pos)
        {
            float x, z;
            if (pos.x >= 0)
                x = ((int)pos.x / TerrainGenerator.chunkSize) * TerrainGenerator.chunkSize;
            else
                x = ((int)(pos.x - TerrainGenerator.chunkSize) / TerrainGenerator.chunkSize) * TerrainGenerator.chunkSize;

            if (pos.z >= 0)
                z = ((int)pos.z / TerrainGenerator.chunkSize) * TerrainGenerator.chunkSize;
            else
                z = ((int)(pos.z - TerrainGenerator.chunkSize) / TerrainGenerator.chunkSize) * TerrainGenerator.chunkSize;

            return new Vector3(x, 0.0f, z);
        }
    }
}