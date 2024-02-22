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
    public int creditCount;


    [Header("Game Parameters --- Do Not Change !!!")]
    public float adjustmentFactor;
    public int gamesPlayed;
    public int totalTicketsAwarded;

    public void SaveGameData(GameData gameData)
    {
        DataForJSON dataForJSON = new DataForJSON(gameData);
        string jsonData = JsonUtility.ToJson(dataForJSON);
        PlayerPrefs.SetString("GameData", jsonData);
        dataForJSON.GetDeserializedData(dataForJSON, gameData); //update current references as well
    }

    public void LoadGameData(GameData gameData)
    {
        if (PlayerPrefs.HasKey("GameData"))
        {
            string jsonData = PlayerPrefs.GetString("GameData");
            DataForJSON dataForJSON = JsonUtility.FromJson<DataForJSON>(jsonData);
            dataForJSON.GetDeserializedData(dataForJSON, gameData);
        }
        else
        {
            SaveGameData(gameData);
        }

    }

}


class DataForJSON
{
    public int bonusTickets;
    public int avgPayout;
    public int minTickets;
    public int perTicketValue;
    public int creditsPerGame;
    public bool freePlayMode;
    public bool ticketRedemptionMode;
    public bool enableRetry;
    public int attractVolume;
    public int gameVolume;
    //public bool isRetrying;
    //public int retryCount;
    //public int creditCount;
    public float adjustmentFactor;
    public int gamesPlayed;
    public int totalTicketsAwarded;

    public DataForJSON(GameData gameData)
    {
        bonusTickets = gameData.bonusTickets;
        avgPayout = gameData.avgPayout;
        minTickets = gameData.minTickets;
        perTicketValue = gameData.perTicketValue;
        creditsPerGame = gameData.creditsPerGame;
        freePlayMode = gameData.freePlayMode;
        ticketRedemptionMode = gameData.ticketRedemptionMode;
        enableRetry = gameData.enableRetry;
        attractVolume = gameData.attractVolume;
        gameVolume = gameData.gameVolume;
        //isRetrying = gameData.isRetrying;
        //retryCount = gameData.retryCount;
        //creditCount = gameData.creditCount;
        adjustmentFactor = gameData.adjustmentFactor;
        gamesPlayed = gameData.gamesPlayed;
        totalTicketsAwarded = gameData.totalTicketsAwarded;

    }

    public void GetDeserializedData(DataForJSON dataForJSON, GameData gameData)
    {
        gameData.bonusTickets = dataForJSON.bonusTickets;
        gameData.avgPayout = dataForJSON.avgPayout;
        gameData.minTickets = dataForJSON.minTickets;
        gameData.perTicketValue = dataForJSON.perTicketValue;
        gameData.creditsPerGame = dataForJSON.creditsPerGame;
        gameData.freePlayMode = dataForJSON.freePlayMode;
        gameData.ticketRedemptionMode = dataForJSON.ticketRedemptionMode;
        gameData.enableRetry = dataForJSON.enableRetry;
        gameData.attractVolume = dataForJSON.attractVolume;
        gameData.gameVolume = dataForJSON.gameVolume;
        //gameData.isRetrying = dataForJSON.isRetrying;
        //gameData.retryCount = dataForJSON.retryCount;
        //gameData.creditCount = dataForJSON.creditCount;
        gameData.adjustmentFactor = dataForJSON.adjustmentFactor;
        gameData.gamesPlayed = dataForJSON.gamesPlayed;
        gameData.totalTicketsAwarded = dataForJSON.totalTicketsAwarded;
    }
}