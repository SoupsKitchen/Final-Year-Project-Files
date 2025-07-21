using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class MalayVoiceRecognizer : MonoBehaviour
{
    public string witApiToken = "YOUR_API_TOKEN_HERE";
    private AudioClip recordedClip;
    private const int sampleRate = 16000;
    private bool isRecording = false;

    public PlayerRunner playerRunner;
    public GhostController ghostController;
    public Pontianak_Behaviour pontianakBehaviour;

    public float commandCooldown = 2f;
    private float lastCommandTime = -10f;
    private string lastRecognizedCommand = "";

    public string[] ghostRepelWords = {
        "pergi", "undur", "keluar", "lari", "jangan dekat",
        "aku tak takut", "berambus", "halau", "aku halau kau", "kau takut"
    };

    public string[] ghostAngerWords = {
        "mari sini", "bodoh", "lemah", "tunjuk diri", "pengecut",
        "aku cabar kau", "kau tak berani", "hantu pengecut", "muncul", "serang aku"
    };

    public float angerIncreaseAmount = 10f;
    public float angerDecreaseAmount = 10f;

    private Coroutine voiceLoopRoutine;
    private VoiceRecognitionPanel ui;

    void Awake()
    {
        ui = FindObjectOfType<VoiceRecognitionPanel>(true);
    }

    public void StartVoiceRecognition()
    {
        if (voiceLoopRoutine == null)
        {
            Debug.Log("Voice recognition started.");
            ClearUI();
            voiceLoopRoutine = StartCoroutine(VoiceLoop());
        }
    }

    public void StopVoiceRecognition()
    {
        if (voiceLoopRoutine != null)
        {
            Debug.Log("Voice recognition stopped.");
            StopCoroutine(voiceLoopRoutine);
            voiceLoopRoutine = null;
        }
    }

    public void ResetRecordingTimer()
    {
        if (voiceLoopRoutine != null)
        {
            Debug.Log("Voice recognition reset.");
            StopCoroutine(voiceLoopRoutine);
            voiceLoopRoutine = null;
        }

        if (isRecording)
        {
            Debug.Log("Stopping microphone.");
            Microphone.End(null);
            isRecording = false;
        }

        ClearUI();
        voiceLoopRoutine = StartCoroutine(VoiceLoop());
    }

    IEnumerator VoiceLoop()
    {
        while (true)
        {
            yield return StartCoroutine(CaptureAndSendAudio());
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator CaptureAndSendAudio()
    {
        if (isRecording) yield break;

        Debug.Log("Recording now...");
        isRecording = true;
        recordedClip = Microphone.Start(null, false, 5, sampleRate);
        yield return new WaitForSeconds(5f);
        Microphone.End(null);
        Debug.Log("Recording finished.");

        byte[] wavData = WavUtility.FromAudioClip(recordedClip);
        Debug.Log("Sending audio to Wit.ai...");
        yield return StartCoroutine(SendToWit(wavData));
        Debug.Log("Response received.");
        isRecording = false;
    }

    IEnumerator SendToWit(byte[] data)
    {
        UnityWebRequest www = UnityWebRequest.Put("https://api.wit.ai/speech?v=20210928", data);
        www.method = "POST";
        www.SetRequestHeader("Authorization", "Bearer " + witApiToken);
        www.SetRequestHeader("Content-Type", "audio/wav");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string json = www.downloadHandler.text;
            HandleWitResponse(json);
        }
        else
        {
            Debug.Log("Wit.ai Error: " + www.error);
        }
    }

    void HandleWitResponse(string jsonResponse)
    {
        var response = JSON.Parse(jsonResponse);
        string spokenText = response["text"];
        if (string.IsNullOrEmpty(spokenText)) return;

        spokenText = spokenText.ToLower();

        // Log to Console
        Debug.Log("Recognized Malay Text: " + spokenText);

        // Display only Malay text in UI
        ui?.DisplayRecognizedText(spokenText);

        // Avoid duplicate triggers
        if (spokenText == lastRecognizedCommand && Time.time - lastCommandTime < commandCooldown)
            return;

        lastRecognizedCommand = spokenText;
        lastCommandTime = Time.time;

        int angerWordCount = 0;
        int repelWordCount = 0;

        foreach (string word in ghostAngerWords)
        {
            if (spokenText.Contains(word)) angerWordCount++;
        }

        foreach (string word in ghostRepelWords)
        {
            if (spokenText.Contains(word)) repelWordCount++;
        }

        float angerChange = (angerWordCount * angerIncreaseAmount) - (repelWordCount * angerDecreaseAmount);

        if (angerChange != 0f && pontianakBehaviour != null)
        {
            pontianakBehaviour.anger += angerChange;

            if (angerChange > 0)
                ghostController?.ChasePlayerFaster();
            else
                ghostController?.RunAwayFromPlayer();
        }

        if (spokenText.Contains("lari"))
        {
            playerRunner?.RunForward();
        }
    }

    void ClearUI()
    {
        ui?.DisplayRecognizedText("");
    }
}