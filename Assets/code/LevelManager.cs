using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetFloat("SavedMusicVol", 1f);
        PlayerPrefs.SetFloat("SavedSFXVol", 1f);
        PlayerPrefs.Save(); 
    }

    void Update() 
    {
        Cursor.visible = true; 
        Cursor.lockState = CursorLockMode.None; 
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}