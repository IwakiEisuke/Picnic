using UnityEngine;

namespace Entity.CommandActions
{
    /// <summary>
    /// 停止
    /// </summary>
    [CreateAssetMenu(fileName = "Stop", menuName = "ScriptableObjects/CommandActions/Stop")]
    public class Stop : CommandActionBase
    {
        private UnitBase _unit;

        public override void Init(GameObject gameObject)
        {
            _unit = gameObject.GetComponent<UnitBase>();
        }

        public override void Enter()
        {
            _unit.Agent.isStopped = true;
        }

        public override void Exit()
        {
            _unit.Agent.isStopped = false;
        }
    }
}