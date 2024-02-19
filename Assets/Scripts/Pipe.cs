using UnityEngine;

public class Pipe : MonoBehaviour
{
    bool isGameOver = false;
    bool scoreUpdated = false;
    Difficulty difficulty;
    public void SetParameters(Vector2 position, float zRotation, Difficulty difficulty)
    {
        this.difficulty = difficulty;
        this.transform.position = position;
        transform.localEulerAngles = new Vector3(0, 0, zRotation);

    }

    private void OnEnable()
    {
        EventsHandler.DeadEvent += GameOver;
    }
    private void OnDisable()
    {
        EventsHandler.DeadEvent -= GameOver;
    }
    void GameOver() => isGameOver = true;
    
    void Update()
    {
        if (isGameOver) return;
        transform.position += Vector3.left * difficulty.pipeSpeed * Time.deltaTime;
        if (transform.position.x < 0 && !scoreUpdated)
        {
            scoreUpdated = true;
            EventsHandler.ScoreUpdateEvent?.Invoke();
        }
        if (transform.position.x < -100)
        {
            Destroy(this.gameObject);
        }
    }

    public void FirstPipe()
    {
        scoreUpdated = true;
        EventsHandler.ScoreUpdateEvent?.Invoke();
        Destroy(this.gameObject);
    }
}

