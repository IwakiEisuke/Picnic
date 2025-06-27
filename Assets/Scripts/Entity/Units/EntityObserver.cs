using UnityEngine;

/// <summary>
/// EntityをEntityManagerへ登録・削除するクラス
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
