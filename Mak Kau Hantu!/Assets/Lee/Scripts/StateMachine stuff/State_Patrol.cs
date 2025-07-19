using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_Patrol : IState
{
    private float _runSpeed = 20f, _turnSpeed = 50f;
    private Pontianak_Behaviour _ctx;

    public State_Patrol(Pontianak_Behaviour ctx)
    {
        this._ctx = ctx;
    }

    // Start is called before the first frame update
    public void OnEnter()
    {
        Debug.Log("Entered Patrol State!");
        _ctx.agent.speed = _runSpeed;
        _ctx.agent.angularSpeed = _turnSpeed;
    }

    // Update is called once per frame
    public void OnUpdate()
    {
        if (!_ctx.agent.pathPending && _ctx.agent.remainingDistance <= _ctx.agent.stoppingDistance + 0.5f)
        {
            _ctx.StartCoroutine(_ctx.ChooseRandomPoint());
        }
    }

    public void OnExit()
    {
        if (_ctx.ChooseRandomPoint() != null)
        {
            _ctx.StopCoroutine(_ctx.ChooseRandomPoint());
        }

    }
    
    
}
