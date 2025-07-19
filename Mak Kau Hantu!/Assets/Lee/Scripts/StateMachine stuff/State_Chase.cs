using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Chase : IState
{
    private float _runSpeed = 40f;
    private Pontianak_Behaviour _ctx;

    public State_Chase(Pontianak_Behaviour ctx)
    {
        this._ctx = ctx;
    }
    // Start is called before the first frame update
    public void OnEnter()
    {
        _ctx.agent.speed = _runSpeed;
        Debug.Log("Pontianak has begun chasing!");
    }

    // Update is called once per frame
    public void OnUpdate()
    {
        _ctx.agent.SetDestination(_ctx.targetLocation.position);
    }

    public void OnExit()
    {
        
    }
}
