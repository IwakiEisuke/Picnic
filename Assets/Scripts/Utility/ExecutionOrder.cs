public enum ExecutionOrder
{
    HitEventManager = -2,
    HitManager = -1,
    GameManager = 0,
    UnitController,
    UnitSelector,

    AttackCollisionController,
    EntityBase,
}
