using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMana : MonoBehaviour
{
    [SerializeField] private string nextScene;
    bool loadingScene = false;

    public void LoadNextScene(string _nextScene)
    {
        if (loadingScene) return;

        loadingScene = true;
        SimpleAudioManager.Manager.instance.StopSong(2f);

        StartCoroutine(DelayedLoad(_nextScene));
    }

    public IEnumerator DelayedLoad(string _nextScene)
    {
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(_nextScene, LoadSceneMode.Single);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Scene transition!");

        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }

    public void CloseApplication()
    {
        Application.Quit();
    }
}
