using UnityEngine;

/// <summary>
/// 蜂蜜などのアイテムを格納するインベントリ
/// </summary>
public class Inventory : MonoBehaviour
{
    [SerializeField] private int _honeyCount = 0; // 蜂蜜の数

    public int HoneyCount => _honeyCount;

    public void AddHoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot add a negative amount of honey.");
            return;
        }
        
        _honeyCount += amount;
        Debug.Log($"Added {amount} honey. Total honey: {_honeyCount}");
    }
}