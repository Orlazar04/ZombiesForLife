// Beta Version
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ZombieSpace;

// Managing level progression in a scene and GUI
// Dependencies: Target Pickup Manager
// Main Contributors: Olivia Lazar
public class LevelManager : MonoBehaviour
{   
    // Level Attributes   
    [SerializeField]
    private int setDifficulty = 1;          // The value levelDifficulty will start as
    [SerializeField]
    private string nextLevel = "End";       // Name of the next level to be loaded 
    public static LevelState levelState;    // Whether the level can operate
    public static int levelDifficulty;      // Scale modifier for level dependent attributes

    // GUI Elements
    public Text scoreText;
    public Text targetSafeText;
    public Text targetThreatenedText;
    public Text targetAttackedText;
    public Slider targetSlider;
    public Text targetText;
    public Image collectedUncheck;
    public Image collectedCheck;

    // Game messages
    public Text lostText;
    public Text cannotEscapeText;
    public Text escapedText;

    // Sound Elements
    public AudioClip lostSFX;
    public AudioClip escapedSFX;

    private int levelScore;                         // Points awarded in the current level
    private bool isTargetCollected;                 // Whether the pickup is collected
    private TargetPickupManager targetPickup;       // The manager script of the target pickup
    private PickupState targetStatus;               // Status of target pickup

    // Returns whether the level is running
    public static bool IsLevelActive()
    {
        return (levelState == LevelState.Active);
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Initialize level management values
        levelState = LevelState.Active;
        levelDifficulty = setDifficulty;

        isTargetCollected = false;
        levelScore = 0;
        UpdateScore(0);

        // Initialize target pickup values
        targetPickup = GameObject.FindGameObjectWithTag("TargetPickup").GetComponent<TargetPickupManager>();
        targetStatus = targetPickup.status;

        // Initialize GUI
        targetSlider.maxValue = targetPickup.integrity;
        targetSlider.value = targetPickup.integrity;
        targetText.text = targetPickup.itemName;
        UpdateTargetStatusGUI();
    }

    // Update is called once per frame
    private void Update()
    {
        // While the level is active
        if(IsLevelActive())
        {
            // Update target pickup status
            if(targetStatus != targetPickup.status)
            {
                targetStatus = targetPickup.status;
                UpdateTargetStatusGUI();
            }
            
            // Update target pickup integrity timer GUI if target pickup is being attacked
            if(targetStatus == PickupState.Attacked)
            {
                targetSlider.value = targetPickup.integrity;
            }

            // Lose level if target pickup is destroyed
            if(targetStatus == PickupState.Destroyed)
            {
                LevelLost(DefeatType.TargetDestroyed);
            }
        }
    }

    // Updates target status GUI
    private void UpdateTargetStatusGUI()
    {
        targetSafeText.gameObject.SetActive(false);
        targetThreatenedText.gameObject.SetActive(false);
        targetAttackedText.gameObject.SetActive(false);
        targetSlider.gameObject.SetActive(false);

        switch(targetStatus)
        {
            // If target pickup is safe
            case PickupState.Safe:
                targetSafeText.gameObject.SetActive(true);
                break;
            // If target pickup is being threatened
            case PickupState.Threatened:
                targetThreatenedText.gameObject.SetActive(true);
                break;
            // If target pickup is being attacked
            case PickupState.Attacked:
                targetAttackedText.gameObject.SetActive(true);
                targetSlider.gameObject.SetActive(true);
                break;
        }
    }

    // Initiates procedure for when current level is lost
    public void LevelLost(DefeatType reason)
    {
        levelState = LevelState.Over;
        
        // Select game over message based on given reason
        string message = "Undefined";
        switch(reason)
        {
            // If the target pickup is destroyed
            case DefeatType.TargetDestroyed:
                message = targetPickup.itemName + " was destroyed!";
                break;
            // If the player is killed
            case DefeatType.PlayerKilled:
                message = "You died!";
                break;
        }
        lostText.gameObject.SetActive(true);
        lostText.text = message;

        // Sound
        Camera.main.GetComponent<AudioSource>().volume = 0f;
        AudioSource.PlayClipAtPoint(lostSFX, Camera.main.transform.position);

        Invoke("ReloadLevel", 2);
    }

    // Loads the current level from the beginning 
    private void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Initiates an attempt to finish the current level
    public void LevelFinishAttempt()
    {
        // Level can only be finished if target pickup is collected
        if(!isTargetCollected)
        {
            StartCoroutine(FlashCannotEscapeText());
        }
        else
        {
            LevelFinished();
        }
    }

    // Show that the level cannot be finished yet
    private IEnumerator FlashCannotEscapeText()
    {
        cannotEscapeText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        cannotEscapeText.gameObject.SetActive(false);
    }

    // Initiates procedure for when current level is finished
    private void LevelFinished()
    {
        levelState = LevelState.Over;
        
        escapedText.gameObject.SetActive(true);

        // Sound
        Camera.main.GetComponent<AudioSource>().volume = 0f;
        AudioSource.PlayClipAtPoint(escapedSFX, Camera.main.transform.position);

        Invoke("LoadNextLevel", 2);
    }

    // Loads the next level with name nextLevel
    // If nextLevel not given, game is won
    private void LoadNextLevel()
    {
        if(nextLevel != "End")
        {
            SceneManager.LoadScene(nextLevel);
        }
        else
        {
            Debug.Log("Add win functionality");
        }
    }

    // Updates the score and GUI by the given amount of points
    public void UpdateScore(int points)
    {
        levelScore += points;
        scoreText.text = levelScore.ToString();
    }

    // Updates the target pickup collection and GUI to true
    public void TargetCollected()
    {
        isTargetCollected = true;
        collectedUncheck.gameObject.SetActive(false);
        collectedCheck.gameObject.SetActive(true);
    }
}
