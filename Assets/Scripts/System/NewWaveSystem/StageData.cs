using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WaveEditor/StageData")]
public class StageData : ScriptableObject
{
    public List<WaveData> waves = new();
}
