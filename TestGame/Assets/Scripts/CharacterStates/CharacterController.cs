using UnityEngine;

public class CharacterController : MonoBehaviour
{
    ///PARAMETERS///
    //Movement
    [Header("Movement")]
    public bool _isRunning;
    public float _run;
    public float _walk;
    [HideInInspector]
    public Vector3 _moveDir;
    [Header("Jump")]
    [Tooltip("Determines how high character will jump")]
    public float _playerJumpHeight;
    public float _maxJumpTime;
    [Header("Jump Gravity")]
    [Tooltip("Gravity while jumping up (Lesser the value slowly character wil jump)")]
    public float _jumpMultiplier;
    [Tooltip("Gravity while falling down after jump (Higher the value faster character will fall)")]
    public float _fallMultiplier;
    public float _gravityMultiplier;

    //Detection
    [Header("Parameter Checks")]
    public float _sphereCastDistance;
    public float _rayCastDistance;
    private RaycastHit _sphereCastHit;
    public float forwardProbeDistance;
    public RaycastHit _hit;
    public RaycastHit _forwardHit;
    public LayerMask _groundMask;

    ///REFERENCE///
    [HideInInspector]
    public Rigidbody _rigidBody;

    //Concrete Instances
    public CharacterState _currentState;
    public StateFactory states;
    [Header("References")]
    public CapsuleCollider _col;
    public Transform _playerCamControllerTransform;
    [System.Serializable]
    public struct Particles
    {
        public ParticleSystem _dustTrialParticles, _stompParticles;
    }
    public Particles _particles;

    private void Awake()
    {
        states = new StateFactory(this);
    }

    void Start()
    {
        _rigidBody = transform.GetComponent<Rigidbody>();
        _currentState = states.IdleState();
        _currentState.EnterState();
    }

    void Update()
    {
        IsMoving();
        DefaultMoveType();
        _currentState.Update();
    }

    private void FixedUpdate()
    {
        _currentState.FixedUpdate();
        OnGround();
        OnSlope();
        SlopeCheck();
    }

    public bool OnGround()
    {
        if (Physics.SphereCast(transform.position, _col.radius - 0.1f, -transform.up, out _sphereCastHit, _sphereCastDistance, _groundMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool OnSlope()
    {
        bool onSlope = false;
        if (Physics.Raycast(transform.position + transform.GetComponent<CapsuleCollider>().center, Vector3.down, out _hit, _rayCastDistance, _groundMask))
        {
            if (_hit.normal == Vector3.up)
            {
                onSlope = true;
            }
            else
            {
                onSlope = false;
            }
        }
        return onSlope;
    }

    private void SlopeCheck()
    {
        Physics.Raycast(transform.position + (transform.forward * forwardProbeDistance) + transform.GetComponent<CapsuleCollider>().center, Vector3.down, out _forwardHit, _rayCastDistance, _groundMask);
    }


    public bool IsMoving()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void DefaultMoveType()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _isRunning = !_isRunning;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position + (-transform.up * _sphereCastDistance), _col.radius - 0.1f);
        Gizmos.DrawLine(transform.position, transform.position + (-transform.up * _rayCastDistance));
        Gizmos.DrawLine(transform.position + transform.forward * forwardProbeDistance, transform.position + transform.forward * forwardProbeDistance + (-transform.up * _rayCastDistance));
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(_hit.point, _hit.normal);
        Gizmos.DrawRay(_forwardHit.point, _forwardHit.normal);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + _moveDir*10);
        Gizmos.color = Color.green; 
        if (_rigidBody != null)
            Gizmos.DrawLine(transform.position, transform.position + _rigidBody.velocity);
    }
}
