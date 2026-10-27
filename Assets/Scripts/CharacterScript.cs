// Utilized these videos: https://www.youtube.com/watch?v=KnAh1kLp2E0, https://www.youtube.com/watch?v=E-vIMamyORg
// And this documentation: https://docs.unity3d.com/6000.6/Documentation/ScriptReference/CharacterController.Move.html
// Additionally utilized LLMs to fix an issue between the camera and player movement: https://claude.ai/share/6da8c5c4-ca8f-40db-ae6b-23cbbf3852e1

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Pool;
using UnityEngine.UI;

public class CharacterScript : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] ThirdPersonCamScript cam;


    [Header("Debug")]
    [SerializeField] TextMeshProUGUI debugText;
    [SerializeField] TextMeshProUGUI debugText2;
    [SerializeField] Slider boostGauge;

    [Header("Bullet Tracer")]
    [SerializeField] Transform orientation;
    [SerializeField] LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] Material trailMaterial;
    [SerializeField] AnimationCurve widthCurve;
    [SerializeField] Gradient colorGradient;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction attackAction;
    InputAction boostAction;
    Rigidbody rigidbod;

    [Header("Important Values")]
    [SerializeField] float movementSpeed = 2f;
    public float displayedSpeed = 0;
    float targetMovementSpeed;
    [SerializeField] float boostSpeed = 20;

    [Tooltip("In seconds")]
    [SerializeField] float boostDuration = 1;
    bool boostActive = false;
    [SerializeField] float currentBoostGauge;
    float maxBoostGauge = 100;
    [SerializeField] float jumpHeight = 2f;

    float gravity = -9.81f;

    Vector3 playerVelocity;
    Vector3 boostVelocity;

    public Vector3 move;


    ObjectPool<TrailRenderer> tracerPool; // pool for the bullet tracers


    [SerializeField] Transform debugTransform;

    [SerializeField] Image reticle;
    [SerializeField] ReticleScript lockonReticle;


    bool haveTarget;

    void Awake()
    {
        rigidbod = gameObject.GetComponent<Rigidbody>();
        rigidbod.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked; //Locks camera to center of screen.\
        tracerPool = new ObjectPool<TrailRenderer>(
            createFunc: createBulletTracer,
            defaultCapacity: 10,
            maxSize: 150
            );
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        attackAction = InputSystem.actions.FindAction("Attack");
        boostAction = InputSystem.actions.FindAction("Boost");

        targetMovementSpeed = movementSpeed;

        boostGauge.maxValue = maxBoostGauge;
        currentBoostGauge = maxBoostGauge;
        haveTarget = true; //debug

        checkForSettings();
    }

    // Update is called once per frame
    void Update()
    {
        workBoostGauge();
        Aim();

        debugText.text = $"Movement Speed: {displayedSpeed:F1}";
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

        if (boostAction.WasPressedThisFrame())
        {
            if (!boostActive)
            {
                tryConsumeBoost();
            }
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

        updateDisplayedSpeed(moveDirection * movementSpeed);

        if (!IsGrounded())
        {
            playerVelocity.y += gravity * Time.deltaTime;
        }

        if (boostActive)
        {
            rigidbod.MovePosition(rigidbod.position + boostVelocity * Time.deltaTime);
        }
        else
        {
            Vector3 velocity = moveDirection.normalized * movementSpeed + Vector3.up * playerVelocity.y;
            rigidbod.MovePosition(rigidbod.position + velocity * Time.deltaTime);
        }
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

    Vector3 getBoostDirection()
    {
        Vector3 boostDirection = orientation.forward * move.z + orientation.right * move.x;

        Vector3 boostVelocity = boostDirection.normalized * boostSpeed
        ;
        return boostVelocity;
    }

    void Aim()
    {
        Vector3 target = Camera.main.WorldToScreenPoint(debugTransform.position);
        Vector2 halfReticleArea = new Vector2(reticle.rectTransform.rect.width / 2, reticle.rectTransform.rect.height / 2);
        Vector2 centerPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        if (Vector2.Distance(target, centerPoint) <= halfReticleArea.x && Vector2.Distance(target, centerPoint) <= halfReticleArea.y && target.z > -1)
        {
            
            debugText2.text = $"target in sight";
            Debug.Log($"{debugTransform.name}");
            haveTarget = true;
            lockonReticle.setReticlePosition(target);
        }
        else
        {
            debugText2.text = $"no target";
            haveTarget = false;
            lockonReticle.hideReticle();
        }
        Debug.Log($"Target pos: {target.x}, {target.y}");
    }

    void Shoot()
    {
        Ray ray;
        if (haveTarget)
        {
            StartCoroutine(showTracer(transform.position, debugTransform.position));
            Debug.Log("shooting target");
            if (Physics.Raycast(transform.position, debugTransform.position, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
            {
            }
        }
        else
        {
            Debug.Log("shooting");
            ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
            {
                Debug.Log("hit");
                StartCoroutine(showTracer(transform.position, raycastHit.point));
            }
            else
            {
                Debug.Log("miss");
                StartCoroutine(showTracer(transform.position, transform.position + cam.getViewDirection() * 999));
            }
        }


    }

    TrailRenderer createBulletTracer()
    {
        GameObject instance = new GameObject("Tracer");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = colorGradient;
        trail.material = trailMaterial;
        trail.widthCurve = widthCurve;
        trail.time = 0.15f;
        trail.minVertexDistance = .1f;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }

    IEnumerator showTracer(Vector3 startPoint, Vector3 endPoint)
    {
        TrailRenderer tracer = tracerPool.Get();
        tracer.gameObject.SetActive(true);
        tracer.transform.position = startPoint;
        yield return null;

        tracer.emitting = true;
        float distance = Vector3.Distance(startPoint, endPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            tracer.transform.position = Vector3.Lerp(startPoint, endPoint, Mathf.Clamp01(1 - (remainingDistance / distance)));

            remainingDistance -= 100f * Time.deltaTime;

            yield return null;
        }

        tracer.transform.position = endPoint;

        yield return new WaitForSeconds(tracer.time);
        yield return null;
        tracer.emitting = false;
        tracer.gameObject.SetActive(false);
        tracerPool.Release(tracer);
    }

    public void SpeedBuff(float inc)
    {
        movementSpeed += inc;
        targetMovementSpeed += inc;
    }


    IEnumerator ActivateBoost()
    {
        Debug.Log("boosting");

        boostActive = true;

        float saveOriginalSpeed = targetMovementSpeed;
        boostVelocity = getBoostDirection();
        movementSpeed = boostSpeed;
        currentBoostGauge -= 30;
        yield return new WaitForSeconds(boostDuration);
        boostActive = false;

        float boostDecay = 1f;
        while (movementSpeed != saveOriginalSpeed)
        {
            movementSpeed -= boostDecay;
            yield return null;
        }

        movementSpeed = saveOriginalSpeed;
    }

    void workBoostGauge()
    {
        if (currentBoostGauge < maxBoostGauge)
        {
            currentBoostGauge += 0.15f;
        }

        boostGauge.value = currentBoostGauge;
    }

    void tryConsumeBoost()
    {
        if (currentBoostGauge >= 30)
        {
            StartCoroutine(ActivateBoost());
        }
    }

    void updateDisplayedSpeed(Vector3 velocity)
    {

        if (velocity == Vector3.zero)
        {
            displayedSpeed = 0f;
        }
        else
        {
            displayedSpeed = movementSpeed;
        }
    }

    void checkForSettings()
    {
        if (SettingsManager.Instance.infiniteBoost)
        {
            maxBoostGauge = 99999f;
            currentBoostGauge = maxBoostGauge;
        }
       
    }
}
