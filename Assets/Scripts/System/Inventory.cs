using UnityEngine;

/// <summary>
/// �I���Ȃǂ̃A�C�e�����i�[����C���x���g��
/// </summary>
public class Inventory : MonoBehaviour
{
    [SerializeField] private int _honeyCount = 0; // �I���̐�

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