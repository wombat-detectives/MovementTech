using TMPro;
using UnityEngine;

public class Speedometer : MonoBehaviour
{
    public float maxSpeed = 100f;

    public float minSpeedArrowAngle = 0.0f;
    public float maxSpeedArrowAngle = -183f;

    [Header("UI")]
    public TextMeshProUGUI speedText;
    public RectTransform arrow;

    public void SetMaxSpeed(float max)
    {
        maxSpeed = max;
    }

    public void SetSpeed(float speed)
    {
        if(speedText != null)
        {
            speedText.text = string.Format("{0:0.0}", speed) + " u/s";
        }
        if(arrow != null)
        {
            arrow.localEulerAngles =
                new Vector3(0, 0, Mathf.Lerp(minSpeedArrowAngle, maxSpeedArrowAngle, speed / maxSpeed));
        }
    }
}
