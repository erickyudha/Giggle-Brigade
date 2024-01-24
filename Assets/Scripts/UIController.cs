using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public AudioSource gameoverSound;
    public HighScoreText highScoreText;
    Player player;
    Text distanceText;
    public GameObject highScoreMark;

    GameObject results;
    Text finalDistanceText;
    private bool isDone = false;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        distanceText = GameObject.Find("DistanceText").GetComponent<Text>();
        results = GameObject.Find("Results");
        finalDistanceText = GameObject.Find("FinalDistanceText").GetComponent<Text>();

        results.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int distance = Mathf.FloorToInt(player.distance);
        distanceText.text = distance + " m";

        if (player.isDead && !isDone)
        {
            results.SetActive(true);
            gameoverSound.Play();
            finalDistanceText.text = distance + " m";
            bool newHigh = highScoreText.SetNewHighScore(distance);
            if (newHigh)
            {
                highScoreMark.SetActive(true);
            }

            isDone = true;
        }
    }


    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Retry()
    {
        SceneManager.LoadScene("SampleScene");
    }


}
