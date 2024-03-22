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
    [SerializeField] TextMeshProUGUI insertCreditsText;

    [Header("Start Screen - Start Screen")]
    [SerializeField] GameObject startScreenObjects;
    [SerializeField] GameObject startScreenLogo;
    [SerializeField] GameObject startScreenInstruction;
    [SerializeField] GameObject tryAgainLabel;


    [Header("Game Over Screen")]
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] Button exitButton;
    [SerializeField] Button resetButton;

    [Header("Operator Menu")]
    [SerializeField] GameObject operatorScreen;


    [Header("In Game UIs")]
    [SerializeField] GameObject inGameUIs;


    GameData gameData;

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
        ArduinoEvent += OnArduinoEventRecieved;
        GetGameDataEvent += OnGetGameData;
    }
    private void OnDisable()
    {
        DeadEvent -= OnBirdDead;
        GameOverEvent -= OnGameOver;
        GameSessionDataEvent -= OnGameSessionData;
        ArduinoEvent -= OnArduinoEventRecieved;
        GetGameDataEvent -= OnGetGameData;
    }

    private void OnGetGameData(GameData gd) => gameData = gd;

    private void OnGameSessionData(GameSessionData gameSessionData)
    {
        this.gameSessionData = gameSessionData;
        gameData.creditCount = gameData.creditsPerGame;
        SetUpUI();
    }
    private void SetUpUI()
    {
        if (gameData.isRetrying) ActivateUI(GameStates.StartScreen);
        else ActivateUI(GameStates.PrePlay);

        freePlayIcon.SetActive(gameSessionData.freePlayMode);
        insertCreditsIcon.SetActive(!gameSessionData.freePlayMode);
        insertCreditsText.text = 1 + "/" + gameSessionData.creditsPerGame;
        bonusTicketsIcon.SetActive(gameSessionData.ticketRedemptionMode);
        logo.SetActive(!gameSessionData.ticketRedemptionMode);
        bonusTicketsText.text = gameSessionData.bonusTickets+ " Tickets";
        tryAgainLabel.SetActive(gameData.isRetrying);

    }

    private void Update()
    {
        return;
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
            if (!gameData.isRetrying && Input.GetKeyDown(KeyCode.C))
            {
                OnCreditInserted();
            }
            else if (gameState == GameStates.StartScreen && Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(StartGame());
            }
        }
    }


    private void OnArduinoEventRecieved(string message)
    {
        Debug.Log("Arduino Event Recieved: " + message);

        if (gameState == GameStates.Playing) return;

        if (gameState == GameStates.PrePlay && message == "operator")
        {
            ActivateUI(GameStates.OperatorMenu);
        }

        if (gameSessionData.freePlayMode)
        {
            if (gameState == GameStates.PrePlay)
            {
                ActivateUI(GameStates.StartScreen);
            }
            else if (gameState == GameStates.StartScreen && message == "spacebar")
            {
                StartCoroutine(StartGame());
            }
        }
        else
        {
            if (!gameData.isRetrying && message == "credit")
            {
                OnCreditInserted();
            }
            else if (gameState == GameStates.StartScreen && message == "spacebar")
            {
                StartCoroutine(StartGame());
            }
        }
    }

    void OnCreditInserted()
    {
        gameData.creditCount -= 1;
        SetCreditCountText(gameData.creditCount);
        if (gameData.creditCount <= 0)
        {
            gameData.creditCount = gameData.creditsPerGame;
            ActivateUI(GameStates.StartScreen);
            // StartCoroutine(StartGame());
        }
    }

    void SetCreditCountText(int cc)
    {
        switch (cc)
        {
            case 1:

                insertCreditsText.text = 3 + "/" + gameSessionData.creditsPerGame;
                break;
            case 2:

                insertCreditsText.text = 2 + "/" + gameSessionData.creditsPerGame;
                break;
            case 3:
                insertCreditsText.text = 1 + "/" + gameSessionData.creditsPerGame;
                break;
        }

    }

    IEnumerator StartGame()
    {
        tryAgainLabel.SetActive(false);
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
            inGameUIs.SetActive(false);
        }
        else if (gm == GameStates.PrePlay)
        {
            operatorScreen.SetActive(false);
            startScreen.SetActive(true);
            gameOverScreen.SetActive(false);
            prePlayObjects.SetActive(true);
            startScreenObjects.SetActive(false);
            inGameUIs.SetActive(false);
        }
        else if (gm == GameStates.StartScreen)
        {
            operatorScreen.SetActive(false);
            startScreen.SetActive(true);
            gameOverScreen.SetActive(false);
            prePlayObjects.SetActive(false);
            startScreenObjects.SetActive(true);
            inGameUIs.SetActive(false);
        }
        else if (gm == GameStates.ReadyToPlay)
        {
            operatorScreen.SetActive(false);
            startScreen.SetActive(true);
            gameOverScreen.SetActive(false);
            startScreenLogo.SetActive(false);
            startScreenObjects.SetActive(true);
            startScreenInstruction.SetActive(false);
            inGameUIs.SetActive(false);
        }
        else if (gm == GameStates.Playing)
        {
            operatorScreen.SetActive(false);
            startScreen.SetActive(false);
            gameOverScreen.SetActive(false);
            startScreenObjects.SetActive(true);
            startScreenInstruction.SetActive(true);
            inGameUIs.SetActive(true);
        }
        else if (gm == GameStates.GameOver)
        {
            operatorScreen.SetActive(false);
            startScreen.SetActive(false);
            gameOverScreen.SetActive(true);
            inGameUIs.SetActive(false);
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
