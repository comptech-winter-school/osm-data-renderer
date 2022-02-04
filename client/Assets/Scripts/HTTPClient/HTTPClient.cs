using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Utilities;

// Здесь лежит код для подключения к серверу

namespace HTTPClientNamespace
{
    public class HTTPClient : MonoBehaviour
    {
        public static bool isRequestSent = false;
        const string serverURL = "http://localhost:5000";

        // Start is called before the first frame update
        void Start()
        {
            //StartCoroutine(GetText());
            //StartCoroutine(GetFile());
        }

        public static IEnumerator SendRequest(Request request)
        {
            if (isRequestSent)
            {
                yield return null;
            }

            isRequestSent = true;

            var uwr = new UnityWebRequest(serverURL, "POST");
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
                Debug.Log("Received: " + uwr.downloadHandler.text);
                Chunks.response = Response.fromJson(uwr.downloadHandler.text);
                isRequestSent = false;
            }
        }
    }
}