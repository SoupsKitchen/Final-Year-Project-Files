using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuUI;
    public GameObject pauseMenuUI;
    public GameObject finalScreenUI;
    public GameObject instructionMenuUI;
    public GameObject voiceTesterPanel;

    [Header("Buttons")]
    public Button startButton;
    public Button mainMenuQuitButton;
    public Button resumeButton;
    public Button pauseQuitButton;
    public Button finalQuitButton;
    public Button instructionOpenButton;
    public Button instructionBackButton;
    public Button voiceTesterOpenButton;
    public Button voiceTesterCloseButton;

    [Header("Game Objects")]
    public GameObject player;
    public GameObject ghost;
    public FPSControllerCharacter fpsController;

    private Vector3 initialPlayerPosition;
    private Quaternion initialPlayerRotation;
    private Vector3 initialGhostPosition;
    private Quaternion initialGhostRotation;

    private bool isGamePaused = false;
    private bool gameStarted = false;
    public bool isReadingNote = false;

    private MalayVoiceRecognizer voiceRecognizer;

    public bool IsGamePaused() => isGamePaused;

    void Start()
    {
        // Store initial spawn positions
        if (player != null)
        {
            initialPlayerPosition = player.transform.position;
            initialPlayerRotation = player.transform.rotation;
            player.SetActive(false);
        }

        if (ghost != null)
        {
            initialGhostPosition = ghost.transform.position;
            initialGhostRotation = ghost.transform.rotation;
            ghost.SetActive(false);
        }

        mainMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        finalScreenUI.SetActive(false);
        instructionMenuUI.SetActive(false);
        voiceTesterPanel.SetActive(false);

        voiceRecognizer = FindObjectOfType<MalayVoiceRecognizer>();

        // Assign button events
        startButton.onClick.AddListener(StartGame);
        mainMenuQuitButton.onClick.AddListener(QuitGame);
        resumeButton.onClick.AddListener(ResumeGame);
        pauseQuitButton.onClick.AddListener(QuitToMainMenu);
        finalQuitButton.onClick.AddListener(QuitGame);
        instructionOpenButton.onClick.AddListener(OpenInstructionMenu);
        instructionBackButton.onClick.AddListener(BackToMainMenu);
        voiceTesterOpenButton.onClick.AddListener(OpenVoiceTesterPanel);
        voiceTesterCloseButton.onClick.AddListener(CloseVoiceTesterPanel);
    }

    void Update()
    {
        if (gameStarted && Input.GetKeyDown(KeyCode.Escape) && !isReadingNote)
        {
            if (!isGamePaused) PauseGame();
            else ResumeGame();
        }
    }

    public void StartGame()
    {
        ResetGameState();

        mainMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        finalScreenUI.SetActive(false);
        instructionMenuUI.SetActive(false);
        voiceTesterPanel.SetActive(false);

        if (player != null) player.SetActive(true);
        if (ghost != null) ghost.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameStarted = true;
        isGamePaused = false;
        Time.timeScale = 1f;

        if (voiceRecognizer != null)
            voiceRecognizer.StartVoiceRecognition();
    }

    public void PauseGame()
    {
        if (isReadingNote) return;

        pauseMenuUI.SetActive(true);
        isGamePaused = true;
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (fpsController != null) fpsController.isPaused = true;

        if (voiceRecognizer != null)
            voiceRecognizer.StopVoiceRecognition();
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        isGamePaused = false;
        Time.timeScale = 1f;

        Cursor.lockState = isReadingNote ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isReadingNote;

        if (fpsController != null) fpsController.isPaused = false;

        if (voiceRecognizer != null)
        {
            voiceRecognizer.ResetRecordingTimer();
            voiceRecognizer.StartVoiceRecognition();
        }
    }

    public void SetReadingNoteState(bool reading)
    {
        isReadingNote = reading;

        if (reading)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;

            if (fpsController != null) fpsController.isPaused = true;
            if (voiceRecognizer != null) voiceRecognizer.StopVoiceRecognition();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;

            if (fpsController != null) fpsController.isPaused = false;
            if (voiceRecognizer != null)
            {
                voiceRecognizer.ResetRecordingTimer();
                voiceRecognizer.StartVoiceRecognition();
            }
        }
    }

    public void QuitToMainMenu()
    {
        mainMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        finalScreenUI.SetActive(false);
        instructionMenuUI.SetActive(false);
        voiceTesterPanel.SetActive(false);

        Time.timeScale = 1f;
        isGamePaused = false;
        gameStarted = false;
        isReadingNote = false;

        if (player) player.SetActive(false);
        if (ghost) ghost.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (voiceRecognizer != null)
            voiceRecognizer.StopVoiceRecognition();
    }

    public void ShowFinalScreen()
    {
        finalScreenUI.SetActive(true);
        mainMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        instructionMenuUI.SetActive(false);
        voiceTesterPanel.SetActive(false);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (voiceRecognizer != null)
            voiceRecognizer.StopVoiceRecognition();
    }

    public void OpenInstructionMenu()
    {
        instructionMenuUI.SetActive(true);
        mainMenuUI.SetActive(false);

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

    public void OpenVoiceTesterPanel()
    {
        voiceTesterPanel.SetActive(true);
        instructionMenuUI.SetActive(false);
        mainMenuUI.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseVoiceTesterPanel()
    {
        voiceTesterPanel.SetActive(false);
        mainMenuUI.SetActive(true);

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

    /// <summary>
    /// Fully resets game state — positions, internal flags, etc.
    /// </summary>
    private void ResetGameState()
    {
        if (player != null)
        {
            player.transform.position = initialPlayerPosition;
            player.transform.rotation = initialPlayerRotation;
        }

        if (ghost != null)
        {
            ghost.transform.position = initialGhostPosition;
            ghost.transform.rotation = initialGhostRotation;
        }

        // Reset other systems like items, win conditions, timers etc.
        // For example:
        // FindObjectOfType<ItemManager>()?.ResetItems();
        // FindObjectOfType<WinCondition>()?.ResetWin();

        isReadingNote = false;
        isGamePaused = false;
        Time.timeScale = 1f;

        if (voiceRecognizer != null)
        {
            voiceRecognizer.ResetRecordingTimer();
            voiceRecognizer.StartVoiceRecognition();
        }

        if (fpsController != null)
        {
            fpsController.ResetControllerState();
        }
    }
}