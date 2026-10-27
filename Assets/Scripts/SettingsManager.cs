using UnityEngine;
public class SettingsManager : MonoBehaviour
{
    public bool infiniteBoost = false;
    public bool disableUI = false;

    private static SettingsManager _instance; // make a static private variable of the component data type
    public static SettingsManager Instance { get { return _instance; } } // make a public way to access the private variable
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

    public void performDisUI()
    {
        disableUI = true;
    }

    public void performinfBoost()
    {
        infiniteBoost = true;
    }
}   
