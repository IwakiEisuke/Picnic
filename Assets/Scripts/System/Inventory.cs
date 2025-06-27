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

    public void RemoveHoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot remove a negative amount of honey.");
            return;
        }
        
        if (_honeyCount < amount)
        {
            Debug.LogWarning("Not enough honey to remove.");
            return;
        }
        
        _honeyCount -= amount;
        Debug.Log($"Removed {amount} honey. Remaining honey: {_honeyCount}");
    }
}