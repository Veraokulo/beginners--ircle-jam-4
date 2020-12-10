using UnityEngine;
using UnityEngine.UI;

public class OxygenBar : MonoBehaviour
{
    public Slider slider;
    public Text text;

    public void SetOxygen(float oxygen)
    {
        slider.value = oxygen;
        text.text = (int) oxygen / 100 + " X " + ((int) oxygen);
    }
}