using UnityEngine;

public class HideOnceOnTerminalOpen : MonoBehaviour
{
    [Header("Settings")]
    public GameObject objectToHide1;
    public GameObject objectToHide2;
    
    public Hint hintScript; 

    private bool hasTriggered = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !hasTriggered)
        {
            if (objectToHide1 != null) objectToHide1.SetActive(false);
            if (objectToHide2 != null) objectToHide2.SetActive(false);

            if (hintScript != null)
            {
                hintScript.ShowHint1();
            }

            hasTriggered = true;
            this.enabled = false;
        }
    }
}