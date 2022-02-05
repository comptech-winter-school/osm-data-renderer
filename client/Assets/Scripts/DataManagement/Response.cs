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
        public static float initialResolution = 2.0f * (float)Math.PI * 6378137.0f / (float)tileSize;
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
                    //float x = buildings[i].polygon[j].x;
                    //float y = buildings[i].polygon[j].y;
                    //x = 6371.0f * Mathf.Cos(buildings[i].polygon[j].x) * Mathf.Cos(buildings[i].polygon[j].y);
                    //y = 6371.0f * Mathf.Cos(buildings[i].polygon[j].x) * Mathf.Sin(buildings[i].polygon[j].y);
                    //Debug.Log("New coords: " + x + " " + y);
                    //buildings[i].polygon[j].x = x;
                    //buildings[i].polygon[j].y = y;

                    buildings[i].polygon[j] = convertToMercator(buildings[i].polygon[j]);
                    Debug.Log("new coords: " + buildings[i].polygon[j].x + " " + buildings[i].polygon[j].y);
                }
                buildings[i] = new Building(buildings[i].polygon, buildings[i].levels);
                GenerateBuilding.createBuilding(buildings[i]);

                Chunks.objectsBuilt = true;
            }
            for (int i = 0; i < highways.Length; i++)
            {
                for (int j = 0; j < highways[i].polygon.Length; j++)
                {
                    highways[i].polygon[j] = convertToMercator(highways[i].polygon[j]);
                }
                highways[i] = new Highway(highways[i].polygon);
                GenerateHighway.createHighway(highways[i]);
                Chunks.objectsBuilt = true;
            }
        }

        public static Point convertToMercator(Point point)
        {
            float x = point.x;
            float y = point.y;
            //x = 6367.0f * Mathf.Cos(point.x) * Mathf.Cos(point.y);
            //y = 6367.0f * Mathf.Cos(point.x) * Mathf.Sin(point.y);
            //x = 111.32f * point.x;
            //y = 40075.0f * (float)Math.Cos(point.x) / 360.0f * point.y;
            //Debug.Log("New coords: " + x + " " + y);
            //point.x = x;
            //point.y = y;
            //return point;

            float meterx, meterz;
            meterx = point.y * originShift / 180.0f;
            meterz = (float)(Math.Log(Math.Tan((90.0f + point.x) * Math.PI / 360.0f)) / (Math.PI / 180.0f));
            meterz = meterz * originShift / 180.0f;
            point.x = meterx + xoffset;
            point.y = meterz + yoffset;

            //point = metertoPixel(meterx, meterz, 1);

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

        public static float Resolution(int zoom)
        {
            return initialResolution / (float)Math.Pow(2, zoom);
        }

        public static Point metertoPixel(float meterx, float meterz, int zoom)
        {
            float res = Resolution(zoom);
            float pixelX, pixelY;

            pixelY = (-meterx + originShift) / res;
            pixelX = (meterz + originShift) / res;

            return new Point(pixelX, pixelY);

        }

        public static Point pixeltoTile(float pixelX, float pixelY)
        {
            int Tilex = (int)(Math.Ceiling(pixelX / (double)tileSize)) - 1;
            int Tiley = (int)(Math.Ceiling(pixelY / (double)tileSize)) - 1;

            return new Point(Tilex, Tiley);
        }

        public static Point MetersToTile(float meterx, float meterz, int zoom)
        {
            Point tempResult = metertoPixel(meterx, meterz, zoom);
            return pixeltoTile(tempResult.x, tempResult.y);
        }
    }
}