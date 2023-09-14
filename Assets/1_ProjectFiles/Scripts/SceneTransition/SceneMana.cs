using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMana : MonoBehaviour
{
    [SerializeField] private string nextScene;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Scene transition!");

        //SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }
}
