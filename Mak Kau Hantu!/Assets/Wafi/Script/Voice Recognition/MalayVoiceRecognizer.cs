using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using SimpleJSON;

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

    public PlayerRunner playerRunner; // Drag Player GameObject with PlayerRunner script here

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

        if (spokenText.Contains("mula permainan"))
        {
            Debug.Log("Voice Command Detected: Start Game");
            StartGame();
        }
        else if (spokenText.Contains("buka pintu"))
        {
            Debug.Log("Voice Command Detected: Open Door");
            OpenDoor();
        }
        else if (spokenText.Contains("ambil kunci"))
        {
            Debug.Log("Voice Command Detected: Pick Up Key");
            PickUpKey();
        }
        else if (spokenText.Contains("lari"))
        {
            Debug.Log("Voice Command Detected: Run");
            if (playerRunner != null)
            {
                playerRunner.RunForward();
            }
            else
            {
                Debug.LogWarning("PlayerRunner not assigned in Inspector.");
            }
        }
        else if (spokenText.Contains("letupkan dinding"))
        {
            Debug.Log("Voice Command Detected: Trigger Explosion");
            TriggerExplosion();
        }
        else
        {
            Debug.Log("Voice Command Detected: No known match found.");
        }
    }

    // Dummy gameplay methods for testing
    void StartGame() => Debug.Log("Game Started!");
    void OpenDoor() => Debug.Log("Door Opened!");
    void PickUpKey() => Debug.Log("Key Picked Up!");
    void TriggerExplosion() => Debug.Log("Explosion Triggered!");
}