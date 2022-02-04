using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Response
{
    BuildingOther[] buildings;
    Highway[] highways;

    public static Response createResponse(string path)
    {
        return JsonUtility.FromJson<Response>(path);
    }
}
