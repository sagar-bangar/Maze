using UnityEngine;

public class CharacterJumpState : CharacterState
{
    private float apexTime;
    private float gravity;
    private float initialVelocity;
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

        // Reset jump flags
        isJumped = false;
    }

    public void CheckSwitchState()
    {
        // Check if the character is on the ground and switch to the grounded state
        if (_character.OnGround())
        {
            SwitchState(_states.GroundedState());
        }
    }

    public override void FixedUpdate()
    {
        // Check if the character has jumped and is now on the ground
        if (isJumped && _character.OnGround())
        {
            // Switch to the grounded state
            SwitchState(_states.GroundedState());
            return;
        }

        // If the character is on the ground, perform a regular jump
        if (_character.OnGround() && Input.GetKey(KeyCode.Space))
        {
            Jump();
        }
        else if (!_character.OnGround())
        {
            // If in the air, calculate gravity, mark as jumped, and handle in-air movement
            CalculateGravity();
            isJumped = true;
            HandleInAir();
        }
    }

    private void Jump()
    {
        Debug.Log("Jumping");

        // Negate y velocity to avoid issues on slopes
        _character._rigidBody.velocity -= Vector3.up * _character._rigidBody.velocity.y;

        // Apply the initial jump force
        _character._rigidBody.AddForce(Vector3.up * initialVelocity * Time.deltaTime, ForceMode.Impulse);
    }

    private void HandleInAir()
    {
        // Check if the jump key is held for applying custom gravity
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyCustomGravity();
        }
        else
        {
            // If the jump key is not held, apply default gravity
            ApplyDefaultGravity();
        }
    }

    private void CalculateJump()
    {
        // Calculate jump parameters based on max jump time, player jump height, and gravity multiplier
        apexTime = _character._maxJumpTime / 2;
        gravity = (-2 * _character._playerJumpHeight * _character._gravityMultiplier) / Mathf.Pow(apexTime, 2);
        initialVelocity = (2 * _character._playerJumpHeight) / apexTime;
    }

    private void CalculateGravity()
    {
        // Calculate gravity for smooth descent
        float previousYVelocity = -1 * _character._gravityMultiplier * _character._fallMultiplier;
        float currentVelocity = previousYVelocity + gravity * Time.deltaTime;
        float newYVelocity = (previousYVelocity + currentVelocity) * 0.5f;
        gravity = newYVelocity;
    }

    public void Move(float moveSpeed)
    {
        // Move the character in the air based on input
        if (!_character.OnGround())
        {
            Vector3 targetDirection = (new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))).normalized;
            Vector3 targetVelocity = targetDirection * moveSpeed;
            _character._moveDir = targetVelocity;
            _character._rigidBody.velocity += targetVelocity;
        }
    }

    private void ApplyCustomGravity()
    {
        // Apply custom gravity while jumping or falling
        if (_character._rigidBody.velocity.y >= 0)
        {
            _character._rigidBody.AddForce(Vector3.up * (gravity * _character._jumpMultiplier) * Time.deltaTime, ForceMode.Acceleration);
        }
        else
        {
            _character._rigidBody.AddForce(Vector3.up * (gravity * _character._fallMultiplier) * Time.deltaTime, ForceMode.Acceleration);
        }
    }

    private void ApplyDefaultGravity()
    {
        // Apply default gravity while falling
        _character._rigidBody.AddForce(Vector3.up * (gravity * _character._fallMultiplier) * Time.deltaTime, ForceMode.Acceleration);
    }

    public override void ExitState()
    {
        // Mark as jumped
        isJumped = true;
        _character._particles._stompParticles.transform.parent = null;
        _character._particles._stompParticles.transform.position = _character.transform.position;
        _character._particles._stompParticles.Play();

        // Reset velocity and log exit message
        _character._rigidBody.velocity = Vector3.zero;
        Debug.Log("Exited JumpState");
    }
}
