using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] AudioClip wonBattle;

    AudioSource source;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }

        source = GetComponent<AudioSource>();
    }

    public void PlayWonBattle()
    {
        if (wonBattle == null) return;

        source.clip = wonBattle;
        source.Play();
    }
}
