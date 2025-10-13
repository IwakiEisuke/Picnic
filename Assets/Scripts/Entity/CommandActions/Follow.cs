using UnityEngine;

namespace Entity.CommandActions
{
    [CreateAssetMenu(fileName = "Follow", menuName = "ScriptableObjects/CommandActions/Follow")]
    public class Follow : CommandActionBase
    {
        private UnitSelector _unitSelector;
        private UnitBase _unit;
        private Transform _target;

        public override void Init(GameObject gameObject)
        {
            _unitSelector = FindAnyObjectByType<UnitSelector>();
            _unit = gameObject.GetComponent<UnitBase>();
        }

        public override void Enter()
        {
            _target = _unitSelector.ControlTarget;
        }

        public override void UpdateAction()
        {
            if (_target == null) return;
            _unit.Agent.SetDestination(_target.position);
        }
    }
}