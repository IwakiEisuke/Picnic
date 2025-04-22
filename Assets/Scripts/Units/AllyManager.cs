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

public class UnitFactory
{
    readonly UnitGenerateStats _generateStats;
    readonly Transform _transform;

    public UnitFactory(UnitGenerateStats generateStats, Transform transform)
    {
        _generateStats = generateStats;
        _transform = transform;
    }

    public void Update()
    {
        if (_generateStats.Target.isSortie && _generateStats.outside < _generateStats.exists)
        {
            Produce(_generateStats, _transform);
        }
    }

    public GameObject Produce(UnitGenerateStats generateStats, Transform transform)
    {
        var newUnitObj = Object.Instantiate(generateStats.Prefab, transform.position, transform.rotation);

        newUnitObj.GetComponent<IHealth>().Health.OnDied.AddListener(() => generateStats.exists -= 1);
        newUnitObj.GetComponent<IUnit>().Destroyed += () => generateStats.outside -= 1;

        generateStats.outside += 1;
        return newUnitObj;
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
    UnitGenerator[] _generators;
    UnitFactory[] _factories;

    public UnitGenerateManager(MonoBehaviour parent, UnitGenerateStats[] units)
    {
        _generators = new UnitGenerator[_generators.Length];
        _factories = new UnitFactory[_factories.Length];

        for (int i = 0; i < units.Length; i++)
        {
            _generators[i] = new(parent, units[i]);
            _factories[i] = new(units[i], parent.transform);
        }
    }
}