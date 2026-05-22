//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class PauzeMenuAndSceneLoader : MonoBehaviour
//{
//    //pauze menu canvas drag inspector
//    public GameObject menuCanvas;

//    //pauze menu status open/closed default false=closed
//    private bool isOpen = false;
//    //current scene index to reload the same scene when restarting not really used but just in case
//    private int currentSceneIndex;

//    private void Start()
//    {
//        //get the current scene index
//        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

//        //make sure the menu is closed and time is normal at the start
//        Time.timeScale = 1f;
//        menuCanvas.SetActive(false);
//    }

//    private void Update()
//    {
//        //check for escape key press to toggle the pause menu
//        if (Input.GetKeyDown(KeyCode.Escape))
//        {
//            PauzeMenu();
//        }
//    }
//    //toggle the pauze menu and freeze/unfreeze time
//    public void PauzeMenu()
//    {
//        isOpen = !isOpen;
//        menuCanvas.SetActive(isOpen);

//        if (isOpen)
//        {
//            Time.timeScale = 0f;
//        }
//        else
//        {
//            Time.timeScale = 1f;
//        }
//    }
//    //reloads the current scene not used
//    public void Restart()
//    {
//        Time.timeScale = 1f;
//        SceneManager.LoadScene(currentSceneIndex);
//    }

//    //loads back to game scene
//    public void Respawn()
//    {
//        Time.timeScale = 1f;
//        SceneManager.LoadScene(1);
//    }
//    //start menu
//    public void StartMenu()
//    {
//        Time.timeScale = 1f;
//        SceneManager.LoadScene(0);
//    }
//    //settings menu not implemented yet
//    public void SettingsMenu()
//    {
//        Time.timeScale = 1f;
//        SceneManager.LoadScene(5);
//    }
//    //levels menu not implemented yet
//    public void LevelsMenu()
//    {
//        Time.timeScale = 1f;
//        SceneManager.LoadScene(4);
//    }

//    //toggle the pauze menu
//    public void Pauzetoggle()
//    {
//        PauzeMenu();
//    }
//    //guide menu
//    public void GuideMenu()
//    {
//        Time.timeScale = 1f;
//        SceneManager.LoadScene(3);
//    }
//    //controls menu
//    public void ControlsMenu()
//    {
//        Time.timeScale = 1f;
//        SceneManager.LoadScene(2);
//    }

//    //quit the application
//    public void quit()
//    {
//        Application.Quit();
//    }
//}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauzeMenuAndSceneLoader : MonoBehaviour
{
    [Header("Setup (Optional)")]
    [Tooltip("The script looks for a GameObject named 'PauseMenuCanvas' automatically if this is left empty!")]
    public GameObject menuCanvas;

    [Header("Arcade Navigation (Just drag your scene buttons here)")]
    public Button[] menuButtons;
    private int currentSelectionIndex = 0;
    
    private bool isOpen = false;
    private int currentSceneIndex;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Time.timeScale = 1f;

        // AUTOMATIC CANVAS FINDER
        if (menuCanvas == null)
        {
            menuCanvas = GameObject.Find("PauseMenuCanvas");
        }

        // Hide it safely at the start if it exists
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(false);
        }

        // SCENE RESTRICTION: Keep menu navigation awake immediately if we are NOT in gameplay levels (1-1 is index 1, 1-2 is index 7)
        if (currentSceneIndex != 1 && currentSceneIndex != 7)
        {
            isOpen = true;
            UpdateMenuSelectionVisuals();
        }
    }

    private void Update()
    {
        // SCENE RESTRICTION: Pressing 'W' now works to open/close the canvas in Level 1-1 (index 1) or level 1-2 (index 7)
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (currentSceneIndex == 1 || currentSceneIndex == 7)
            {
                PauzeMenu();
            }
        }

        // ARCADE MENU NAVIGATION
        if (isOpen && menuButtons != null && menuButtons.Length > 0)
        {
            // L-joy up = Up Arrow
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentSelectionIndex--;
                if (currentSelectionIndex < 0) currentSelectionIndex = menuButtons.Length - 1;
                UpdateMenuSelectionVisuals();
            }

            // L-joy down = Down Arrow
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentSelectionIndex++;
                if (currentSelectionIndex >= menuButtons.Length) currentSelectionIndex = 0;
                UpdateMenuSelectionVisuals();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (currentSelectionIndex >= 0 && currentSelectionIndex < menuButtons.Length)
                {
                    Button targetButton = menuButtons[currentSelectionIndex];
                    if (targetButton != null)
                    {
                        ExecuteSceneLoadByButtonName(targetButton.gameObject.name);
                    }
                }
            }
        }
    }

    private void ExecuteSceneLoadByButtonName(string buttonName)
    {
        Time.timeScale = 1f;
        string cleanedName = buttonName.Replace(" ", "").ToLower();

        if (cleanedName.Contains("start") || cleanedName.Contains("main"))
        {
            SceneManager.LoadScene(0);
        }
        else if (cleanedName.Contains("1-1") || cleanedName.Contains("level1"))
        {
            SceneManager.LoadScene(1); 
        }
        else if (cleanedName.Contains("control"))
        {
            SceneManager.LoadScene(2); 
        }
        else if (cleanedName.Contains("guide"))
        {
            SceneManager.LoadScene(3);
        }
        else if (cleanedName.Contains("level"))
        {
            SceneManager.LoadScene(4);
        }
        else if (cleanedName.Contains("death"))
        {
            SceneManager.LoadScene(5);
        }
        else if (cleanedName.Contains("win"))
        {
            SceneManager.LoadScene(6);
        }
        else if (cleanedName.Contains("1-2") || cleanedName.Contains("level2"))
        {
            SceneManager.LoadScene(7);
        }

        else if (cleanedName.Contains("restart") || cleanedName.Contains("reload"))
        {
            SceneManager.LoadScene(currentSceneIndex); 
        }
        else if (cleanedName.Contains("resume") || cleanedName.Contains("pauze") || cleanedName.Contains("pause"))
        {
            PauzeMenu();
        }
        else if (cleanedName.Contains("quit") || cleanedName.Contains("exit"))
        {
            Application.Quit();
        }
    }

    private void UpdateMenuSelectionVisuals()
    {
        for (int i = 0; i < menuButtons.Length; i++)
        {
            if (menuButtons[i] != null)
            {
                if (i == currentSelectionIndex)
                {
                    menuButtons[i].Select();
                }
            }
        }
    }

    public void PauzeMenu()
    {
        if (menuCanvas == null)
        {
            menuCanvas = GameObject.Find("PauseMenuCanvas");
        }

        isOpen = !isOpen;
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(isOpen);
        }

        if (isOpen)
        {
            Time.timeScale = 0f;
            currentSelectionIndex = 0;
            UpdateMenuSelectionVisuals();
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void Restart() { Time.timeScale = 1f; SceneManager.LoadScene(currentSceneIndex); }
    public void Respawn() { Time.timeScale = 1f; SceneManager.LoadScene(1); }
    public void StartMenu() { Time.timeScale = 1f; SceneManager.LoadScene(0); }
    public void SettingsMenu() { Time.timeScale = 1f; }
    public void LevelsMenu() { Time.timeScale = 1f; SceneManager.LoadScene(4); }
    public void Pauzetoggle() { PauzeMenu(); }
    public void GuideMenu() { Time.timeScale = 1f; SceneManager.LoadScene(3); }
    public void ControlsMenu() { Time.timeScale = 1f; SceneManager.LoadScene(2); }
    public void quit() { Application.Quit(); }
}