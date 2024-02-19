using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bird : MonoBehaviour
{
    [SerializeField] float jumpSpeed = 100f;
    [SerializeField] GameObject hitEffect;
    private Rigidbody2D rb;
    private State state;
    Vector3 resetPosition;
    bool passedObstacle = false;
    Animator animator;
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
        resetPosition = transform.position;

    }
    private void OnEnable()
    {
        EventsHandler.ScoreUpdateEvent += OnPassObstacle;
        EventsHandler.GameStartEvent += OnGameStart;
    }
    private void OnDisable()
    {
        EventsHandler.ScoreUpdateEvent -= OnPassObstacle;
        EventsHandler.GameStartEvent -= OnGameStart;
    }

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
    private void ResetBird()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void Jump()
    {
        rb.velocity = Vector2.up * jumpSpeed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!passedObstacle)
        {
            state = State.Dead;
            StartCoroutine(WaitandReload());
            return;
        }
        if (state == State.Dead) return;
        Instantiate(hitEffect, transform.position, hitEffect.transform.rotation, transform);
        state = State.Dead;
        StartCoroutine(WaitandFinish());
    }

    IEnumerator WaitandReload()
    {

        EventsHandler.DeadEvent?.Invoke();
        animator.SetTrigger("Hit");
        yield return new WaitForSeconds(1.5f);
        ResetBird();
    }

    IEnumerator WaitandFinish()
    {
        EventsHandler.DeadEvent?.Invoke();
        animator.SetTrigger("Hit");
        yield return new WaitForSeconds(1.5f);
        EventsHandler.GameOverEvent?.Invoke();
    }

}
