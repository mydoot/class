using UnityEngine;

public class SpawnerScript : MonoBehaviour
{


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
        GameObject newObj = Instantiate(GameManager.Instance.getRandObj(), other.gameObject.transform.position, Quaternion.identity);
    }
}
