using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float levelDuration = 60.0f;
    float countDown = 0;
    public static bool isGameOver = false;
    public AudioClip gameOverSFX;
    public AudioClip gameWonSFX;
    public string nextlevel;
    public Text timerText;
    public Text gameText;
    public Text scoreText;
    private int score = 0;
    public float fallThreshold = -.5f;
    public GameObject player;

    void Start()
    {
        isGameOver = false;
        countDown = levelDuration;
        SetTimerText();
        UpdateScoreText();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("Player not found. Make sure the player has the 'Player' tag.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            if (countDown > 0)
            {
                countDown -= Time.deltaTime;
            }
            else
            {
                countDown = 0.0f;
                LevelLost();
            }
            SetTimerText();
        }

        if (player != null && player.transform.position.y < fallThreshold)
        {
            LevelLost();
        }
        //Debug.Log(countDown);
        //Debug.Log(timerText);
    }


    private void OnGUI()
    {
        //GUI.Box(new Rect(10, 10, 40, 30), countDown.ToString("0.00"));
    }

    void SetTimerText()
    {
        countDown.ToString("f2");
        timerText.text = countDown.ToString("f2");
    }

    public void LevelLost()
    {
        isGameOver = true;
        gameText.text = "GAME OVER!";
        gameText.gameObject.SetActive(true);
        Camera.main.GetComponent<AudioSource>().pitch = 1;
        AudioSource.PlayClipAtPoint(gameOverSFX, Camera.main.transform.position);
        Invoke("LoadCurrentLevel", 2);

    }

    public void LevelBeat()
    {
        isGameOver = true;
        gameText.text = "YOU WIN!";
        gameText.gameObject.SetActive(true);

        Camera.main.GetComponent<AudioSource>().pitch = 2;
        AudioSource.PlayClipAtPoint(gameWonSFX, Camera.main.transform.position);
        if (SceneManager.GetActiveScene().name.Contains("3"))
        {
            gameText.text = "CONGRATULATIONS! YOU COMPLETED THE GAME!";
        }
        else if (!string.IsNullOrEmpty(nextlevel))
        {
            Invoke("LoadNextLevel", 2);
        }
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(nextlevel);
    }

    void LoadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void AddScore(int value)
    {
        if (countDown > levelDuration / 2)
        {
            // Double the score if collected in the first half of the level
            value *= 2;
        }
        score += value;
        UpdateScoreText();
    }
}
