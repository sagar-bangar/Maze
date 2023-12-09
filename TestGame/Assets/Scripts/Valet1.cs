using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valet1 : MonoBehaviour
{
    Rigidbody r;
    public CapsuleCollider capsule;
    //public Animator animator;
    public GameObject maleModel;
    [SerializeField] float stepsSinceLastGrounded = 0;

    public float maleModelCapsuleOffset;
    public float maleModelRigidbodyOffset;
    public float sphereCheckNumber;

    public float raycastDistance;
    public bool _rayDidHit;

    public Vector3 DownDir;
    public float RideSpringDamper;
    public float RideSpringStrength;
    public float RideHeight;
    RaycastHit _rayHit;

    //standard
    //Base Movement
    public float horizontalInput;
    public float verticalInput;
    public Vector3 moveDirection;
    public Vector3 moveDirectionRaw;
    public int rotDegPerSecond = 720;
    public float speed;
    public Transform cameraTransform;


    //GroundChecking
    public bool isGroundedCloser;
    public Transform groundCheckTransform;
    public float groundCheckRadius = 0.25f;
    public LayerMask groundLayerMask;
    public float distanceToGround;

    //FloatChecking
    public bool isFloating;
    public float stepsDistanceToGroundGreater = 0.0f;
    public float stepsSinceLastJump = 0.0f;

    //jump
    public bool jump;
    //public ultimate_timer jumpTimer;
    [SerializeField] float jumpForce = 500f;
    [SerializeField] float snapAfterJumpLimit = 1.6f;

    // Start is called before the first frame update
    void Awake()
    {
        r = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        if (Physics.SphereCast(transform.position, sphereCheckNumber, -transform.up, out _rayHit, raycastDistance))
        {
            _rayDidHit = true;
        }
        else
        {
            _rayDidHit = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stepsSinceLastJump = 0;
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        GroundCheck();
        FloatCheck();



        Ray();


        moveDirection = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;
        moveDirection = (Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * moveDirection).normalized;
        moveDirectionRaw = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical")).normalized;
        moveDirectionRaw = (Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * moveDirectionRaw).normalized;



        if (jump == true)
        {
            raycastDistance = 0f;
            r.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jump = false;
        }

        if (distanceToGround <= snapAfterJumpLimit)
        {
            raycastDistance = 82.5f;
        }




        if (moveDirectionRaw != Vector3.zero && moveDirection != Vector3.zero)
        {
            //animator.SetBool("isMoving", true);
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection); //or moveDirectionRaw
            targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotDegPerSecond * Time.deltaTime);
            r.MoveRotation(targetRotation);
        }
        else
        {
            //animator.SetBool("isMoving", false);
        }

        Vector3 force = moveDirection * speed;
        r.AddForce(force, ForceMode.VelocityChange);


        GroundStopSlide();
        UpdateState();
    }

    void UpdateState()
    {
        stepsSinceLastJump += 1;

        stepsSinceLastGrounded += 1;


        if (distanceToGround > 1.9)
        {
            stepsDistanceToGroundGreater += 1;
        }
        else
        {
            stepsDistanceToGroundGreater = 0;
        }
    }

    void GroundStopSlide()
    {
        if (moveDirectionRaw == Vector3.zero)
        {
            Vector3 zeroMe = new Vector3(0, r.velocity.y, 0);
            r.velocity = zeroMe;
        }
    }

    void Ray()
    {
        if (_rayDidHit)
        {
            Vector3 vel = r.velocity;
            Vector3 rayDir = transform.TransformDirection(DownDir);

            Vector3 otherVel = Vector3.zero;
            Rigidbody hitBody = _rayHit.rigidbody;

            if (hitBody != null)
            {
                otherVel = hitBody.velocity;
            }

            float rayDirVel = Vector3.Dot(rayDir, vel);
            float otherDirVel = Vector3.Dot(rayDir, otherVel);

            float relVel = rayDirVel - otherDirVel;

            float x = _rayHit.distance - RideHeight;

            float springForce = (x * RideSpringStrength) - (relVel * RideSpringDamper);

            Debug.DrawLine(transform.position, transform.position + (rayDir * springForce), Color.yellow);

            r.AddForce(rayDir * springForce);

            if (hitBody != null)
            {
                hitBody.AddForceAtPosition(rayDir * -springForce, _rayHit.point);
            }
        }

    }
    public void GroundCheck()
    {
        isGroundedCloser = Physics.CheckSphere(groundCheckTransform.position, groundCheckRadius, groundLayerMask);

        RaycastHit groundHit = new RaycastHit();
        if (Physics.Raycast(transform.position, -Vector3.up, out groundHit))
        {
            distanceToGround = groundHit.distance;
        }

        maleModel.transform.position = new Vector3(capsule.transform.position.x, Mathf.Max(capsule.transform.position.y, 1.69346f), capsule.transform.position.z)
            + new Vector3(0, maleModelRigidbodyOffset, 0);


        if (isGroundedCloser)
        {
            stepsSinceLastGrounded = 0;
        }
    }

    public void FloatCheck()
    {

        if (stepsSinceLastJump > 60 && stepsDistanceToGroundGreater > 10)
        {
            r.AddForce(Vector3.down * 100f, ForceMode.Force);
            isFloating = true;
            print("Floating for too long- Adding force");
        }
    }

}
