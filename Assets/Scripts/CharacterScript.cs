// Utilized these videos: https://www.youtube.com/watch?v=KnAh1kLp2E0, https://www.youtube.com/watch?v=E-vIMamyORg
// And this documentation: https://docs.unity3d.com/6000.6/Documentation/ScriptReference/CharacterController.Move.html
// Additionally utilized LLMs to fix an issue between the camera and player movement: https://claude.ai/share/6da8c5c4-ca8f-40db-ae6b-23cbbf3852e1

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterScript : MonoBehaviour
{
    [SerializeField] ThirdPersonCamScript cam;

    [SerializeField] Transform orientation;
    [SerializeField] LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] Material trailMaterial;
    [SerializeField] AnimationCurve widthCurve;
    [SerializeField] Gradient colorGradient;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction attackAction;
    Rigidbody rigidbod;

    public float movementSpeed = 2f;
    [SerializeField] float jumpHeight = 2f;

    float gravity = -9.81f;

    Vector3 playerVelocity;

    public Vector3 move;

    float yaw;
    float pitch;

    void Awake()
    {
        rigidbod = gameObject.GetComponent<Rigidbody>();
        rigidbod.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked; //Locks camera to center of screen.
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        attackAction = InputSystem.actions.FindAction("Attack");

    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("is jump pressed: " + jumpAction.IsPressed());
        //Debug.Log("is player grounded: " + IsGrounded());

        if (jumpAction.IsPressed())
        {
            Jump();
        }

        if (attackAction.IsPressed())
        {
            Shoot();
        }

    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector2 movementValue = moveAction.ReadValue<Vector2>();

        move = new Vector3(movementValue.x, 0, movementValue.y);
        move = Vector3.ClampMagnitude(move, 1f);

        Vector3 moveDirection = orientation.forward * move.z + orientation.right * move.x;

        Vector3 velocity = moveDirection.normalized * movementSpeed + Vector3.up * playerVelocity.y;    

        if (!IsGrounded())
        {
            playerVelocity.y += gravity * Time.deltaTime;
        }

        rigidbod.MovePosition(rigidbod.position + velocity * Time.deltaTime);
    }

    void Jump()
    {
        if (IsGrounded())
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            //Debug.Log("jump");
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 5f);
    }

    void Shoot()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            Debug.Log("hit");
            StartCoroutine(showTracer(transform.position, raycastHit.point, raycastHit));
        }
        else
        {
            Debug.Log("miss");
             StartCoroutine(showTracer(transform.position, transform.position + cam.getViewDirection() * 999, raycastHit));
        }
    }

    TrailRenderer createBulletTracer()
    {
        GameObject instance = new GameObject("Tracer");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = colorGradient;
        trail.material = trailMaterial;
        trail.widthCurve = widthCurve;
        trail.time = 0.5f;
        trail.minVertexDistance = .1f;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }

    IEnumerator showTracer(Vector3 startPoint, Vector3 endPoint, RaycastHit hit)
    {
        TrailRenderer tracer = createBulletTracer();
        tracer.transform.position = startPoint;
        yield return null;

        tracer.emitting = true;
        float distance = Vector3.Distance(startPoint, endPoint);
        float remainingDistance = distance;

        while(remainingDistance > 0)
        {
            tracer.transform.position = Vector3.Lerp(startPoint, endPoint, Mathf.Clamp01(1 - (remainingDistance/distance)));

            remainingDistance -= 100f * Time.deltaTime;

            yield return null;
        }

        tracer.transform.position = endPoint;

        yield return new WaitForSeconds(1);
        yield return null;
        tracer.emitting = false;
        Destroy(tracer);
    }

    public void SpeedBuff(float inc)
    {
        movementSpeed += inc;
    }
}
