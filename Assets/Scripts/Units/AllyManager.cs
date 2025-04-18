using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyManager : MonoBehaviour
{
    [SerializeField] UnitGenerateStats[] units;

    readonly Dictionary<GameObject, UnitGenerateStats> unitInstances = new();

    private void Start()
    {
        for (int i = 0; i < units.Length; i++)
        {
            units[i].Init();
            units[i].Target.Init();
            StartCoroutine(Generate(units[i]));
        }
    }

    private void Update()
    {
        foreach (var generateStats in units)
        {
            if (generateStats.Target.isSortie && generateStats.outside < generateStats.exists)
            {
                var newUnitObj = GameObject.Instantiate(generateStats.Prefab);
                newUnitObj.GetComponent<IHealth>().Health.OnDied.AddListener(() => generateStats.exists -= 1);
                newUnitObj.GetComponent<IHealth>().Health.OnDestroyEvent += () => generateStats.outside -= 1;
                generateStats.outside += 1;
                unitInstances[newUnitObj] = generateStats;
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
        print($"{units[i].name} {units[i].Target.isSortie}");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody.TryGetComponent<Ally>(out var ally))
        {
            if (unitInstances.TryGetValue(ally.gameObject, out var gen) && !ally.Stats.isSortie)
            {
                Destroy(ally.gameObject);
                unitInstances.Remove(ally.gameObject);
            }
        }
    }
}