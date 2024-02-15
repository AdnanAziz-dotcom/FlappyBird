using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMovement : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] Renderer bgRenderer;
    bool move = false;

    private void OnEnable()
    {
        EventsHandler.OnGameStart += StartGame;
        EventsHandler.OnDead += GameOver;
    }
    private void OnDisable()
    {
        EventsHandler.OnGameStart -= StartGame;
        EventsHandler.OnDead -= GameOver;
    }
    private void StartGame()
    {
      move = true;
    }

    private void GameOver()
    {
       move = false;
    }

    private void Update()
    {
        if (!move) return;
        bgRenderer.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0f);
    }
}
