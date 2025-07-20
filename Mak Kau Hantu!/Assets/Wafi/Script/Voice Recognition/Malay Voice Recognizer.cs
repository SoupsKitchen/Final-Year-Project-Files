using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using SimpleJSON;

public class MalayVoiceRecognizer : MonoBehaviour
{
    public string witApiToken = "YZWBZXKB43VPNBPIFZZMFKWY27SSHE3O";
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
    private VoiceDemoUI voiceDemoUI;

    [Header("Debug Output")]
    public UnityEvent<string> onLog;

    private float currentRecordTime = 0f;

    void Awake()
    {
        voiceDemoUI = FindObjectOfType<VoiceDemoUI>();
    }

    public void StartVoiceRecognition()
    {
        if (voiceDemoUI == null)
            voiceDemoUI = FindObjectOfType<VoiceDemoUI>(); // Reassign if it's null

        if (voiceLoopRoutine == null)
        {
            voiceDemoUI?.ClearLog();
            Debug.Log("Voice recognition started.");
            onLog?.Invoke("Voice recognition started.");
            voiceLoopRoutine = StartCoroutine(VoiceLoop());
        }
    }

    public void StopVoiceRecognition()
    {
        if (voiceLoopRoutine != null)
        {
            StopCoroutine(voiceLoopRoutine);
            voiceLoopRoutine = null;
            Debug.Log("Voice recognition stopped.");
            onLog?.Invoke("Voice recognition stopped.");
        }
    }

    public void ResetRecordingTimer()
    {
        currentRecordTime = 0f;

        if (voiceDemoUI == null)
            voiceDemoUI = FindObjectOfType<VoiceDemoUI>();

        if (voiceLoopRoutine != null)
        {
            StopCoroutine(voiceLoopRoutine);
            voiceLoopRoutine = null;
        }

        if (isRecording)
        {
            Microphone.End(null);
            isRecording = false;
            Debug.Log("Recording manually reset and stopped.");
            onLog?.Invoke("Recording manually reset and stopped.");
        }

        voiceDemoUI?.ClearLog();
        Debug.Log("Voice recognition restarted after reset.");
        onLog?.Invoke("Voice recognition restarted after reset.");
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

        isRecording = true;

        recordedClip = Microphone.Start(null, false, 5, sampleRate);
        Debug.Log("Recording started...");
        onLog?.Invoke("Recording started...");
        yield return new WaitForSeconds(5f);
        Microphone.End(null);
        Debug.Log("Recording ended.");
        onLog?.Invoke("Recording ended.");

        byte[] wavData = WavUtility.FromAudioClip(recordedClip);
        yield return StartCoroutine(SendToWit(wavData));

        isRecording = false;
    }

    IEnumerator SendToWit(byte[] data)
    {
        UnityWebRequest www = UnityWebRequest.Put("https://api.wit.ai/speech?v=20210928", data);
        www.method = "POST";
        www.SetRequestHeader("Authorization", "Bearer " + witApiToken);
        www.SetRequestHeader("Content-Type", "audio/wav");

        Debug.Log("Sending audio to Wit.ai...");
        onLog?.Invoke("Sending audio to Wit.ai...");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string json = www.downloadHandler.text;
            Debug.Log("Wit.ai Response received.");
            onLog?.Invoke("Wit.ai Response received.");
            Debug.Log("Wit.ai Response: " + json);
            HandleWitResponse(json);
        }
        else
        {
            string errorMsg = "Wit.ai Error: " + www.error;
            Debug.LogError(errorMsg);
            onLog?.Invoke(errorMsg);
        }
    }

    void HandleWitResponse(string jsonResponse)
    {
        var response = JSON.Parse(jsonResponse);
        string spokenText = response["text"];
        if (string.IsNullOrEmpty(spokenText)) return;

        spokenText = spokenText.ToLower();
        string logLine = "Recognized Text: " + spokenText;
        Debug.Log(logLine);
        onLog?.Invoke(logLine);

        voiceDemoUI?.ShowRecognizedText(spokenText);

        if (spokenText == lastRecognizedCommand && Time.time - lastCommandTime < commandCooldown)
        {
            Debug.Log("Ignored: Same command repeated too soon.");
            return;
        }

        lastRecognizedCommand = spokenText;
        lastCommandTime = Time.time;

        int angerWordCount = 0;
        int repelWordCount = 0;

        foreach (string word in ghostAngerWords)
        {
            if (spokenText.Contains(word))
                angerWordCount++;
        }

        foreach (string word in ghostRepelWords)
        {
            if (spokenText.Contains(word))
                repelWordCount++;
        }

        float angerChange = (angerWordCount * angerIncreaseAmount) - (repelWordCount * angerDecreaseAmount);

        if (angerChange != 0f && pontianakBehaviour != null)
        {
            pontianakBehaviour.anger += angerChange;
            Debug.Log($"Pontianak anger changed by {angerChange}, new anger: {pontianakBehaviour.anger}");

            if (angerChange > 0)
                ghostController?.ChasePlayerFaster();
            else
                ghostController?.RunAwayFromPlayer();
        }

        if (spokenText.Contains("lari"))
        {
            Debug.Log("Voice Command Detected: Run");
            playerRunner?.RunForward();
        }
    }
}