using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class StateMachine
{
    public IState currentState;

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        else
        {
            currentState = newState;
            currentState.OnEnter();
        }
        

    }

    public void Update() => currentState?.OnUpdate();
}
