using UnityEngine;

namespace Entity.CommandActions
{
    [CreateAssetMenu(fileName = "InteractTarget", menuName = "ScriptableObjects/CommandActions/InteractTarget")]
    public class InteractTarget : CommandActionBase
    {
        UnitBase _unit;
        Transform _target;
        IInteractable _interactable;

        readonly Timer _timer = new();

        public override void Init(GameObject gameObject)
        {
            _unit = gameObject.GetComponent<UnitBase>();
        }

        public override void Enter()
        {
            _target = FindAnyObjectByType<UnitSelector>().ControlTarget;
            _interactable = _target.GetComponent<IInteractable>();
            if (_interactable != null)
            {
                _timer.TimeUp += _interactable.Interact;
                _unit.Agent.SetDestination(_target.position);
            }
            else
            {
                Debug.Log("No target to interact with");
                Cancel();
            }
        }

        public override void Exit()
        {
            _timer.Cancel();
            if (_interactable != null)
            {
                _timer.TimeUp -= _interactable.Interact;
            }
        }

        public override void UpdateAction()
        {
            if (_interactable == null || !_interactable.IsInteractable) return;

            _unit.Agent.SetDestination(_unit.transform.position);

            if (Vector3.Distance(_target.position, _unit.transform.position) < _unit.Stats.AttackRadius)
            {
                if (!_timer.IsActive) _timer.Set(_interactable.Duration);
            }
        }

        private void Cancel()
        {
            if (_unit.TryGetComponent<CommandExecutor>(out var commandExecutor))
            {
                commandExecutor.SetDefault();
            }
        }
    }
}