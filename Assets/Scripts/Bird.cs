using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static EventsHandler;
public class Bird : MonoBehaviour
{
    [SerializeField] float jumpSpeed = 100f;
    [SerializeField] GameObject hitEffect;
    private Rigidbody2D rb;
    private State state;
    bool passedObstacle = false;
    Animator animator;
    GameSessionData gameSessionData;
    [SerializeField] GameData gameData;
    public enum State
    {
        WaitingToPlay,
        Playing,
        Dead,
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        state = State.WaitingToPlay;
        animator = GetComponent<Animator>();

    }
    private void OnEnable()
    {
        ScoreUpdateEvent += OnPassObstacle;
        GameStartEvent += OnGameStart;
        GameSessionDataEvent += OnGameSessionData;
    }
    private void OnDisable()
    {
        ScoreUpdateEvent -= OnPassObstacle;
        GameStartEvent -= OnGameStart;
        GameSessionDataEvent -= OnGameSessionData;
    }
    private void OnGameSessionData(GameSessionData gameSessionData) => this.gameSessionData = gameSessionData;
    void OnGameStart()
    {
        state = State.Playing;
        rb.bodyType = RigidbodyType2D.Dynamic;
        Jump();
        animator.SetTrigger("Fly");
    }
    private void Update()
    {
        switch (state)
        {
            case State.WaitingToPlay:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //EventsHandler.GameStartEvent?.Invoke();
                    //OnGameStart();
                }
                break;
            case State.Playing:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                }
                break;
            case State.Dead:
                break;
        }
    }

    private void OnPassObstacle() => passedObstacle = true;

    private void Jump()
    {
        rb.velocity = Vector2.up * jumpSpeed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (state == State.Dead) return;
        if (gameSessionData.enableRetry && !passedObstacle)
            Retry();
        else
            StartCoroutine(WaitandFinish());
    }


    private void Retry()
    {
        state = State.Dead;
        if (PlayerData.GetRetryCount() < 3)
            StartCoroutine(WaitandReload());
        else
            StartCoroutine(WaitandFinish());

    }

    IEnumerator WaitandReload()
    {
        gameData.isRetrying = true;
        Debug.Log("try : " + PlayerData.GetRetryCount());
        PlayerData.AddRetryCount();
        DeadEvent?.Invoke();
        animator.SetTrigger("Hit");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator WaitandFinish()
    {
        gameData.isRetrying = false;
        state = State.Dead;
        PlayerData.ResetRetryCount();
        Instantiate(hitEffect, transform.position, hitEffect.transform.rotation, transform);
        DeadEvent?.Invoke();
        animator.SetTrigger("Hit");
        yield return new WaitForSeconds(1.5f);
        GameOverEvent?.Invoke();
    }
    private void OnApplicationQuit()
    {
        PlayerData.ResetRetryCount();
        gameData.isRetrying = false;
    }
}
