using UnityEngine;

public class CharacterJumpState : CharacterState
{
    private float apexTime;
    private float gravity;
    private float initialVelocity;
    private bool doubleJump;
    private bool isJumped;

    public CharacterJumpState(CharacterController characterController, StateFactory states)
    {
        _character = characterController;
        _states = states;

    }

    public override void EnterState()
    {
        Debug.Log("Entered JumpState");
        CalculateJump();

        isJumped = false;
        doubleJump = false;
    }

    public void CheckSwitchState()
    {
        if (_character.OnGround())
        {
            SwitchState(_states.GroundedState());
        }
    }

    public override void FixedUpdate()
    {
        if (isJumped && _character.OnGround())
        {
            SwitchState(_states.GroundedState());
            return;
        }
        if (_character.OnGround())
        {
            Jump();
        }
        else
        {
            CalculateGravity();
            isJumped = true;
            HandleInAir();
        }
    }

    private void Jump()
    {
        Debug.Log("Jumping");
        _character._rigidBody.velocity -= Vector3.up * _character._rigidBody.velocity.y; // negating the y velocity while moving up and down as y velocity increases while moving up and decreases while moving down resulting in very high and very small jumps respectively on slope
        _character._rigidBody.AddForce(Vector3.up * initialVelocity * Time.deltaTime, ForceMode.Impulse);
    }

    private void HandleInAir()
    {
        if (Input.GetKey(KeyCode.Space) && !doubleJump)
        {
            ApplyCustomGravity();
        }
        else
        {
            ApplyDefaultGravity();
        }
        if (Input.GetKeyDown(KeyCode.Space) && !doubleJump)
        {
            _character._rigidBody.velocity -= Vector3.up * _character._rigidBody.velocity.y;
            _character._rigidBody.AddForce(Vector3.up * _character._playerJumpHeight * 10f * Time.deltaTime, ForceMode.Impulse);
            doubleJump = true;
        }
    }

    private void CalculateJump()
    {
        apexTime = _character._maxJumpTime / 2;
        gravity = (-2 * _character._playerJumpHeight * _character._gravityMultiplier) / Mathf.Pow(apexTime, 2);
        initialVelocity = (2 * _character._playerJumpHeight) / apexTime;
    }

    private void CalculateGravity()
    {
        float previousYVelocity = -1 * _character._gravityMultiplier * _character._fallMultiplier;
        float currentVelocity = previousYVelocity + gravity * Time.deltaTime;
        float newYVelocity = (previousYVelocity + currentVelocity) * 0.5f;
        gravity = newYVelocity;
    }

    public void Move(float moveSpeed)
    {
        if (!_character.OnGround())
        {
            Vector3 targetDirection = (new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))).normalized;
            Vector3 targetVelocity = targetDirection * moveSpeed;
            _character._moveDir = targetVelocity;
            //_character._rigidBody.AddForce(_character._moveDir * Time.deltaTime, ForceMode.Impulse);
            _character._rigidBody.velocity += targetVelocity;
        }
    }

    private void ApplyCustomGravity()
    {
        if (_character._rigidBody.velocity.y >= 0) // while jumping
        {
            _character._rigidBody.AddForce(Vector3.up * (gravity * _character._jumpMultiplier) * Time.deltaTime, ForceMode.Acceleration); // apply while jumping Gravity (jumpmultiplier=0-1)
        }
        else if (_character._rigidBody.velocity.y <= 0) // while falling
        {
            _character._rigidBody.AddForce(Vector3.up * (gravity * _character._fallMultiplier) * Time.deltaTime, ForceMode.Acceleration); // apply while falling Gravity (fallmultipier=1-2)
        }
        else
        {
            _character._rigidBody.AddForce(Vector3.up * (gravity) * Time.deltaTime, ForceMode.Acceleration);
        }
    }

    private void ApplyDefaultGravity()
    {
        _character._rigidBody.AddForce(Vector3.up * (gravity * _character._fallMultiplier) * Time.deltaTime, ForceMode.Acceleration);
    }

    public override void ExitState()
    {
        isJumped = true;
        doubleJump = true;
        _character._rigidBody.velocity = Vector3.zero;
        Debug.Log("Exited JumpState");
    }
}
