using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ZombieSpace;

// Managing level progression in a scene and GUI 
// Main Contributors: Olivia Lazar
public class LevelManager : MonoBehaviour
{   
    // Level Initialization Values   
    public int setDifficulty = 1;       // The value levelDifficulty will start as
    public string nextLevel = "End";    // Name of the next level to be loaded 

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

    // Level Attributes
    public static LevelState levelState;     // Whether the level can operate
    public static int levelDifficulty;       // Scale modifier for level dependent attributes

    private int levelScore;                     // Points awarded in the current level
    private bool isTargetCollected;             // Whether the pickup is collected
    private TargetPickupBehavior targetPickup;  // The pickup needed to progress to the next level
    private string targetName;                  // Name of the target pickup
    private PickupState targetStatus;           // Status of target pickup

    // Start is called before the first frame update
    private void Start()
    {
        // Initialize level management values
        levelState = LevelState.Active;
        levelDifficulty = setDifficulty;

        isTargetCollected = false;
        levelScore = 0;
        UpdateScore(0);

        // Ininitialize target pickup stats
        targetPickup = GameObject.FindGameObjectWithTag("Target Pickup").GetComponent<TargetPickupBehavior>();
        if(targetPickup != null)
        {
            targetName = targetPickup.itemName;
            targetStatus = targetPickup.status;

            // GUI
            targetSlider.maxValue = targetPickup.health;
            targetSlider.value = targetPickup.health;
        }
        else
        {
            targetName = "Undefined";
            targetStatus = PickupState.Safe;
            Debug.Log("Target pickup not found.");
        }

        targetText.text = targetName;
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
            
            // Update target health timer GUI if target pickup is being attacked
            if(targetStatus == PickupState.Attacked)
            {
                targetSlider.value = targetPickup.health;
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

    // Returns whether the level is running
    public static bool IsLevelActive()
    {
        return (levelState == LevelState.Active);
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
                message = targetName + "was destroyed!";
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

    // Loads the current level from the beginning 
    private void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    // Pauses the game
    public static void PauseLevel()
    {
        levelState = LevelState.Paused;
    }

    // Unpauses the game
    public static void UnpauseLevel()
    {
        levelState = LevelState.Active;
    }
}
