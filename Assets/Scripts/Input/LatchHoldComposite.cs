using System.ComponentModel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

/// <summary>
/// 同時入力後、Button長押し中であればModifierを離しても入力を継続させるModifier
/// </summary>
[DisplayName("Latch Hold Modifier")]
public class LatchHoldComposite : InputBindingComposite<float>
{
    [InputControl(layout = "Button")]
    public int modifier; // Ctrl
    [InputControl(layout = "Button")]
    public int button;   // 任意キー

    private bool isButtonOnlyPressed = false;
    private bool wasPressed = false;

    /// <summary>
    /// 初期化
    /// </summary>
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
#endif
    private static void Initialize()
    {
        // 初回にCompositeBindingを登録する必要がある
        InputSystem.RegisterBindingComposite(typeof(DisallowOneModifierComposite), "DisallowOneModifierComposite");
    }

    public override float ReadValue(ref InputBindingCompositeContext context)
    {
        bool modPressed = context.ReadValueAsButton(modifier);
        bool btnPressed = context.ReadValueAsButton(button);

        // buttonを押している間に後から modifierを押しても反応しないように
        if (btnPressed && !modPressed)
        {
            isButtonOnlyPressed = true;
        }
        if (!btnPressed)
        {
            isButtonOnlyPressed = false;
        }

        // 同時押ししたときに判定を開始
        if (modPressed && btnPressed && !isButtonOnlyPressed)
        {
            wasPressed = true;
            return 1f;
        }

        // Buttonが長押しされている間は判定し続ける
        if (wasPressed && btnPressed)
        {
            return 1f;
        }

        // 両方離れたらリセット
        wasPressed = false;
        return 0f;
    }

    /// <summary>
    /// 入力値の大きさを取得する
    /// modifier入力の押下判定（Press Pointとの閾値判定）のために実装必須
    /// </summary>
    public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
    {
        return ReadValue(ref context);
    }
}
