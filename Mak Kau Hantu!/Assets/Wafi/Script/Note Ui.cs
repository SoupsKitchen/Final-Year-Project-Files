using UnityEngine;
using UnityEngine.UI;

public class NoteReader : MonoBehaviour
{
    [Header("UI")]
    public GameObject notePanel;
    public Button closeButton;

    [Header("Note Detection")]
    public float detectDistance = 3f;

    private bool isReading = false;
    private UIManager uiManager;

    void Start()
    {
        if (notePanel != null)
            notePanel.SetActive(false);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseNote);

        uiManager = FindObjectOfType<UIManager>();
    }

    void Update()
    {
        if (!isReading && Input.GetKeyDown(KeyCode.E))
        {
            TryReadNote();
        }
        else if (isReading && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E)))
        {
            CloseNote();
        }
    }

    void TryReadNote()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, detectDistance))
        {
            if (hit.collider.CompareTag("Note"))
            {
                ShowNote();
            }
        }
    }

    void ShowNote()
    {
        if (notePanel == null || uiManager == null) return;

        notePanel.SetActive(true);
        isReading = true;

        uiManager.PauseGame();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseNote()
    {
        if (notePanel == null || uiManager == null) return;

        notePanel.SetActive(false);
        isReading = false;

        uiManager.ResumeGame();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}