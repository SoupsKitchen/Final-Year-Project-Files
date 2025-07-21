using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_Scared : IState
{
    private float _runSpeed = 40f, _turnSpeed = 100f;
    private Pontianak_Behaviour _ctx;

    public State_Scared(Pontianak_Behaviour ctx)
    {
        this._ctx = ctx;
    }

    // Start is called before the first frame update
    public void OnEnter()
    {
        _ctx.agent.speed = _runSpeed;
        _ctx.agent.angularSpeed = _turnSpeed;
        Debug.Log("Pontianak is running away!");
    }

    // Update is called once per frame
    public void OnUpdate()
    {
        if (!_ctx.agent.pathPending && _ctx.agent.remainingDistance <= _ctx.agent.stoppingDistance + 0.5f)
        {
            _ctx.StartCoroutine(_ctx.ScaredCooldown());
        }
    }

    public void OnExit()
    {

    }
}
