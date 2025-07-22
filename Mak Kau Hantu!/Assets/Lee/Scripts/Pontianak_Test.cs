using System.Collections;
using System.Collections.Generic;
using UnityEditor.EventSystems;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;
public class Pontianak_Test : MonoBehaviour
{
    public Transform eyes;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 toPlayer = (player.position - eyes.position);
        RaycastHit hit;
        if (Physics.Raycast(eyes.position, toPlayer, out hit, 25f))
        {
            Debug.Log(hit.collider.gameObject.name);
        }
        Debug.DrawRay(eyes.position, toPlayer, Color.red);
    }
}
