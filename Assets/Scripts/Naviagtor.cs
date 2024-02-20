using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Naviagtor : MonoBehaviour
{
    public TMP_InputField[] inputFields;
    private int currentFieldIndex = 0;

    private void OnEnable()
    {
        currentFieldIndex = 0;
        inputFields[0].ActivateInputField();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Move to the previous input field
            SwitchInputField(-1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // Move to the next input field
            SwitchInputField(1);
        }
    }

    void SwitchInputField(int direction)
    {
        // Disable the current input field
        inputFields[currentFieldIndex].DeactivateInputField();

        // Move to the next/previous input field
        currentFieldIndex = (currentFieldIndex + direction + inputFields.Length) % inputFields.Length;

        // Enable the new input field
        inputFields[currentFieldIndex].ActivateInputField();
    }
}
