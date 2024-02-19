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

    [Header("Game Over Screen")]
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] Button restartButton;
    [SerializeField] Button exitButton;
    [SerializeField] Button resetButton;

    [Header("Operator Menu")]
    [SerializeField] GameObject operatorScreen;

    GameSessionData gameSessionData;

    

    private void OnEnable()
    {
        GameStartEvent += OnStartGame;
        GameOverEvent += OnGameOver;
        GameSessionDataEvent += OnGameSessionData;
    }

    private void OnDisable()
    {
        GameStartEvent -= OnStartGame;
        GameOverEvent -= OnGameOver;
        GameSessionDataEvent -= OnGameSessionData;
    }
    private void OnGameSessionData(GameSessionData gameSessionData)
    {
        this.gameSessionData = gameSessionData;
        SetUpUI();
        Debug.Log("Bonus Tickets: " + gameSessionData.bonusTickets);
        Debug.Log("Average Payout: " + gameSessionData.avgPayout);
        Debug.Log("Minimum Tickets: " + gameSessionData.minTickets);
        Debug.Log("Per Ticket Value: " + gameSessionData.perTicketValue);
        Debug.Log("Credits Per Game: " + gameSessionData.creditsPerGame);
        Debug.Log("Free Play Mode: " + gameSessionData.freePlayMode);
        Debug.Log("Ticket Redemption Mode: " + gameSessionData.ticketRedemptionMode);
        Debug.Log("Enable Retry: " + gameSessionData.enableRetry);
        Debug.Log("Attract Volume: " + gameSessionData.attractVolume);
        Debug.Log("Game Volume: " + gameSessionData.gameVolume);

    }
    private void SetUpUI()
    {
        AssignListeners();
        operatorScreen.SetActive(false);
        startScreen.SetActive(true);
        prePlayObjects.SetActive(true);
        if (gameSessionData.ticketRedemptionMode)
        {
            bonusTicketsIcon.SetActive(true);
            logo.SetActive(false);
            bonusTicketsText.text = gameSessionData.bonusTickets.ToString();
        }
        else
        {
            bonusTicketsIcon.SetActive(false);
            logo.SetActive(true);
        }

        if (gameSessionData.freePlayMode)
        {
            freePlayIcon.SetActive(true);
            insertCreditsIcon.SetActive(false);
        }
        else
        {
            freePlayIcon.SetActive(false);
            insertCreditsIcon.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            operatorScreen.SetActive(true);
            startScreen.SetActive(false);

        }
    }
    private void AssignListeners()
    {
        gameOverScreen.SetActive(false);
        restartButton.onClick.RemoveAllListeners();
        resetButton.onClick.RemoveAllListeners();
        resetButton.onClick.AddListener(() =>
        {
            PlayerData.ResetPlayerData();
            DataExporter.DeleteData();
            SceneManager.LoadScene(0);
        });
        restartButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        exitButton.onClick.AddListener(() => Application.Quit());
    }

    private void OnStartGame()
    {
        startScreen.SetActive(false);
    }
    private void OnGameOver()
    {
        gameOverScreen.SetActive(true);
    }
}
