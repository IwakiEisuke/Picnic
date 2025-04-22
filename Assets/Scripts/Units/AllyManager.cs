using System.Collections.Generic;
using UnityEngine;

public class AllyManager : MonoBehaviour
{
    [SerializeField] UnitGenerateStats[] units;

    readonly Dictionary<GameObject, UnitGenerateStats> unitInstances = new();

    UnitGenerateManager generateManager;

    private void Start()
    {
        generateManager = new UnitGenerateManager(this, units);
    }

    private void Update()
    {
        foreach (var generateStats in units)
        {
            if (generateStats.Target.isSortie && generateStats.outside < generateStats.exists)
            {
                var newUnitObj = Instantiate(generateStats.Prefab);

                newUnitObj.GetComponent<IHealth>().Health.OnDied.AddListener(() => generateStats.exists -= 1);
                newUnitObj.GetComponent<IUnit>().Destroyed += () => generateStats.outside -= 1;

                generateStats.outside += 1;
                unitInstances[newUnitObj] = generateStats;
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

public class UnitGenerator
{
    MonoBehaviour _parent;
    UnitGenerateStats _unit;
    Coroutine _generate;

    public UnitGenerator(MonoBehaviour parent, UnitGenerateStats unit)
    {
        _unit = unit;
        _unit.Init();
        _unit.Target.Init();
        _parent = parent;
        _generate = _parent.StartCoroutine(unit.Generate());
    }
}

public class UnitGenerateManager
{
    UnitGenerator[] _units;

    public UnitGenerateManager(MonoBehaviour parent, UnitGenerateStats[] units)
    {
        _units = new UnitGenerator[_units.Length];

        for (int i = 0; i < units.Length; i++)
        {
            _units[i] = new(parent, units[i]);
        }
    }
}