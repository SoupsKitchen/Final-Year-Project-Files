using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class StateMachine
{
    public IState _currentState;

    public void ChangeState(IState newState)
    {
        if (_currentState != null)
        {
            _currentState.OnExit();
        }
        else
        {
            _currentState = newState;
            _currentState.OnEnter();
        }
        

    }

    public void Update() => _currentState?.OnUpdate();
}
