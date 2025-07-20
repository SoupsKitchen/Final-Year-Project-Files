using UnityEngine;
using UnityEngine.UI;

public class PlayerDies : MonoBehaviour
{
    public GameObject deathScreen;
    public Button quitButton;
    public Button mainMenuButton;

    public GameObject mainMenuPanel;
    public GameObject playerObject;

    public Camera gameplayCamera;       // Reference this in Inspector
    public Camera mainMenuCamera;       // Reference this in Inspector

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

        if (playerObject != null)
            playerObject.SetActive(false);

        // Cameras
        if (gameplayCamera != null)
            gameplayCamera.enabled = false;

        if (mainMenuCamera != null)
            mainMenuCamera.enabled = true;

        if (deathScreen != null)
        {
            deathScreen.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Button listeners
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

        // Hide death screen
        if (deathScreen != null)
            deathScreen.SetActive(false);

        // Show main menu UI
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);

        // Enable main menu camera
        if (mainMenuCamera != null)
            mainMenuCamera.enabled = true;

        // Disable gameplay camera (safety)
        if (gameplayCamera != null)
            gameplayCamera.enabled = false;

        // Cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isDead = false;
    }
}