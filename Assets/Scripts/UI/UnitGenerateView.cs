using UnityEngine;
using UnityEngine.UI;

public class UnitGenerateView : MonoBehaviour
{
    [SerializeField] UnitGenerateStats stats;
    [SerializeField] Image generateBar;

    public void View(UnitGenerateStats stats)
    {
        generateBar.fillAmount = stats.ProgressRatio;
    }

    private void Update()
    {
        View(stats);
    }
}
