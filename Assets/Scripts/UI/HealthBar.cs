using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Health target;
    [SerializeField] Image barImage;
    [SerializeField] Image back;

    private void Update()
    {
        back.transform.rotation = Camera.main.transform.rotation;
        barImage.fillAmount = target.HealthRatio;
    }
}
