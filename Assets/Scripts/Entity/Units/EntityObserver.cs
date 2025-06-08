using UnityEngine;

/// <summary>
/// Entity��EntityManager�֓o�^�E�폜����N���X
/// </summary>
public class EntityObserver
{
    private string _name;

    public EntityObserver(string name)
    {
        _name = name;
    }

    public void Register()
    {
        EnemyManager.AddEnemy(_name);
    }

    public void Remove()
    {
        EnemyManager.RemoveEnemy(_name);
    }
}
