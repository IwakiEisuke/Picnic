public class Enemy : UnitBase
{
    public FSM movementFSM;
    public FSM attackFSM;

    private void Start()
    {
        movementFSM =
            new(
                new()
                {
                    { new MoveToHive(this), new(){ new FSM.Transition(1, () => false) } },
                }
            );

        attackFSM =
            new(
                new()
                {
                    { new NearTargetAttack(this), new(){ new FSM.Transition(3, () => false) } },
                }
            );
    }

    private void Update()
    {
        movementFSM.Update();
        attackFSM.Update();
    }
}
