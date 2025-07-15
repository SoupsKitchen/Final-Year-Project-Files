    using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class StateMachine
{
    private IState _currentState;

    public void ChangeState(IState newState)
    {

    }

    public void Update() => _currentState?.OnUpdate();
}
