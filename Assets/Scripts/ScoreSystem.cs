using System;
using TMPro;
using Uduino;
using UnityEngine;
using static EventsHandler;
public class ScoreSystem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] TextMeshProUGUI ticketsText;
    [SerializeField] TextMeshProUGUI averageScoreText;

    float score = 0;

    //Data to be exported
    string timeStamp;
    GameSessionData gameSessionData;
    private void Start()
    {
        scoreText.text = 0.ToString();
        finalScoreText.text = 0.ToString();
        averageScoreText.text = PlayerData.GetAverageScore().ToString();

    }
    private void StartGame()
    {
        timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
    private void OnEnable()
    {
        DeadEvent += SetScore;
        ScoreUpdateEvent += UpdateScore;
        GameStartEvent += StartGame;
        GameSessionDataEvent += OnGameSessionData;
        GameOverEvent += OnGameOver;
    }
    private void OnDisable()
    {
        DeadEvent -= SetScore;
        ScoreUpdateEvent -= UpdateScore;
        GameStartEvent -= StartGame;
        GameSessionDataEvent -= OnGameSessionData;
        GameOverEvent -= OnGameOver;
    }

    private void OnGameOver()
    {
        Debug.Log("Game Over");
        ticketsText.transform.parent.gameObject.SetActive(gameSessionData.ticketRedemptionMode);
        int ticketWon = GetTicketsEvent?.Invoke((int)score) ?? 0;
        ticketsText.text = ticketWon.ToString();
        int numberOfTickets = ticketWon / gameSessionData.perTicketValue;
        UduinoManager.Instance.SendMessage("print:" + numberOfTickets);
    }

    private void OnGameSessionData(GameSessionData gameSessionData) => this.gameSessionData = gameSessionData;

    private void UpdateScore()
    {
        score++;
        scoreText.text = score.ToString();
        finalScoreText.text = score.ToString();
        GetUpdatedScoreEvent?.Invoke(score);
    }

    private void SetScore()
    {
        PlayerData.SetPlayerScore(score);
        PlayerData.SetPlayerGames();
        SaveRecord();
    }

    void SaveRecord()
    {
        // Example data
        Record newData = new Record
        {
            Time_Stamp = timeStamp,
            EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            Score = (int)score,
            Average_Score = PlayerData.GetAverageScore()
        };

        // Write data to CSV with the specified path
        DataExporter.WriteData(newData);

    }
}
