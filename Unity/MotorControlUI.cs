
using UnityEngine;
using UnityEngine.UI;

public class MotorControlUI : MonoBehaviour
{
    public Slider velocidadSlider;
    public Text valorTexto;
    public UnityWebSocketSenderMotor motorSender;

    void Start()
    {
        velocidadSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnSliderChanged(float value)
    {
        if (motorSender != null)
        {
            motorSender.frecuencia = value;
            motorSender.rpm = value * 60f;
            valorTexto.text = "Velocidad: " + value.ToString("F1");
        }
    }
}
