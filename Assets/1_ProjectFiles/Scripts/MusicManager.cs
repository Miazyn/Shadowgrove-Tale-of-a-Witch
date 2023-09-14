using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioClip PlayerBase;
    [SerializeField] AudioClip Town;
    [SerializeField] AudioClip Woods;

    AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();

        source.clip = PlayerBase;
        source.Play();
    }

}
