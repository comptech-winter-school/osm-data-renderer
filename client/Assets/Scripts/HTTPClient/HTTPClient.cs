using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using DataManagement;
using ChunkSystem;

// Здесь лежит код для подключения к серверу

namespace HTTPClientNamespace
{
    public class HTTPClient : MonoBehaviour
    {
        public static bool isRequestSent = false;
        const string serverURL = "http://159.223.28.18:8090/apiv1/objects";

        public static IEnumerator SendRequest(Request request)
        {
            if (isRequestSent)
            {
                yield return null;
            }

            isRequestSent = true;

            Debug.Log("camera pos (Converted): " + request.position.x + " " + request.position.y);

            var uwr = new UnityWebRequest(serverURL, "POST");
            //string test = request.encode();
            //File.WriteAllText(Path.Combine(Application.dataPath, "Resources/requestOutput"), test);
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(request.encode());
            uwr.uploadHandler = new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");

            //Send the request then wait here until it returns
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error While Sending: " + uwr.error);
                isRequestSent = false;
            }
            else
            {
                Debug.Log("Sent: " + request.encode());
                Debug.Log("Received: " + uwr.downloadHandler.text);
                Chunks.response = Response.fromJson(uwr.downloadHandler.text);
                isRequestSent = false;
            }
        }
    }
}