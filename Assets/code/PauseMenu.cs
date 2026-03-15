using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public MonoBehaviour playerController; 
    public ToggleCanvas terminalController; 
    
    private bool isPaused = false;

    void Start()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1f; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (terminalController != null && terminalController.isOpen == true)
            {
                terminalController.CloseTerminal(); 
                
                return; 
            }

            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        settingsPanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (playerController != null) playerController.enabled = false; 
    }

    public void ResumeGame()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (playerController != null) playerController.enabled = true; 
    }
}