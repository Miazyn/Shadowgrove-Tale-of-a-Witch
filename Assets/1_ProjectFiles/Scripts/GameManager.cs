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

    public class PlayerStat
    {
        public int endurance;
        public int health;
        public List<Player.FriendshipStats> playerFriendships;
        public int money;
        public string playerName;
        public bool firstStart;
        public PlayerStat(int _e, int _h, int _m, string _pn, List<Player.FriendshipStats> friendshipStats, bool _firstStart)
        {
            endurance = _e;
            health = _h;
            money = _m;
            playerName = _pn;
            playerFriendships = friendshipStats;
            firstStart = _firstStart;
        }
    }

    PlayerStat curPlayerStat;

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


    public void SetPlayerStat()
    {
        Debug.Log("Before player stats");

        

        //Debug.Log($"Set player stats: {curPlayerStat.money}");

        //Player.instance.SetEndurance(curPlayerStat.endurance);
        //Player.instance.SetHealth(curPlayerStat.health);
        //Player.instance.SetMoney(curPlayerStat.money);
        //Player.instance.PlayerName = curPlayerStat.playerName;

        //Player.instance.firstStart = curPlayerStat.firstStart;

        //Player.instance.allFriendships = curPlayerStat.playerFriendships;
    }

    public void SavePlayerStat(int _endurance, int _health, List<Player.FriendshipStats> friendshipStats, int _money, string _name, bool firstStart)
    {
        Debug.Log("Before Save");
        curPlayerStat = new PlayerStat(_endurance, _health, _money, _name, friendshipStats, firstStart);
        Debug.Log($"After Save: {_money}");
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
