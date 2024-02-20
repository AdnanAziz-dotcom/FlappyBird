using System;
using System.Collections;
using UnityEngine;
using static EventsHandler;

public class GameManager : MonoBehaviour
{
    [SerializeField] Pipe[] pipePrefabs;
    Difficulty currentDifficulty = null;
    bool gameOver = false;

    private void Awake()
    {
        SetUpGame();
    }


    private void SetUpGame()
    {
        currentDifficulty = EventsHandler.SetDifficultyEvent?.Invoke(PlayerData.GetAverageScore());
    }

    private void OnEnable()
    {
        GameStartEvent += OnStartGame;
        DeadEvent += OnDead;
        GetUpdatedScoreEvent += OnUpdateDifficulty;
    }

    private void OnDisable()
    {
        GameStartEvent -= OnStartGame;
        DeadEvent -= OnDead;
        GetUpdatedScoreEvent -= OnUpdateDifficulty;
    }
   
    public void OnStartGame()
    {
        StartCoroutine(SpawnPipes());
    }

    public void OnDead()
    {
        gameOver = true;
    }
    public Difficulty GetDifficulty() => currentDifficulty;

    int i = 0;
    IEnumerator SpawnPipes()
    {
        yield return new WaitForSeconds(2);
        while (!gameOver)
        {
            i++;
            Pipe pipe = Instantiate(pipePrefabs[UnityEngine.Random.Range(0,2)]);
            if (i % 2 == 0)
            {
                pipe.SetParameters(GetSpawnPosition(currentDifficulty.topPipePositionRange), -180, currentDifficulty);
            }
            else
            {
                pipe.SetParameters(GetSpawnPosition(currentDifficulty.bottomPipePositionRange), 0, currentDifficulty);
            }

            float randomSpaenInterval = UnityEngine.Random.Range(currentDifficulty.spawnInterval - 0.15f, currentDifficulty.spawnInterval + 0.15f);
            yield return new WaitForSeconds(randomSpaenInterval);
        }

        Vector2 GetSpawnPosition(Range range)
        {
            return new Vector2(100, UnityEngine.Random.Range(range.min, range.max));
        }
    }
    private void OnUpdateDifficulty(float score)
    {
       // DebugLog(currentDifficulty, "Original Difficulty");
        GetUpdatedDifficultyEvent?.Invoke(currentDifficulty);
       // DebugLog(currentDifficulty, "Changed Difficulty");
    }
    void DebugLog(Difficulty currentDifficulty, string msg)
    {
        Debug.Log(msg +
            "\n Pipe Speed " + currentDifficulty.pipeSpeed +
            "\n Spawn Interval " + currentDifficulty.spawnInterval +
            "\n Top Pipe Lower Limit " + currentDifficulty.topPipePositionRange.min +
            "\n Top Pipe Upper Limit " + currentDifficulty.topPipePositionRange.max +
            "\n Bottom Pipe Lower Limit " + currentDifficulty.bottomPipePositionRange.min +
            "\n Bottom Pipe Upper Limit " + currentDifficulty.bottomPipePositionRange.max);
    }

}
