using UnityEngine;
using UnityEngine.UI;

public class ReticleScript : MonoBehaviour
{
    Image targetLockedReticle;
    //bool hasTarget = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetLockedReticle = GetComponent<Image>();
        hideReticle();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setReticlePosition(Vector3 pos)
    {
        Color color = targetLockedReticle.color;
        color.a = 1f;
        targetLockedReticle.color = color;
        targetLockedReticle.rectTransform.position = pos;
        
    }
    

    public void hideReticle()
    {
        Color color = targetLockedReticle.color;
        color.a = 0f;
        targetLockedReticle.color = color;
    }
}
