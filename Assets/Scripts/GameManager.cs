using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                _instance = go.AddComponent<GameManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Additional GameManager logic can be added here

    public static void LoadGameSceneAsync()
    {
        _instance.StartCoroutine(LoadGameAsync());
    }

    static IEnumerator LoadGameAsync()
    {
        Debug.Log("LoadingScreen scene loading started.");

        // Load the LoadingScreen scene asynchronously
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync("LoadingScreen");

        // Wait until the loading is complete
        while (!loadingOperation.isDone)
        {
            // You can use loadingOperation.progress to get the loading progress if needed
            yield return null;
        }

        Debug.Log("LoadingScreen scene loaded. Starting to load SampleScene.");

        // The LoadingScreen scene is loaded; now load the actual game scene
        SceneManager.LoadScene("SampleScene");
    }
}
