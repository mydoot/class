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

        CharacterController charaController;
        Rigidbody rigidbod;

        [SerializeField] float movementSpeed = 2f;

        float yaw;
        float pitch;
    
    void Awake()
    {
        charaController = gameObject.GetComponent<CharacterController>(); 
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
        Debug.Log(move);

        Vector3 moveDirection = transform.forward * move.z + transform.right * move.x;

        Vector3 finalMovement = moveDirection * movementSpeed;
        charaController.Move(finalMovement * Time.deltaTime);
    }

    void Look()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        yaw += mouseDelta.x * 0.2f;
        pitch -= mouseDelta.y * 0.2f;

        pitch = Mathf.Clamp(pitch, -30f, 60f);

        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

    }

    void CamPosition()
    {
        Quaternion camRotation = Quaternion.Euler(pitch, yaw, 0f);

        playerCamera.rotation = camRotation;
    }
}
