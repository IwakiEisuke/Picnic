using UnityEngine;

[CreateAssetMenu(fileName = "TestEffect", menuName ="Effects/TestEffect")]
public class TestEffect : StatusEffectAssetBase
{
    [SerializeField] float effectValue = 1;

    public override void Apply(UnitGameStatus target)
    {
        target.speed += effectValue;
    }
}