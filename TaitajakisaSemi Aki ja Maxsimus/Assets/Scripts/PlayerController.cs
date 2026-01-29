using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Definitions")]
    [SerializeField]private float playerHeight = 1.8f; //Metres
    [SerializeField] private float playerHeightCrouching = 1.2f;
    [SerializeField] private float playerCRadius = 0.3f;

    [Header("")]
    [SerializeField] private CapsuleCollider playerCollider;
    [SerializeField] private GameObject Head;
    [SerializeField] private Rigidbody rb;

    [Header("Materials")]
    [SerializeField] private PhysicsMaterial PhyMaterial; //Low Friction For Sliding

    [Header("Player Definitions")]
    [SerializeField] private float movingSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    public float MouseSensitivity = 2f;
    [Header("Jumping")]
    [SerializeField] private bool allowJumping = true;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] Animator leganimator;
    [SerializeField] Collider legcollider;
    [Header("Crouching")]
    [SerializeField] private bool allowCrouching = true;
    [SerializeField] private float crouchingSpeed = 2f;
    [Header("Camera")]
    [SerializeField] private float cameraSmooth = 30f;
    [SerializeField] private float maxCameraAngle;
    [SerializeField] private float minCameraAngle;
    private float cameraAngleX;
    private float cameraAngleY;



    //<============> Privates and Variables not shown in editor <============>

    //Inputs
    Vector2 movement;
    Vector2 look;
    bool crouch;
    bool jump;
    bool walk;

    //Is something

    bool grounded;
    bool roof;
    bool crouched;
    int jumpcounter;
    //Vector3's

    private Vector3 originalCenter;
    private Vector3 offset = new Vector3(0f, 0f, 0f);
    private Vector3 rayRoofOffset = Vector3.zero;
    private Vector3 rayGroundOffset = Vector3.zero;

    private Quaternion cachedYawRotation;

    //OtherShit
    private float playerCurrentHeight;

    //Input actions
    [SerializeField] private InputActionAsset Assets;

    private InputAction _move;
    private InputAction _look;
    private InputAction _crouch;
    private InputAction _jump;
    private InputAction _walk;
    private InputAction _LegJump;

    private void OnEnable()
    {
        _move = Assets.FindAction("Player/Move");
        _look = Assets.FindAction("Player/Look");
        _crouch = Assets.FindAction("Player/Crouch");
        _jump = Assets.FindAction("Player/Jump");
        _walk = Assets.FindAction("Player/Walk");
        _LegJump = Assets.FindAction("Player/LegJump");

        _move.started += OnMove;
        _move.performed += OnMove;
        _move.canceled += OnMove;
        _move.Enable();

        _look.started += OnLook;
        _look.performed += OnLook;
        _look.canceled += OnLook;
        _look.Enable();

        _crouch.started += OnCrouch;
        _crouch.performed += OnCrouch;
        _crouch.canceled += OnCrouch;
        _crouch.Enable();

        _jump.started += OnJump;
        _jump.performed += OnJump;
        _jump.canceled += OnJump;
        _jump.Enable();

        _walk.started += OnWalk;
        _walk.performed += OnWalk;
        _walk.canceled += OnWalk;
        _walk.Enable();

        _LegJump.started += OnLegJump;
        _LegJump.performed += OnLegJump;
        _LegJump.canceled += OnLegJump;
        _LegJump.Enable();


    }
    private void OnDisable()
    {
        _move.started -= OnMove;
        _move.performed -= OnMove;
        _move.canceled -= OnMove;
        _move.Disable();

        _look.started -= OnLook;
        _look.performed -= OnLook;
        _look.canceled -= OnLook;
        _look.Disable();

        _crouch.started -= OnCrouch;
        _crouch.performed -= OnCrouch;
        _crouch.canceled -= OnCrouch;
        _crouch.Disable();

        _jump.started -= OnJump;
        _jump.performed -= OnJump;
        _jump.canceled -= OnJump;
        _jump.Disable();

        _walk.started -= OnWalk;
        _walk.performed -= OnWalk;
        _walk.canceled -= OnWalk;
        _walk.Disable();

        _LegJump.started -= OnLegJump;
        _LegJump.performed -= OnLegJump;
        _LegJump.canceled -= OnLegJump;
        _LegJump.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }
    private void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }
    private void OnCrouch(InputAction.CallbackContext context)
    {
        crouch = context.ReadValueAsButton();
    }
    private void OnJump(InputAction.CallbackContext context)
    {
        jump = context.ReadValueAsButton();
    }
    private void OnWalk(InputAction.CallbackContext context)
    {
        walk = context.ReadValueAsButton();
    }

    private void OnLegJump(InputAction.CallbackContext context)
    {
        bool LegJump = context.ReadValueAsButton();
        if (LegJump)
        {
            legcollider.enabled = true;
            leganimator.SetTrigger("Kick");
        }
       
        
    }

    //Update Cycles
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        originalCenter = playerCollider.center;
        playerCollider.radius = playerCRadius;
        playerCurrentHeight = playerHeight;
        playerCollider.height = playerHeight;

        playerCollider.material = PhyMaterial;
    }

    private void FixedUpdate()
    {
        if (allowJumping) { Jumping(); }
        Moving();
        if (allowCrouching) { Crouching(); }
    }
    private void Update()
    {
        Looking();

        
    }
    void LateUpdate()
    {
        Vector3 targetPosition = transform.position + offset;
        Head.transform.position = Vector3.Lerp(
            Head.transform.position,
            targetPosition,
            1f - Mathf.Exp(-cameraSmooth * Time.deltaTime)
        );
    }
    private void Moving()
    {
        float newMovingSpeed;

        if ((crouched && grounded) || walk)
        {
            newMovingSpeed = movingSpeed/2;
        }
        else
        {
            newMovingSpeed = movingSpeed;
        }

        Vector3 moveDirection = new Vector3(movement.x, 0f, movement.y);
        moveDirection = cachedYawRotation * moveDirection;

        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (grounded)
        {
            horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, moveDirection * newMovingSpeed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            horizontalVelocity += moveDirection * (acceleration * 0.2f * Time.fixedDeltaTime);
            horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, newMovingSpeed);
        }

        rb.linearVelocity = horizontalVelocity + Vector3.up * rb.linearVelocity.y;
    }

    private void Looking()
    {
        cameraAngleX += look.x * MouseSensitivity * Time.timeScale;
        cameraAngleY -= look.y * MouseSensitivity * Time.timeScale;
        cameraAngleY = Mathf.Clamp(cameraAngleY, minCameraAngle, maxCameraAngle);

        cachedYawRotation = Quaternion.Euler(0f, cameraAngleX, 0f);
        transform.rotation = cachedYawRotation;

        transform.rotation = Quaternion.Euler(0f, cameraAngleX, 0f);
        Head.transform.rotation = Quaternion.Euler(cameraAngleY, cameraAngleX, 0f);
    }

    private void Crouching()
    {
        //VERY FUCKING BROKEN RN, will be fixed.
        RaycastHit hit;
        rayRoofOffset = new Vector3(transform.position.x, transform.position.y - playerHeight / 2f + playerHeightCrouching - playerCRadius, transform.position.z);
        roof = Physics.SphereCast(rayRoofOffset, playerCRadius - 0.05f , Vector3.up,out hit, 0.4f,LayerMask.GetMask("Default", "Ground", "Roof", "Vaultable"));

        float difference = (playerHeight - playerCollider.height) * 0.5f;

        playerCurrentHeight = Mathf.Clamp(playerCurrentHeight, playerHeightCrouching, playerHeight);

        if (crouch == true || roof == true)
        {
            crouched = true;
            playerCollider.height = playerCurrentHeight;
            playerCollider.center = originalCenter + Vector3.down * difference;
            playerCurrentHeight -= crouchingSpeed * Time.deltaTime;

            offset = new Vector3(0f, playerCurrentHeight - 1.15f, 0f);
        }   
        if (crouch == false && roof == false)
        {
            crouched = false;
            playerCollider.height = playerCurrentHeight;
            playerCollider.center = originalCenter + Vector3.down * difference;
            playerCurrentHeight += crouchingSpeed * Time.deltaTime;

            offset = new Vector3(0f, playerCurrentHeight - 1.15f, 0f);
        }
    }

    private void Jumping()
    {
        //Check Ground
        RaycastHit hit;
        rayGroundOffset = new Vector3(transform.position.x, transform.position.y - playerHeight / 2f + playerCRadius, transform.position.z);
        grounded = Physics.SphereCast(rayGroundOffset, playerCRadius - 0.01f, Vector3.down, out hit, 0.05f, LayerMask.GetMask("Default", "Ground", "Roof", "Vaultable"));
        //========//

        //if (grounded == false) { jump = false; }
        
        if (jump && grounded)
        {
            Vector3 velocity = rb.linearVelocity;
            velocity.y = jumpForce;
            rb.linearVelocity = velocity;
            jump = false;
        }
    }

    public void OnHit(bool hit, Collider other, Collider hitbox)
    {
        if (!hit) return;

        /*Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;
        */
        Vector3 direction;

        //direction = transform.TransformDirection(direction);
        Debug.Log("jump using leg");
        
        rb.AddForce(other.transform.position * jumpForce * 10, ForceMode.Impulse);
        hitbox.enabled = false;
    }
}



