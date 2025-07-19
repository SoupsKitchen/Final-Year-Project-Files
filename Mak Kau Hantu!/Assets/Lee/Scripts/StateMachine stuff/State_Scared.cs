using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_Scared : IState
{
    private float _runSpeed = 40f;
    private Pontianak_Behaviour _ctx;

    public State_Scared(Pontianak_Behaviour ctx)
    {
        this._ctx = ctx;
    }

    // Start is called before the first frame update
    public void OnEnter()
    {
        _ctx.agent.speed = _runSpeed;
        Debug.Log("Pontianak is running away!");
    }

    // Update is called once per frame
    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}
