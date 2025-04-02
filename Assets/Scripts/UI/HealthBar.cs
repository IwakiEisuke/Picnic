using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Health target;
    [SerializeField] Image barImage;

    private void Update()
    {
        barImage.transform.rotation = Camera.main.transform.rotation;
        barImage.fillAmount = target.HealthRatio;
    }
}
