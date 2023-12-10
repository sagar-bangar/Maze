public class StateFactory
{
    private CharacterController _characterController;

    private CharacterState _idleState;
    private CharacterState _moveState;
    private CharacterState _jumpState;
    private CharacterState _groundedState;

    public StateFactory(CharacterController characterController)
    {
        _characterController = characterController;
        _idleState = new CharacterIdleState(_characterController, this);
        _moveState = new CharacterMoveState(_characterController, this);
        _jumpState = new CharacterJumpState(_characterController, this);
        _groundedState = new CharacterGroundedState(characterController, this);
    }

    public CharacterState IdleState()
    {
        return _idleState;
    }

    public CharacterState MoveState()
    {
        return _moveState;
    }

    public CharacterState JumpState()
    {
        return _jumpState;
    }

    public CharacterState GroundedState()
    {
        return _groundedState;
    }
}
