using System.Linq.Expressions;
using UnityEditor.HardwareProfiles;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubeScript : MonoBehaviour
{
        InputAction stateAction;
        InputAction moveAction;   
        InputAction rotateAction;
        InputAction scaleAction;

    
        bool isActive = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stateAction = InputSystem.actions.FindAction("State");
        moveAction = InputSystem.actions.FindAction("Move");
        scaleAction = InputSystem.actions.FindAction("Scale");
        rotateAction = InputSystem.actions.FindAction("Rotate");
    }

    // Update is called once per frame
    void Update()
    {

        Movement();

        if (stateAction.IsPressed())
        {
            if (isActive)
            {
                DisableCubes();
            }
            else
            {
                EnableCubes();
            }
        }

        if (rotateAction.IsPressed())
        {
            Rotate();
        }

        if (scaleAction.IsPressed())
        {
           Scale();
        }
    }

    void EnableCubes(){
        isActive = true;
        foreach (Transform child in transform)
        {
         child.gameObject.SetActive(true);
        }
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    void DisableCubes(){
        isActive = false;
        foreach (Transform child in transform)
        {
         child.gameObject.SetActive(false);
        }
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    void Movement()
    {
        Vector2 movementValue = moveAction.ReadValue<Vector2>();

        transform.Translate(transform.up * movementValue);
        transform.Translate(transform.right * movementValue);
    }

    void Rotate()
    {
        gameObject.transform.Rotate(transform.localRotation.x, transform.localRotation.y + 1f, transform.localRotation.z);
    }

    void Scale()
    {
        gameObject.transform.localScale += new Vector3(0.01f,0.01f,0.01f);
    }
}
