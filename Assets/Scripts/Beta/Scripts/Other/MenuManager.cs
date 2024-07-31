// Beta Version
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using ZombieSpace;

// Managing pause menu and settings
// Main Contributors: Olivia Lazar
public class MenuManager : MonoBehaviour
{
    // Pause Menu
    public GameObject pauseMenu;
    public Button resumeButton;
    public Button settingsButton;
    public Button exitButton;

    private PostProcessVolume renderingEffects;     // Camera effect on level behind the menu

    // Start is called before the first frame update
    void Start()
    {
        // Initialize pause buttons
        resumeButton.onClick.AddListener(UnpauseLevel);
        settingsButton.onClick.AddListener(OpenSettings);
        exitButton.onClick.AddListener(ExitLevel);

        // Initialize camera effects
        renderingEffects = Camera.main.gameObject.GetComponent<PostProcessVolume>();
        renderingEffects.enabled = false;

        LockCursor();
    }

    // Update is called once per frame
    void Update()
    {
        // Pause an active level when exit button is pressed
        if(LevelManager.IsLevelActive() && Input.GetButton("Cancel"))
        {
            PauseLevel();
        }
    }

    // Pauses the game
    private void PauseLevel()
    {
        LevelManager.levelState = LevelState.Paused;
        pauseMenu.SetActive(true);
        renderingEffects.enabled = true;
        UnlockCursor();
    }

    // Unpauses the game
    private void UnpauseLevel()
    {
        LockCursor();
        pauseMenu.SetActive(false);
        renderingEffects.enabled = false;
        LevelManager.levelState = LevelState.Active;
    }

    // Opens the settings menu
    private void OpenSettings()
    {
        Debug.Log("Not added yet!");
    }

    // Exits to the main menu
    private void ExitLevel()
    {
        Debug.Log("Not added yet!");
    }

    // Locks the cursor for gameplay
    private void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Unlocks the cursor for menus
    private void UnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
