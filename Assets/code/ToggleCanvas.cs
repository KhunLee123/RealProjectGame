using UnityEngine;
using TMPro;

public class ToggleCanvas : MonoBehaviour
{
    [Header("Window Animation Settings")]
    public Transform terminalWindow; 
    public float popSpeed = 15f; 

    [Header("Instant UI Panels")]
    public Transform extraPanel; 

    [Header("Elements to HIDE on Submit")]
    public GameObject inputArea; 
    public GameObject extraHideElement1; 
    public GameObject extraHideElement2; 

    [Header("Elements to SHOW on Submit")]
    public GameObject outputArea; 

    [Header("Hide Object when open Terminal")]
    public GameObject objectToHideWhenOpen; 

    [Header("Sound Effects")]
    public AudioSource audioSource; 
    public AudioClip openSound;     
    public AudioClip closeSound;    

    private bool isOpen = false;

    void Start()
    {
        if (terminalWindow != null) terminalWindow.localScale = Vector3.zero;
        if (extraPanel != null) extraPanel.localScale = Vector3.zero;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isOpen)
        {
            OpenTerminal();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isOpen)
        {
            CloseTerminal();
        }

        Vector3 targetScale = isOpen ? Vector3.one : Vector3.zero;

        if (terminalWindow != null)
            terminalWindow.localScale = Vector3.Lerp(terminalWindow.localScale, targetScale, Time.unscaledDeltaTime * popSpeed);

        if (extraPanel != null)
            extraPanel.localScale = targetScale;
    }

    public void OpenTerminal()
    {
        isOpen = true;
        Time.timeScale = 0f; 
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;

        if (audioSource != null && openSound != null)
        {
            audioSource.PlayOneShot(openSound);
        }

        if (inputArea != null) inputArea.SetActive(true);
        if (extraHideElement1 != null) extraHideElement1.SetActive(true);
        if (extraHideElement2 != null) extraHideElement2.SetActive(true);
        if (outputArea != null) outputArea.SetActive(false);

        if (objectToHideWhenOpen != null) objectToHideWhenOpen.SetActive(false);

        if (inputArea != null)
        {
            TMP_InputField input = inputArea.GetComponentInChildren<TMP_InputField>();
            if (input != null)
            {
                input.Select();
                input.ActivateInputField();
            }
        }
    }

    public void CloseTerminal()
    {
        isOpen = false;
        Time.timeScale = 1f; 
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;

        if (audioSource != null && closeSound != null)
        {
            audioSource.PlayOneShot(closeSound);
        }

        if (objectToHideWhenOpen != null) objectToHideWhenOpen.SetActive(true);
    }

    public void ShowOutputState()
    {
        if (inputArea != null) inputArea.SetActive(false);
        if (extraHideElement1 != null) extraHideElement1.SetActive(false);
        if (extraHideElement2 != null) extraHideElement2.SetActive(false);
        if (outputArea != null) outputArea.SetActive(true);
    }
}