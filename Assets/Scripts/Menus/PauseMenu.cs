using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private GameObject pauseMenu;
    //private GameObject controlsMenu;
    private GameObject playerControlsMenu;
    private GameObject subControlsMenu;

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    void Start (){
        //mainMenu = GameObject.Find("PauseCanvas");
        //controlsMenu = GameObject.Find("ControlsCanvas");

        pauseMenuUI.SetActive(false);
        Cursor.visible = false;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        // Time.timeScale = 0f;
        GameIsPaused = true;
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowPlayerControls()
    {
        pauseMenu.GetComponent<Canvas>().enabled = false;
        subControlsMenu.GetComponent<Canvas>().enabled = false;
        playerControlsMenu.GetComponent<Canvas>().enabled = true;
    }

    public void ShowSubControls()
    {
        pauseMenu.GetComponent<Canvas>().enabled = false;
        playerControlsMenu.GetComponent<Canvas>().enabled = false;
        subControlsMenu.GetComponent<Canvas>().enabled = true;
    }

    public void BackButton()
    {
        //controlsMenu.GetComponent<Canvas>().enabled = false;
        pauseMenu.GetComponent<Canvas>().enabled = true;
    }

    void Resume(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    

    public void QuitGame()
    {
        Application.Quit();
    }
}
