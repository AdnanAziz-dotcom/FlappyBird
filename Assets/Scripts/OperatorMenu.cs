using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OperatorMenu : MonoBehaviour
{
    [SerializeField] GameData gameData;
    [SerializeField] TMP_InputField bonusTickets;
    [SerializeField] TMP_InputField avgPayout;
    [SerializeField] TMP_InputField minTickets;
    [SerializeField] TMP_InputField perTicketValue;
    [SerializeField] TMP_InputField creditsPerGame;
    [SerializeField] TMP_InputField freePlayMode;
    [SerializeField] TMP_InputField ticketRedemptionMode;
    [SerializeField] TMP_InputField enableRetry;
    [SerializeField] TMP_InputField attractVolume;
    [SerializeField] TMP_InputField gameVolume;
    [SerializeField] Button saveButton;
    GameSessionData gameSessionData;

    
    private void Start()
    {
        gameData.LoadGameData(gameData);
        EventsHandler.GetGameDataEvent?.Invoke(gameData);
        PopulateFields();
        gameSessionData = new GameSessionData();
        SaveSessionData();
        saveButton.onClick.AddListener(() => SaveData());
    }

    private void PopulateFields()
    {
        bonusTickets.text = gameData.bonusTickets.ToString();
        avgPayout.text = gameData.avgPayout.ToString();
        minTickets.text = gameData.minTickets.ToString();
        perTicketValue.text = gameData.perTicketValue.ToString();
        creditsPerGame.text = gameData.creditsPerGame.ToString();
        freePlayMode.text = Convert.ToInt16(gameData.freePlayMode).ToString();
        ticketRedemptionMode.text = Convert.ToInt16(gameData.ticketRedemptionMode).ToString();
        enableRetry.text = Convert.ToInt16(gameData.enableRetry).ToString();
        attractVolume.text = gameData.attractVolume.ToString();
        gameVolume.text = gameData.gameVolume.ToString();
    }
    private void SaveSessionData()
    {
        gameSessionData.bonusTickets = gameData.bonusTickets;
        gameSessionData.avgPayout = gameData.avgPayout;
        gameSessionData.minTickets = gameData.minTickets;
        gameSessionData.perTicketValue = gameData.perTicketValue;
        gameSessionData.creditsPerGame = gameData.creditsPerGame;
        gameSessionData.freePlayMode = gameData.freePlayMode;
        gameSessionData.ticketRedemptionMode = gameData.ticketRedemptionMode;
        gameSessionData.enableRetry = gameData.enableRetry;
        gameSessionData.attractVolume = gameData.attractVolume;
        gameSessionData.gameVolume = gameData.gameVolume;
        EventsHandler.GameSessionDataEvent?.Invoke(gameSessionData);
    }
    private void SaveData()
    {
        gameData.bonusTickets = int.Parse(bonusTickets.text);
        gameData.avgPayout = int.Parse(avgPayout.text);
        gameData.minTickets = int.Parse(minTickets.text);
        gameData.perTicketValue = int.Parse(perTicketValue.text);
        gameData.creditsPerGame = int.Parse(creditsPerGame.text);
        gameData.freePlayMode = Convert.ToBoolean(int.Parse(freePlayMode.text));
        gameData.ticketRedemptionMode = Convert.ToBoolean(int.Parse(ticketRedemptionMode.text));
        gameData.enableRetry = Convert.ToBoolean(int.Parse(enableRetry.text));
        gameData.attractVolume = int.Parse(attractVolume.text);
        gameData.gameVolume = int.Parse(gameVolume.text);
        SaveSessionData();
        gameData.SaveGameData(gameData);
    }
}
public class GameSessionData
{
    public int bonusTickets; //Number of Tickets when player successfully passes 100 pipes
    public int avgPayout; //Average tickets the game should pay out.
    public int minTickets; //Minimum tickets for game play regardless of play
    public int perTicketValue; //Value of each ticket.
    public bool freePlayMode; //If set to Yes , then instead of INSERT CREDIT , FREE PLAY should flash and pressing any button will start the game
    public bool ticketRedemptionMode; //Yes will give SCORE and TICKETS Won , No will give only SCORE
    public bool enableRetry; //If set to yes , it will give 3 retries untill first obstacle is passed.
    public int attractVolume; //Volume of main scene
    public int gameVolume; //Volume of subsequent scenes
    public int creditsPerGame; //Number of times credit key needs to be pressed to start game.

}

