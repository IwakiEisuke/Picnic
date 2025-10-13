using UnityEngine;

namespace Entity.CommandActions
{
    /// <summary>
    /// 自動移動
    /// </summary>
    [CreateAssetMenu(fileName = "AutoMove", menuName = "ScriptableObjects/CommandActions/AutoMove")]
    public class AutoMove : CommandActionBase
    {
        UnitBase _unit;

        public override void Init(GameObject gameObject)
        {
            _unit = gameObject.GetComponent<UnitBase>();
        }

        public override void Enter()
        {
            _unit.ActionManager.enabled = true;
        }

        public override void Exit()
        {
            _unit.ActionManager.enabled = false;
        }
    }
}