using System.ComponentModel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

[DisplayName("Disallow One Modifier")]
public class DisallowOneModifierComposite : InputBindingComposite<float>
{
    // このキーが押されていなければbuttonのActionを実行する
    [InputControl(layout = "Button")] public int modifier;

    // 排他制御対象のボタン
    [InputControl(layout = "Button")] public int button;

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

    /// <summary>
    /// 一方のボタンが押されていない時だけ値を返す
    /// </summary>
    public override float ReadValue(ref InputBindingCompositeContext context)
    {
        // modifierのボタンが押されていない時だけbuttonの入力を通す
        if (!context.ReadValueAsButton(modifier))
            return context.ReadValue<float>(button);

        return default;
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