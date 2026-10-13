using Unity.VectorGraphics;
using UnityEngine;

public class DestroyerScript : MonoBehaviour
{

    public GameObject[] ballObjs;

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
        ballObjs = GameObject.FindGameObjectsWithTag("Ball");

        foreach(GameObject ball in ballObjs)
        {
            Destroy(ball);
        }
    }
}
