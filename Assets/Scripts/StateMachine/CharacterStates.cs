namespace GameStates
{
    public enum DirectionStates
    {
        None,
        Forward,
        Left,
        Right,
        Backward,
        ForwardLeft,
        ForwardRight,
        BackwardLeft,
        BackwardRight
    }

    public enum MovementStates
    {
        None,
        Idle,
        Walking,
        Running,
        Sprinting,
        InAir
    }

    public enum ActionStates
    {
        None,
        AbilityActive,
        HitActive,
        MoveAbilityActive,
        AimActive,
        Blocked
    }

}