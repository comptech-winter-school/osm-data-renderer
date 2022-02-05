using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using DataManagement;
using ChunkSystem;

// «десь лежит код дл€ подключени€ к серверу

namespace HTTPClient
{
    public class HTTPClient : MonoBehaviour
    {
        public static bool isRequestSent = false;
        const string serverURL = "http://159.223.28.18:8090/apiv1/objects";

        IEnumerator GetText()
        {
            /**
             * «десь ничего сложного с точки зрени€ кода не происходит, достаточно
             * узнать что такое UnityWebRequest чтобы пон€ть что тут творитс€ 
             */
            UnityWebRequest request = UnityWebRequest.Get("http://localhost:5000/test.txt");
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                TestingHTTPdata.setData(request.downloadHandler.text);

                Debug.Log("Finished getting data");
            }
        }

            var uwr = new UnityWebRequest(serverURL, "POST");
            //string test = request.encode();
            //File.WriteAllText(Path.Combine(Application.dataPath, "Resources/requestOutput"), test);
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(request.encode());
            uwr.uploadHandler = new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("Successfully downloaded and saved to " + path);
            }
        }
    }
}