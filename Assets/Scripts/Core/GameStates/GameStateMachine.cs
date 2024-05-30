using System;
using UnityEngine;

public class GameStateMachine : IGameStateMachine
{
    private IGameState _currentState;

    public async void Initialize(IGameState startState)
    {
        Debug.Log($"GameStateMachine.Initialize{startState}");

        if (_currentState != null)
        {
            throw new InvalidOperationException($"{nameof(GameStateMachine)} is already Initialized!");
        }

        _currentState = startState;
        await _currentState.Enter();
        Debug.Log($"GameStateMachine.Initialize{startState} is FINISHED!");
    }

    public async void ChangeState(IGameState newState)
    {
        await _currentState.Exit();
        _currentState = newState;
        await _currentState.Enter();
    }
}