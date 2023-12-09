using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterState
{
    protected CharacterController _character;
    protected StateFactory _states;

    public virtual void EnterState()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void ExitState()
    {

    }

    protected void SwitchState(CharacterState newState)
    {
        _character._currentState.ExitState();
        _character._currentState = newState;
        newState.EnterState();
    }
}

