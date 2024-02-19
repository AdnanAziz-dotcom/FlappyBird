using System;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] TextMeshProUGUI averageScoreText;

    float score = 0;
    
    //Data to be exported
    string timeStamp;
    private void Start()
    {
        scoreText.text = 0.ToString();
        finalScoreText.text = 0.ToString();
        averageScoreText.text =PlayerData.GetAverageScore().ToString();

    }
    private void StartGame()
    {
        timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
    private void OnEnable()
    {
        EventsHandler.DeadEvent += SetScore;
        EventsHandler.ScoreUpdateEvent += UpdateScore;
        EventsHandler.GameStartEvent += StartGame;
    }
    private void OnDisable()
    {
        EventsHandler.DeadEvent -= SetScore;
        EventsHandler.ScoreUpdateEvent -= UpdateScore;
        EventsHandler.GameStartEvent -= StartGame;
    }

    private void UpdateScore()
    {
        score++;
        scoreText.text = score.ToString();
        finalScoreText.text = score.ToString();
        EventsHandler.GetUpdatedScoreEvent?.Invoke(score);
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
