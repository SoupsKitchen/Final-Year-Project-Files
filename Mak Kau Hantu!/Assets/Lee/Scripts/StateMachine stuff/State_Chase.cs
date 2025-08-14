using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Chase : IState
{
    private float _runSpeed = 10f, _accelSpeed = 10f, _turnSpeed = 600f;
    private Pontianak_Behaviour _ctx;

    public State_Chase(Pontianak_Behaviour ctx)
    {
        this._ctx = ctx;
    }
    // Start is called before the first frame update
    public void OnEnter()
    {
        _ctx.agent.speed = _runSpeed;
        _ctx.agent.acceleration = _accelSpeed;
        _ctx.agent.angularSpeed = _turnSpeed;
        Debug.Log("Pontianak has begun chasing!");
    }

    // Update is called once per frame
    public void OnUpdate()
    {
        if (_ctx.seesPlayer())
        {
            _ctx.targetLocation = _ctx.player.transform.position;
        }
        //Once it reaches the location, the Pontianak will patrol around if it doesn't see the player
        else if (_ctx.ReachedDestination() && !_ctx.seesPlayer())
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
