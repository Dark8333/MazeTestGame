using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldScript : MonoBehaviour
{
    public void CheckingForAnEmptyString()
    {
        InputField textField = GetComponent<InputField>();
        if (textField.text.Length == 0)
        {
            textField.text = "0";
        }

    }
}
