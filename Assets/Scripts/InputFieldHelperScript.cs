using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 


public class InputFieldHelperScript : MonoBehaviour
{
    private TMP_InputField _inputField; 
    void Start()
    {
        _inputField = GetComponent<TMP_InputField>(); 
        _inputField.onValueChanged.AddListener(delegate{CheckInput(); }); 
    }

    void CheckInput()
    {
        if(_inputField.text == null || _inputField.text == "")
            return; 

        int i = int.Parse( _inputField.text); 
        if(i > 0 && i < 7)
            return; 

        // if code reached this point, input is not valid
        _inputField.text = null; 
    }
}
