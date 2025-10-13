using UnityEngine;

namespace Entity.CommandActions
{
    [CreateAssetMenu(fileName = "MoveToHive", menuName = "ScriptableObjects/CommandActions/MoveToHive")]
    public class MoveToHive : CommandActionBase
    {
        private UnitBase _unit;

        public override void Init(GameObject gameObject)
        {
            _unit = gameObject.GetComponent<UnitBase>();
        }

        public override void Enter()
        {
            var hive = GameObject.Find("Hive");
            if (hive != null)
            {
                _unit.Agent.SetDestination(hive.transform.position);
            }
            else
            {
                Debug.Log("Hive not found, setting destination to zero.");
                _unit.Agent.SetDestination(Vector3.zero);
            }
        }
    }
}