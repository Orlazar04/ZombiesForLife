using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

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
        // Initialize buttons
        resumeButton.onClick.AddListener(ResumeLevel);
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
        // Pause level
        if(LevelManager.IsLevelActive() && Input.GetButton("Cancel"))
        {
            LevelManager.PauseLevel();
            pauseMenu.SetActive(true);
            UnlockCursor();

            renderingEffects.enabled = true;
        }
    }

    // Closes the pause menu
    private void ResumeLevel()
    {
        LockCursor();
        pauseMenu.SetActive(false);
        LevelManager.UnpauseLevel();

        renderingEffects.enabled = false;
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
