using UnityEngine;
using UnityEngine.UI;

public class OxygenBar : MonoBehaviour
{
    public Slider slider;
    public Text text;

    public void SetOxygen(float oxygen)
    {
        slider.value = oxygen;
        var baloonCount = (int) oxygen / 100;
        text.text = baloonCount > 0 ? baloonCount + " X Baloons + " : "";
        var rest = oxygen % 100;
        text.text += rest > 0 ? rest + "%" : "";
    }
}