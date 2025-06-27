using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitGenerateView : MonoBehaviour
{
    [SerializeField] UnitGenerateStats stats;
    [SerializeField] AllyManager allyManager;
    [SerializeField] Button buttonToggleSortie;
    [SerializeField] Image generateBar;
    [SerializeField] TextMeshProUGUI countText;

    private void Start()
    {
        buttonToggleSortie.onClick.AddListener(() => allyManager.ToggleSortie(stats));
    }

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
