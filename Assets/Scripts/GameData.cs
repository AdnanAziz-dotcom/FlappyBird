using UnityEngine;


[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
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
}
