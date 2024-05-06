using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private GameObject controlsMenu;
    private GameObject pauseMenu;
    // private GameObject loading;

    public GameObject pauseMenuUI;
    public GameObject controls;

    public static bool GameIsPaused = false;

    void Start (){
        pauseMenu = GameObject.Find("PauseMenu");
        controlsMenu = GameObject.Find("ControlsCanvas");

        controlsMenu.GetComponent<Canvas>().enabled = false;

        pauseMenuUI.SetActive(false);
        pauseMenu.GetComponent<Canvas>().enabled = false;
        controlsMenu.GetComponent<Canvas>().enabled = false;

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

    void Resume(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause(){
        pauseMenuUI.SetActive(true);
        pauseMenu.GetComponent<Canvas>().enabled = true;

        Time.timeScale = 0f;
        GameIsPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        // Time.timeScale = 0f;
        GameIsPaused = true;
        SceneManager.LoadScene("MainMenu");
    }

    public void BackButton()
    {
        pauseMenu.GetComponent<Canvas>().enabled = true;
        controlsMenu.GetComponent<Canvas>().enabled = false;
    }

    public void LoadControls()
    {
        pauseMenu.GetComponent<Canvas>().enabled = false;
        controlsMenu.GetComponent<Canvas>().enabled = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
