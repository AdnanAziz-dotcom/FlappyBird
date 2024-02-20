using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventsHandler;
public class AudioController : MonoBehaviour
{
    [SerializeField] AudioSource attractMusic;
    [SerializeField] AudioSource gameMusic;
    private void OnEnable()
    {
        GameStartEvent += OnGameStart;
        GameSessionDataEvent += OnGameSessionData;
    }

    private void OnDisable()
    {
        GameStartEvent -= OnGameStart;
        GameSessionDataEvent -= OnGameSessionData;
    }

    private void OnGameSessionData(GameSessionData gameSessionData)
    {
        attractMusic.volume = Mathf.Clamp((gameSessionData.attractVolume/10), 0, 1);
        gameMusic.volume = Mathf.Clamp((gameSessionData.gameVolume/10), 0, 1);
        if (!attractMusic.isPlaying) attractMusic.Play();
    }

    private void OnGameStart()
    {
        attractMusic.Stop();
        gameMusic.Play();
    }
}
