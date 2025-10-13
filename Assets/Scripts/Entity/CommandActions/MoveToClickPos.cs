using UnityEngine;
using UnityEngine.InputSystem;

namespace Entity.CommandActions
{
    [CreateAssetMenu(fileName = "MoveToClickPos", menuName = "ScriptableObjects/CommandActions/MoveToClickPos")]
    public class MoveToClickPos : CommandActionBase
    {
        private UnitBase unit;

        public override void Init(GameObject gameObject)
        {
            unit = gameObject.GetComponent<UnitBase>();
        }

        public override void Enter()
        {
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out var hit))
            {
                unit.Agent.SetDestination(hit.point);
            }
        }
    }
}