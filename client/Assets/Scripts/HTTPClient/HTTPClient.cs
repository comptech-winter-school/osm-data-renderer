using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Utilities;

// «десь лежит код дл€ подключени€ к серверу

namespace HTTPClient
{
    public class HTTPClient : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            /** 
             * ≈сли не знаешь про StartCoroutine(), советую почитать, становитс€ более €сно почему функции
             * общени€ с сервером должны быть устроены именно так как они устроены
             */
            StartCoroutine(GetText());
            StartCoroutine(GetFile());
        }

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

        IEnumerator GetFile()
        {
            // “о же самое, что и с GetText()
            UnityWebRequest request = new UnityWebRequest("http://localhost:5000/test.json", UnityWebRequest.kHttpVerbGET);
            string path = Path.Combine(Application.dataPath, "Resources/test.json");
            request.downloadHandler = new DownloadHandlerFile(path);

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