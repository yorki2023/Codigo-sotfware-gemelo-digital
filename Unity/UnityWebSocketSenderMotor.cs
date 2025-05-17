
using UnityEngine;
using WebSocketSharp;

public class UnityWebSocketSenderMotor : MonoBehaviour
{
    WebSocket ws;
    public float frecuencia = 0f;
    public float rpm = 0f;
    public float corriente = 0f;
    public bool pulsador = false;

    void Start()
    {
        Debug.Log("Iniciando conexión WebSocket...");
        try
        {
            ws = new WebSocket("ws://localhost:1880/UnityData");
            ws.OnOpen += (sender, e) => Debug.Log("Conexión WebSocket establecida.");
            ws.OnError += (sender, e) => Debug.LogError($"Error WebSocket: {e.Message}");
            ws.Connect();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error al conectar WebSocket: {ex.Message}");
        }
    }

    void Update()
    {
        if (ws != null && ws.IsAlive)
        {
            MessageData data = new MessageData
            {
                frecuencia = this.frecuencia,
                rpm = this.rpm,
                corriente = this.corriente,
                pulsador = this.pulsador
            };

            string jsonData = JsonUtility.ToJson(data);
            Debug.Log($"Mensaje enviado: {jsonData}");
            ws.Send(jsonData);
        }
    }

    void OnDestroy()
    {
        if (ws != null) ws.Close();
    }

    [System.Serializable]
    public class MessageData
    {
        public float frecuencia;
        public float rpm;
        public float corriente;
        public bool pulsador;
    }
}
