using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitGenerateView : MonoBehaviour
{
    [SerializeField] UnitGenerateStats stats;
    [SerializeField] Image generateBar;
    [SerializeField] TextMeshProUGUI countText;
    
    public void View(UnitGenerateStats stats)
    {
        generateBar.fillAmount = stats.ProgressRatio;
        countText.text = $"{stats.exists}/{stats.MaxCount}";
    }

    private void Update()
    {
        View(stats);
    }
}
