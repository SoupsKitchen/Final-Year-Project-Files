using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class State_Stalk : IState
{
    private float _runSpeed = 7f, _accelSpeed = 10f, _turnSpeed = 600f;
    private float _minDistance = 10f, _maxDistance = 40f;

    private Transform _chosenSpot;
    private List<Transform> _peekSpots;
    private Pontianak_Behaviour _ctx;

    public State_Stalk(Pontianak_Behaviour ctx)
    {
        this._ctx = ctx;
    }

    // Start is called before the first frame update
    public void OnEnter()
    {
        _ctx.agent.speed = _runSpeed;
        _ctx.agent.angularSpeed = _turnSpeed;
        GameObject[] spotGOs = GameObject.FindGameObjectsWithTag("Peek");
        _peekSpots = new List<Transform>(spotGOs.Select(go => go.transform));
        ChoosePeekSpot();
        _ctx.targetLocation = _chosenSpot.position;
        Debug.Log("Pontianak is stalking!");
    }

    // Update is called once per frame
    public void OnUpdate()
    {
        //Always look at player
        Quaternion targetRot = Quaternion.LookRotation(_ctx.toPlayer);
        _ctx.transform.rotation = Quaternion.Slerp(_ctx.transform.rotation, targetRot, _ctx.agent.angularSpeed * Time.deltaTime);

        // If we reached the spot, pick another spot (or switch state)
        if (!_ctx.agent.pathPending && _ctx.agent.remainingDistance <= _ctx.agent.stoppingDistance + 0.5f)
        {
            ChoosePeekSpot();
            _ctx.targetLocation = _chosenSpot.position;
        }
    }

    public void OnExit()
    {

    }

    private void ChoosePeekSpot()
    {
        var candidates = _peekSpots
            .Where(spot =>
            {
                float d = Vector3.Distance(_ctx.player.transform.position, spot.position);
                return d >= _minDistance && d <= _maxDistance;
            })
            .ToList();
        

        if (candidates.Count == 0)
            {
                // fallback to any spot if none in range
                candidates = _peekSpots;
            }

        _chosenSpot = candidates[UnityEngine.Random.Range(0, candidates.Count)];
    }
}
