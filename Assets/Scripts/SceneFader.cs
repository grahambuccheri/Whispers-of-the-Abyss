// using UnityEngine;
// using UnityEngine.SceneManagement;
// using System.Collections;

// puclic class SceneFader : MonoBehaviour
// {
//     public Animator animator;
//     private string sceneName;

//     void Update ()
//     {
//         FadeToLevel(sceneName);
//     }

//     public void FadeToLevel(string sceneName)
//     {
//         animator.SetTrigger("FadeOut");
//     }


// }

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Animator animator;
    private static SceneFader instance;

    void Awake() {
        // Ensure the fader persists across scenes
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject); //gameObject being the image
        } else {
            Destroy(gameObject);
        }
    }

    // Function to be called when you want to fade to a scene
    public void FadeToScene(string sceneName)
    {
        Debug.Log("fadetoscene inside");
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    // Coroutine to fade out and load scene asynchronously
    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        Debug.Log("fadeoutandload inside " + sceneName);
        // animator.SetTrigger("FadeOut");
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);
        
        FadeIn();
        SceneManager.LoadScene(sceneName);
    }

    // Function to be called to start fade in
    public void FadeIn()
    {
        Debug.Log("fadein begin");
        animator.SetTrigger("FadeIn");
    }
}