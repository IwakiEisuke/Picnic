using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hive : MonoBehaviour
{
    [SerializeField] UnitGenerateSettings[] units;
    [SerializeField] int unitTest;

    readonly Dictionary<string, UnitGenerateState> unitStates = new();

    private void Start()
    {
        for (int i = 0; i < units.Length; i++)
        {
            StartCoroutine(Generate(units[i]));
        }
    }

    private void Update()
    {
        foreach (var unit in units)
        {
            if (unitStates.TryGetValue(unit.name, out var state))
            {
                if (state.isSortie && state.outside < state.exists)
                {
                    var newUnitObj = Instantiate(unit.Prefab);
                    newUnitObj.GetComponent<Health>().OnDied += () => unitStates[unit.name].exists -= 1;
                    state.outside += 1;
                }
            }
        }
    }

    [ContextMenu("Test")]
    private void Sortie()
    {
        var i = unitTest;
        unitStates[units[i].name].isSortie = !unitStates[units[i].name].isSortie;
    }

    private IEnumerator Generate(UnitGenerateSettings unit)
    {
        unitStates.Add(unit.name, new UnitGenerateState());

        while (true)
        {
            yield return new WaitForSeconds(unit.TimeToGenerate);

            if (unitStates[unit.name].exists < unit.MaxCount)
            {
                unitStates[unit.name].exists += 1;
            }
        }
    }
}

[Serializable]
public class UnitGenerateState
{
    public int exists;
    public int outside;
    public bool isSortie;

    public UnitGenerateState(int exists = 0)
    {
        this.exists = exists;
    }
}