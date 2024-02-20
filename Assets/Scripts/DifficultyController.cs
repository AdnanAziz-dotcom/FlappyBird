using UnityEngine;

public class DifficultyController : MonoBehaviour
{

    [Header("Hover Mouse for details")]
    [Tooltip("It isThe percentage difficulty added/subtracted from the current difficulty Parameters, \n 0.1f means 10% increase or decrease in the difficulty")]
    [SerializeField, Range(0.01f, 0.05f)] float baseLearningRate = 0.2f; // % value that added/subtracted from the difficulty Parameters
    //[SerializeField, Range(0.1f, 0.2f)] float multiplier = 0.1f; // % value that added/subtracted to the base learning rate, at start each time


    //[Header("Hover Mouse for details")]
    //[Tooltip("The Average score around which the player needs to stay")]
   // [SerializeField] float targetScore = 30f; //after this score, we will start increasing the difficulty

    [Header("The Upper and Lower Limit for the Pipe Position")]
    //determine upper and lower limit for the pipe position
    [SerializeField] float topLimitMax = 85f;
    [SerializeField] float topLimitMin = 50f;
    [SerializeField] float bottomLimitMax = -50f;
    [SerializeField] float bottomLimitMin = -85f;

    //Pipe Speed and Spawning Interval Limits, so that we can not go beyond these limits
    [Header("High Value = Higher Speed")]
    [SerializeField, Range(20, 35)] float minPipeSpeed = 20f;
    [SerializeField, Range(60, 85)] float maxPipeSpeed = 45f;
    // with the values, for instance, the Final range for the pipe speed will be 20 - 50

    [Header("Hover Mouse for details")]
    [Tooltip("Lower Value = Higher Spawning Speed,\n values are inversely proportional to difficluty, \n Lower values means higher diffuculty")]
    [SerializeField, Range(1.5f, 2.5f)] float minSpawnSpeed = 1.6f; //Higher Value means slower spawning
    [SerializeField, Range(0.5f, 1.3f)] float maxSpawnSpeed = 1.3f; //Lower Value means faster spawning
                                                                    // with the values, for instance, the Final range for the spawning interval will be 1.2 - 1.9





    float topMaxDefault = 60;  // 90 
    float topMinDefault = 40; // 60
    float bottomMaxDefault = -40; // -60
    float bottomMinDefault = -55; // -90
    [Header("Initial Difficulty Parameters")]
    [SerializeField] float pipeSpeedDefault = 30f;
    [SerializeField] float spawnIntervalDefault = 2f;

    private void OnEnable()
    {
        EventsHandler.SetDifficultyEvent += SetDifficultyParameters;
        EventsHandler.GetUpdatedDifficultyEvent += UpdateDifficulty;
    }
    private void OnDisable()
    {
        EventsHandler.SetDifficultyEvent -= SetDifficultyParameters;
        EventsHandler.GetUpdatedDifficultyEvent -= UpdateDifficulty;
    }

    private Difficulty SetDifficultyParameters(float averageScore)
    {
       // baseLearningRate = PlayerData.GetBaseLearningRate() == 0 ? baseLearningRate : PlayerData.GetBaseLearningRate();
        if (averageScore == 0) return GetDefaultDifficultyData(); ;//if player is playing for the first time, then return


        //if (averageScore >= targetScore) baseLearningRate *= 1 + multiplier;
        //else baseLearningRate *= 1 - multiplier;

        //Clamp the learning rate between 0.01 and 0.05
        //if (baseLearningRate < 0.01) baseLearningRate = 0.01f;
        //if (baseLearningRate > 0.05) baseLearningRate = 0.05f;

        PlayerData.SetBaseLearningRate(baseLearningRate);
        return GetDefaultDifficultyData();

        Difficulty GetDefaultDifficultyData()
        {
            Difficulty difficulty = new Difficulty();
            difficulty.topPipePositionRange = new Range(topMinDefault, topMaxDefault);
            difficulty.bottomPipePositionRange = new Range(bottomMinDefault, bottomMaxDefault);
            difficulty.pipeSpeed = pipeSpeedDefault;
            difficulty.spawnInterval = spawnIntervalDefault;
            return difficulty;
        }
    }

    private void UpdateDifficulty(Difficulty currentDifficulty)
    {
        IncreaseDifficulty(baseLearningRate, currentDifficulty);
    }

