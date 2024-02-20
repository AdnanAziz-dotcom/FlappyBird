using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketPayoutSystem : MonoBehaviour
{
    [SerializeField] GameData gameData;
    [SerializeField] int MaxScore = 100;

    private void OnEnable()
    {
        EventsHandler.GetTicketsEvent += CalculateTickets;
    }

    private void OnDisable()
    {
        EventsHandler.GetTicketsEvent -= CalculateTickets;
    }
    public int CalculateTickets(int score)
    {
        float baseTickets = (score / (float)MaxScore) * gameData.avgPayout * gameData.adjustmentFactor;
        int tickets = Mathf.RoundToInt(baseTickets);

        if (score >= MaxScore) // if level completed //// or maximum score is reached
            tickets += gameData.bonusTickets;

        AdjustAdjustmentFactor(tickets);
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
