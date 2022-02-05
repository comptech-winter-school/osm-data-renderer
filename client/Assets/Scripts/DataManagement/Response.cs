using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ObjectsDefinition;
using System.IO;
using Generation;

namespace DataManagement
{
    [Serializable]
    public class Response
    {
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
                buildings[i] = new Building(buildings[i].polygon, buildings[i].levels);
                GenerateBuilding.createBuilding(buildings[i]);
            }
            for (int i = 0; i < highways.Length; i++)
            {
                highways[i] = new Highway(highways[i].polygon);
                GenerateHighway.createHighway(highways[i]);
            }
        }
    }
}