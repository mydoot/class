// Used this video for the new camera: https://www.youtube.com/watch?v=UCwwn2q4Vys

using UnityEngine;

public class ThirdPersonCamScript : MonoBehaviour
{
    [SerializeField] Transform orientation;
    [SerializeField] Transform lookAt;
    [SerializeField] Transform player;
    [SerializeField] Rigidbody playerRigidBod;

    [SerializeField] CharacterScript playerCharacterScript;

    [SerializeField] float rotationSpeed = 10;
    
    void Update()
    {
        View();
    }

    void View()
    {
        Vector3 viewDirection = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDirection.normalized;

        Vector3 inputDirection = orientation.forward * playerCharacterScript.move.z + orientation.right * playerCharacterScript.move.x;
        

       /*  if (inputDirection != Vector3.zero)
        {
            player.forward = Vector3.Slerp(player.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
        } */

        /* Vector3 lockedLookAt = lookAt.position - new Vector3(transform.position.x, lookAt.position.y, transform.position.z);
        orientation.forward = lockedLookAt.normalized; */
    
        player.forward = viewDirection.normalized;
    }

    public Vector3 getViewDirection()
    {
        return player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
    }
}

