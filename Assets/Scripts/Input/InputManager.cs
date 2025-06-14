using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] InputManagerBase[] InputManagers;

    private void Start()
    {
        foreach (var manager in InputManagers)
        {
            manager.Init();
        }
    }

    private void Update()
    {
        foreach (var manager in InputManagers)
        {
            manager.Update();
        }
    }

    private void OnDestroy()
    {
        foreach (var manager in InputManagers)
        {
            manager.ResetActions();
        }
    }
}

/// <summary>
/// InputManagerが呼び出す必要のあるメソッドを定義するインターフェース
/// </summary>
public interface IInputManager
{
    public void Init();
    public void Update();
    public void ResetActions();
}

/// <summary>
/// InputManagerの基本実装を提供する抽象クラス
/// </summary>
public abstract class InputManagerBase : ScriptableObject, IInputManager
{
    public abstract void Init();
    public abstract void Update();
    public abstract void ResetActions();
}