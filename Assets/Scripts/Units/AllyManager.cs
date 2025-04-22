using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 味方ユニットの管理
/// </summary>
public class AllyManager : MonoBehaviour
{
    [SerializeField] UnitGenerateStats[] units;

    readonly Dictionary<GameObject, UnitGenerateStats> unitInstances = new();

    UnitGenerateManager generateManager;

    private void Start()
    {
        generateManager = new UnitGenerateManager(this, units, unitInstances);
    }

    private void Update()
    {
        generateManager.Update();
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

/// <summary>
/// ユニットGameObjectの生成を管理するクラス
/// </summary>
public class UnitFactory
{
    readonly public UnitGenerateStats generateStats;
    readonly Transform _transform;

    public UnitFactory(UnitGenerateStats generateStats, Transform transform)
    {
        this.generateStats = generateStats;
        _transform = transform;
    }

    public GameObject Update()
    {
        if (generateStats.Target.isSortie && generateStats.outside < generateStats.exists)
        {
            return Produce(generateStats, _transform);
        }

        return null;
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

/// <summary>
/// ユニットの初期化と生産処理を行うクラス
/// </summary>
public class UnitGenerator
{
    readonly MonoBehaviour _parent;
    readonly UnitGenerateStats _unit;
    readonly Coroutine _generate;

    public UnitGenerator(MonoBehaviour parent, UnitGenerateStats unit)
    {
        _unit = unit;
        _unit.Init();
        _unit.Target.Init();
        _parent = parent;
        _generate = _parent.StartCoroutine(unit.Generate());
    }
}

/// <summary>
/// 複数種類のユニット生産を管理するクラス
/// </summary>
public class UnitGenerateManager
{
    readonly UnitGenerator[] _generators;
    readonly UnitFactory[] _factories;
    readonly Dictionary<GameObject, UnitGenerateStats> _unitInstances;

    public UnitGenerateManager(MonoBehaviour parent, UnitGenerateStats[] units, Dictionary<GameObject, UnitGenerateStats> unitInstances)
    {
        _generators = new UnitGenerator[units.Length];
        _factories = new UnitFactory[units.Length];
        _unitInstances = unitInstances;

        for (int i = 0; i < units.Length; i++)
        {
            _generators[i] = new(parent, units[i]);
            _factories[i] = new(units[i], parent.transform);
        }
    }

    public void Update()
    {
        for (int i = 0; i < _factories.Length; i++)
        {
            var product = _factories[i].Update();

            if (product)
            {
                _unitInstances.Add(product, _factories[i].generateStats);
            }
        }
    }
}