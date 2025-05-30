using System.ComponentModel;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

[DisplayName("Hold Consumption Modifier")]
public class CtrlAndKeyHoldComposite : InputBindingComposite<float>
{
    [InputControl(layout = "Button")]
    public int modifier; // Ctrl
    [InputControl(layout = "Button")]
    public int button;   // �C�ӃL�[

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
            // �ǂ��炩�������ꂽ���A�܂���������Ă��Ȃ�
            return 1f;
        }
        // ���������ꂽ�烊�Z�b�g
        wasPressed = false;
        return 0f;
    }
}
