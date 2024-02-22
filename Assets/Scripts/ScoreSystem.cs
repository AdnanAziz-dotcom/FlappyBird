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
    GameData gameData;
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
        GetGameDataEvent += OnGetGameData;
    }
    private void OnDisable()
    {
        DeadEvent -= SetScore;
        ScoreUpdateEvent -= UpdateScore;
        GameStartEvent -= StartGame;
        GameSessionDataEvent -= OnGameSessionData;
        GameOverEvent -= OnGameOver;
        GetGameDataEvent -= OnGetGameData;
    }

    private void OnGetGameData(GameData gd) => gameData = gd;

    private void OnGameOver()
    {
        Debug.Log("Game Over");
        ticketsText.transform.parent.gameObject.SetActive(gameSessionData.ticketRedemptionMode);
        if (gameSessionData.ticketRedemptionMode)
        { 
            // only come here if ticket redemption mode is enabled
            int ticketWon = GetTicketsEvent?.Invoke((int)score) ?? 0; // Ticket Calculation algorithm
            ticketsText.text = ticketWon.ToString();
            int numberOfTickets = ticketWon / gameSessionData.perTicketValue;
            UduinoManager.Instance.SendMessage("TICKETS=" + numberOfTickets); // Send tickets to the Arduino
        }
        gameData.SaveGameData(gameData);
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
