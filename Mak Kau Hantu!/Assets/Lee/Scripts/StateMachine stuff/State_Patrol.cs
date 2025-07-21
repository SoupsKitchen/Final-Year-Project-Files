using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_Patrol : IState
{
    private float _runSpeed = 40f, _accelSpeed = 10f, _turnSpeed = 600f;
    private Pontianak_Behaviour _ctx;

    public State_Patrol(Pontianak_Behaviour ctx)
    {
        this._ctx = ctx;
    }

    // Start is called before the first frame update
    public void OnEnter()
    {
        Debug.Log("Pontianak is patrolling!");
        _ctx.agent.speed = _runSpeed;
        _ctx.agent.angularSpeed = _turnSpeed;
    }

    // Update is called once per frame
    public void OnUpdate()
    {
        if (_ctx.ReachedDestination())
        {
            _ctx.StartCoroutine(_ctx.ChooseRandomSpot());
        }
    }

    public void OnExit()
    {
        if (_ctx.ChooseRandomSpot() != null)
        {
            _ctx.StopCoroutine(_ctx.ChooseRandomSpot());
        }

    }
    
    
}
