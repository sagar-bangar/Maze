using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGroundedState : CharacterState
{
    public CharacterGroundedState(CharacterController characterController,StateFactory states)
    {
        _character = characterController;
        _states = states;
    }

    public override void EnterState()
    {
        //Debug.Log("Entered Grounded State");
        CheckSwitchChildState();
    }

    public void CheckSwitchChildState()
    {
        if (_character.IsMoving())
        {
            SwitchState(_states.MoveState());
        }
        if (!_character.IsMoving())
        {
            SwitchState(_states.IdleState());
        }
    }

    public override void Update()
    {
        CheckSwitchState();
    }

    public void CheckSwitchState()
    {
        if(_character.OnGround())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SwitchState(_states.JumpState());
            }
        }
    }

    public override void ExitState()
    {
        //Debug.Log("Exited Grounded State");
    }

    #region common Child Behaviour
    protected void ApplyGravity()
    {
        if (!_character.OnGround())
        {
            _character._rigidBody.AddForce(Vector3.down * _character._gravityMultiplier * _character._fallMultiplier * Time.deltaTime, ForceMode.Acceleration);
            //var gravity = Vector3.down * _character._gravityMultiplier * _character._fallMultiplier * Time.deltaTime;
            //_character._rigidBody.velocity = gravity;
        }
    }
    #endregion
}
