using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActionAsset;
    [SerializeField] InputManagerBase[] inputManagers;

    bool isFirstFrame = true;

    private void Start()
    {
        foreach (var manager in inputManagers)
        {
            manager.Init();
        }
    }

    private void Update()
    {
        foreach (var manager in inputManagers)
        {
            manager.Update();
        }

        if (isFirstFrame)
            isFirstFrame = false;
        else
            inputActionAsset.FindActionMap("UI").Disable();
    }

    private void OnDestroy()
    {
        foreach (var manager in inputManagers)
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