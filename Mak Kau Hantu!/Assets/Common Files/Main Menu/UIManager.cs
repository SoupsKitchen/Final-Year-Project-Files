using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuUI;
    public GameObject pauseMenuUI;
    public GameObject finalScreenUI;
    public GameObject instructionMenuUI; // New Instruction Panel

    [Header("Buttons")]
    public Button startButton;
    public Button mainMenuQuitButton;
    public Button resumeButton;
    public Button pauseQuitButton;
    public Button finalQuitButton;
    public Button instructionOpenButton; // New button to open instruction
    public Button instructionBackButton; // New button to return to main menu

    [Header("Game Objects")]
    public GameObject player;
    public GameObject ghost;
    public FPSControllerCharacter fpsController;

    private bool isGamePaused = false;
    private bool gameStarted = false;

    void Start()
    {
        // Initial UI state
        mainMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        finalScreenUI.SetActive(false);
        instructionMenuUI.SetActive(false); // Hide instruction panel initially

        // Disable gameplay objects
        if (player != null) player.SetActive(false);
        if (ghost != null) ghost.SetActive(false);

        // Button hooks
        startButton.onClick.AddListener(StartGame);
        mainMenuQuitButton.onClick.AddListener(QuitGame);
        resumeButton.onClick.AddListener(ResumeGame);
        pauseQuitButton.onClick.AddListener(QuitToMainMenu);
        finalQuitButton.onClick.AddListener(QuitGame);

        // New buttons for instruction panel
        instructionOpenButton.onClick.AddListener(OpenInstructionMenu);
        instructionBackButton.onClick.AddListener(BackToMainMenu);
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
        instructionMenuUI.SetActive(false);

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
        instructionMenuUI.SetActive(false);
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
        instructionMenuUI.SetActive(false);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OpenInstructionMenu()
    {
        instructionMenuUI.SetActive(true);
        mainMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        finalScreenUI.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void BackToMainMenu()
    {
        instructionMenuUI.SetActive(false);
        mainMenuUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}