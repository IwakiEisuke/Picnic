using System.Collections;
using UnityEngine;

public class Hive : MonoBehaviour
{
    [SerializeField] UnitGenerateStats[] units;

    private void Start()
    {
        for (int i = 0; i < units.Length; i++)
        {
            StartCoroutine(Generate(units[i]));
        }
    }

    private void Update()
    {
        foreach (var generateStats in units)
        {
            if (generateStats.Target.isSortie && generateStats.outside < generateStats.exists)
            {
                var newUnitObj = Instantiate(generateStats.Prefab);
                newUnitObj.GetComponent<Health>().OnDied += () => generateStats.exists -= 1;
                generateStats.outside += 1;
            }
        }
    }

    private IEnumerator Generate(UnitGenerateStats unit)
    {
        while (true)
        {
            yield return new WaitForSeconds(unit.TimeToGenerate);

            if (unit.exists < unit.MaxCount)
            {
                unit.exists += 1;
            }
        }
    }

    public void ToggleSortie(int i)
    {
        units[i].Target.isSortie = !units[i].Target.isSortie;
    }
}

public class UnitGenerateState
{
    public UnitGenerateStats settings;
    public bool isSortie;

    public UnitGenerateState(UnitGenerateStats settings)
    {
        this.settings = settings;
    }
}