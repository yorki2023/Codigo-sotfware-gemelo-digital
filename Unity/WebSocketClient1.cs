
using System;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class WebSocketClient1 : MonoBehaviour
{
    private WebSocket ws;
    public Dictionary<string, float> receivedVariables = new Dictionary<string, float>();

    void Start()
    {
        ws = new WebSocket("ws://localhost:1880/UnityData");
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Received: " + e.Data);
            HandleIncomingData(e.Data);
        };

        try
        {
            ws.Connect();
            Debug.Log("WebSocket connected successfully!");
        }
        catch (Exception ex)
        {
            Debug.LogError("WebSocket connection error: " + ex.Message);
        }
    }

    void HandleIncomingData(string jsonData)
    {
        try
        {
            var data = JsonUtilityExtended.FromJson<Dictionary<string, float>>(jsonData);

            if (data != null && data.Count == 5)
            {
                Debug.Log("All 5 variables received successfully!");
                receivedVariables = new Dictionary<string, float>(data);

                foreach (var pair in data)
                {
                    Debug.Log($"{pair.Key}: {pair.Value}");
                }
            }
            else
            {
                Debug.LogWarning($"Data is incomplete or null. Received {data?.Count ?? 0} variables.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error deserializing data: {e.Message}\nData: {jsonData}");
        }
    }

    void OnDestroy()
    {
        if (ws != null && ws.IsAlive)
        {
            ws.Close();
            Debug.Log("WebSocket closed.");
        }
    }
}

public static class JsonUtilityExtended
{
    public static T FromJson<T>(string json)
    {
        return JsonUtility.FromJson<Wrapper<T>>(WrapJson(json)).Data;
    }

    private static string WrapJson(string json)
    {
        return "{"Data":" + json + "}";
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T Data;
    }
}
