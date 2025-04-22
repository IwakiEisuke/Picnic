using System.Collections.Generic;
using UnityEngine;

public class UtilityManager : MonoBehaviour
{
    public static List<IUtilityUpdate> updates = new();

    [RuntimeInitializeOnLoadMethod]
    public static void Setup()
    {
        var obj = new GameObject { name = nameof(UtilityManager) };
        obj.AddComponent<UtilityManager>();
        updates.Clear();
        DontDestroyOnLoad(obj);
    }

    public static void Subscribe(IUtilityUpdate utility)
    {
        updates.Add(utility);
    }

    public static void UnSubscribe(IUtilityUpdate utility)
    {
        updates.Remove(utility);
    }

    private void Update()
    {
        var dummy = new List<IUtilityUpdate>();

        for (int i = 0; i < updates.Count; i++)
        {
            dummy.Add(updates[i]);
        }

        for (int i = 0; i < dummy.Count; i++)
        {
            dummy[i].Update();
        }
    }
}

public interface IUtilityUpdate
{
    public void Update();
}