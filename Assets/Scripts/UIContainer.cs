using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static EventsHandler;

public class UIContainer : MonoBehaviour
{
    [Header("Start Screen - PrePlay Screen")]
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject prePlayObjects;
    [SerializeField] GameObject logo;
    [SerializeField] GameObject bonusTicketsIcon;
    [SerializeField] TextMeshProUGUI bonusTicketsText;
    [SerializeField] GameObject freePlayIcon;
    [SerializeField] GameObject insertCreditsIcon;

    [Header("Start Screen - Start Screen")]
    [SerializeField] GameObject startScreenObjects;
    [SerializeField] GameObject startScreenLogo;
    [SerializeField] GameObject startScreenInstruction;

    [Header("Game Over Screen")]
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] Button exitButton;
    [SerializeField] Button resetButton;

    [Header("Operator Menu")]
    [SerializeField] GameObject operatorScreen;

    GameSessionData gameSessionData;
    enum GameStates
    {
        OperatorMenu,
        PrePlay,
        StartScreen,
        ReadyToPlay,
        Playing,
        GameOver,
        CreditScreen
    }
    GameStates gameState;
    private void Awake() => AssignListeners();
    private void OnEnable()
    {
        DeadEvent += OnBirdDead;
        GameOverEvent += OnGameOver;
        GameSessionDataEvent += OnGameSessionData;
    }
    private void OnDisable()
    {
        DeadEvent -= OnBirdDead;
        GameOverEvent -= OnGameOver;
        GameSessionDataEvent -= OnGameSessionData;
    }
    private void OnGameSessionData(GameSessionData gameSessionData)
    {
        this.gameSessionData = gameSessionData;
        SetUpUI();
    }
    private void SetUpUI()
    {
        ActivateUI(GameStates.PrePlay);
        freePlayIcon.SetActive(gameSessionData.freePlayMode);
        insertCreditsIcon.SetActive(!gameSessionData.freePlayMode);
        bonusTicketsIcon.SetActive(gameSessionData.ticketRedemptionMode);
        logo.SetActive(!gameSessionData.ticketRedemptionMode);
        bonusTicketsText.text = gameSessionData.bonusTickets.ToString();

    }

    private void Update()
    {

        if (gameState == GameStates.Playing) return;

        if (gameState == GameStates.PrePlay && Input.GetKeyDown(KeyCode.O))
        {
            ActivateUI(GameStates.OperatorMenu);
        }

        if (gameSessionData.freePlayMode)
        {
            if (gameState == GameStates.PrePlay && Input.anyKeyDown)
            {
                ActivateUI(GameStates.StartScreen);
            }
            else if (gameState == GameStates.StartScreen && Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(StartGame());
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                //Open Credit Screen
            }
        }
    }

    IEnumerator StartGame()
    {
        ActivateUI(GameStates.ReadyToPlay);
        startScreenObjects.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(3);
        startScreenObjects.GetComponent<Animator>().enabled = false;
        ActivateUI(GameStates.Playing);
        GameStartEvent?.Invoke();
    }

    private void ActivateUI(GameStates gm)
    {
        gameState = gm;
        if (gm == GameStates.OperatorMenu)
        {
            operatorScreen.SetActive(true);
            startScreen.SetActive(false);
            gameOverScreen.SetActive(false);
        }
        else if (gm == GameStates.PrePlay)
        {
            operatorScreen.SetActive(false);
            startScreen.SetActive(true);
            gameOverScreen.SetActive(false);
            prePlayObjects.SetActive(true);
            startScreenObjects.SetActive(false);
        }
        else if (gm == GameStates.StartScreen)
        {
            operatorScreen.SetActive(false);
            startScreen.SetActive(true);
            gameOverScreen.SetActive(false);
            prePlayObjects.SetActive(false);
            startScreenObjects.SetActive(true);
        }
        else if (gm == GameStates.ReadyToPlay)
        {
            operatorScreen.SetActive(false);
            startScreen.SetActive(true);
            gameOverScreen.SetActive(false);
            startScreenLogo.SetActive(false);
            startScreenObjects.SetActive(true);
            startScreenInstruction.SetActive(false);
        }
        else if (gm == GameStates.Playing)
        {
            operatorScreen.SetActive(false);
            startScreen.SetActive(false);
            gameOverScreen.SetActive(false);
            startScreenObjects.SetActive(true);
            startScreenInstruction.SetActive(true);
        }
        else if (gm == GameStates.GameOver)
        {
            operatorScreen.SetActive(false);
            startScreen.SetActive(false);
            gameOverScreen.SetActive(true);
        }
    }
    private void AssignListeners()
    {
        resetButton.onClick.AddListener(() =>
        {
            PlayerData.ResetPlayerData();
            DataExporter.DeleteData();
            SceneManager.LoadScene(0);
        });
        exitButton.onClick.AddListener(() => Application.Quit());
    }

    private void OnBirdDead()
    {
        // isPlaying = false;
    }
    private void OnGameOver()
    {
        ActivateUI(GameStates.GameOver);
        StartCoroutine(RestartAfterDelay());
    }
    IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene(0);
    }
}
