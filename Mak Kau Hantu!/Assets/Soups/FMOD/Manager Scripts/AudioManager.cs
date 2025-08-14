using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    public static AudioManager AudioPlayer { get; private set; }

    private void Awake()
    {
        if (AudioPlayer != null && AudioPlayer != this)
        {
            Destroy(gameObject);
        }
        else
        {
            AudioPlayer = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayOneShot(EventReference soundEvent, Vector3 position)
    {
        RuntimeManager.PlayOneShot(soundEvent, position);
    }

    public void ChangeParameterValues(StudioEventEmitter emitter, string parametername, float value)
    {
        if (emitter != null && emitter.IsPlaying())
        {
            emitter.SetParameter(parametername, value);
        }
        else
        {
            Debug.LogWarning("No Emitter Found");
        }
    }

}
