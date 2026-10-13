using UnityEngine;

public class SpeedUpColliderScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<CharacterScript>().SpeedBuff(5);
            Debug.Log("movementspeed: " + other.gameObject.GetComponent<CharacterScript>());
        }
    }
}
