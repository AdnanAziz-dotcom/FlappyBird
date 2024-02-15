using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Pipe[] pipePrefabs;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] Button restartButton;
    [SerializeField] Button exitButton;
    [SerializeField] Button resetButton;
    Difficulty currentDifficulty = null;
    bool gameOver = false;

    private void Awake()
    {
        SetUpUI();
        SetUpGame();
    }

    private void SetUpUI()
    {
        mainMenu.SetActive(true);
        gameOverScreen.SetActive(false);
        restartButton.onClick.RemoveAllListeners();
        resetButton.onClick.RemoveAllListeners();
        resetButton.onClick.AddListener(() =>
        {
            PlayerData.ResetPlayerData();
            DataExporter.DeleteData();
            SceneManager.LoadScene(0);
        });
        restartButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        exitButton.onClick.AddListener(() => Application.Quit());
    }

    private void SetUpGame()
    {
        currentDifficulty = EventsHandler.OnSetDifficulty?.Invoke(PlayerData.GetAverageScore());
    }

    private void OnEnable()
    {
        EventsHandler.OnGameStart += StartGame;
        EventsHandler.OnDead += OnDead;
        EventsHandler.OnGetUpdatedScore += UpdateDifficulty;
        EventsHandler.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        EventsHandler.OnGameStart -= StartGame;
        EventsHandler.OnDead -= OnDead;
        EventsHandler.OnGetUpdatedScore -= UpdateDifficulty;
        EventsHandler.OnGameOver -= OnGameOver;
    }
    public void StartGame()
    {
        mainMenu.SetActive(false);
        StartCoroutine(SpawnPipes());
    }

    public void OnDead()
    {
        gameOver = true;
    }
    void OnGameOver()
    {
        gameOverScreen.SetActive(true);
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
    private void UpdateDifficulty(float score)
    {
        DebugLog(currentDifficulty, "Original Difficulty");
        EventsHandler.OnGetUpdatedDifficulty?.Invoke(currentDifficulty);
        DebugLog(currentDifficulty, "Changed Difficulty");
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
