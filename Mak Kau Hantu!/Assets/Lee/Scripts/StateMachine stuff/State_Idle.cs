using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Idle : IState
{
    private float _runSpeed = 10f;
    private Pontianak_Behaviour _ctx;

    public State_Idle(Pontianak_Behaviour ctx)
    {
        this._ctx = ctx;
    }

    // Start is called before the first frame update
    public void OnEnter()
    {
        _ctx.agent.speed = _runSpeed;
        Debug.Log("Pontianak is idle!");
    }

    // Update is called once per frame
    public void OnUpdate()
    {

    }

    public void OnExit()
    {
        Debug.Log("Exitted idle state!");
    }
}
