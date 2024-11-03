using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // use enum state machine instead of generic BaseState classes for less bloat
    public enum GameState
    {
        PLAYING,
        PAUSED,
        SHOPPING
    }
    public GameState CurrentState { get; private set; }
    public Action<GameState> OnStateChanged = delegate { };

    private void Start()
    {
        ChangeState(GameState.PLAYING);
    }

    private void Update()
    {
        UpdateState(CurrentState);

        if(Input.GetKeyDown(KeyCode.P))
        {
            ChangeState(GameState.SHOPPING);
        }
    }

    private void UpdateState(GameState state)
    {
        switch (state)
        {
            case GameState.PLAYING:
                break;
            case GameState.PAUSED:
                break;
            case GameState.SHOPPING:
                break;
            default:
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        // exit current state
        switch(CurrentState)
        {
            case GameState.PLAYING:
                break;
            case GameState.PAUSED:
                break;
            case GameState.SHOPPING:
                break;
            default:
                break;
        }

        CurrentState = newState;

        // enter new state
        switch (newState)
        {
            case GameState.PLAYING:
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1f;
                break;
            case GameState.PAUSED:
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0f;
                break;
            case GameState.SHOPPING:
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0f;
                break;
            default:
                break;
        }

        OnStateChanged?.Invoke(CurrentState);
    }
}
