using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{

    [SerializeField] private float timeBtwEnemySpawn = 2f;

    [SerializeField] private SO_Unit[] EnemiesToSpawn;

    [SerializeField] private BattleSystem battleSystem;

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (battleSystem.state == BattleState.INACTIVE)
            {
                Debug.Log("Enemy could spawn");

                bool spawnEnemy = Random.Range(0, 2) == 1 ? true : false;

                if (Player.instance.GetComponent<PlayerController>().moveDirection != Vector2.zero)
                {
                    if (spawnEnemy)
                    {
                        Debug.Log("Spawn Enemyfight!");
                        SpawnEnemy();
                    }
                    else
                    {
                        Debug.Log("No enemy spawned");
                    }
                }
            }
            yield return new WaitForSeconds(timeBtwEnemySpawn);
        }
    }

    private void SpawnEnemy()
    {
        if(EnemiesToSpawn.Length <= 0)
        {
            Debug.LogError("No enemies to be spawned");
            return;
        }

        int randomIndex = Random.Range(0, EnemiesToSpawn.Length);

        SO_Unit randomEnemy = EnemiesToSpawn[randomIndex];

        //SPAWN ENEMY IN AND START BATTLE SEQUENCE
        if(battleSystem == null)
        {
            Debug.LogError("Battle System not selected!");
            return;
        }

        battleSystem.StartBattle(randomEnemy);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Player")
        {
            Debug.Log("Exit");
            StopCoroutine(SpawnEnemies());
        }
    }
}
