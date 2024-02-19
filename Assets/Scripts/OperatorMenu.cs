using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OperatorMenu : MonoBehaviour
{
    [SerializeField] TMP_InputField bonusTickets;
    [SerializeField] TMP_InputField avgPayout;
    [SerializeField] TMP_InputField minTickets;
    [SerializeField] TMP_InputField perTicketValue;
    [SerializeField] TMP_InputField creditsPerGame;
    [SerializeField] Toggle freePlayMode;
    [SerializeField] Toggle ticketRedemptionMode;
    [SerializeField] Toggle enableRetry;
    [SerializeField] TMP_InputField attractVolume;
    [SerializeField] TMP_InputField gameVolume;
    [SerializeField] Button saveButton;

    GameSessionData gameSessionData;

    private void Start()
    {
        gameSessionData = new GameSessionData();
        saveButton.onClick.AddListener(() => SaveData());
        SaveData();
    }

    private void SaveData()
    {
        gameSessionData.bonusTickets = float.Parse(bonusTickets.text);
        gameSessionData.avgPayout = float.Parse(avgPayout.text);
        gameSessionData.minTickets = float.Parse(minTickets.text);
        gameSessionData.perTicketValue = float.Parse(perTicketValue.text);
        gameSessionData.creditsPerGame = float.Parse(creditsPerGame.text);
        gameSessionData.freePlayMode = freePlayMode.isOn;
        gameSessionData.ticketRedemptionMode = ticketRedemptionMode.isOn;
        gameSessionData.enableRetry = enableRetry.isOn;
        gameSessionData.attractVolume = float.Parse(attractVolume.text);
        gameSessionData.gameVolume = float.Parse(gameVolume.text);
        EventsHandler.GameSessionDataEvent?.Invoke(gameSessionData);
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