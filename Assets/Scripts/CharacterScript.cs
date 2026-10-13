// Utilized this video: https://www.youtube.com/watch?v=KnAh1kLp2E0
// And this documentation: https://docs.unity3d.com/6000.6/Documentation/ScriptReference/CharacterController.Move.html

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class CharacterScript : MonoBehaviour
{
        [SerializeField] Transform playerCamera;
        InputAction moveAction;   
        InputAction jumpAction;
        Rigidbody rigidbod;

        [SerializeField] float movementSpeed = 2f;
        [SerializeField] float jumpHeight = 2f;

        float gravity = -9.81f;

        Vector3 playerVelocity;

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

    }

    // Update is called once per frame
    void Update()
    {
        Look();
        CamPosition();
        
        Debug.Log("is jump pressed: " + jumpAction.IsPressed());
        Debug.Log("is player grounded: " + IsGrounded());

        if (jumpAction.IsPressed()){
            Jump();
        }

    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector2 movementValue = moveAction.ReadValue<Vector2>();

        Vector3 move = new Vector3(movementValue.x, 0, movementValue.y);
        move = Vector3.ClampMagnitude(move, 1f);
        //Debug.Log(move);

        Vector3 moveDirection = transform.forward * move.z + transform.right * move.x;

        Vector3 velocity = moveDirection * movementSpeed;
        //velocity.y = rigidbod.linearVelocity.y;
        rigidbod.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    void Jump()
    {
        if (IsGrounded())
        {
            rigidbod.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1f);
    }

    void Look()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        yaw += mouseDelta.x * 0.1f;
        pitch -= mouseDelta.y * 0.1f;

        pitch = Mathf.Clamp(pitch, -30f, 50f);

        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

    }

    void CamPosition()
    {
        Quaternion camRotation = Quaternion.Euler(pitch, yaw, 0f);

        playerCamera.rotation = camRotation;
    }
}
