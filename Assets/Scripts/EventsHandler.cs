using UnityEngine;

public class EventsHandler : MonoBehaviour
{
    public delegate void GameEvents();
    public static GameEvents OnGameStart;
    public static GameEvents OnDead;
    public static GameEvents OnGameOver;
    public static GameEvents OnScoreUpdate;

    public delegate void ScoreUpdate(float score);
    public static ScoreUpdate OnGetUpdatedScore;

    public delegate Difficulty SetDifficulty(float avgScore);
    public static SetDifficulty OnSetDifficulty;

    public delegate void UpdateDifficulty(Difficulty difficulty);
    public static UpdateDifficulty OnGetUpdatedDifficulty;

}
