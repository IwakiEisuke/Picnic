using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hive : MonoBehaviour
{
    [SerializeField] UnitGenerateStats[] units;

    readonly Dictionary<string, List<GameObject>> generatedUnits = new();

    private void Start()
    {
        for (int i = 0; i < units.Length; i++)
        {
            StartCoroutine(Generate(units[i]));
        }
    }

    private IEnumerator Generate(UnitGenerateStats unit)
    {
        generatedUnits.Add(unit.name, new List<GameObject>());

        while (true)
        {
            yield return new WaitForSeconds(unit.TimeToGenerate);

            if (generatedUnits[unit.name].Count < unit.MaxCount)
            {
                var newUnitObj = Instantiate(unit.Prefab);
                
                generatedUnits[unit.name].Add(newUnitObj);

                newUnitObj.GetComponent<Health>().OnDied += () => generatedUnits[unit.name].Remove(newUnitObj);
            }
        }
    }
}
