using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using static EventsHandler;
public class ArduinoEvents : MonoBehaviour
{

    //private void OnEnable()
    //{
    //    UduinoManager.Instance.OnDataReceived += OnDataReceived;
    //}

    //private void OnDisable()
    //{
    //    UduinoManager.Instance.OnDataReceived -= OnDataReceived;
    //}
    //private void OnDataReceived(string data, UduinoDevice device)
    //{
    //    if (data.Trim() == "operator")
    //    {
    //        ArduinoEvent?.Invoke("operator");
    //    }
    //    else if (data.Trim() == "spacebar")
    //    {
    //        ArduinoEvent?.Invoke("spacebar");
    //    }
    //    else if (data.Trim() == "credit")
    //    {
    //        ArduinoEvent?.Invoke("credit");
    //    }
    //}
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
