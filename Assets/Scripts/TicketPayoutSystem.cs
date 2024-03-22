using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventsHandler;

public class TicketPayoutSystem : MonoBehaviour
{
    GameData gameData;
    [SerializeField] int MaxScore = 100;

    private void OnEnable()
    {
        GetTicketsEvent += CalculateTickets;
        GetGameDataEvent += OnGetGameData;
    }

    private void OnDisable()
    {
        GetTicketsEvent -= CalculateTickets;
        GetGameDataEvent -= OnGetGameData;
    }
    private void OnGetGameData(GameData gd) => gameData = gd;
    public int CalculateTickets(int score)
    {
        float baseTickets = (score / (float)MaxScore) * gameData.avgPayout * gameData.adjustmentFactor;
        Debug.Log("Before Round OFF : " + baseTickets);
        int tickets = Mathf.RoundToInt(baseTickets);

        if (score >= MaxScore) // if level completed //// or maximum score is reached
            tickets += gameData.bonusTickets;

        AdjustAdjustmentFactor(tickets); // adjust TicketPayoutSystem adjustment factor for future payouts
        return tickets;
    }

    private void AdjustAdjustmentFactor(int tickets)
    {
        gameData.totalTicketsAwarded += tickets;
        gameData.gamesPlayed++;
        float actualPR = gameData.totalTicketsAwarded / gameData.gamesPlayed; // actual PayoutRate or Average PayoutRate
        if (actualPR > gameData.avgPayout)   // game data avgPayout is the expected/targeted payout rate
        {
            gameData.adjustmentFactor *= 0.95f; // Decrease AF to reduce future payouts
        }
        else if (actualPR < gameData.avgPayout)
        {
            gameData.adjustmentFactor *= 1.05f; // Increase AF to enhance future payouts
        }
    }
}
