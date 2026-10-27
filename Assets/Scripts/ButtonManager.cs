using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] Image creditsPanel;
    [SerializeField] Image optionsPanel;

    bool optionsOn = false;
    bool creditsOn = false;

    public void showOptions()
    {
        if (!optionsOn)
        {
            optionsOn = true;
            optionsPanel.gameObject.SetActive(true);
        }
        else
        {
            optionsOn = false;
            optionsPanel.gameObject.SetActive(false);
        }
    }
    public void showCredits()
    {
        if (!creditsOn)
        {
            creditsOn = true;
            creditsPanel.gameObject.SetActive(true);
        }
        else
        {
            creditsOn = false;
            creditsPanel.gameObject.SetActive(false);
        }
    }
}

