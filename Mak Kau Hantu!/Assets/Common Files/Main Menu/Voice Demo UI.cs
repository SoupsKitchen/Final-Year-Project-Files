using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VoiceDemoUI : MonoBehaviour
{
    public TextMeshProUGUI debugText;
    public Button startButton;
    public Button stopButton;

    private MalayVoiceRecognizer recognizer;

    void Start()
    {
        recognizer = FindObjectOfType<MalayVoiceRecognizer>();

        // Removed recognizer.onLog.AddListener(AppendLog); — no more log spam in UI

        if (startButton != null)
            startButton.onClick.AddListener(() => recognizer?.StartVoiceRecognition());

        if (stopButton != null)
            stopButton.onClick.AddListener(() => recognizer?.StopVoiceRecognition());
    }

    // Used only when we want to show recognized text
    public void ShowRecognizedText(string message)
    {
        debugText.text = message;
    }

    public void ClearLog()
    {
        debugText.text = "";
    }

    void OnDisable()
    {
        MalayVoiceRecognizer recognizer = FindObjectOfType<MalayVoiceRecognizer>();
        recognizer?.StopVoiceRecognition();
    }
}