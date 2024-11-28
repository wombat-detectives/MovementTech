using UnityEngine;
using UnityEngine.UI;

public class DashDisplay : MonoBehaviour
{
    public Slider slider;

    public void SetMaxCooldown(float cooldown)
    {
        slider.maxValue = cooldown;
        slider.value = cooldown;
    }

    public void SetCooldown(float cooldown)
    {
        slider.value = cooldown;
    }

}
