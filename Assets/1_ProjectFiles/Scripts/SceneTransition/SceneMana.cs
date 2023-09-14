using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMana : MonoBehaviour
{
    [SerializeField] private string nextScene;

    public void LoadNextScene(string _nextScene)
    {
        SceneManager.LoadScene(_nextScene, LoadSceneMode.Single);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Scene transition!");

        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }
}
