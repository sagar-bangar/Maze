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
        if(!_character.IsMoving())
        {
            /*Vector3 friction = new Vector3(_character._rigidBody.velocity.x, 0, _character._rigidBody.velocity.z).normalized;
            Vector3 frictionAmount = Vector3.MoveTowards(friction, Vector3.zero, 0.1f * Time.deltaTime);
            float amount = Mathf.Abs(frictionAmount.magnitude);
            amount *= Mathf.Sign(frictionAmount.magnitude) * 0.3f;
            _character._rigidBody.AddForce(friction * -amount, ForceMode.Impulse);
            if (friction.magnitude < 1f)
            {
                _character._rigidBody.velocity = Vector3.zero;
            }*/
            _character._rigidBody.velocity = Vector3.zero;
        }
    }
}
