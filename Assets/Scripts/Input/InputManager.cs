using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActionAsset;
    [SerializeField] InputManagerBase[] inputManagers;

    /// <summary>
    /// Initializes all configured input managers and configures input action maps for gameplay.
    /// </summary>
    /// <remarks>
    /// Calls Init() on each entry in <c>inputManagers</c>, enables the "Player" action map, and disables the "UI" action map.
    /// </remarks>
    private void Start()
    {
        foreach (var manager in inputManagers)
        {
            manager.Init();
        }

        inputActionAsset.FindActionMap("Player").Enable();
        inputActionAsset.FindActionMap("UI").Disable();
    }

    /// <summary>
    /// Calls the per-frame update on each configured input manager.
    /// </summary>
    private void Update()
    {
        foreach (var manager in inputManagers)
        {
            manager.Update();
        }
    }

    /// <summary>
    /// Resets actions on all configured input managers when this object is destroyed.
    /// </summary>
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