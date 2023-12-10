using UnityEngine;

public class CharacterMoveState : CharacterGroundedState
{
    private float lastGrounded;

    public CharacterMoveState(CharacterController characterController, StateFactory states) : base(characterController, states)
    {
        _character = characterController;
        _states = states;
    }

    public override void EnterState()
    {
        Debug.Log("Entered MoveState");
        _character._particles._dustTrialParticles.Play();
    }

    public override void Update()
    {
        base.CheckSwitchState();
        CheckSwitchStateGrounded();
        if (_character.OnGround())
        {
            CalculateMoveDirection();
            if (Input.GetKey(KeyCode.LeftShift) || _character._isRunning)
            {
                CalculateSpeed(_character._run);
            }
            else
            {
                CalculateSpeed(_character._walk);
            }
        }
    }

    public void CheckSwitchStateGrounded()
    {
        if (!_character.IsMoving())
        {
            _character._rigidBody.velocity = Vector3.zero;
            SwitchState(_states.IdleState());
        }
    }

    public override void FixedUpdate()
    {
        if (!_character.OnGround())
        {
            base.ApplyGravity();
            lastGrounded += Time.deltaTime;
        }
        else
        {
            Move();
            lastGrounded = 0;

        }
        Rotate();
        if (lastGrounded < 0.2f)
        {
            Vector3 currentVelocity = _character._rigidBody.velocity;
            Vector3 ProjectedVeocity = Vector3.ProjectOnPlane(_character._rigidBody.velocity, (_character._hit.normal + _character._forwardHit.normal) * 0.5f);
            _character._rigidBody.velocity = Vector3.Lerp(currentVelocity,ProjectedVeocity, Time.deltaTime/0.02f);
        }
    }

    public override void ExitState()
    {
        // Debug.Log("Exited MoveState");
        _character._particles._dustTrialParticles.Clear();
        _character._particles._dustTrialParticles.Stop();
    }

    private float charAcceleration;
    private void CalculateMoveDirection()
    {

        float time = 0;
        time += Time.deltaTime;
        Vector3 targetDirection = Vector3.zero;
        Vector3 forwardTargetDirection = Vector3.zero;
        Vector3 averageDirection = Vector3.zero;
        {
            targetDirection = (new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))).normalized;
            targetDirection = targetDirection.z * _character._playerCamControllerTransform.forward + targetDirection.x * _character._playerCamControllerTransform.right;
            forwardTargetDirection = targetDirection;
            float desiredDirection = Vector3.Dot(_character.transform.up, _character._hit.normal);
            float angle = Mathf.Acos(desiredDirection) * Mathf.Rad2Deg;
            Vector3 rotAxis = Vector3.Cross(_character.transform.up, _character._hit.normal);
            Quaternion rot = Quaternion.AngleAxis(angle, rotAxis);
            targetDirection = rot * targetDirection;
            /*float forwardProjection = Vector3.Dot(_character._forwardHit.normal, forwardTargetDirection);
            forwardTargetDirection = (forwardTargetDirection - _character._forwardHit.normal * forwardProjection).normalized;
            float projection = Vector3.Dot(_character._hit.normal, targetDirection);
            targetDirection = (targetDirection - _character._hit.normal * projection).normalized;
            averageDirection = (targetDirection + forwardTargetDirection) * 0.5f;*/
        }
        _character._moveDir = (targetDirection);
    }

    private void CalculateSpeed(float moveSpeed)
    {
        Vector3 targetVelocity = _character._moveDir * moveSpeed;
        Vector3 initalVelocity = (_character._rigidBody.velocity);
        Vector3 finalVelocity = targetVelocity;
        Vector3 dispacement = initalVelocity + (10 * Time.deltaTime) * (finalVelocity - initalVelocity);
        //Vector3 speedDelta = Vector3.MoveTowards(_character._rigidBody.velocity, targetVelocity, 20 * Time.deltaTime); // 20 * 0.2 = 4
        charAcceleration = dispacement.magnitude;
        //Debug.Log("Velocity " + finalVelocity.magnitude);
    }

    public void Move()
    {
        _character._rigidBody.velocity = _character._moveDir.normalized * charAcceleration;
    }

    public void Rotate()
    {
        if (_character._moveDir != Vector3.zero)
        {
            Vector3 lookDirection = new Vector3(_character._moveDir.x, 0, _character._moveDir.z);
            Quaternion rotateTowards = Quaternion.LookRotation(lookDirection, Vector3.up);
            _character.transform.localRotation = Quaternion.Slerp(_character.transform.rotation, rotateTowards, (100f * Time.fixedDeltaTime) * 0.1f);
        }
    }
}
