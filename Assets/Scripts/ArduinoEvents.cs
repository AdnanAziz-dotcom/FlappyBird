using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using static EventsHandler;
public class ArduinoEvents : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {

        //operator, spacebar, credit
        if (Input.GetKeyDown(KeyCode.O))
        {
            ArduinoEvent?.Invoke("operator");
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            ArduinoEvent?.Invoke("spacebar");
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            ArduinoEvent?.Invoke("credit");
        }
    }
}
