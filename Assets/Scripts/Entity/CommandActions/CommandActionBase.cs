using UnityEngine;

namespace Entity.CommandActions
{
    /// <summary>
    /// コマンド選択時に実行されるユニット行動の基底クラス
    /// </summary>
    public abstract class CommandActionBase : ScriptableObject
    {
        public virtual void Init(GameObject gameObject) { }
        public virtual void Enter() { }
        public virtual void UpdateAction() { }
        public virtual void Exit() { }
    }
}