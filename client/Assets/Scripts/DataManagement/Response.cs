using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ObjectsDefinition;
using System.IO;
using Generation;
using ChunkSystem;
using TerrainGeneration;

namespace DataManagement
{
    [Serializable]
    public class Response
    {
        public static int tileSize = TerrainGenerator.chunkSize;
        public static float originShift = 6378137 * (float)Math.PI;
        const float xoffset = -2283256.0f;
        const float yoffset = -7305116.0f;



        public Building[] buildings = { };
        public Highway[] highways = { };

        public static Response createResponse(string path)
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<Response>(json);
        }
        public static Response fromJson(string json)
        {
            return JsonUtility.FromJson<Response>(json);
        }
        public static void encode(Response response)
        {
            string str = JsonUtility.ToJson(response, true);
            File.WriteAllText(Path.Combine(Application.dataPath, "Resources/responseencode.json"), str);
        }
        public void generateObjects()
        {
            for (int i = 0; i < buildings.Length; i++)
            {
                //Array.Reverse(buildings[i].polygon, 0, buildings[i].polygon.Length);
                for (int j = 0; j < buildings[i].polygon.Length; j++)
                {
                    buildings[i].polygon[j] = convertToMercator(buildings[i].polygon[j]);
                    //Debug.Log("new coords: " + buildings[i].polygon[j].x + " " + buildings[i].polygon[j].y);
                }
                buildings[i] = new Building(buildings[i].polygon, buildings[i].levels);
                GenerateBuilding.createBuilding(buildings[i]);

                Chunks.objectsBuilt = true;
            }
            //for (int i = 0; i < highways.Length; i++)
            //{
            //    for (int j = 0; j < highways[i].polygon.Length; j++)
            //    {
            //        highways[i].polygon[j] = convertToMercator(highways[i].polygon[j]);
            //    }
            //    highways[i] = new Highway(highways[i].polygon);
            //    GenerateHighway.createHighway(highways[i]);
            //    Chunks.objectsBuilt = true;
            //}
        }

        public static Point convertToMercator(Point point)
        {
            float x = point.x;
            float y = point.y;

            float meterx, meterz;
            meterx = point.y * originShift / 180.0f;
            meterz = (float)(Math.Log(Math.Tan((90.0f + point.x) * Math.PI / 360.0f)) / (Math.PI / 180.0f));
            meterz = meterz * originShift / 180.0f;
            point.x = meterx + xoffset;
            point.y = meterz + yoffset;

            return point;
        }

        public static Point convertToWGS84(Point point)
        {
            float lat, lon;
            point.x -= xoffset;
            point.y -= yoffset;
            lon = (point.x / (originShift)) * 180.0f;
            lat = (point.y / (originShift)) * 180.0f;
            lat = 180.0f / (float)Math.PI * (float)(2.0f * Math.Atan(Math.Exp(lat * Math.PI / 180.0f)) - (float)Math.PI / 2.0f);

            point.x = lat;
            point.y = lon;

            return point;
        }
    }
}