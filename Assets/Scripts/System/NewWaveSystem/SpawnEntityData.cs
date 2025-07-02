using UnityEngine;

/// <summary>
/// スポーンイベント用のエンティティデータを定義するクラス
/// </summary>
[CreateAssetMenu(fileName = "SpawnEntityData", menuName = "Wave System/SpawnEntityData", order = 1)]
public class SpawnEntityData : ScriptableObject
{
    public Texture2D icon;
    public GameObject entityPrefab;
}