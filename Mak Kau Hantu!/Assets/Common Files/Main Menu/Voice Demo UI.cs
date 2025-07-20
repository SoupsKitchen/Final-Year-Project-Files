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
        // Find recognizer even if it's in a disabled GameObject
        recognizer = FindObjectOfType<MalayVoiceRecognizer>(true);

        if (recognizer == null)
        {
            Debug.LogWarning("MalayVoiceRecognizer not found in scene.");
        }

        if (startButton != null)
        {
            startButton.onClick.AddListener(() =>
            {
                if (recognizer != null)
                {
                    recognizer.StartVoiceRecognition();
                    Debug.Log("Voice recognition started from demo UI.");
                }
            });
        }

        if (stopButton != null)
        {
            stopButton.onClick.AddListener(() =>
            {
                if (recognizer != null)
                {
                    recognizer.StopVoiceRecognition();
                    Debug.Log("Voice recognition stopped from demo UI.");
                }
            });
        }
    }

    // Display recognized text in the UI
    public void ShowRecognizedText(string message)
    {
        if (debugText != null)
        {
            debugText.text = "Recognized: " + message;
        }
        else
        {
            Debug.LogWarning("debugText (TMP) is not assigned in VoiceDemoUI.");
        }
    }

    public void ClearLog()
    {
        if (debugText != null)
            debugText.text = "";
    }

    void OnDisable()
    {
        recognizer?.StopVoiceRecognition();
    }
}