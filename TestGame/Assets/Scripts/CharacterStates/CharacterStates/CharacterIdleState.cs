using UnityEngine;

public class CharacterIdleState : CharacterGroundedState
{
    public CharacterIdleState(CharacterController characterController, StateFactory states) : base(characterController, states)
    {
        _character = characterController;
        _states = states;
    }

    public override void EnterState()
    {
        Debug.Log("Entered IdleState");
    }

    public override void Update()
    {
        base.CheckSwitchState();
        CheckSwitchStateGrounded();
    }

    public void CheckSwitchStateGrounded()
    {
        if (_character.IsMoving())
        {
            SwitchState(_states.MoveState());
        }
    }

    public override void FixedUpdate()
    {
        if (_character.OnGround())
        {
            SetIdle();
        }
        else
        {
            base.ApplyGravity();
        }
    }

    public override void ExitState()
    {
        //Debug.Log("Exited IdleState");
    }

    private void SetIdle()
    {
        // set velocity to zero
        if(!_character.IsMoving())
        {
            _character._rigidBody.velocity = Vector3.zero;
        }
    }
}
