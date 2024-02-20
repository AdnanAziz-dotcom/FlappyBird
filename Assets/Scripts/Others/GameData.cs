using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    [Header("Operator Menu")]
    public int bonusTickets;
    public int avgPayout;
    public int minTickets;
    public int perTicketValue;
    public int creditsPerGame;
    public bool freePlayMode;
    public bool ticketRedemptionMode;
    public bool enableRetry;
    [Range(0, 10)]
    public int attractVolume;
    [Range(0, 10)]
    public int gameVolume;

    [Space(5), Header("In Game Data --- Do Not Change !!!")]
    public bool isRetrying;
    public int retryCount;


    [Header("Game Parameters --- Do Not Change !!!")]
    public float adjustmentFactor;
    public int gamesPlayed;
    public int totalTicketsAwarded;
}
