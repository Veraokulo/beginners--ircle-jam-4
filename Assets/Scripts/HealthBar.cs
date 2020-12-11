using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Text text;


    public void SetMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
    }
    
    public void SetHealth(float health)
    {
        slider.value = health;
        if (text != null)
            text.text = ((int) health).ToString();
    }
}