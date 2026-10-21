using UnityEngine;
public class GameManager : MonoBehaviour
{
    public GameObject ball;
    public GameObject cube;
    public GameObject cylinder;

    private static GameManager _instance; // make a static private variable of the component data type
    public static GameManager Instance { get { return _instance; } } // make a public way to access the private variable
    private void Awake()
    {
        if (_instance != null && _instance != this)
        { // if there is already a value assigned to the private variable and its not this, destroy this
            Destroy(this.gameObject);
        }
        else
        { // if there is no value assigned to the private variable, assign this as the reference
            _instance = this;

        }
    }

    float getRandomNum(float min, float max)
    {
        return Random.Range(min, max);
    }

    public GameObject getRandObj()
    {
        float determine = getRandomNum(0f, 3f) + getRandomNum(0, 3f);

        GameObject newSpawnedObj = ball;

        if (determine >= 3 && determine <= 5)
        {
            newSpawnedObj = ball;
        }
        else if (determine == 6)
        {
            newSpawnedObj = cube;
        }
        else if (determine <= 2)
        {
            newSpawnedObj = cylinder;
        }

        return newSpawnedObj;
    }





}   
