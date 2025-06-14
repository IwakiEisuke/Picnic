using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] Image barImage;
    [SerializeField] Image back;

    Health health;

    private void Start()
    {
        health = target.GetComponent<Health>();
    }

    private void Update()
    {
        back.transform.rotation = Camera.main.transform.rotation;
        barImage.fillAmount = health.HealthRatio;
    }
}
