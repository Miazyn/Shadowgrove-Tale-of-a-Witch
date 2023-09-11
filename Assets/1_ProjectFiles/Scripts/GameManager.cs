using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState CurrentState { get; private set; }

    
    public delegate void OnAnyMenuToggled(UIController.Menu _menu);
    public OnAnyMenuToggled onAnyMenuToggleCallback;

    public delegate void OnMenuClosed();
    public OnMenuClosed onMenuClosedCallback;

    public delegate void OnPlayerHealthChange(int maxhealth, int curhealth);
    public OnPlayerHealthChange onPlayerHealthChangeCallback; 
    
    public delegate void OnPlayerEnduranceChange(int maxEndurance, int curEndurance);
    public OnPlayerEnduranceChange onPlayerEnduranceChangeCallback;
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
