using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class JsonDecoder<E, D>
    where E : System.Enum
    where D : JsonDetails<E>
{

    private Dictionary<E, D> map;

    protected virtual string GetJsonFileName()
    {
        Debug.LogError("Override this please!");
        return "";
    }

    public D Load(E enumCode)
    {
        if (map == null)
        {
            initDictionary();
        }
        if (!map.ContainsKey(enumCode))
        {
            Debug.LogError("Cannot find " + typeof(E).FullName + " details for " + enumCode);
            return null;
        }
        return map[enumCode];
    }

    private void initDictionary()
    {
        map = new Dictionary<E, D>();
        TextAsset textAsset = Resources.Load<TextAsset>("Jsons/" + GetJsonFileName());
        List<D> details = JsonConvert.DeserializeObject<List<D>>(textAsset.text);
        foreach (D d in details)
        {
            d.Init();
            E enumCode = d.GetEnumCode();
            if (map.ContainsKey(enumCode))
            {
                Debug.LogError("Double entry in " + typeof(D).FullName + " for " + enumCode);
            }
            else
            {
                map.Add(enumCode, d);
            }
        }
    }
}