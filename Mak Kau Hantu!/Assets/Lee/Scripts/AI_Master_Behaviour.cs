using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AI_Master_Behaviour : MonoBehaviour
{
    public float currentGameHour = 0f; // 0 = 12:00 AM, 6 = 6:00 AM
    public float gameEndHour = 6f;
    public float realTimeDurationMinutes = 10f;

    private GameObject _pontianak;
    private float secondsPerGameHour;
    private float elapsedRealTime = 0f;
    private int lastCalledHour = -1;

    void Start()
    {
        _pontianak = GameObject.FindGameObjectWithTag("Pontianak");
        secondsPerGameHour = (realTimeDurationMinutes * 60f) / gameEndHour; // 300 seconds = 5 mins
    }

    void Update()
    {
        elapsedRealTime += Time.deltaTime;
        currentGameHour = Mathf.Clamp(elapsedRealTime / secondsPerGameHour, 0f, gameEndHour);

        int currentHourInt = Mathf.FloorToInt(currentGameHour);

        if (currentHourInt != lastCalledHour && currentHourInt <= gameEndHour)
        {
            OnHour(currentHourInt);
            lastCalledHour = currentHourInt;
        }

        if (currentGameHour >= gameEndHour)
        {
            EndNight();
        }

        Debug.Log($"Current Game Hour: {currentGameHour}, Floor: {currentHourInt}, Last Called: {lastCalledHour}");
    }

    void OnHour(int hour)
    {
        Pontianak_Behaviour _pontianakBehaviour = _pontianak.GetComponent<Pontianak_Behaviour>();
        if (_pontianakBehaviour != null)
        {
            Debug.Log("It's the hour!");
            _pontianakBehaviour.UpdatePatrol();
        }
    }

    void EndNight()
    {
        Debug.Log("Night cycle complete.");
        // Handle end-of-night logic
        enabled = false; // Stop updating time
    }
}
