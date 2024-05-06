using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLogic : MonoBehaviour
{
    public RunStats runInformation;
    
    private GameObject mainMenu;
    private GameObject creditsMenu;
    public AudioSource buttonSound;
    public SceneFader sceneFader;

    void Start()
    {
        mainMenu = GameObject.Find("MainMenuCanvas");
        creditsMenu = GameObject.Find("CreditsCanvas");
        // controlsMenu = GameObject.Find("ControlsCanvas");

        // Enable main menu canvas and disable others
        mainMenu.GetComponent<Canvas>().enabled = true;
        //controlsMenu.GetComponent<Canvas>().enabled = false;
        creditsMenu.GetComponent<Canvas>().enabled = false;
    }

    void Update()
    {

    }

    public void StartButton()
    {
        mainMenu.GetComponent<Canvas>().enabled = false;
        buttonSound.Play();
        
        runInformation.Reset();
        
        sceneFader.FadeToScene("Playspace");
    }

    public void CreditsButton()
    {
        buttonSound.Play();
        mainMenu.GetComponent<Canvas>().enabled = false;
        creditsMenu.GetComponent<Canvas>().enabled = true;
    }

    // public void CreditsButton()
    // {
    //     buttonSound.Play();
    //     mainMenu.GetComponent<Canvas>().enabled = false;
    //     creditsMenu.GetComponent<Canvas>().enabled = true;
    // }

    public void ExitGameButton()
    {
        buttonSound.Play();
        Application.Quit();
    }

    public void ReturnToMainMenuButton()
    {
        buttonSound.Play();
        mainMenu.GetComponent<Canvas>().enabled = true;
        //controlsMenu.GetComponent<Canvas>().enabled = false;
        creditsMenu.GetComponent<Canvas>().enabled = false;
    }


}