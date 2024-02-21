using Unity.VisualScripting;
using UnityEngine;

public class ParallexEffect : MonoBehaviour
{
    [SerializeField] bool moveWithPipes = false;
    Difficulty difficulty;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] Transform[] backgrounds;
    [SerializeField] float threshold = -250;
    Vector3 resetPosition;
    bool move = false;  
    private void OnEnable()
    {
       EventsHandler.GameStartEvent += OnStartMoving;
        EventsHandler.DeadEvent += OnStopMoving;
    }
    private void OnDisable()
    {
        EventsHandler.GameStartEvent -= OnStartMoving;
        EventsHandler.DeadEvent -= OnStopMoving;
    }
    private void OnStopMoving()=> move = false;

    private void OnStartMoving()=> move = true;

    
    private void Start()
    {
        move = true;
        resetPosition = backgrounds[backgrounds.Length-1].position;
    }

    private void Update()
    {
        if (move)
        {
            MoveBackgrounds();
        }
    }

    private void MoveBackgrounds()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            SetSpeed();
            backgrounds[i].position += Vector3.left * moveSpeed * Time.deltaTime;
            if (backgrounds[i].position.x < threshold)
            {
                backgrounds[i].position = new Vector3(resetPosition.x, backgrounds[i].position.y, backgrounds[i].position.z);
            }
        }
        
    }

    private void SetSpeed()
    {
        if(moveWithPipes)
        {
            if (difficulty != null)
            {
                moveSpeed = difficulty.pipeSpeed;
            }
            else
            {
                difficulty = FindAnyObjectByType<GameManager>().GetDifficulty();
                moveSpeed = difficulty.pipeSpeed;
            }
        }
    }
}
