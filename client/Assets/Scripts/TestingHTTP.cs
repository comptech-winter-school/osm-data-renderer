using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Этот скрипт нужен только для проверки подключения к серверу
 * Он создаёт текстовый объект и на основании того подключен
 * клиент к серверу или нет ставит текст (Connected! или Not connected)
 * 
 * Пока решил не удалять скрипт на всякий случай
*/

// Мини-класс для перекидывания данными между скриптами
public static class TestingHTTPdata
{
    public static string data = "";
    
    public static void setData(string _data)
    {
        data = _data;
    }
}

public class TestingHTTP : MonoBehaviour
{
    public string Data;
    private Text mytext;
    private Canvas canvas;
    
    // Start is called before the first frame update
    void Start()
    {
        Font arial;
        arial = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

        GameObject canvasGO = new GameObject();
        canvasGO.name = "Canvas";
        canvasGO.AddComponent<Canvas>();
        canvasGO.AddComponent<CanvasRenderer>();

        canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        GameObject textGO = new GameObject();
        textGO.transform.parent = canvasGO.transform;
        textGO.AddComponent<Text>();

        
        mytext = textGO.GetComponent<Text>();
        mytext.font = arial;
        mytext.color = new Color(0.0f, 0.0f, 0.0f);
        mytext.text = "Not connected";
        mytext.fontSize = 48;
        mytext.alignment = TextAnchor.MiddleCenter;

        RectTransform rectTransform;
        rectTransform = mytext.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(0, 0, 0);
        rectTransform.sizeDelta = new Vector2(600, 200);

        
    }

    // Update is called once per frame
    void Update()
    {
        if (TestingHTTPdata.data != "")
        {
            mytext.text = TestingHTTPdata.data;
        }
    }
}
