using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Death : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public RunStats runInformation;

    void Start()
    {
        scoreText.text = runInformation.score + " points";
    }

    public void EndGame()
    {
        // Load the Death Screen scene
        SceneManager.LoadScene("DeathScreen");
    }

    public void RestartGame()
    {
        runInformation.Reset();
        
        SceneManager.LoadScene("Playspace");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}