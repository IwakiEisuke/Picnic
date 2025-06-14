using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] MouseInputManager mouseInputManager;

    public MouseInputManager MouseInput => mouseInputManager;

    private void Start()
    {
        mouseInputManager.Init();
    }

    private void Update()
    {
        mouseInputManager.Update();
    }

    private void OnDestroy()
    {
        mouseInputManager.ResetActions();
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