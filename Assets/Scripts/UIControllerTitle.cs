using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControllerTitle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void returns()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void play()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (highScore > 0)
        {
            GameManager.LoadGameSceneAsync();
        }
        else
        {
            SceneManager.LoadScene("Tutorial");
        }
    }

    IEnumerator LoadGameAsync()
    {
        // Load the LoadingScreen scene asynchronously
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync("LoadingScreen");

        // Wait until the loading is complete
        while (!loadingOperation.isDone)
        {
            // You can use loadingOperation.progress to get the loading progress if needed
            yield return null;
        }

        // The LoadingScreen scene is loaded; now load the actual game scene
        SceneManager.LoadScene("SampleScene");
    }
    public void quit()
    {
        Application.Quit();
    }

    public void credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

}
