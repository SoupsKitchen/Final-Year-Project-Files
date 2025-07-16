using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuUI;
    public GameObject pauseMenuUI;
    public GameObject finalScreenUI;

    [Header("Buttons")]
    public Button startButton;
    public Button mainMenuQuitButton; // From main menu
    public Button resumeButton;
    public Button pauseQuitButton;    // From pause menu
    public Button finalQuitButton;    // From final screen

    [Header("Game Objects")]
    public GameObject player;
    public GameObject ghost;
    public FPSControllerCharacter fpsController; // Drag Player here

    private bool isGamePaused = false;
    private bool gameStarted = false;

    void Start()
    {
        // Initial UI state
        mainMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        finalScreenUI.SetActive(false);

        // Disable gameplay objects
        if (player != null) player.SetActive(false);
        if (ghost != null) ghost.SetActive(false);

        // Button hooks
        startButton.onClick.AddListener(StartGame);
        mainMenuQuitButton.onClick.AddListener(QuitGame);
        resumeButton.onClick.AddListener(ResumeGame);
        pauseQuitButton.onClick.AddListener(QuitToMainMenu);

        finalQuitButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        if (gameStarted && Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isGamePaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void StartGame()
    {
        Debug.Log("Starting Game...");
        mainMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        finalScreenUI.SetActive(false);

        if (player != null) player.SetActive(true);
        if (ghost != null) ghost.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameStarted = true;
        isGamePaused = false;
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        isGamePaused = true;
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (fpsController != null) fpsController.isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        isGamePaused = false;
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (fpsController != null) fpsController.isPaused = false;
    }

    public void QuitToMainMenu()
    {
        pauseMenuUI.SetActive(false);
        finalScreenUI.SetActive(false);
        mainMenuUI.SetActive(true);

        if (player) player.SetActive(false);
        if (ghost) ghost.SetActive(false);

        Time.timeScale = 1f;
        isGamePaused = false;
        gameStarted = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ShowFinalScreen()
    {
        Debug.Log("Game Completed! Showing final screen...");
        finalScreenUI.SetActive(true);
        mainMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}