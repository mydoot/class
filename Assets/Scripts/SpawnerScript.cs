using UnityEngine;

public class SpawnerScript : MonoBehaviour
{

    [SerializeField] GameObject ball;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject newBall = Instantiate(ball, other.gameObject.transform.position, Quaternion.identity);
    }
}
