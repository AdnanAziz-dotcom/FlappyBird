using UnityEngine;

public class EventsHandler : MonoBehaviour
{
    public delegate void GameEvents();
    public static GameEvents GameStartEvent;
    public static GameEvents DeadEvent;
    public static GameEvents GameOverEvent;
    public static GameEvents ScoreUpdateEvent;

    public delegate void ScoreUpdate(float score);
    public static ScoreUpdate GetUpdatedScoreEvent;

    public delegate Difficulty SetDifficulty(float avgScore);
    public static SetDifficulty SetDifficultyEvent;

    public delegate void UpdateDifficulty(Difficulty difficulty);
    public static UpdateDifficulty GetUpdatedDifficultyEvent;

    public delegate void GetGameSessionData(GameSessionData gameSessionData);
    public static GetGameSessionData GameSessionDataEvent;

}
