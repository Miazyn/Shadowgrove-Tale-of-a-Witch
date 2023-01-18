using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState CurrentState { get; private set; }

    public enum GameState
    {
        Normal,
        BuildMode,
        Dialog,
    }

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        CurrentState = GameState.Normal;
    }

    public void ChangeGameState(GameState _gameState)
    {
        CurrentState = _gameState;
    }

    public GameState GetCurrentState()
    {
        return CurrentState;
    }
}
