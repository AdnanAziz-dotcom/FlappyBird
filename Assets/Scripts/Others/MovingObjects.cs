using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjects : MonoBehaviour
{
    private void Start()
    {
        Texture2D texture = GetComponent<SpriteRenderer>().sprite.texture;
        Debug.Log("Widht :" + texture.width);
        Debug.Log("    =====   "+ texture.width / GetComponent<SpriteRenderer>().sprite.pixelsPerUnit);
    }
    private void OnBecameInvisible()
    {
        Time.timeScale = 0;
    }
}
