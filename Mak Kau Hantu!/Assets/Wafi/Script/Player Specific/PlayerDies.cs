using UnityEngine;
using UnityEngine.UI;

public class PlayerDies : MonoBehaviour
{
    public GameObject deathScreen;       // The death screen panel (already in scene)
    public Button quitButton;            // Button to quit the game
    public Button mainMenuButton;        // Button to go to main menu
    public GameObject mainMenuPanel;     // Main menu panel (already in scene)
    public GameObject playerObject;      // Player GameObject to disable

    private bool isDead = false;

    void Update()
    {
        if (!isDead && Input.GetKeyDown(KeyCode.K)) // For testing
        {
            KillPlayer();
        }
    }

    void KillPlayer()
    {
        isDead = true;

        // Disable player
        if (playerObject != null)
            playerObject.SetActive(false);

        // Show death screen
        if (deathScreen != null)
        {
            deathScreen.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Assign button actions
            if (quitButton != null)
            {
                quitButton.onClick.RemoveAllListeners();
                quitButton.onClick.AddListener(QuitGame);
            }

            if (mainMenuButton != null)
            {
                mainMenuButton.onClick.RemoveAllListeners();
                mainMenuButton.onClick.AddListener(ReturnToMainMenu);
            }
        }
    }

    void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        if (deathScreen != null) deathScreen.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);

        isDead = false; // Allow player to die again after returning
    }
}