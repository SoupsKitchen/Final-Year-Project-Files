using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class MalayVoiceRecognizer : MonoBehaviour
{
    public string witApiToken = "YZWBZXKB43VPNBPIFZZMFKWY27SSHE3O";
    private AudioClip recordedClip;
    private const int sampleRate = 16000;
    private bool isRecording = false;

    public PlayerRunner playerRunner;
    public GhostController ghostController;

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

    void Start()
    {
        StartCoroutine(VoiceLoop());
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
        yield return new WaitForSeconds(5f);
        Microphone.End(null);

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

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Wit.ai Response: " + www.downloadHandler.text);
            HandleWitResponse(www.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Wit.ai Error: " + www.error);
        }
    }

    void HandleWitResponse(string jsonResponse)
    {
        var response = JSON.Parse(jsonResponse);
        string spokenText = response["text"];
        Debug.Log("Recognized Text: " + spokenText);

        // === Cooldown + duplicate check ===
        if (spokenText == lastRecognizedCommand && Time.time - lastCommandTime < commandCooldown)
        {
            Debug.Log("Ignored: Same command repeated too soon.");
            return;
        }

        lastRecognizedCommand = spokenText;
        lastCommandTime = Time.time;

        // === Ghost repel commands ===
        foreach (string word in ghostRepelWords)
        {
            if (spokenText.Contains(word))
            {
                Debug.Log("Voice Command Detected: Repel Ghost");
                ghostController?.RunAwayFromPlayer();
                return;
            }
        }

        // === Ghost anger commands ===
        foreach (string word in ghostAngerWords)
        {
            if (spokenText.Contains(word))
            {
                Debug.Log("Voice Command Detected: Anger Ghost");
                ghostController?.ChasePlayerFaster();
                return;
            }
        }

        // === Player commands ===
        if (spokenText.Contains("lari"))
        {
            Debug.Log("Voice Command Detected: Run");
            playerRunner?.RunForward();
        }
    }
}