    private void IncreaseDifficulty(float learningRate, Difficulty currentDifficulty)
    {

        IncreasePipeSpeed();
        IncreaseSpawnSpeed();
        UpperPipe();
        LowerPipe();

        void UpperPipe()
        {
            //Decrease the range of the top pipe position, upto the minimum limit, so move down by decreasing
            if (currentDifficulty.topPipePositionRange.max > topLimitMin)
                currentDifficulty.topPipePositionRange.max *= 1 - learningRate;
            if (currentDifficulty.topPipePositionRange.max < topLimitMin)
                currentDifficulty.topPipePositionRange.max = topLimitMin;
        }

        void LowerPipe()
        {
            //Increase the range of the bottom pipe position, upto the maximum limit, so move up by increasing
            if (currentDifficulty.bottomPipePositionRange.min < bottomLimitMax) //-50
                currentDifficulty.bottomPipePositionRange.min *= 1 - learningRate;
            if (currentDifficulty.bottomPipePositionRange.min > bottomLimitMax) //-50
                currentDifficulty.bottomPipePositionRange.min = bottomLimitMax; // if exceeds the range, Reset to limit value
        }

        void IncreasePipeSpeed()
        {
            if (currentDifficulty.pipeSpeed < maxPipeSpeed) //50
                currentDifficulty.pipeSpeed *= 1 + learningRate;
            if (currentDifficulty.pipeSpeed > maxPipeSpeed) //50
                currentDifficulty.pipeSpeed = maxPipeSpeed; // if exceeds the range, Reset to limit value
        }

        void IncreaseSpawnSpeed()
        {
            if (currentDifficulty.spawnInterval > maxSpawnSpeed)
                currentDifficulty.spawnInterval *= 1 - learningRate;
            if (currentDifficulty.spawnInterval < maxSpawnSpeed)
                currentDifficulty.spawnInterval = maxSpawnSpeed; // if exceeds the range, Reset to limit value

        }
    }
    private void DecreaseDifficulty(float learningRate, Difficulty currentDifficulty)
    {

        DecreasePipeSpeed();
        DecreaseSpawnSpeed();
        UpperPipe();
        LowerPipe();


        void UpperPipe()
        {
            if (currentDifficulty.topPipePositionRange.min < topLimitMax) // 85
                currentDifficulty.topPipePositionRange.min *= 1 + learningRate;
            if (currentDifficulty.topPipePositionRange.min > topLimitMax) // 85
                currentDifficulty.topPipePositionRange.min = topLimitMax; // if exceeds the range, Reset to limit value
        }

        void LowerPipe()
        {
            if (currentDifficulty.bottomPipePositionRange.max > bottomLimitMin) //-85
                currentDifficulty.bottomPipePositionRange.max *= 1 + learningRate;
            if (currentDifficulty.bottomPipePositionRange.max < bottomLimitMin) //-85
                currentDifficulty.bottomPipePositionRange.max = bottomLimitMin; // if exceeds the range, Reset to limit value
        }

        void DecreasePipeSpeed()
        {
            if (currentDifficulty.pipeSpeed > minPipeSpeed) //20
                currentDifficulty.pipeSpeed *= 1 - learningRate;
            if (currentDifficulty.pipeSpeed < minPipeSpeed) //20
                currentDifficulty.pipeSpeed = minPipeSpeed; // if exceeds the range, Reset to limit value
        }

        void DecreaseSpawnSpeed()
        {
            if (currentDifficulty.spawnInterval < minSpawnSpeed)
                currentDifficulty.spawnInterval *= 1 + learningRate;
            if (currentDifficulty.spawnInterval > minSpawnSpeed)
                currentDifficulty.spawnInterval = minSpawnSpeed; // if exceeds the range, Reset to limit value
        }
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


public class Difficulty
{
    public Range topPipePositionRange;
    public Range bottomPipePositionRange;
    public float pipeSpeed;
    public float spawnInterval;
}
public class Range
{
    public float min;
    public float max;
    public Range(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}

public class PlayerData
{
    public static void SetPlayerScore(float score) => PlayerPrefs.SetFloat("Score", PlayerPrefs.GetFloat("Score", 0) + score);
    public static void SetPlayerGames() => PlayerPrefs.SetInt("Games", PlayerPrefs.GetInt("Games", 0) + 1);
    public static float GetAverageScore()
    {
        float totalScore = PlayerPrefs.GetFloat("Score", 0);
        float totalGames = PlayerPrefs.GetInt("Games", 0);
        return totalGames == 0 ? totalScore : (float)System.Math.Round(totalScore / totalGames, 2);
    }
    public static void SetBaseLearningRate(float LR) => PlayerPrefs.SetFloat("LR", LR);
    public static float GetBaseLearningRate() => PlayerPrefs.GetFloat("LR", 0);

    public static void ResetPlayerData()
    {
        PlayerPrefs.SetFloat("Score", 0);
        PlayerPrefs.SetInt("Games", 0);
        PlayerPrefs.SetFloat("LR", 0);
    }
}


