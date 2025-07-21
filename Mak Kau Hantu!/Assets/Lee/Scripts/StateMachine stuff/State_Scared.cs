using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class State_Scared : IState
{
    private float _runSpeed = 40f, _accelSpeed = 10f, _turnSpeed = 600f;
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

        bool validLocation = false;
        int attempts = 0;

        //Calculating offsets
        Vector3 oppDirFromPlyr = -(_ctx.toPlayer);
        Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * 25f;
        Vector3 randomOffset3D = new Vector3(randomOffset.x, 0f, randomOffset.y);

        //Begin finding a suitable location to path to
        NavMeshHit hit;
        while (!validLocation && attempts <= 10)
        {
            Vector3 newTargetLocation = _ctx.transform.position + oppDirFromPlyr * 50f + randomOffset3D;
            if (NavMesh.SamplePosition(newTargetLocation, out hit, 25f, NavMesh.AllAreas))
            {
                _ctx.targetLocation = newTargetLocation;
                validLocation = true;
            }
            else
            {
                attempts++;
            }
        }

        if (!validLocation)
        {
            Debug.LogWarning("The Pontianak could not find a suitable location!");
        }
    }

    // Update is called once per frame
    public void OnUpdate()
    {
        if (_ctx.ReachedDestination() && !_ctx.isResting)
        {
            _ctx.StartCoroutine(_ctx.BeginCooldown(10f));
        }
    }

    public void OnExit()
    {

    }
}
