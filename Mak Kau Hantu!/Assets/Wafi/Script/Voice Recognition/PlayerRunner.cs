using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerRunner : MonoBehaviour
{
    public float runSpeed = 5f;
    private bool shouldRun = false;
    private float runTime = 0f;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (shouldRun)
        {
            Vector3 move = transform.forward * runSpeed;
            controller.Move(move * Time.deltaTime);

            runTime -= Time.deltaTime;
            if (runTime <= 0f)
            {
                shouldRun = false;
                Debug.Log("Player stopped running.");
            }
        }
    }

    public void RunForward(float duration = 2f)
    {
        shouldRun = true;
        runTime = duration;
        Debug.Log("Player started running.");
    }
}