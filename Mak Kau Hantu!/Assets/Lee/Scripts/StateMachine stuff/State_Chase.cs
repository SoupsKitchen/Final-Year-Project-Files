using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Chase : IState
{
    private float _runSpeed = 40f, _turnSpeed = 100f;
    private Pontianak_Behaviour _ctx;

    public State_Chase(Pontianak_Behaviour ctx)
    {
        this._ctx = ctx;
    }
    // Start is called before the first frame update
    public void OnEnter()
    {
        _ctx.agent.speed = _runSpeed;
        _ctx.agent.angularSpeed = _turnSpeed;
        Debug.Log("Pontianak has begun chasing!");
    }

    // Update is called once per frame
    public void OnUpdate()
    {
        _ctx.agent.SetDestination(_ctx.targetLocation.position);

        //Once it reaches the location, the Pontianak will patrol around if it doesn't see the player
        if (!_ctx.agent.pathPending && _ctx.agent.remainingDistance <= _ctx.agent.stoppingDistance + 0.5f && _ctx.ChooseRandomPoint() == null)
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
