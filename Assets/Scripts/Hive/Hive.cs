using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hive : MonoBehaviour
{
    [SerializeField] UnitGenerateSettings[] units;
    [SerializeField] int unitTest;

    readonly List<UnitGenerateState> unitStates = new();

    private void Start()
    {
        for (int i = 0; i < units.Length; i++)
        {
            StartCoroutine(Generate(units[i]));
        }
    }

    private void Update()
    {
        foreach (var state in unitStates)
        {
            if (state.isSortie && state.outside < state.exists)
            {
                var newUnitObj = Instantiate(state.settings.Prefab);
                newUnitObj.GetComponent<Health>().OnDied += () => state.exists -= 1;
                state.outside += 1;
            }
        }
    }

    [ContextMenu("Test")]
    private void Sortie()
    {
        var i = unitTest;
        unitStates[i].isSortie = !unitStates[i].isSortie;
    }

    private IEnumerator Generate(UnitGenerateSettings unit)
    {
        var generateState = new UnitGenerateState(unit);
        unitStates.Add(generateState);

        while (true)
        {
            yield return new WaitForSeconds(unit.TimeToGenerate);

            if (generateState.exists < unit.MaxCount)
            {
                generateState.exists += 1;
            }
        }
    }
}

[Serializable]
public class UnitGenerateState
{
    public UnitGenerateSettings settings;
    public int exists;
    public int outside;
    public bool isSortie;

    public UnitGenerateState(UnitGenerateSettings settings)
    {
        this.settings = settings;
    }
}