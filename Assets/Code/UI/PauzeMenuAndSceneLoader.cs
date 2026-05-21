using UnityEngine;
using UnityEngine.SceneManagement;

public class PauzeMenuAndSceneLoader : MonoBehaviour
{
    //pauze menu canvas drag inspector
    public GameObject menuCanvas;

    //pauze menu status open/closed default false=closed
    private bool isOpen = false;
    //current scene index to reload the same scene when restarting not really used but just in case
    private int currentSceneIndex;

    private void Start()
    {
        //get the current scene index
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        //make sure the menu is closed and time is normal at the start
        Time.timeScale = 1f;
        menuCanvas.SetActive(false);
    }

    private void Update()
    {
        //check for escape key press to toggle the pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauzeMenu();
        }
    }
    //toggle the pauze menu and freeze/unfreeze time
    public void PauzeMenu()
    {
        isOpen = !isOpen;
        menuCanvas.SetActive(isOpen);

        if (isOpen)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
    //reloads the current scene not used
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(currentSceneIndex);
    }

    //loads back to game scene
    public void Respawn()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
    //start menu
    public void StartMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    //settings menu not implemented yet
    public void SettingsMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(5);
    }
    //levels menu not implemented yet
    public void LevelsMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(4);
    }

    //toggle the pauze menu
    public void Pauzetoggle()
    {
        PauzeMenu();
    }
    //guide menu
    public void GuideMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(3);
    }
    //controls menu
    public void ControlsMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(2);
    }

    //quit the application
    public void quit()
    {
        Application.Quit();
    }
}