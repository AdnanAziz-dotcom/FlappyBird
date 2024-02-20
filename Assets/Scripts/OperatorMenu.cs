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
    }
}
public class GameSessionData
{
    public float bonusTickets; //Number of Tickets when player successfully passes 100 pipes
    public float avgPayout; //Average tickets the game should pay out.
    public float minTickets; //Minimum tickets for game play regardless of play
    public float perTicketValue; //Value of each ticket.
    public bool freePlayMode; //If set to Yes , then instead of INSERT CREDIT , FREE PLAY should flash and pressing any button will start the game
    public bool ticketRedemptionMode; //Yes will give SCORE and TICKETS Won , No will give only SCORE
    public bool enableRetry; //If set to yes , it will give 3 retries untill first obstacle is passed.
    public float attractVolume; //Volume of main scene
    public float gameVolume; //Volume of subsequent scenes
    public float creditsPerGame; //Number of times credit key needs to be pressed to start game.

}



//1.) Bonus Tickets : { bonustickets}
//vairable integer - Number of Tickets when player successfully passes 100 pipes

//2.) Average Ticket Payout : { avgpayout}
//Variable integer - Average tickets the game should pay out.

//3.) Minimum TIckets : { minticket}
//variable integer:  Minimum tickets for game play regardless of play

//4.) Per Ticket Value : { ticket_value}
//variable integer : Value of each ticket. 

//5.) Free Play Mode : { free_play_mode}
//Variable { 1 for yes, 0 for no - If set to Yes , then instead of INSERT CREDIT , FREE PLAY should flash and pressing any button will start the game}

//6.) Ticket Redemption Mode : { Yes or No }
//{ Yes will give SCORE and TICKETS Won , No will give only SCORE}

//7.) Enable Retry : If set to yes , it will give 3 retries untill first obstacle is passed.

//8.) Attract Volume : 0 - 10 { Volume of main scene}
//9.) Game Volume : 0 - 10 { Volume of subsequent scenes }

//10.) No of Credits Required Per Game : { { credit_per_game} }
//-Number of times credit key needs to be pressed to start game.

//10.)  Exit : exit game

//11.) Quit : Shutdown system