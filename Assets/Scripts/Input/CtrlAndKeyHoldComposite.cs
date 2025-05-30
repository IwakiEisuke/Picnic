using System.ComponentModel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

[DisplayName("Hold Consumption Modifier")]
public class CtrlAndKeyHoldComposite : InputBindingComposite<float>
{
    [InputControl(layout = "Button")]
    public int modifier; // Ctrl
    [InputControl(layout = "Button")]
    public int button;   // 任意キー

    private bool wasPressed = false;

    public override float ReadValue(ref InputBindingCompositeContext context)
    {
        bool modPressed = context.ReadValueAsButton(modifier);
        bool btnPressed = context.ReadValueAsButton(button);

        if (modPressed && btnPressed)
        {
            wasPressed = true;
            return 1f;
        }
        if (wasPressed && (!modPressed || !btnPressed))
        {
            // どちらかが離されたが、まだ両方離れていない
            return 1f;
        }
        // 両方離されたらリセット
        wasPressed = false;
        return 0f;
    }
}
