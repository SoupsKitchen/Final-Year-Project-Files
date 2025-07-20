using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VoiceRecognitionPanel : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI resultText;
    public Button startListeningButton;
    public Button stopListeningButton;

    private MalayVoiceRecognizer voiceRecognizer;

    void Awake()
    {
        // Find the recognizer in the scene (including inactive objects)
        voiceRecognizer = FindObjectOfType<MalayVoiceRecognizer>(true);

        if (voiceRecognizer == null)
        {
            Debug.LogError("MalayVoiceRecognizer not found in the scene.");
        }

        if (startListeningButton != null)
            startListeningButton.onClick.AddListener(OnStartListening);

        if (stopListeningButton != null)
            stopListeningButton.onClick.AddListener(OnStopListening);
    }

    void OnStartListening()
    {
        if (voiceRecognizer != null)
        {
            voiceRecognizer.StartVoiceRecognition();
        }
    }

    void OnStopListening()
    {
        if (voiceRecognizer != null)
        {
            voiceRecognizer.StopVoiceRecognition();
        }
    }

    public void DisplayRecognizedText(string message)
    {
        if (resultText != null)
            resultText.text = message;
    }

    public void ClearText()
    {
        if (resultText != null)
            resultText.text = "";
    }

    void OnDisable()
    {
        if (voiceRecognizer != null)
            voiceRecognizer.StopVoiceRecognition();
    }
}