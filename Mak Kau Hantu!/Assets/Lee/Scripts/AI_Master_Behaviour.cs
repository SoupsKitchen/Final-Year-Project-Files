using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AI_Master_Behaviour : MonoBehaviour
{
    private GameObject _pontianak;
    int _hourIdx = -1;
    float minutesPerHour = 300f; //One minute is 60f.

    void Awake()
    {
        _pontianak = GameObject.FindGameObjectWithTag("Pontianak");
        
    }
    void Start()
    {
        StartCoroutine(OnHour());
    }
    IEnumerator OnHour()
    {
        _hourIdx++;
        Pontianak_Behaviour _pAI = _pontianak.GetComponent<Pontianak_Behaviour>();
        _pAI.ProgressToNextSpot();
        
        yield return new WaitForSeconds(minutesPerHour);
    }

}
