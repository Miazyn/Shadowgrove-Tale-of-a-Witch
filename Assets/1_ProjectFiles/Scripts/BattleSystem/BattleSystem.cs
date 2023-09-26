using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState
{
    INACTIVE, START, PLAYERTURN, ENEMYTURN, WON, LOST
}

public class BattleSystem : MonoBehaviour
{
    public float battleCooldown = 2f;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    private Unit playerUnit;
    private Unit enemyUnit;

    public BattleHUD playerHud;
    public BattleHUD enemyHud;

    public TextMeshProUGUI dialogueText;

    public BattleState state;

    private int SongBefore;

    [SerializeField] private SimpleAudioManager.Manager songManager;

    void Start()
    {
        //TODO Manage this outside or inside other function.
        //ONly for testing purposes as of now!
        state = BattleState.INACTIVE;
    }

    public void StartBattle(SO_Unit _curEnemyUnit)
    {
        state = BattleState.START;

        SongBefore = songManager._currentSongIndex;

        Debug.Log($"Current song {songManager._currentSongIndex}");

        songManager.PlaySong(3);

        UIController.Instance.OpenBattleScreen();

        StartCoroutine(SetupBattle(_curEnemyUnit));
    }
    IEnumerator EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";

            StartCoroutine(UIController.Instance.CloseBattleScreen());

            MusicManager.Instance.PlayWonBattle();

            songManager.PlaySong(SongBefore);

            state = BattleState.INACTIVE;
            Debug.Log("Inactive");

            yield return new WaitForSeconds(battleCooldown);

        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated...";

            StartCoroutine(UIController.Instance.CloseBattleScreen());

            songManager.PlaySong(SongBefore);

            state = BattleState.INACTIVE;
            Debug.Log("Inactive");

            yield return new WaitForSeconds(battleCooldown);

        }
        
    }

    IEnumerator SetupBattle(SO_Unit _enemy)
    {
        GameObject playerGO = Instantiate(playerPrefab, transform);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, transform);
        enemyUnit = enemyGO.GetComponent<Unit>();

        if (_enemy != null)
        {
            enemyUnit.unitName = _enemy.unitName;
            enemyUnit.unitLevel = _enemy.unitLevel;

            enemyUnit.damage = _enemy.damage;

            enemyUnit.maxHP = _enemy.maxHP;
            enemyUnit.currentHP = enemyUnit.maxHP;
        }

        dialogueText.SetText("A wild " + enemyUnit.unitName + " approaches...");

        //playerUnit.untiName = Player.instance.PlayerName;

        playerHud.SetHUD(playerUnit);
        enemyHud.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;

        PlayerTurn();
    }

    private void PlayerTurn()
    {
        dialogueText.text = "Choose an action...";
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN) return;

        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN) return;

        StartCoroutine(PlayerHeal());
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHud.SetHP(enemyUnit.currentHP);

        dialogueText.text = "The attack is successful";

        state = BattleState.ENEMYTURN;

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(5);

        playerHud.SetHP(playerUnit.currentHP);
        dialogueText.text = "You feel renewed strength";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + " attacks!";

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHud.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }

    }

    
}
